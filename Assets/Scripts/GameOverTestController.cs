using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class GameOverTestController : MonoBehaviour {

	public float endEnding = 2;
	public float alphaSpeed = 1;
	float timeHappyEnding;
	float alphaParam;

	UnityEngine.UI.RawImage fade;
	
	public void Start(){
		timeHappyEnding = 0;
		alphaParam = 0;
		fade = GetComponentInChildren<UnityEngine.UI.RawImage>();
	}

	public void Update(){
		if(alphaParam >= 1){
			if(Input.GetMouseButtonDown(0) || Input.GetKey(KeyCode.Escape))
				Exit();
		}else if(timeHappyEnding > endEnding){
			alphaParam += alphaSpeed * Time.deltaTime;
			fade.color = Color.Lerp( Color.white, new Color(1f,1f,1f,0f), alphaParam);

		}else{
			timeHappyEnding += Time.deltaTime;

		}		
	}	


	public void Exit () {
		Application.Quit();
		Debug.Log("FECHOU");
	}

}
