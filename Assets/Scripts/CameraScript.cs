using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class CameraScript : MonoBehaviour {
	public float impressedPedestrians = 0;
	public float totalPedestrians = 0;
	public Text scoreText;
	public Text endGameScoreText;
	public Text successMessage;
	public Text failureMessage;

    public GameObject StartGameObject;

    void Awake()
    {
        Time.timeScale = 0.0f;
    }

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		decimal score = (decimal) (impressedPedestrians / totalPedestrians * 100);

		// update onscreen score
		scoreText.text = string.Format("Score: {0:C2}%", score);

		// update endgame score
		endGameScoreText.text = string.Format("{0:C2}", score);
        
		// set success/failure text on endgame depending on score
		if (score > 50) {
			successMessage.enabled = true;
			failureMessage.enabled = false;
		} else {
			successMessage.enabled = false;
			failureMessage.enabled = true;
		}

		// listen to escape button press
		if (Input.GetKey("escape")) {
			ReturnToStartScreen();
		}
	}

	public void IncreaseScore () {
		impressedPedestrians++;
	}

	public void AddPedestrians (int count) {
		totalPedestrians += count;
	}

    public void ReturnToStartScreen()
    {
        SceneManager.LoadScene(0);
    }

    public void BeginGame()
    {
       Time.timeScale = 1.0f;
       StartGameObject.SetActive(false);
    }

    public void RestartMission()
    {
        SceneManager.LoadScene(0);
    }

	public void ExitGame () {
		Application.Quit();
	}
}
