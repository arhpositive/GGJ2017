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
	void Update () {
		if (Input.GetMouseButton(0)) {
			handwave.SetBool("IsWaving", true);
		} else {
			handwave.SetBool("IsWaving", false);
		}
	}
}
