using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LifeDestroy : MonoBehaviour
{

	[SerializeField] private float life=100;
	[SerializeField] private Lifebar lifebar;
	private float initLife=0;
	private void Awake()
	{
		initLife = life;
		lifebar.transform.SetParent(GameObject.Find("CanvasUP").transform);
		lifebar.gameObject.SetActive(false);
		
	}

	public void Damage(float damage, float multiplier)
	{
		float totalDamage = damage * multiplier;

		string messsage="";

		if (multiplier >= 0.0f && multiplier < 0.25f)
		{
			messsage = "Muy poco efectivo";
		}
		else if (multiplier >= 0.25f && multiplier < 0.5f)
		{
			messsage = "Poco efectivo";
		}
		else if (multiplier >= 0.5f && multiplier < 1.0f)
		{
			messsage = "Inmune";
		}
		else if (multiplier >= 1.0f && multiplier < 1.5f)
		{
			messsage = "";
		}
		else if (multiplier >= 1.5f && multiplier < 2.0f)
		{
			Debug.Log("Muy efectivo");
		}
		else
		{
			messsage = "Super efectivo";
		}
		
		ManagerUITextsUp.instance.SetText(messsage,transform.position-new Vector3(0,2.5f,0),Color.white);
		ManagerUITextsUp.instance.SetText((totalDamage).ToString("0"),transform.position,Color.red);

		lifebar.gameObject.SetActive(true);
		lifebar.ChangeLife((life-totalDamage)/initLife);
		lifebar.GetComponent<UIWorldPostion>().SetTransform(transform, new Vector3(0,-30,0));
		
		life -= totalDamage;
	}
	
	// Update is called once per frame
	void Update () {
		if (life<=0)
		{
			Destroy(gameObject);
		}
	}
	
	//TODO: barra de vida
	//refactorizar el ataque
}
