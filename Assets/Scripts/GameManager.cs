using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour {

	static public GameManager instance;
	[SerializeField] private GameObject menuEndGame;
	private void Awake()
	{
			instance = this;
			Time.timeScale = 1;
	}

	private void Start()
	{
		menuEndGame.SetActive(false);
	}

	public void StartGame()
	{
		
	}

	public void EndGame()
	{
		menuEndGame.SetActive(true);
		Time.timeScale = 0;
	}

	public void ChangePlayer()
	{
		
	}

	public void RestartGame()
	{
		SceneManager.LoadScene(SceneManager.GetActiveScene().name);
	}

	// Update is called once per frame
	void Update () {
		
	}
}
