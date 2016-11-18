using UnityEngine;
using System.Collections;

public class EndGame : MonoBehaviour {

	void OnTriggerEnter(Collider c){
		Application.LoadLevel("gameOverTest");
	}
}
