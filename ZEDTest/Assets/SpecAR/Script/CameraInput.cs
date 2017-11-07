using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class CameraInput : MonoBehaviour {

	void Start () {
		WebCamDevice[] devices = WebCamTexture.devices;
		string backCamName = "";
		if(devices.Length > 0) backCamName = devices[0].name;
		for(int i = 0; i < devices.Length; i++) {
			Debug.Log("Device:" + devices[i].name + "IS FRONT FACING:" + devices[i].isFrontFacing);
			if(!devices[i].isFrontFacing) {
				backCamName = devices[i].name;
			}
		}
		WebCamTexture CameraTexture = new WebCamTexture("ZED", 2560, 720, 30);
		CameraTexture.Play();
		Renderer renderer = GetComponent<Renderer>();
		renderer.material.mainTexture = CameraTexture;
	}
}
