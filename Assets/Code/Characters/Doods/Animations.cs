using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Code.Characters.Doods;
using UnityEngine;

public class Animations : MonoBehaviour
{

	private Rigidbody _rb;
	private Animator _anim;
	
	// Use this for initialization
	void Start () {
		_rb = GetComponent<Rigidbody>();
		_anim = GetComponent<Animator>();
	}
	
	// Update is called once per frame
	void Update () {
		if (_rb.velocity.sqrMagnitude >= 0.01f) {
			_anim.SetBool("isWalking", true);
		}
		else {
			_anim.SetBool("isWalking", false);
		}
	}
}
