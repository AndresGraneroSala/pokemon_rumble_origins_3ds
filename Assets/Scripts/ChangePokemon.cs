using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ChangePokemon : MonoBehaviour {

	[SerializeField] private float timeToChange;
	public List<GameObject> pokemons = new List<GameObject>();
	private new GameObject camera;
	private int _currentIndex = 0,indexToChange = 0;

	[SerializeField] private GameObject listSelect;
	private Button[] listButtons;
	public static ChangePokemon instance;
	public bool isAttacked,isChanging;

	[SerializeField] private GameObject key;
	
	private void Awake()
	{
		instance = this;

	}

	public void AddPokemon(GameObject pokemon)
	{
		pokemons.Add(pokemon);
	}

	// Use this for initialization
	void Start () {
		
		camera = GetComponentInChildren<Camera>().gameObject;
		
		listButtons = listSelect.GetComponentsInChildren<Button>();

		for (int i = 0; i < listButtons.Length; i++)
		{
			var i1 = i;
			listButtons[i].onClick.AddListener(delegate { ChangeIndex(i1); });
		}
		listSelect.SetActive(false);
		
		Change(0);

	}
	private float timer = 0;
	// Update is called once per frame
	void Update () {

		if (isChanging)
		{
			timer += Time.deltaTime;
			if (isAttacked)
			{
				isChanging = false;
				isAttacked = false;
				timer = 0;
				GameManager.instance.MovePlayer();
				key.SetActive(false);

			}
		}

		if (timer >= timeToChange)
		{
			Change(indexToChange);
			isChanging = false;
			timer = 0;
			GameManager.instance.MovePlayer();
			key.SetActive(false);
		}
		

		if (Input.GetKey(KeyCode.E)|| UnityEngine.N3DS.GamePad.GetButtonTrigger(N3dsButton.X))
		{
			//GameManager.instance.PauseGame();
			ShowListSelect();
		}
		
	}

	private void ChangeIndex(int index)
	{
		indexToChange = index;
		isChanging = true;
		GameManager.instance.ResumeGame();
		GameManager.instance.StopPlayer();
		
		listSelect.SetActive(false);

		key.SetActive(true);
		key.transform.position = new Vector3(pokemons[_currentIndex].transform.position.x,key.transform.position.y ,pokemons[_currentIndex].transform.position.z);


	}
	
	
	private void Change(int pokemonIndex)
	{
		pokemons[_currentIndex].SetActive(false);
		pokemons[pokemonIndex].SetActive(true);
		key.SetActive(false);
		
		pokemons[pokemonIndex].transform.position = pokemons[_currentIndex].transform.position;
		camera.transform.SetParent(pokemons[pokemonIndex].transform);
		_currentIndex = pokemonIndex;
		GameManager.instance.MovePlayer();

	}


	

	private void ShowListSelect()
	{
		GameManager.instance.PauseGame();
		listSelect.SetActive(true);
		for (int i = 0; i < listButtons.Length; i++)
		{
			if (i<pokemons.Count)
			{
				listButtons[i].GetComponentInChildren<Text>().text = pokemons[i].GetComponent<PlayerStats>().Name;
			}
			else
			{
				listButtons[i].gameObject.SetActive(false);
			}
		}
	}
}
