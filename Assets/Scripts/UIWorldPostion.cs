using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIWorldPostion : MonoBehaviour
{

	private Vector3 _startPos;
	private Camera _upCamera;
	private Canvas _canvas;
	private Transform _transformFollow;
	private Vector3 additionalPostion;
	// Use this for initialization
	void Start()
	{
		_upCamera = GameObject.FindWithTag("Player").GetComponentInChildren<Camera>();

		_canvas = GameObject.Find("CanvasUP").GetComponent<Canvas>();
	}

	public void SetPos(Vector3 pos)
	{
		_startPos = pos;
	}
	public void SetTransform(Transform transformFoll, Vector3? additional = null)
	{
		_transformFollow = transformFoll;
		additionalPostion = additional.GetValueOrDefault();
	}

	// Update is called once per frame
	void Update()
	{

		Vector3 screenPosition = !_transformFollow
			? _upCamera.WorldToScreenPoint(_startPos)
			: _upCamera.WorldToScreenPoint(_transformFollow.position);
		screenPosition += additionalPostion;
		
		if (screenPosition.z > 0)
		{
			// Convierte las coordenadas de pantalla a coordenadas del Canvas
			Vector2 localPoint;
			RectTransformUtility.ScreenPointToLocalPointInRectangle(
				_canvas.GetComponent<RectTransform>(),
				screenPosition,
				_canvas.worldCamera,
				out localPoint
			);



			// Coloca el marcador en la posición calculada
			transform.localPosition = localPoint;
		}
	}
}
