using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LifeDestroy : MonoBehaviour
{

	[SerializeField] private float life=100;
	[SerializeField] private Lifebar lifebar;
	private float initLife=0;
	private bool isPlayer = false;
	private void Start()
	{
		isPlayer = gameObject.tag == "Player";
		initLife = life;
		//lifebar.transform.SetParent(GameObject.Find("CanvasUP").transform);
		lifebar.gameObject.SetActive(false);
		
	}

	public void Damage(float damage, float multiplier)
	{
		float totalDamage = damage * multiplier;

		string messsage="";
		Color color = Color.white;

		if (multiplier == 0.0f)
		{
			messsage = "Inmune";
			color = new Color(0.0f, 0.0f, 0.6f); 

		}
		else if (multiplier > 0.0f && multiplier < 0.25f)
		{
			messsage = "Muy poco efectivo";
			color = new Color(.6784f, 0.8471f, 0.9019f);			
		}
		else if (multiplier >= 0.25f && multiplier < 0.5f)
		{
			messsage = "Poco efectivo";
			color = new Color(0.8f, 0.8f, 0.2f); // azul
		}
		else if (multiplier >= 0.5f && multiplier < 1.0f)
		{
			messsage = "Normalillo";
			color = new Color(1f, 1f, 1f); // Blanco
		}
		else if (multiplier >= 1.0f && multiplier < 1.5f)
		{
			messsage = "Eficaz";
			color = new Color(0.6f, 1f, 0.6f); // blanco
		}
		else if (multiplier >= 1.5f && multiplier < 2.0f)
		{
			messsage = "Muy efectivo";
			color = new Color(1f, 0.275f, 0f); // Naranja
		}
		else
		{
			messsage = "Super efectivo";
			color = new Color(1f, 0f, 0f); // Rojo brillante
		}
		
		//TextManager.instance.InstanceText(messsage,transform.position-new Vector3(0,2.5f,0),color);
		//TextManager.instance.InstanceText(totalDamage.ToString("0"),transform.position,color);
		
		if (!lifebar.isActiveAndEnabled)
		{
			lifebar.gameObject.SetActive(true);
		}
		lifebar.ChangeLife((life-totalDamage)/initLife);
		//lifebar.GetComponent<UIWorldPostion>().SetTransform(transform, new Vector3(0,-20,0));
		
		life -= totalDamage;
		
		
		if (life<=0)
		{
			if (isPlayer)
			{
				GameManager.instance.EndGame();
			}
			else
			{
				//Destroy(gameObject);
				CoinManager.instance.SumCombo();
				CoinManager.instance.SpawnCoins(gameObject.transform.position);
				GetComponent<CatchPokemon>().ChangeToPlayer();
			}
		}
	}
}
