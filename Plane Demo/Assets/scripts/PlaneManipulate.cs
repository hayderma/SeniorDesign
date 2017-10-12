using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlaneManipulate : MonoBehaviour {

	private bool realign;
	private GameObject movementPoint;
	private Vector3 movementVector;
	public GameObject sphere1;
	public GameObject sphere2;
	public GameObject sphere3;
	private GameObject sphere4;
	private Vector3 center;
	private Vector3 gravity;
	private RaycastHit hitInfo;
	// Use this for initialization
	void Start () {
		sphere4 = GameObject.CreatePrimitive (PrimitiveType.Sphere);
		movementPoint = sphere2;
		movementVector = (Camera.main.transform.position - movementPoint.transform.position).normalized;
		realign = true;
		RealignMesh ();
	}
	
	// Update is called once per frame
	void Update(){
		if (realign) {
			RealignMesh ();
		}

		//Turn on or off dynamic realign
		if (Input.GetKeyDown (KeyCode.Q)) {
			realign = !realign;
		}

		//Making sphere1 the moving point
		if (Input.GetKeyDown (KeyCode.E)) {
			movementPoint = sphere1;
			movementVector = (Camera.main.transform.position - movementPoint.transform.position).normalized;
		}

		//Making sphere2 the moving point
		if(Input.GetKeyDown(KeyCode.R)){
			movementPoint = sphere2;
			movementVector = (Camera.main.transform.position - movementPoint.transform.position).normalized;
		}

		//Making sphere3 the moving point
		if (Input.GetKeyDown (KeyCode.T)) {
			movementPoint = sphere3;
			movementVector = (Camera.main.transform.position - movementPoint.transform.position).normalized;
		}

		//Move the moving point closer to or further than the camera.
		if (Input.GetAxis ("Vertical") != 0) {
			print (movementVector * Input.GetAxis ("Vertical"));
			movementPoint.transform.Translate (movementVector * Input.GetAxis ("Vertical"));
		}
	}
	public void RealignMesh(){
		center = ((sphere3.transform.position - sphere1.transform.position) / 2) + sphere1.transform.position;
		sphere4.transform.position = sphere2.transform.position - (sphere2.transform.position - center)*2;
		Mesh mesh = GetComponent<MeshFilter>().mesh;
		mesh.Clear();
		mesh.vertices = new Vector3[] {sphere1.transform.position, sphere2.transform.position, sphere3.transform.position, sphere4.transform.position};
		mesh.triangles =  new int[] {0, 1, 2, 0, 2, 3};
		GetComponent<MeshCollider> ().sharedMesh = mesh;
		Physics.Raycast (center + new Vector3(0,1,0), new Vector3(0,-1,0), out hitInfo, Mathf.Infinity, 256);
		gravity = -hitInfo.normal;

	}


	public Vector3  GetGravity(){
		return gravity;
	}
	public Vector3 GetSpawnPoint(){
		return (center - gravity);
	}
}
