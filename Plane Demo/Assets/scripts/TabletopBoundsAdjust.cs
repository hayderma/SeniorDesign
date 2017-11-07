using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Perspective's a hell of a drug

public class TabletopBoundsAdjust : MonoBehaviour {
	private Mesh mesh;
	private int iterator;
	public GameObject sphere1;
	public GameObject sphere2;
	public GameObject sphere3;
	private GameObject sphere4;
	private RaycastHit hitInfo;
	private Vector3 gravity;
	private float distanceScale;
	public List<float> pointDistances;
	private float distanceRatio;
	private Vector3 center;
	public GameObject sphere;
	// Use this for initialization
	void Start () {
		distanceRatio = 4.3f;
		sphere2.transform.position = Camera.main.transform.position + ((sphere2.transform.position - Camera.main.transform.position).normalized * pointDistances [1] / distanceRatio);
		sphere3.transform.position = Camera.main.transform.position + ((sphere3.transform.position - Camera.main.transform.position).normalized * pointDistances [2] / distanceRatio);
		distanceScale = (sphere2.transform.position - Camera.main.transform.position).magnitude / (sphere1.transform.position - Camera.main.transform.position).magnitude;
		sphere2.transform.localScale = new Vector3 (distanceScale, distanceScale, distanceScale);
		distanceScale = (sphere3.transform.position - Camera.main.transform.position).magnitude / (sphere1.transform.position - Camera.main.transform.position).magnitude;
		sphere3.transform.localScale = new Vector3 (distanceScale, distanceScale, distanceScale);
		sphere4 = GameObject.CreatePrimitive (PrimitiveType.Sphere);
		RealignMesh ();
		sphere.GetComponent<Gravity> ().SetGravity (gravity);
	}

	public void RealignMesh(){
		center = ((sphere3.transform.position - sphere1.transform.position) / 2) + sphere1.transform.position;
		//sphere4.transform.RotateAround (center, sphere3.transform.position - sphere1.transform.position, 180);
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
