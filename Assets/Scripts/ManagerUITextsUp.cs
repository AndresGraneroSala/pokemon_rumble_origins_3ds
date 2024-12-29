using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ManagerUITextsUp : MonoBehaviour
{
	public GameObject textPrefab; // Prefab con un componente Text
	public Transform store;

	[SerializeField] private Camera upCamera;

	public static ManagerUITextsUp instance;

	private void Awake()
	{
		instance = this;
	}

	public void SetText(string message, Vector3 positionWorld,Color color)
	{
		if (textPrefab == null || store == null)
		{
			Debug.LogError(gameObject.name+ ": El prefab de texto o el Canvas no están configurados.");
			return;
		}

		// Instanciar el prefab
		GameObject textInstance = Instantiate(textPrefab, store);

		// Configurar el texto
		Text textComponent = textInstance.GetComponent<Text>();
		textComponent.text = message;
		textComponent.color = color;

		textComponent.GetComponent<UIWorldPostion>().SetPos(positionWorld);

	}
	
	public void ChangeLifeBar(string message, Vector3 positionWorld)
	{
		if (textPrefab == null || store == null)
		{
			Debug.LogError("El prefab de texto o el Canvas no están configurados.");
			return;
		}

		// Instanciar el prefab
		GameObject textInstance = Instantiate(textPrefab, store);

		// Configurar el texto
		Text textComponent = textInstance.GetComponent<Text>();
		textComponent.text = message;

		//textComponent.GetComponent<UIWorldPostion>().SetPos(positionWorld);

	}
	
}
