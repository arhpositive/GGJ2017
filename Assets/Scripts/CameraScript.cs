using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CameraScript : MonoBehaviour {
	public float impressedPedestrians = 0;
	public float totalPedestrians = 0;
	public Text scoreText;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		decimal score = (decimal) (impressedPedestrians / totalPedestrians * 100);
		scoreText.text = string.Format("Score: {0:C2}%", score);
	}

	public void IncreaseScore () {
		impressedPedestrians++;
	}

	public void AddPedestrians (int count) {
		totalPedestrians += count;
	}
}
