using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;
using OpenCVForUnity;

/*
 * FaceDetector:Face1  {}
 * @brief Face Detection Implementation
 * 
 * This objects intent is to provide functions that help with face detection. Heavely relies on OpenCV functions to implement face detection and masking
 * 

*/

namespace myFaceDetector
{
	public class Face1 
	{
		CascadeClassifier Cascade;
		public MatOfRect faceSquares;
		EigenFaceRecognizer recognizer;
		Mat grey;
		Size min;
		Size max;
		Scalar rectColor;
		Point minPoint;
		Point maxPoint;

		/**
		 * Face1(String classifierFileXml)
		 * @brief Constructor for our Face1 class
		 * 
		 * @var classifierFileXml     String of the path toan xml file with the training set of Haarcascade classifiers. 
		 * String path = Application.dataPath + "\Assets" + "\haarcascade_frontalface_default.xml";
		 */
		 
		public Face1(String classifierFileXml)
		{
		  //String path = Application.dataPath + "\Assets" + "\haarcascade_frontalface_default.xml";
		  //Using https://github.com/opencv/opencv/blob/master/data/haarcascades/haarcascade_frontalface_default.xml to test with.

		  Cascade = new CascadeClassifier(classifierFileXml);
		  Debug.Log (classifierFileXml + "Is being setup as the set of classifier(s).");
		}

		/**
		 * Face1()
		 * @brief Default Constructor for our Face1 class
		 * Uses OpenCV's haar cascade classifieres classifierFileXml  
		 */

		public Face1()
		{
			String myString = Application.streamingAssetsPath +"/haarcascade_frontalface_default.xml";
			grey = new Mat (640, 480, CvType.CV_8UC4);   //! Hardcoded width and height
			faceSquares = new MatOfRect ();
			min = new Size (75, 75);
			max = new Size (300, 300);
			rectColor = new Scalar (255, 20, 0, 255);
			minPoint = new Point (0,0);
			maxPoint = new Point (0,0);
			Cascade = new CascadeClassifier(myString);
			//recognizer = EigenFaceRecognizer.create ();
			//recognizer.read("eigenFace.yaml");  // @TODO File not uploaded problems with OpenCV's EigneFaceRecognizer.Train()
			//recognizer.setThreshold(0.0);
		}
			
		/**
		 * detectFaces()
		 * 
		 * @var Mat frame  The OpenCV matt used to detect faces. Is converted to greyscale.
		 * @var double guessCascadeSize Intended to resize at runtime the cascading window.
		 * Converts Mat frame to greyscale then uses Haar Cascades to apply the classifiers in it.
		 * 
		 * @TODO guessCascadeSize is intended to provide a feedback representation of the size based on series of guesses. Not implemented yet.
		*/
		
		public void detectFaces(Mat frame,double guessCascadeSize)
		{
			frame.copyTo (grey);
			Imgproc.cvtColor (frame, grey, Imgproc.COLOR_BGR2GRAY);
			//Equalise the image to reduce lighting impact. Done on color image in main program loop!
			//Imgproc.equalizeHist(grey,grey);
			//This is openCv doing the heavy lifting!
			Cascade.detectMultiScale(grey,faceSquares, 1.1,3,0 | Objdetect.CASCADE_FIND_BIGGEST_OBJECT | Objdetect.CASCADE_SCALE_IMAGE,min,max);
			//doDrawRects (faceSquares);
		}

		/**
		 * doDrawRects
		 * @var Mat img The mat used to draw rectangles.  
		 * @brief Draws a box around the regions recognized as faces
		 * 
		 * This function takes class internal although MatOfRects and draws them to the passed Mat img. If called before Face1.detectFaces() it will do nothing.
		 */

		public void doDrawRects(Mat img)
		{
			foreach (OpenCVForUnity.Rect face in faceSquares.toArray()) 
			{
				//@TODO Mat ROI = new Mat(img,face) for isPersonOfInterest in doDrawRects;
				//if (isPersonOfInterest (ROI)){	
					minPoint.x = face.x;
					minPoint.y = face.y;
					maxPoint.x = face.x + face.width;
					maxPoint.y = face.y + face.height;
					Imgproc.rectangle (img, minPoint, maxPoint, rectColor, 2);
					Debug.Log ("<color=green>" + string.Format ("pos x: {0},  pos y:{1}", face.x, face.y) + "</color>");
					Debug.Log ("<color=blue>" + string.Format ("width: {0} by  height:{1}", face.width, face.height) + "</color>");
				    OpenCVForUnity.Imgproc.GaussianBlur(new Mat(img,face),new Mat(img,face),new Size(0,0),10);
				//}
			}
		}

		//Used to save pictures of faces to a jpg in the parent folder of the project, used in training the Eigen Face Recognizer.

		public bool outRectToFile(Mat frame,ref int fileCounter)
		{
			Mat localCpy = new Mat ();
			int counter = 0;
			String myFile;
			foreach (OpenCVForUnity.Rect face in faceSquares.toArray()) 
			{
				myFile = "face" + fileCounter +".jpg";
				fileCounter++;
				counter++;
				//localCpy = new Mat (new Size (face.width, face.height))
				localCpy = new Mat (frame,face);
				Imgcodecs.imwrite (myFile, localCpy);
			}	
			if (counter == 0)
				return false;
			else
				return true;
		}

		//Is not fully implemented. Issues with training Recognizer!!!
		public bool isPersonOfInterest(Mat face)
		{
			int[] label = { -1,-1,-1,-1 }; 
			double[] confidence = {-1.0,-1.0,-1.0,-1,0};
			recognizer.predict (face, label, confidence);
			//int prediction = recognizer.predict_label(face);
			Debug.Log(string.Format("Confidence is :{0}, Label is {1} ",confidence[0], label[0]));
			if (confidence[0] >= 0)
				return true;
			else
				return false;
		}
    }
}
