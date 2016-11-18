using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class TitleScreenManager : MonoBehaviour {

	public float fadeTime = 2f;

	UnityEngine.UI.Image fader;
	GameObject titleScreen;
	DialogueSimple dialogue;
	GameObject controls;

	bool openControls;

	void Start () {
		fader = transform.Find("Fader").GetComponent<UnityEngine.UI.Image>();
		titleScreen	= transform.Find("TitleScreen").gameObject;
		dialogue = transform.Find("Dialog").GetComponent<DialogueSimple>();
		controls = transform.Find("Controls").gameObject;

		dialogue.gameObject.SetActive(false);
		fader.gameObject.SetActive(false);
		controls.SetActive(false);

		openControls = false;
	}
		
	void Update () {
	
	}

	public void StartGame(){
		print("COMECOU");
		FadeIn();
	}

	public void ExitGame(){
		print("VALEU");
		Application.Quit();
	}

	public void FadeIn(){
		StartCoroutine("IEStartGame");
	}

	public void GoToGame(){
		openControls = true;
	}

	IEnumerator IEStartGame(){
		yield return StartCoroutine(IEFadeIn());
		titleScreen.SetActive(false);
		dialogue.StartDialogue(GoToGame);
		yield return StartCoroutine(IEFadeOut());
		while(!openControls)
			yield return null;
		yield return new WaitForSeconds(1);
		dialogue.gameObject.SetActive(false);
		controls.SetActive(true);
		yield return StartCoroutine(IEFadeOut());
		yield return new WaitForSeconds(2);
		controls.transform.Find("Loading").gameObject.SetActive(true);
		Application.LoadLevel(1);
	}

	IEnumerator IEFadeIn(){
		float fadeSpeed = 1/fadeTime;
		float percentage = 0;
		//Set visible and transparent
		fader.gameObject.SetActive(true);
		Color c = fader.color;
		c.a = 0;
		fader.color = c;
		//Start iterating
		while(percentage<1){
			percentage += fadeSpeed * Time.deltaTime;
			c.a = percentage;
			fader.color = c;
			yield return null;
		}
	}

	IEnumerator IEFadeOut(){
		float fadeSpeed = 1/fadeTime;
		float percentage = 1;
		//Set visible and transparent
		fader.gameObject.SetActive(true);
		Color c = fader.color;
		c.a = 1;
		fader.color = c;
		//Start iterating
		while(percentage>0){
			
			percentage -= fadeSpeed * Time.deltaTime;
			c.a = percentage;
			fader.color = c;
			yield return null;
		}
		fader.gameObject.SetActive(false);
	}

}
