using UnityEngine;
using System.Collections;

public class Player : MonoBehaviour {

	public float walkVel = 5;
	public float sensitivityX = 0;
	public float sensitivityY = 10;
	public bool invertAxisY = true;
	public float interactionRange = 3f;

	public AudioClip jumpScareKill;

	Camera camera;

	Transform cameraParentTransform;
	Transform cameraTransform;
	int acquiredKey;

	bool dying;
	Transform killer;

	public bool moveCamera=true;

	// Use this for initialization
	void Start () {
		Cursor.visible = false;

		acquiredKey = 0;
		dying = false;

		camera = GetComponentInChildren<Camera>();

		cameraParentTransform = transform;
		cameraTransform = camera.transform;
	}
	
	void Update () {
		if(!dying){
			if(moveCamera)
				ControlCamera(Time.deltaTime);				
			if(Input.GetMouseButtonDown(0))
				Interact();
		}
	}
	void FixedUpdate () {
		if(!dying){
			if(moveCamera)
				ControlCamera(Time.deltaTime);
			Move(Time.deltaTime);

		}else{
			camera.transform.LookAt(killer.position + Vector3.up * 1.3f);//Match angel's face
			if(camera.fieldOfView > 10)
				camera.fieldOfView = camera.fieldOfView - 30*Time.deltaTime;
		}


		if(Input.GetKey(KeyCode.Escape))
			Application.Quit();
	}



	void ControlCamera(float deltaTime){
		float rotationX = Input.GetAxis("Mouse X") * sensitivityX;
		float rotationY = Input.GetAxis("Mouse Y") * sensitivityY;

		if(invertAxisY)
			rotationY*=-1;

		rotationX += cameraParentTransform.localEulerAngles.y;

		//Limit vertical rotation
		rotationY+=cameraTransform.localEulerAngles.x;
		if(cameraTransform.localEulerAngles.x <= 90 && rotationY > 90)
			rotationY = 90;
		if(cameraTransform.localEulerAngles.x >= 270 && rotationY < 270)
			rotationY = 270;

		//cameraParentTransform.Rotate(new Vector3(0, rotationX, 0), Space.Self);
		cameraParentTransform.localEulerAngles = new Vector3(0, rotationX, 0);
		//cameraTransform.Rotate(new Vector3(rotationY, 0, 0), Space.Self);
		cameraTransform.localEulerAngles = new Vector3(rotationY, 0, 0);

	}

	void Move(float deltaTime){
		//Get direction vectors
		Vector3 forwardDir = Vector3.ProjectOnPlane(Vector3.forward, Vector3.up);
		Vector3 rightDir = Vector3.ProjectOnPlane(Vector3.right, Vector3.up);

		//Debug.Log(cameraTransform.forward);

		float moveZ=0, moveX=0;
		//Get movement keycodes
		if(Input.GetKey(KeyCode.W)){
			moveZ += 1;
		}else if(Input.GetKey(KeyCode.S)){
			moveZ -= 1;
		}
		if(Input.GetKey(KeyCode.D)){
			moveX += 1;
		}else if(Input.GetKey(KeyCode.A)){
			moveX -= 1;
		}

		//Apply movement
		Vector3 finalMove = Vector3.Normalize((moveZ * forwardDir) + (moveX * rightDir));
		finalMove *= deltaTime * walkVel;
		transform.Translate(finalMove);
	}

	void Interact(){
		Ray r = new Ray(cameraTransform.position,
							cameraTransform.forward);
		RaycastHit h = new RaycastHit();
		if(Physics.Raycast(r, out h,  Mathf.Abs(interactionRange))){
			if(h.collider.gameObject.tag=="Key"){
				GetKey(h.collider.gameObject);
			}else if(h.collider.gameObject.tag=="Padlock"){
				UnlockPadlock(h.collider.gameObject);
			}
		}
		Debug.DrawRay(r.origin, r.direction * interactionRange);				         
	}

	void GetKey(GameObject key){
		string keyName = key.name;
		acquiredKey = keyName[keyName.Length-1]-48;
		GetComponent<AudioSource>().PlayOneShot(key.GetComponent<AudioSource>().clip);
		Destroy(key);
		Debug.Log("=> GOT KEY " + acquiredKey + "!");
	}

	void UnlockPadlock(GameObject padlock){
		string padlockName = padlock.name;		
		int acquiredPadlock = padlockName[padlockName.Length-1]-48;
		GetComponent<AudioSource>().PlayOneShot(padlock.GetComponent<AudioSource>().clip);
		if(acquiredKey==acquiredPadlock){
			acquiredKey = 0;
			Destroy(padlock);
			Debug.Log("=> UNLOCKED PADLOCK " + acquiredPadlock + "!");
		}

	}

	public void Die(Transform k){
		dying = true;
		killer = k;
		GetComponent<AudioSource>().PlayOneShot(jumpScareKill);
	}

}
