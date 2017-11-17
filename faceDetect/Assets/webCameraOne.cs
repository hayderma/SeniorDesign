using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using OpenCVForUnity;
using myFaceDetector;

/**
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
	Mat myMatt;
	Face1 myDetector;
	List<Mat> channels;
	Mat equalizedMatt;
	int guessCascadeSize = 0;

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
		inputDevice = WebCamTexture.devices [0];
		Debug.Log (inputDevice.name);
		myCameraTest = new WebCamTexture (inputDevice.name, 640, 480);
		gameObject.transform.localScale = new Vector3 (640, 480, 1);
		data = new Color32[640*480];	
		myTexture = new Texture2D (640, 480,TextureFormat.RGBA32, false);
		myMatt = new Mat (new Size (640, 480), CvType.CV_8UC4);
		equalizedMatt = new Mat (new Size (640, 480), CvType.CV_8UC4);
		channels = new List<Mat>();
		//Setting up render and telling camera to start playing.
		//gameObject.GetComponent<Renderer> ().material.mainTexture = myCameraTest;
		//GameObject.Find ("other").GetComponent<Renderer> ().material.mainTexture = myTexture;
		gameObject.GetComponent<Renderer> ().material.mainTexture = myTexture;
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
		///.................................................. myDetector.faceSquares
		myDetector.doDrawRects (myMatt);
			
		/* Output some face detections to train eigen face.
		if (frameCounter < 30) 
		{
			if(myDetector.outRectToFile (myMatt,ref fileCounter))
			  frameCounter++;
		}*/

		Utils.matToTexture2D (myMatt, myTexture);
		//This seems important
		myTexture.Apply();
	}
}
