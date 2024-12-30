using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackSprites : MonoBehaviour {

	private SpriteRenderer spriteRenderer;

	public Sprite[] sprites;
	private int currentSpriteIndex = 0;
	
	[SerializeField] private float timeBetweenSprites = 0.1f;
	private float timer = 0;
	[SerializeField] int repeatTimes = 1;
	void Start()
	{
		// Obtener el SpriteRenderer del objeto
		spriteRenderer = GetComponent<SpriteRenderer>();

		if (sprites.Length > 0 && spriteRenderer != null)
		{
			// Configurar el primer sprite
			spriteRenderer.sprite = sprites[currentSpriteIndex];
		}
		else
		{
			Debug.LogWarning("Asegúrate de asignar sprites en el inspector y que el objeto tenga un SpriteRenderer.");
		}
	}


	private void Update()
	{
		timer += Time.deltaTime;
		if (timer >= timeBetweenSprites)
		{
			timer = 0;
			NextSprite();
		}
	}

	// Método para cambiar al siguiente sprite
	public void NextSprite()
	{
		if (sprites.Length == 0 || spriteRenderer == null) return;

		
		// Incrementar el índice y asegurarse de que esté en rango
		currentSpriteIndex++;

		if (currentSpriteIndex >= sprites.Length)
		{
			repeatTimes--;
			currentSpriteIndex = 0;
			if (repeatTimes<=0)
			{
				//Destroy(gameObject);
				gameObject.SetActive(false);
				return;
			}
			
			
		}
		
		// Cambiar el sprite
		spriteRenderer.sprite = sprites[currentSpriteIndex];
	}

	// Método para cambiar al sprite anterior
	public void PreviousSprite()
	{
		if (sprites.Length == 0 || spriteRenderer == null) return;

		// Decrementar el índice y asegurarse de que esté en rango
		currentSpriteIndex = (currentSpriteIndex - 1 + sprites.Length) % sprites.Length;

		// Cambiar el sprite
		spriteRenderer.sprite = sprites[currentSpriteIndex];
	}
}
