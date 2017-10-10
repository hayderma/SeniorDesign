using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Runtime.InteropServices;

public class testplugin : MonoBehaviour {

	[DllImport ("ConsoleApplication1")]
	private static extern int GetRandom ();
	// Use this for initialization
	void Start () {
		print ("Native Random Number:" + GetRandom ());
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
