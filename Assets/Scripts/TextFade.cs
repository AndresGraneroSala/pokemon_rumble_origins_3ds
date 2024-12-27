using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TextFade : MonoBehaviour
{

	[SerializeField] private float timeToFade = 2;

	private float _timer = 0;

	private Text _text;
	private Color _color;

	private void Start()
	{
		_text = GetComponent<Text>();
		_color = _text.color;
	}



	// Update is called once per frame
	void Update()
	{

		_timer += Time.deltaTime;



		if (_timer >= timeToFade)
		{
			Destroy(gameObject);
		}
		else
		{
			_color.a -= Time.deltaTime / timeToFade;
			_text.color = _color;
		}

		// Convertir la posición del mundo a pantalla

	}


}

