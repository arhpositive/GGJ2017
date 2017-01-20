using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PrezMovement : MonoBehaviour {
	private Rigidbody rb;
	public int speed = 10;
	// Use this for initialization
	void Start () {
		rb = GetComponent<Rigidbody>();
	}
	
	// Update is called once per frame
	void FixedUpdate () {
		GameObject ground = GameObject.FindWithTag("Ground");
		Vector3 boundaries = ground.transform.localScale;
		
		float minX = -boundaries.x / 2 + transform.localScale.x / 2;
		float maxX = -minX;
		float minZ = -boundaries.z / 2 + transform.localScale.z / 2;
		float maxZ = -minZ;

		Vector3 movement = new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical")).normalized;

		transform.Translate(movement * speed * Time.fixedDeltaTime);

		float xClamped = Mathf.Clamp(transform.position.x, minX, maxX);
		float zClamped = Mathf.Clamp(transform.position.z, minZ, maxZ);

		transform.position = new Vector3(xClamped, transform.position.y, zClamped);
	}
}
