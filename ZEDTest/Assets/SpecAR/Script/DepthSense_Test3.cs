using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DepthSense_Test3 : MonoBehaviour {

	public void run_depth_sense () {
		Depth_Coroutine ();
	}

	 void Depth_Coroutine () {
	 	sl.ZEDCamera zedCamera;
		//   Divide by half for faster rendering
	 	int width = 0;
	 	int height = 0;
	 	int win_min_width, win_max_width, win_min_height, win_max_height;
	 	float[,] depth;
		// Use this for initialization

		zedCamera = sl.ZEDCamera.GetInstance ();

		//Get center
		width = zedCamera.ImageWidth/4;
		height= zedCamera.ImageHeight/4;

		//Window size
		win_min_width = width - 5;
		win_max_width = width + 5;

		win_min_height = height - 5;
		win_max_height = height + 5;
		print ("Window size: [" + width + "," + height + "]");
		print ("Window min size: [" + win_min_width + "," + win_min_height + "]");
		print ("Window max size: [" + win_max_width + "," + win_max_height + "]");

		// get total number of pixels
		int num_x = win_max_width - win_min_width;
		int num_y = win_max_height - win_min_height;

		//new depth instance
		depth = new float[num_x,num_y];

		//for average distance
		float sum = 0;
		float count = 0;
		float first_found = -5;

		for (int i = 0; i < num_x; i++) {
			for (int j = 0; j < num_y; j++) {
				int x = i  + win_min_width;
				int y = j  + win_min_height;

				depth[i,j] = zedCamera.GetDepthValue( (uint)(i*8 + win_max_width), (uint)(j*8 + win_min_height) );

				// Convert to inches
				depth [i, j] *= (float)39.3701 ;

				// Check if past convergence distance and within max distance
				if (depth [i, j] > 12 && depth [i, j] < 240) {
					
					// If not measuring first pixel, all values 1.5x the distance of the first pixel are excluded
					// Removes outliers
					if (first_found == -5){
						sum += ((depth [i, j]) );
						count += 1;
						print("Depth at [" + x + "," + y +"] = " + (depth[i,j]));
						first_found = depth [i, j];
					}
					else if ( depth[i,j] <= (1.5*first_found)) {
						// Add sum all the values
						sum += ((depth [i, j]) );
						count += 1;
						print("Depth at [" + x + "," + y +"] = " + (depth[i,j]));
					}

				}
			}
		}
		sum = sum / count;
		print (sum + " inches. Count:" + count);
		//yield return new WaitForSeconds(2f);

	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
