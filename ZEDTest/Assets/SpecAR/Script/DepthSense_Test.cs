using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DepthSense_Test : MonoBehaviour {
	bool waitActive = false;
	sl.ZEDCamera zedCamera;
	//   Divide by half for faster rendering
	int width = 0;
	int height = 0;
	int win_min_width, win_max_width, win_min_height, win_max_height;
	float[,] depth;
	int num_x, num_y;

	void Start () {
		
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
		num_x = win_max_width - win_min_width;
		num_y = win_max_height - win_min_height;

	}

	float DepthSense () {
		
		//new depth instance
		depth = new float[num_x,num_y];

		//for average distance
		float average = 0;
		//float count = 0;
		float first_found = -1;

		float[] sum_b = { 0,0,0,0}; 
		int[] count = {0,0,0,0};
		int highest_count = 0;
		int highest_counter = 0;

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
					if (first_found == -1){
						sum_b[0] += ((depth [i, j]) );
						count[0] += 1;
						print("Added to bracket 1. Depth at [" + x + "," + y +"] = " + (depth[i,j]));
						first_found = depth [i, j];

					}
					else if ( depth[i,j] > (0.9*first_found) && depth[i,j] <= (1.2*first_found)) {
						// Add sum all the values
						sum_b[0] += ((depth [i, j]) );
						count[0] += 1;
						print("Added to bracket 1. Depth at [" + x + "," + y +"] = " + (depth[i,j]));
					}
					else if ( depth[i,j] > (1.2*first_found) && depth[i,j] <= (1.35*first_found)) {
						// Add sum all the values
						sum_b[1] += ((depth [i, j]) );
						count[1] += 1;
						print("Added to bracket 2. Depth at [" + x + "," + y +"] = " + (depth[i,j]));
					}
					else if ( depth[i,j] > (1.36*first_found) && depth[i,j] <= (1.50*first_found)) {
						// Add sum all the values
						sum_b[2] += ((depth [i, j]) );
						count[2] += 1;
						print("Added to bracket 3. Depth at [" + x + "," + y +"] = " + (depth[i,j]));
					}
					else if ( depth[i,j] > (1.50*first_found)) {
						// Add sum all the values
						sum_b[3] += ((depth [i, j]) );
						count[3] += 1;
						print("Added to bracket 4. Depth at [" + x + "," + y +"] = " + (depth[i,j]));
					}
						
				}
			}
		}

		// Get the depth bracket with the highest counter
		for (int i = 0; i < count.Length; i++) {
			if (highest_count < count[i]) {
				highest_count = count [i];
				highest_counter = i;
			}
		}
		print ("Highest count " + highest_count);
		print ("Highest index" + highest_counter);
		// Get the average of the sum/counter
		average = sum_b[highest_counter]/highest_count;

		print (average + " inches. Count:" + highest_count);
		return average;
	}

	// Update is called once per frame
	void Update () {
		if (Input.GetKey (KeyCode.Space)) {
			if (waitActive == false) {
				// Start depth sensing
				DepthSense ();
				// Wait 2 seconds
				StartCoroutine (Wait ());
			}
				
		}
	}

	// Wait coroutine is called after every depth grab
	IEnumerator Wait() {
		waitActive = true;
		yield return new WaitForSeconds(2f);
		waitActive = false;
	}
}
