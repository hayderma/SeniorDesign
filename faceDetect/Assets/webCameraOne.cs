using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using OpenCVForUnity;
using myFaceDetector;

/**
 * Jeremiah Cox
 * public class webCameraOne : MonoBehaviour{}
 * 
 * @Brief   A Monostyle object hook to run our OpenCV program.
 * 
 * A Monostyle Script Used To Render a texture to a unity objects material. 
 * - Add this script to the object, I used a quad. 
 * - It requires the main camera object in Unity, assumed to be pointed at the above object. 
 *  Set to Orthographic of size ##
 * - Uses OpenCVForUnity
 * - Uses our Face1.cs class 
 * Uses a unity style webcamtexture to pull a Color32 formatted frame
 * from the host machines web cam. Assumes the default web cam device is what you want to render.
 * 
 */

public class webCameraOne : MonoBehaviour {
	Color32[] data;
	WebCamTexture myCameraTest;
	WebCamDevice inputDevice;
	Texture2D myTexture;
	Texture2D leftEye;
	Texture2D rightEye;
	Mat myMatt;
	Face1 myDetector;
	List<Mat> channels;
	Mat equalizedMatt;
	int guessCascadeSize = 0;
	int camWidth = 2560; //Probly should be enums
	int camHeight = 720; //Probly should be enums
	/* Used to out put a few faces 
	int frameCounter=0;
	int fileCounter =0;
	*/

	// Use this for initialization
	void Start () {
		//Setting up my class that uses opencv
		myDetector = new Face1();

		//Setting Up Webcam
		Debug.Log(WebCamTexture.devices);
		//Choose device here
		inputDevice = WebCamTexture.devices [0];
		Debug.Log (inputDevice.name);
		myCameraTest = new WebCamTexture (inputDevice.name,camWidth,camHeight);
		//This scales the object we are drawing to to match camera input
		gameObject.transform.localScale = new Vector3 (camWidth, camHeight, 1);
		data = new Color32[camWidth * camHeight];	
		//myTexture will process be used to process face detection then
		//Split into leftEye and rightEye
		myTexture = new Texture2D (camWidth, camHeight,TextureFormat.RGBA32, false);
		rightEye = new Texture2D (camWidth/2, camHeight/2,TextureFormat.RGBA32, false);
		leftEye = new Texture2D (camWidth/2, camHeight/2,TextureFormat.RGBA32, false);
		myMatt = new Mat (new Size (camWidth, camHeight), CvType.CV_8UC4);
		equalizedMatt = new Mat (new Size (camWidth, camHeight), CvType.CV_8UC4);
		channels = new List<Mat>();
		//Setting up render and telling camera to start playing.
		//gameObject.GetComponent<Renderer> ().material.mainTexture = myCameraTest;
		GameObject.Find ("other").GetComponent<Renderer> ().material.mainTexture = rightEye;
		gameObject.GetComponent<Renderer> ().material.mainTexture = leftEye;
		myCameraTest.Play ();
		//frameCounter = 0;
	}
	
	// Update is called once per frame
	void Update () {
		//Retrieve pixel array from webcam texture and convert to an OpenCV Mat
		myCameraTest.GetPixels32 (data);
		myTexture.SetPixels32 (data);
		Utils.texture2DToMat (myTexture, myMatt);

		//Equalise lighting before doing detection
		Imgproc.cvtColor (myMatt,equalizedMatt,Imgproc.COLOR_BGR2YCrCb);
		OpenCVForUnity.Core.split (equalizedMatt, channels);
		Imgproc.equalizeHist (channels [0], channels [0]);
		OpenCVForUnity.Core.merge (channels, myMatt);
		Imgproc.cvtColor (equalizedMatt,myMatt,Imgproc.COLOR_YCrCb2BGR);

        //Haar Cascades face detection. detectFaces converts it to greyscale.
		myDetector.detectFaces (myMatt, guessCascadeSize);
		myDetector.doDrawRects (myMatt);
		Utils.matToTexture2D (myMatt, myTexture);
		leftEye.SetPixels(myTexture.GetPixels (0, 0, camWidth / 2, camHeight));
		leftEye.Apply ();
		rightEye.SetPixels (myTexture.GetPixels (camWidth / 2, 0, camWidth / 2, camHeight));
		rightEye.Apply ();
		//This seems important
		myTexture.Apply();
	}
}
