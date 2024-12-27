using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotateSimple : MonoBehaviour {

	[SerializeField] private Vector3 rotation;
	[SerializeField] private float rotationSpeed;
	
	
	// Update is called once per frame
	void Update () {
		transform.Rotate(rotation * rotationSpeed * Time.deltaTime);
	}
}
