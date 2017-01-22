using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TextScript : MonoBehaviour {
	int initialFontSize;
	Text text;

	void Start () {
		text = GetComponent<Text>();
		initialFontSize = text.fontSize;
	}

	void Update () {
		text.fontSize = (int) ((float) Screen.width / (float) 590 * (float) initialFontSize);
	}
	
}
