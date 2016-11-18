using UnityEngine;
using System.Collections;

public class Angel : MonoBehaviour {

	float DISTSKIN = 0.1f;
	float ROTSKIN = .25f;
	float SKINPLAYER  =1;

	enum States {Idle, Searching, Warping, Killing};
	States state;

	Waypoint[] waypoints;
	Waypoint target;

	Transform player;

	public float movementSpeed = 5;
	public float rotationSpeed = 90;

	public float forwardLookAngle = 20;
	public float forwardLookDist = 15;
	public float nearLookAngle = 60;
	public float nearLookDist = 4;

	void Start () {
		waypoints = GameObject.Find("Waypoints").GetComponentsInChildren<Waypoint>();
		player = GameObject.Find("Player").transform;
		state = States.Idle;

	}	

	void Update(){
		switch(state){
			case States.Idle:	
				FSM_Idle(Time.deltaTime);
				break;
			case States.Searching:	
				FSM_Searching(Time.deltaTime);
				break;
			case States.Killing:	
				FSM_Killing(Time.deltaTime);
				break;				
		}
	}

	void OnDrawGizmos(){
		Gizmos.color = new Color(1, 0, 0, 1);
		Gizmos.DrawRay(transform.position, RotateVector(transform.forward, transform.right, forwardLookAngle) * forwardLookDist);
		Gizmos.DrawRay(transform.position, RotateVector(transform.forward, transform.right, -forwardLookAngle) * forwardLookDist);
		Gizmos.DrawRay(transform.position, RotateVector(transform.forward, transform.right, nearLookAngle) * nearLookDist);
		Gizmos.DrawRay(transform.position, RotateVector(transform.forward, transform.right, -nearLookAngle) * nearLookDist);

	}
	Vector3 RotateVector(Vector3 v, Vector3 r, float a){
		Vector3 vv = r * Mathf.Sin(Mathf.Deg2Rad * a) + v * Mathf.Cos(Mathf.Deg2Rad * a) ;		
		return vv;
	}



	void FSM_Idle(float deltaTime){

	}

	public void StartSearching(Waypoint w){
		target = w;
		state = States.Searching;

	}

	void FSM_Searching (float deltaTime){
		if(SeesPlayer()){
			Kill();
		}else if(ArrivedAt(target)){ //If arrived at target, select a new one
			//Debug.Log(target.name);
			target = SetNewTarget(target);
			//Debug.Log(target.name);
		}
		Move(deltaTime);
	}



	Waypoint SetNewTarget(Waypoint target){
		/*if(PlayerTooFar()){
			return WarpToNearWaypoint(player);
		}else{*/
			Waypoint[] candidates = target.adjacency;
			int rand = Random.Range(0, candidates.Length);
			return candidates[rand];
		//}
	}



	bool SeesPlayer(){
		float rotateDist = RotationTo(player.position, transform.rotation.eulerAngles.y);
		//Check forward vision
		if(Mathf.Abs(rotateDist)<=forwardLookAngle){
			if(Vector3.Distance(transform.position, player.position) <= forwardLookDist){
				//RAYCAST NO DANADO
				if(GotOnRaycast(forwardLookDist))
					return true;
			}
		}else if(Mathf.Abs(rotateDist)<=nearLookAngle && 
					Vector3.Distance(transform.position, player.position) <= nearLookDist){
			//RAYCAST NO DANADO
			if(GotOnRaycast(nearLookDist))
				return true;
		}
		//Check near vision
		return false;
	}

	bool GotOnRaycast(float dist){
		Ray r = new Ray(transform.position, player.position - transform.position + Vector3.up * .5f);
		RaycastHit h = new RaycastHit();
		Debug.DrawRay(r.origin, r.direction * dist);
		if(Physics.Raycast(r, out h, dist)){
			print(h.collider.gameObject.tag);
			if(h.collider.gameObject.tag == "Player")
				return true;			
		}
		return false;
	}


	void Move(float deltaTime){
		float rotateDist = RotationTo(target.transform.position, transform.rotation.eulerAngles.y);		

		if(Mathf.Abs(rotateDist) > 2*ROTSKIN){
			//Rotate			
			int sign = (int)Mathf.Sign(rotateDist);
			float valueToRotate = Mathf.Min(rotationSpeed * deltaTime, Mathf.Abs(rotateDist))
									 * sign;
			transform.Rotate(new Vector3(0,valueToRotate,0));
		}else{
			//Move
			float valueToMove = Mathf.Min(movementSpeed * deltaTime, Vector3.Distance(transform.position, target.transform.position));
			transform.Translate(Vector3.forward * valueToMove);
		}
	}

	void Kill(){
		Debug.Log("MORREU TROXÃO!");
		state = States.Killing;
		transform.Rotate(0, RotationTo(player.position, transform.rotation.eulerAngles.y),0);
		player.gameObject.GetComponent<Player>().Die(transform);
		GameObject.Find("MusicManager").GetComponent<AudioSource>().pitch = 3;
	}

	void FSM_Killing(float deltaTime){
		if(Vector3.Distance(transform.position, player.position) > SKINPLAYER){
			transform.Rotate(0, RotationTo(player.position, transform.rotation.eulerAngles.y),0);
			float valueToMove = Mathf.Min(movementSpeed * deltaTime, Vector3.Distance(transform.position, target.transform.position));
			transform.Translate(Vector3.forward * valueToMove);
		}else{
			Debug.Break();
			Application.LoadLevel("mainTest");
		}

	}






	bool ArrivedAt(Waypoint t){
		if(Vector3.Distance(transform.position, t.transform.position) <= DISTSKIN)
			return true;
		return false;
	}

	float RotationTo(Vector3 t, float rot){
		Vector3 cat = t - transform.position;
		cat.y = 0;
		float hip = Vector3.Distance(t, transform.position);		
		float angle = Mathf.Rad2Deg * (Mathf.Atan(cat.x/cat.z));

		if(cat.z<0){				
			angle=(180+angle);
			if(cat.x<0)
				angle-=360;
		}	
	
		rot = ConvertAngletoNeg(rot);
		return angle - rot;	
	}

	float ConvertAngleto360(float a){
		if(a < 0)
			a+=360;
		return a;
	}
	float ConvertAngletoNeg(float a){
		if(a > 180)
			a-=360;
		return a;
	}
}
