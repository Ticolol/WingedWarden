using UnityEngine;
using System.Collections;

public class DrawWallBox : MonoBehaviour {

	public float SKIN = 0.01f;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}


	void OnDrawGizmos(){		
		Collider c = GetComponent<BoxCollider>();
 

		Gizmos.color = new Color(1, 1, 0, 0.5f);
        Gizmos.DrawCube(c.bounds.center + transform.forward * SKIN, c.bounds.size);

	}
}
