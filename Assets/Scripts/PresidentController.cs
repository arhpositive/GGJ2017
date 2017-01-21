using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PresidentController : MonoBehaviour {
	public int staminaIncreaseRate = 5;
	public int staminaDecreaseRate = 10;
	Animator handwave;
	public int staminaMax = 100;
	public int staminaMin = 0;
	public int stamina = 100;
	public float staminaFillInterval = 1;
	public float idleTime = 0;
    private GameObject _coneGameObject;
    private GameObject _handGameObject;
    // Use this for initialization
    public Text staminaText;
	void Start () {
		handwave = GetComponent<Animator>();
        _coneGameObject = GameObject.FindGameObjectWithTag("Cone");
        _coneGameObject.GetComponent<Renderer>().material.color = new Color(1, 1, 1, 0.2f);
        _handGameObject = GameObject.FindGameObjectWithTag("hand");
    }
	
	// Update is called once per frame
	void Update () {
		if (Input.GetMouseButton(0)) {
			if (stamina >= staminaDecreaseRate) {
				handwave.SetBool("IsWaving", true);
			}
			else {
				handwave.SetBool("IsWaving", false);
			}
		} else {
			handwave.SetBool("IsWaving", false);
		}

		if (handwave.GetBool("IsWaving")) {
			idleTime = 0;
		} else {
			idleTime = idleTime + Time.deltaTime;

			if (idleTime > staminaFillInterval) {
				idleTime = 0;
				IncreaseStamina();
			}
		}
	}

	private void SetStamina (int nextStamina) {
		stamina = Mathf.Clamp(nextStamina, staminaMin, staminaMax);
		staminaText.text = "Stamina: " + stamina;
	    float staminaPercentage = (float) stamina/staminaMax;
        _handGameObject.GetComponent<Renderer>().material.color = new Color(1, staminaPercentage, staminaPercentage, 1);
    }
	
	public void IncreaseStamina () {
		SetStamina(stamina + staminaIncreaseRate);
	}
	
	public void DecreaseStamina () {
		SetStamina(stamina - staminaDecreaseRate);
	}

    public void ActivateConeForHandwave()
    {
        _coneGameObject.GetComponent<Collider>().enabled = true;
        _coneGameObject.GetComponent<Renderer>().material.color = new Color(1,0,0,0.2f);
    }

    public void DeactivateConeForHandwave()
    {
        _coneGameObject.GetComponent<Collider>().enabled = false;
        _coneGameObject.GetComponent<Renderer>().material.color = new Color(1, 1, 1, 0.2f);
    }
}
