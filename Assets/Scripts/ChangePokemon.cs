using System;
using System.Collections;
using System.Collections.Generic;
using JetBrains.Annotations;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

public class ChangePokemon : MonoBehaviour {

	[SerializeField] private int maxPokemon=6;
	[SerializeField] private float timeToChange;
	public List<GameObject> pokemons = new List<GameObject>();
	private new GameObject camera;
	private int _currentIndex = 0,indexToChange = 0;

	[SerializeField] private GameObject listSelect,optionCatch;
	private Button[] listButtons;
	public static ChangePokemon instance;
	public bool isAttacked,isChanging;

	[SerializeField] private Button yes, no;
	[FormerlySerializedAs("del")] [SerializeField] private Button delOption;
	[SerializeField] private Text infoCathed;
	[SerializeField] private GameObject key;

	private Queue<GameObject> _queuePokemons = new Queue<GameObject>();
	
	private bool isDeleting = false;
	public bool IsAttacked
	{
		set
		{
			if (isChanging)
			{
				isAttacked = value;
			}
		}
	}

	
	private void Awake()
	{
		instance = this;

	}

	// Use this for initialization
	void Start () {
		camera = GetComponentInChildren<Camera>().gameObject;
		
		listButtons = listSelect.GetComponentsInChildren<Button>();
		
		ButtonsToChange();
		
		Change(0);
		
		optionCatch.SetActive(false);
	}

	public void EnqueuePokemon(GameObject pokemon)
	{
		_queuePokemons.Enqueue(pokemon);
	}
	
	private PlayerStats tempStats;

	private void OMGnewPokemon()
	{
		ShowCatchOptions(_queuePokemons.Peek());
	}
	
	public void ShowCatchOptions(GameObject pokemon)
	{
		GameManager.instance.PauseGame();
		optionCatch.SetActive(true);
		tempStats = pokemon.GetComponent<PlayerStats>();
		infoCathed.text = "You've captured " + tempStats.PlayerName + ". Would you like to keep it?";
		
		yes.onClick.RemoveAllListeners();
		no.onClick.RemoveAllListeners();
		delOption.onClick.RemoveAllListeners();
		
		yes.onClick.AddListener(delegate { AddPokemon(pokemon); });
		no.onClick.AddListener(delegate { NoPokemon(pokemon); });
		delOption.onClick.AddListener(delegate { DelPokemon(pokemon); });
	}
	
	public void AddPokemon(GameObject pokemon)
	{
		if (pokemons.Count >= maxPokemon)
		{
			infoCathed.text = "You can only have up to 6 Pokémon. Please choose another option for " + tempStats.PlayerName + ".";
			return;
		}
		
		pokemons.Add(pokemon);
		
		optionCatch.SetActive(false);
		GameManager.instance.ResumeGame();
		
		_queuePokemons.Dequeue();
		CheckPokemonQueue();
	}
	
	public void NoPokemon(GameObject pokemon)
	{
		Destroy(pokemon);
		optionCatch.SetActive(false);
		GameManager.instance.ResumeGame();
		_queuePokemons.Dequeue();
		CheckPokemonQueue();
	}
	
	public void DelPokemon(GameObject pokemon)
	{
		if (pokemons.Count==1)
		{
			infoCathed.text = "Only one left. Can't remove more.";
			return;
		}
		
		isDeleting = true;
		ShowListSelect(true);
	}

	private void CheckPokemonQueue()
	{
		if (_queuePokemons.Count > 0)
		{
			ShowCatchOptions(_queuePokemons.Peek());
		}
		else
		{
			ShowListSelect();
		}
	}

	private void ButtonsToChange()
	{
		for (int i = 0; i < listButtons.Length; i++)
		{
			var i1 = i;
			listButtons[i].onClick.RemoveAllListeners();
			listButtons[i].onClick.AddListener(delegate { ChangeIndex(i1); });
			
			Button tmpButton = listButtons[i].GetComponentsInChildren<Button>(true)[1];
			tmpButton.gameObject.SetActive(true);
			tmpButton.onClick.AddListener(delegate { RemovePokemon(i1); });

		}
		listSelect.SetActive(false);
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
		

		if (Input.GetKeyDown(KeyCode.E)|| UnityEngine.N3DS.GamePad.GetButtonTrigger(N3dsButton.X))
		{
			//GameManager.instance.PauseGame();
			ShowListSelect();
		}
		
	}

	private void ChangeIndex(int index)
	{
		if (isDeleting)
		{
			return;
		}
		
		if (index == _currentIndex)
		{
			CloseListSelect();
			return;
		}

		indexToChange = index;
		isChanging = true;
		GameManager.instance.ResumeGame();
		GameManager.instance.StopPlayer();
		
		listSelect.SetActive(false);

		key.SetActive(true);
		key.transform.position = new Vector3(pokemons[_currentIndex].transform.position.x,key.transform.position.y ,pokemons[_currentIndex].transform.position.z);
	}

	private void RemovePokemon(int index)
	{
		pokemons.RemoveAt(index);

		if (_currentIndex > index)
		{
			_currentIndex--;
		}

		if (isDeleting)
		{
			isDeleting = false;
			CloseListSelect();
			CheckPokemonQueue();
		}
		else
		{
			ShowListSelect(true);
		}
		
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
		
		EnemyManager.Instance.ChangeTarget(pokemons[pokemonIndex].transform);

	}




	private void ShowListSelect(bool update = false)
	{
		if (Time.timeScale == 0 && !update)
		{
			if (_queuePokemons.Count <= 0)
			{
				CloseListSelect();
			}
			return;
		}

		if (_queuePokemons.Count > 0 && !isDeleting)
		{
			OMGnewPokemon();
			return;
		}
		
		listSelect.SetActive(true);
		for (int i = 0; i < listButtons.Length; i++)
		{

			if (i <= pokemons.Count - 1)
			{
				listButtons[i].gameObject.SetActive(true);
				PlayerStats playerStats = pokemons[i].GetComponent<PlayerStats>();
				listButtons[i].GetComponentInChildren<Text>().text = playerStats.PlayerName + "---" + playerStats.CP;

				if (i == _currentIndex)
				{
					listButtons[i].transform.Find("bin").gameObject.SetActive(false);
				}
				else
				{
					listButtons[i].transform.Find("bin").gameObject.SetActive(true);
				}

			}
			else
			{
				listButtons[i].gameObject.SetActive(false);
			}
		}

		GameManager.instance.PauseGame();
	}

	private void CloseListSelect()
	{
		listSelect.SetActive(false);
		GameManager.instance.ResumeGame();
	}
}
