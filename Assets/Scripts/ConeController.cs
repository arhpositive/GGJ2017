using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConeController : MonoBehaviour {
	Plane hPlane;

	// Use this for initialization
	void Start () {
		hPlane = new Plane(Vector3.up, transform.position);

	}
	
	// Update is called once per frame
	void Update () {
		Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
		// Plane.Raycast stores the distance from ray.origin to the hit point in this variable:
		float distance = 0; 
		// if the ray hits the plane...
		if (hPlane.Raycast(ray, out distance)) {
			// get the hit point:
			transform.LookAt(ray.GetPoint(distance));
		}
	}
}
