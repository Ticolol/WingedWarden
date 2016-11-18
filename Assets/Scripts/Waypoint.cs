using UnityEngine;
using System.Collections;

public class Waypoint : MonoBehaviour {

	public Waypoint[] adjacency;

	void OnDrawGizmos(){		
		Gizmos.color = new Color(0, 0, 1, 1);
		Gizmos.DrawSphere(transform.position, .5f);
		foreach (Waypoint adj in adjacency){
			Gizmos.DrawLine(transform.position, adj.transform.position);
		}
	}

}
