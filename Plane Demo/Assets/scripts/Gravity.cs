using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gravity : MonoBehaviour {
	private Rigidbody rb;
	private Vector3 gravity;
	// Use this for initialization
	void Start () {
		rb = this.GetComponent<Rigidbody> ();
	}
	
	// Update is called once per frame
	void Update () {
		rb.AddForce (gravity*9.8f);
	}

	public void SetGravity(Vector3 input){
		gravity = input;
	}
}
