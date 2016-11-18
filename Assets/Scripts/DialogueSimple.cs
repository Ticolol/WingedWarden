using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class DialogueSimple : MonoBehaviour {

	[TextAreaAttribute(0,5)]
	public string[] lines;
	int lineIndex;

	UnityEngine.UI.Text textField;

	public delegate void EndFunction();

	EndFunction endFunction;

	// Use this for initialization
	void Awake () {

		textField = GetComponentInChildren<UnityEngine.UI.Text>();
		textField.text = "";
	}
	
	// Update is called once per frame
	void Update () {
		if(Input.GetMouseButtonDown(0))
			NextDialogue();
	}

	public void StartDialogue(EndFunction e){
		endFunction = e;
		gameObject.SetActive(true);
		lineIndex = 0;
		textField.text = lines[lineIndex];
	}

	public void NextDialogue(){
		lineIndex += 1;
		if(lineIndex < lines.Length){
			textField.text = lines[lineIndex];
		}else{
			gameObject.SetActive(false);
			endFunction();
		}
	}


}
