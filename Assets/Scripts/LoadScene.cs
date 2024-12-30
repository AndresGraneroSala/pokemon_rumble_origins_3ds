using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LoadScene : MonoBehaviour {
	private void Start()
	{
		LoadSceneAsync("main");
	}

	public void LoadSceneAsync(string sceneName)
	{
		StartCoroutine(LoadSceneCoroutine(sceneName));
	}

	private IEnumerator LoadSceneCoroutine(string sceneName)
	{
		AsyncOperation asyncOperation = SceneManager.LoadSceneAsync(sceneName);
		while (!asyncOperation.isDone)
		{
			Debug.Log("Progreso de carga: " + (asyncOperation.progress * 100) + "%");
			yield return null;
		}
	}
}
