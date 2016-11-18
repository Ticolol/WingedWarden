using UnityEngine;
using System.Collections;

public class AngelTriggers : MonoBehaviour {

	public bool warp = true;
	public bool disappear = false;
	public Waypoint waypoint;
	public float delayForFX;
	bool triggered;
	Angel angel;

	void Start(){
		angel = GameObject.Find("Angel").GetComponent<Angel>();
		triggered = false;
	}

	void OnTriggerEnter(){
		if(!triggered){
			triggered = true;
			if(!warp){
				angel.StartSearching(waypoint);
			}else{
				//angel
			}

			float timeToDisappear = 0;
			AudioSource a = GetComponent<AudioSource>();
			if(a!=null){
				timeToDisappear += a.clip.length;
				a.PlayDelayed(delayForFX);
			}

			if(disappear)
				Destroy(this.gameObject, timeToDisappear);
		}
	}
}
