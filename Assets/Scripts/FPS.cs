﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FPS : MonoBehaviour {

	[SerializeField] private Text text;
	
	// Update is called once per frame
	void Update () {
		text.text = (1/Time.deltaTime).ToString();
	}
}
