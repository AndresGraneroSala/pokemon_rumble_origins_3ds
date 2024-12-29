using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class PlayerAttack : MonoBehaviour
{
	[SerializeField] RotateBone rotateBone;

	[SerializeField] private Attack attack1;
	[SerializeField] private Attack attack2;
	private bool isAttacking = false;
	[SerializeField] private Transform model;
	private float timer = 0f;

	private PlayAttack _playAttack;
	
	public float Timer
	{ 
		set { timer = value; }
	}

	[SerializeField] private Transform spawnBullets;

	private PlayerMove _playerMove;
	// Use this for initialization
	void Start () {
		_playerMove = gameObject.GetComponent<PlayerMove>();
		_playAttack = GetComponent<PlayAttack>();
	}
	
	// Update is called once per frame
	void Update()
	{
		if (isAttacking)
		{
			timer -= Time.deltaTime;
			if (timer<=0)
			{
				isAttacking = false;
				_playerMove.block = false;
			}
		}
		
		
		if (!isAttacking && (Input.GetKeyDown(KeyCode.Mouse0) || UnityEngine.N3DS.GamePad.GetButtonTrigger(N3dsButton.A)))
		{
			isAttacking = true;
			 StartCoroutine(_playAttack.Play(attack1));
		}
		
		if (!isAttacking && (Input.GetKeyDown(KeyCode.Mouse1) || UnityEngine.N3DS.GamePad.GetButtonTrigger(N3dsButton.B)))
		{
			isAttacking = true;
			StartCoroutine(_playAttack.Play(attack2));
		}

	}
}
