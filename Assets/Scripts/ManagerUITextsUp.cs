using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ManagerUITextsUp : MonoBehaviour
{
	public GameObject textPrefab; // Prefab con un componente Text
	public Transform store, pool;

	[SerializeField] private Camera upCamera;

	public static ManagerUITextsUp instance;

	[SerializeField] private int numOfTexts=10;
	private Queue<GameObject> textPool = new Queue<GameObject>();
    [SerializeField] private Color alpha;
	private void Awake()
	{
		instance = this;
	}

	[ContextMenu("num")]
	private void GetNumOfTexts()
	{
		print(textPool.Count);
	}
	
	private void Start()
	{
		for (int i = 0; i < numOfTexts; i++)
		{
			GameObject textInstance = Instantiate(textPrefab, pool);
			textInstance.SetActive(false);
			textPool.Enqueue(textInstance);
		}

	}

	public void AddText(GameObject textInstance)
	{
		textInstance.SetActive(false);
		//textInstance.transform.SetParent(pool);
		textPool.Enqueue(textInstance);
	}

	public void SetText(string message, Vector3 positionWorld,Color color)
	{
		if (textPrefab == null || store == null)
		{
			Debug.LogError(gameObject.name+ ": El prefab de texto o el Canvas no están configurados.");
			return;
		}

		// Instanciar el prefab
		GameObject textInstance = textPool.Dequeue();
		textInstance.SetActive(true);
		// Configurar el texto
		Text textComponent = textInstance.GetComponent<Text>();
		textComponent.text = message;
		textComponent.color = color;
		
		var fadeComponent = textInstance.GetComponent<TextFade>();
		fadeComponent.Run(() => AddText(textInstance));
		
		textComponent.GetComponent<UIWorldPostion>().SetPos(positionWorld);

	}
	

	
}
