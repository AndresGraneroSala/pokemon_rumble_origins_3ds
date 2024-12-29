using System;

using UnityEngine;
using UnityEngine.Serialization;

[CreateAssetMenu(fileName = "NewAttack", menuName = "Pokemon/Attack", order = 1)]
public class Attack: ScriptableObject
{
	public enum TypeAttack
	{
		Normal,
		Fire,
		Water,
		Electric,
		Grass,
		Ice,
		Fighting,
		Poison,
		Ground,
		Flying,
		Psychic,
		Bug,
		Rock,
		Ghost,
		Dragon,
		Dark,
		Steel,
		Fairy,
		Stellar,
		None
	}

	[SerializeField] private TypeAttack type;

	public TypeAttack Type
	{
		get { return type; }
	}

	[SerializeField] private float damage = 50;

	public float Damage
	{
		get { return damage; }
	}

	[SerializeField] private float fireRate = 2;

	public float FireRate
	{
		get { return fireRate; }
	}

	[SerializeField] private float delay = 0;

	public float Delay
	{
		get { return delay; }
	}
	
	[SerializeField] private float delayOpponent = 0;

	public float DelayOpponent
	{
		get { return delayOpponent; }
	}

	[SerializeField] private GameObject bullet;

	public GameObject Bullet
	{
		get { return bullet; }
	}

	[SerializeField] private bool isBulletBefore = false;

	public bool IsBulletBefore
	{
		get { return isBulletBefore; }
	}

	[SerializeField] private float movePlayer = 0;

	public float MovePlayer
	{
		get { return movePlayer; }
	}

	[SerializeField] private float movePlayerSpeed = 1;

	public float MovePlayerSpeed
	{
		get { return movePlayerSpeed; }
	}

	[SerializeField] private Rotate rotate;

	public Rotate Rotate
	{
		get { return rotate; }
	}

	public Effect effect;

	public Effect Effect
	{
		get { return effect; }
	}

	public static float GetMultiplicatorType(TypeAttack attack, TypeAttack defend)
	{
		if (attack== TypeAttack.Stellar|| defend== TypeAttack.Stellar)
		{
			return 1;
		}
		
		switch (defend)
		{
			case TypeAttack.Normal:

				switch (attack)
				{
					case TypeAttack.Fighting: return 2;
					case TypeAttack.Ghost: return 0;
				}

				break;

			case TypeAttack.Fighting:

				switch (attack)
				{
					case TypeAttack.Flying: return 2;
					case TypeAttack.Rock: return 0.5f;
					case TypeAttack.Bug: return 0.5f;
					case TypeAttack.Psychic: return 2;
					case TypeAttack.Dark: return 0.5f;
					case TypeAttack.Fairy: return 2;
					default: return 1;
				}

			case TypeAttack.Flying:

				switch (attack)
				{
					case TypeAttack.Fighting: return 0.5f;
					case TypeAttack.Ground: return 0;
					case TypeAttack.Rock: return 2;
					case TypeAttack.Bug: return 0.5f;
					case TypeAttack.Grass: return 0.5f;
					case TypeAttack.Electric: return 2;
					case TypeAttack.Ice: return 2;
					default: return 1;
				}

			case TypeAttack.Poison:

				switch (attack)
				{
					case TypeAttack.Fighting: return 0.5f;
					case TypeAttack.Poison: return 0.5f;
					case TypeAttack.Ground: return 2;
					case TypeAttack.Bug: return 0.5f;
					case TypeAttack.Grass: return 0.5f;
					case TypeAttack.Psychic: return 2;
					case TypeAttack.Fairy: return 0.5f;
					default: return 1;

				}

			case TypeAttack.Ground:

				switch (attack)
				{
					case TypeAttack.Poison: return 0.5f;
					case TypeAttack.Rock: return 0.5f;
					case TypeAttack.Water: return 2;
					case TypeAttack.Grass: return 2;
					case TypeAttack.Electric: return 0;
					case TypeAttack.Ice: return 2;
					default: return 1;

				}

			case TypeAttack.Rock:
				switch (attack)
				{
					case TypeAttack.Normal: return 0.5f;
					case TypeAttack.Fighting: return 2;
					case TypeAttack.Flying: return 0.5f;
					case TypeAttack.Poison: return 0.5f;
					case TypeAttack.Ground: return 2;
					case TypeAttack.Steel: return 2;
					case TypeAttack.Fire: return 0.5f;
					case TypeAttack.Water: return 2;
					case TypeAttack.Grass: return 2;
					default: return 1;
				}

			case TypeAttack.Bug:
				switch (attack)
				{
					case TypeAttack.Fighting: return 0.5f;
					case TypeAttack.Flying: return 2;
					case TypeAttack.Ground: return 0.5f;
					case TypeAttack.Rock: return 2;
					case TypeAttack.Fire: return 2;
					case TypeAttack.Grass: return 0.5f;
					default: return 1;
				}

			case TypeAttack.Ghost:
				switch (attack)
				{
					case TypeAttack.Normal: return 0;
					case TypeAttack.Fighting: return 0;
					case TypeAttack.Poison: return 0.5f;
					case TypeAttack.Bug: return 0.5f;
					case TypeAttack.Ghost: return 2;
					case TypeAttack.Dark: return 2;
					default: return 1;
				}

			case TypeAttack.Steel:
				switch (attack)
				{
					case TypeAttack.Normal: return 0.5f;
					case TypeAttack.Fighting: return 2;
					case TypeAttack.Flying: return 0.5f;
					case TypeAttack.Poison: return 0;
					case TypeAttack.Ground: return 2;
					case TypeAttack.Rock: return 0.5f;
					case TypeAttack.Bug: return 0.5f;
					case TypeAttack.Steel: return 0.5f;
					case TypeAttack.Fire: return 2;
					case TypeAttack.Grass: return 0.5f;
					case TypeAttack.Psychic: return 0.5f;
					case TypeAttack.Ice: return 0.5f;
					case TypeAttack.Dragon: return 0.5f;
					case TypeAttack.Fairy: return 0.5f;
					default: return 1;
				}

			case TypeAttack.Fire:
				switch (attack)
				{
					case TypeAttack.Ground: return 2;
					case TypeAttack.Rock: return 2;
					case TypeAttack.Bug: return 0.5f;
					case TypeAttack.Steel: return 0.5f;
					case TypeAttack.Fire: return 0.5f;
					case TypeAttack.Water: return 2;
					case TypeAttack.Grass: return 0.5f;
					case TypeAttack.Ice: return 0.5f;
					case TypeAttack.Fairy: return 0.5f;
					default: return 1;
				}

			case TypeAttack.Water:
				switch (attack)
				{
					case TypeAttack.Steel: return 0.5f;
					case TypeAttack.Fighting: return 0.5f;
					case TypeAttack.Water: return 0.5f;
					case TypeAttack.Grass: return 2;
					case TypeAttack.Electric: return 2;
					case TypeAttack.Ice: return 0.5f;


					default: return 1;
				}

			case TypeAttack.Grass:
				switch (attack)
				{
					case TypeAttack.Flying: return 2;
					case TypeAttack.Poison: return 2;
					case TypeAttack.Ground: return 0.5f;
					case TypeAttack.Bug: return 2;
					case TypeAttack.Fire: return 2;
					case TypeAttack.Water: return 0.5f;
					case TypeAttack.Grass: return 0.5f;
					case TypeAttack.Electric: return 0.5f;
					case TypeAttack.Ice: return 2;
					default: return 1;
				}

			case TypeAttack.Electric:
				switch (attack)
				{
					case TypeAttack.Ground: return 2;
					case TypeAttack.Flying: return 0.5f;
					case TypeAttack.Steel: return 0.5f;
					case TypeAttack.Electric: return 0.5f;
					default: return 1;
				}

			case TypeAttack.Psychic:
				switch (attack)
				{
					case TypeAttack.Fighting: return 0.5f;
					case TypeAttack.Bug: return 2;
					case TypeAttack.Ghost: return 2;
					case TypeAttack.Psychic: return 0.5f;
					case TypeAttack.Dark: return 2;
					default: return 1;
				}

			case TypeAttack.Ice:
				switch (attack)
				{
					case TypeAttack.Fighting: return 2;
					case TypeAttack.Rock: return 2;
					case TypeAttack.Steel: return 2;
					case TypeAttack.Fire: return 2;
					case TypeAttack.Ice: return 0.5f;
					default: return 1;
				}

			case TypeAttack.Dragon:
				switch (attack)
				{
					case TypeAttack.Fire: return 0.5f;
					case TypeAttack.Water: return 0.5f;
					case TypeAttack.Grass: return 0.5f;
					case TypeAttack.Electric: return 0.5f;
					case TypeAttack.Ice: return 2;
					case TypeAttack.Dragon: return 2;
					case TypeAttack.Fairy: return 2;
					default: return 1;
				}

			case TypeAttack.Dark:
				switch (attack)
				{
					case TypeAttack.Fighting: return 2;
					case TypeAttack.Bug: return 2;
					case TypeAttack.Ghost: return 0.5f;
					case TypeAttack.Psychic: return 0;
					case TypeAttack.Dark: return 0.5f;
					case TypeAttack.Fairy: return 2;
					default: return 1;
				}

			case TypeAttack.Fairy:
				switch (attack)
				{
					case TypeAttack.Fighting: return 0.5f;
					case TypeAttack.Poison: return 2;
					case TypeAttack.Bug: return 0.5f;
					case TypeAttack.Steel: return 2;
					case TypeAttack.Dragon: return 0;
					case TypeAttack.Dark: return 0.5f;
					default: return 1;
				}

			default: return 1;
		}

		return 1;
	}
}

public class Effect
{
	public float playerSpeed=1;
}

[Serializable]
public class Rotate
{
	// Vector3 para definir la rotación máxima deseada
	[SerializeField] private Vector3 maxRotationAngles = new Vector3(30f, 30f, 30f);

	public Vector3 MaxRotationAngles
	{
		get { return maxRotationAngles; }
	}

	// Velocidad de rotación
	[SerializeField, Range(0.1f, 1000f)]
	private float rotationSpeed = 250f;

	public float RotationSpeed
	{
		get { return rotationSpeed; }
	}
}
