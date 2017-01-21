using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConeController : MonoBehaviour {
	Vector3 distanceToPlayer;
	GameObject prez;
	Plane hPlane;

	// Use this for initialization
	void Start () {
		prez = GameObject.FindWithTag("Player");
		distanceToPlayer = transform.position - prez.transform.position;
		hPlane = new Plane(Vector3.up, transform.position);

	}
	
	// Update is called once per frame
	void FixedUpdate () {
		transform.position = prez.transform.position + distanceToPlayer;
		// Vector3 mouse = Input.mousePosition;
		// mouse.z = 10f - .45f / 2f;
		// Vector3 worldPoint = Camera.main.ScreenToWorldPoint(mouse);
		// worldPoint.y = 0;
		// transform.LookAt(worldPoint);

		Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
		// Plane.Raycast stores the distance from ray.origin to the hit point in this variable:
		float distance = 0; 
		// if the ray hits the plane...
		if (hPlane.Raycast(ray, out distance)) {
			// get the hit point:
			transform.LookAt(ray.GetPoint(distance));
		}
		// Quaternion angle = Quaternion.AngleAxis(0, Vector3.right);
		// transform.rotation = angle;
		// Vector3 prezPos = Camera.main.WorldToScreenPoint(prez.transform.position);
		// Vector3 relative = prezPos - Input.mousePosition;
        // float angle = Mathf.Atan2(relative.x, relative.z) * Mathf.Rad2Deg;
		// //print(prezPos);
		// //print(relative);
        // transform.rotation = Quaternion.Euler(90, angle, 0);
	}
}
