using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Serialization;

public class CoinManager : MonoBehaviour {

	public static CoinManager instance;

	[SerializeField] private int combo=0;
	private float timer=0;
	[SerializeField] private float timeToClaimCoins=0.5f;

	public float TimeToClaimCoins
	{
		get { return timeToClaimCoins; }
	}

	[SerializeField] private CoinCombo [] comboCoins;
	[FormerlySerializedAs("resetTimer")] [SerializeField] private float timeOrigin=0;
	[SerializeField] private int totalCoins=0;
	[FormerlySerializedAs("text")] [SerializeField] private Text textCoins;
	[SerializeField] private Text textTimeToClaim;
	[SerializeField] private Text textCombos;
	private void Awake()
	{
		if (instance == null)
		{
			instance = this;
		}
		else
		{
			Destroy(gameObject);
		}
	}

	private void Start()
	{
		textCoins.text = totalCoins.ToString();
	}

	public void ClaimCoin(int coin)
	{
		totalCoins+=coin;
		textCoins.text = totalCoins.ToString();
	}

	private void Update()
	{
		timer -= Time.deltaTime;
		textTimeToClaim.text = timer.ToString("0.0");
		if (timer < 0)
		{
			ResetTimer();
			combo = 0;
			textCombos.text = combo.ToString();
		}
	}

	public void ResetTimer()
	{
		timer = timeOrigin;
	}

	public void SumCombo()
	{
		combo++;
		timer = timeOrigin;
		textCombos.text = combo.ToString();
	}
	
	[ContextMenu("spawn") ]
	public void SpawnCoins(Vector3 position)
	{
		ResetTimer();
		
		for (int i = comboCoins.Length-1; i >=0 ; i--)
		{
			 if (comboCoins[i].Combo > combo)
			 {
				 continue;
			 }
			 
			 Instantiate(comboCoins[i].Prefab, position, comboCoins[i].Prefab.transform.rotation);
			 break;
		}
		
		
	}

	[Serializable]
	class CoinCombo
	{
		[SerializeField] private int combo;
		public int Combo
		{
			get { return combo; }
		}
		
		[SerializeField] private GameObject prefab;
		public GameObject Prefab
		{
			get { return prefab; }
		}
	}
	
	
}
