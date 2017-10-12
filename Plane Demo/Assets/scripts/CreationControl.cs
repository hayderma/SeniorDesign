using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CreationControl : MonoBehaviour {

	private int currBigIndex;
	private int iterator;
	public GameObject table;
	private GameObject dummyObject;
	public GameObject cube;
	public GameObject sphere;
	public List<GameObject> turnOffObjects;
	// Use this for initialization
	void Start () {
		print (Camera.main.GetComponent<Camera> ().cullingMask);
		
	}
	
	// Update is called once per frame
	void Update () {
		if (Input.GetKeyDown (KeyCode.Escape)) {
			Application.Quit ();
		}
		if (Input.GetKeyDown (KeyCode.A)) {
			foreach (GameObject hide in turnOffObjects) {
				hide.GetComponent<MeshRenderer> ().enabled = !hide.GetComponent<MeshRenderer> ().enabled;
			}
		}
		if (Input.GetKeyDown (KeyCode.F)) {
			dummyObject = GameObject.Instantiate (cube);
			dummyObject.transform.position = table.GetComponent<PlaneManipulate>().GetSpawnPoint();
			dummyObject.GetComponent<Gravity> ().SetGravity (table.GetComponent<PlaneManipulate> ().GetGravity ());
		}
		if (Input.GetKeyDown (KeyCode.D)) {

			dummyObject = GameObject.Instantiate (sphere);
			dummyObject.transform.position = table.GetComponent<PlaneManipulate>().GetSpawnPoint();
			dummyObject.GetComponent<Gravity> ().SetGravity (table.GetComponent<PlaneManipulate> ().GetGravity ());
		}
	}
}
