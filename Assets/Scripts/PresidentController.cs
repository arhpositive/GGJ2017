using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PresidentController : MonoBehaviour {
	public int staminaIncreaseRate = 5;
	public int staminaDecreaseRate = 10;
	Animator handwave;
	public int staminaMax = 100;
	public int staminaMin = 0;
	public int stamina = 100;
	public float staminaFillInterval = 1;
	public float idleTime = 0;
	// Use this for initialization
	void Start () {
		handwave = GetComponent<Animator>();
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
	}
	
	public void IncreaseStamina () {
		SetStamina(stamina + staminaIncreaseRate);
	}
	
	public void DecreaseStamina () {
		SetStamina(stamina - staminaDecreaseRate);
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
