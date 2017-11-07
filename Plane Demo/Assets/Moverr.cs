using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Moverr : MonoBehaviour {
	Rigidbody rb;
	Vector3 moveVector;
	// Use this for initialization
	void Start () {
		rb = GetComponent<Rigidbody> ();
	}
	
	// Update is called once per frame
	void Update () {
		moveVector = Input.GetAxis ("Horizontal")*transform.right + Input.GetAxis("Vertical")*transform.forward;
		transform.TransformVector (moveVector);
		rb.AddForce (5 *moveVector);
		if (rb.velocity.magnitude > 5) {
			rb.velocity = rb.velocity * 5 / rb.velocity.magnitude;
		}
	}
}
