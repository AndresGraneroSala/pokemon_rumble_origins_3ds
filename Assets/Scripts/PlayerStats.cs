using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class PlayerStats : MonoBehaviour
{

	[SerializeField] private Attack.TypeAttack type1;

	public Attack.TypeAttack Type1
	{
		get { return type1; }
	}
	[SerializeField] private Attack.TypeAttack type2;

	public Attack.TypeAttack Type2
	{
		get { return type2; }
	}

	[FormerlySerializedAs("name")] [SerializeField] private string playerName;

	public string PlayerName
	{
		get { return playerName; }
	}
}
