using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PresidentController : MonoBehaviour {
	Animator handwave;
	public int stamina = 100;
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

	public void DecreaseStamina () {
		stamina = stamina - 10;
	}

    public void ActivateConeForHandwave()
    {
        GameObject go1 = GameObject.FindGameObjectWithTag("Cone");
        go1.GetComponent<Collider>().enabled = true;
    }

    public void DeactivateConeForHandwave()
    {
        GameObject go1 = GameObject.FindGameObjectWithTag("Cone");
        go1.GetComponent<Collider>().enabled = false;
    }
}
