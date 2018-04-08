using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TreeWave : MonoBehaviour {

	public float period1;
	public float amplitude1;
	public Transform bone1;
	public float period2;
	public float amplitude2;
	public Transform bone2;

	float time1;
	float time2;

	Quaternion rot1;
	Quaternion rot2;

	private void Start ()
	{
		time1 = Random.value;
		time2 = Random.value;
		rot1 = bone1.localRotation;
		rot2 = bone2.localRotation;
	}

	// Update is called once per frame
	void Update ()
	{
		time1 += Time.deltaTime / period1;
		time1 %= 1f;
		time2 += Time.deltaTime / period2;
		time2 %= 1f;
		bone1.localRotation = rot1 * Quaternion.AngleAxis (Mathf.Sin (time1 * 2f * Mathf.PI), Vector3.right);
		bone2.localRotation = rot2 * Quaternion.AngleAxis (Mathf.Sin (time2 * 2f * Mathf.PI), Vector3.right);
	}
}
