/*

Process:
*steps not shown
1. Generate three virtual points.
*2. User looks at the points in the real world that the virtual points represent.
*3. At each point on confirmation from the user, the ZED saves the distance and the Vive saves the rotation and position;
*4. The points are placed at the Vive's position at the time of capturing that point offset by the distance scaled by a distance factor along the forward vector at that rotation 
	(at this point, not all of this process is shown, but the point offset by distance is).
5. Generate a 4th point and place it at the virtual point that creates a parallelogram.
6. Generate a new mesh that is connected by the four points and assign it to both the table instance's mesh collider and mesh renderer components(though the 
	mesh renderer will be disabled shortly)
7. Disable the table instance's mesh renderer component and the connection points.
8. 

Project comes with two demos: Plane Manipulation and Tabletop

Plane Manipulation
The plane manipulation shows the process of configuring a plane to provide a virtual tabletop in line with the user's perspective. You are able to select the three points and move
	them closer to or further from the camera.

Tabletop Demo
This demo shows the expected perspective of the user when a tabletop is instantiated. Recommended to immediately hide renderers to see from the users point of view.

	The following commands are available in both demos.
	A - turn on/off point and tabletop renderers
	F - instantiate a physically interactive cube
	D - instantiate a physically interactive sphere

	The following commands are only available in the Plane Manipulation demo.
	Q - turn on/off dynamic realign (Dynamic realign will recalculate the mesh every frame, whether or not points are moved. By default, on)
	E - select point 1
	R - select point 2
	T - select point 3
	Note that there is no option to select point 4. Point 4 is calculated based on the positions of the other three points and is handled automatically.
	W/Up Arrow - Move selected point further from camera.
	S/Down Arrow - Move selected point closer to camera.

*/