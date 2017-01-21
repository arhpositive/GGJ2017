using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PresidentController : MonoBehaviour {
	Animator handwave;
	// Use this for initialization
	void Start () {
		handwave = GetComponent<Animator>();
	}
	
	// Update is called once per frame
	void FixedUpdate () {
		if (Input.GetMouseButtonUp(0)) {
			handwave.Stop();
		}
	}
}
