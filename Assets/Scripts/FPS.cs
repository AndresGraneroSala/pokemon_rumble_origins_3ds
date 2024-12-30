using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FPS : MonoBehaviour {

	[SerializeField] private Text text;
	private float elapsedTime = 0f;
	private int frameCount = 0;
	private float updateInterval = 0.5f; // Actualizar cada 1 segundo

	void Update()
	{
		elapsedTime += Time.deltaTime;
		frameCount++;

		if (elapsedTime >= updateInterval)
		{
			int fps = Mathf.RoundToInt(frameCount / elapsedTime);
			text.text = fps + " FPS";
			elapsedTime = 0f;
			frameCount = 0;
		}
	}
}
