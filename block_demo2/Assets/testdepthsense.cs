using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Runtime.InteropServices;

public class testdepthsense : MonoBehaviour {

	[DllImport ("depth_sense")]
	private static extern int main();

	// Use this for initialization
	void Start () {
		main();
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
