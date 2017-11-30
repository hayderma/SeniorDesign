using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ViveGrab : MonoBehaviour {

    private SteamVR_TrackedObject trackedObj;
    private GameObject collidingObject;
    private GameObject objectInHand;
    private Vector3 hitPoint;

	public static int width = 64;
	public static int depth = 64;
	public static int height = 64;

	public GameObject whiteBlock;
	public GameObject blueBlock;
	public GameObject redBlock;
	public GameObject invisibleBlock;
	Block[,,] worldBlocks = new Block[width,height,depth];

	public int selected =1;

    private SteamVR_Controller.Device Controller
    {
        get { return SteamVR_Controller.Input((int)trackedObj.index); }
    }

    SteamVR_TrackedController controller;

    public GameObject placeBlock;

    void Awake()
    {
        //trackedObj = GetComponent<SteamVR_TrackedObject>();
    }

    private void Start()
    {
        controller = GetComponent<SteamVR_TrackedController>();
        if (controller == null)
        {
            controller = gameObject.AddComponent<SteamVR_TrackedController>();
        }
        //controller.TriggerClicked += new ClickedEventHandler(placeBlock.GetComponent<GenerateLandscape>().PlaceBlock);
		controller.TriggerClicked += new ClickedEventHandler(PlaceBlock);
        ;
    }

	public void PlaceBlock (object sender, ClickedEventArgs e)
	{
		Debug.Log("Trigger Pressed");
		int layerMask = 10;
		RaycastHit hit;

		if (Physics.Raycast(transform.position, transform.forward * 10, out hit, layerMask))
		{
			Debug.Log("=======Block Created================");
			Vector3 blockPos = hit.transform.position;
			Debug.Log("1. hit.transform: " + hit.transform.position);
			Vector3 hitVector = blockPos - hit.point;
			hitVector.x = Mathf.Abs(hitVector.x);
			hitVector.y = Mathf.Abs(hitVector.y);
			hitVector.z = Mathf.Abs(hitVector.z);
			Debug.Log("2. hit.vector: " + hitVector);
			//Ray ray = Camera.main.ScreenPointToRay(new Vector3(Screen.width / 2.0f, Screen.height / 2.0f, 0));
			Ray ray = Camera.main.ScreenPointToRay(new Vector3(hit.transform.position.x,hit.transform.position.y));
			//Debug.Log(hit.transform.position);
			if (hitVector.x > hitVector.z && hitVector.x > hitVector.y)
				blockPos.x -= (int)Mathf.RoundToInt(ray.direction.x) *.1f;
			else if (hitVector.y > hitVector.x && hitVector.y > hitVector.z)
				blockPos.y -= (int)Mathf.RoundToInt(ray.direction.y)*.1f;
			else
				blockPos.z -= (int)Mathf.RoundToInt(ray.direction.z)*.1f;
			Debug.Log("3. Create Block:" +  blockPos.y + ", "+blockPos);
			CreateBlock((float)blockPos.y, blockPos, true);
			CheckObscuredNeighbours(blockPos);
		}
	}

	int NeighbourCount(Vector3 blockPos)
	{
		int nCount = 0;
		for(int x = -1; x <=1; x++)
			for(int y = -1; y <=1; y++)
				for(int z = -1; z <= 1; z++)
				{
					if(!(x == 0 && y == 0 && z == 0))
					{
						if(worldBlocks[(int)(blockPos.x+(x*.1f)),(int)(blockPos.y+(y*.1f)),(int)(blockPos.z+(z*.1f))] != null)
							nCount++;
					}
				}
		return(nCount);
	}

	void CheckObscuredNeighbours(Vector3 blockPos)
	{
		for(int x = -1; x <=1; x++)
			for(int y = -1; y <=1; y++)
				for(int z = -1; z <= 1; z++)
				{
					if(!(x == 0 && y == 0 && z == 0))
					{
						Vector3 neighbour = new Vector3(blockPos.x+(x*.1f), blockPos.y+(y*.1f), blockPos.z+(z*.1f));
						Debug.Log("4. Neighbour :" +  neighbour);
						//if it is outside the map
						if(neighbour.x < .1 || neighbour.x > width-.2 ||
							neighbour.y < .1 || neighbour.y > height-.2 ||
							neighbour.z < .1 || neighbour.z > depth-.2)	continue;


						if(worldBlocks[(int)neighbour.x,(int)neighbour.y,(int)neighbour.z] != null)
						{
							Debug.Log("5. World :" +  worldBlocks);
							if(NeighbourCount(neighbour) == 26)
							{
								Destroy(worldBlocks[(int)neighbour.x,(int)neighbour.y,(int)neighbour.z].block);
								worldBlocks[(int)neighbour.x,(int)neighbour.y,(int)neighbour.z] = null;
							}
						}
					}
				}
	}

	void CreateBlock(float y, Vector3 blockPos, bool create)
	{
		if(blockPos.x < 0 || blockPos.x >= width || blockPos.y < 0 || blockPos.y >= height || blockPos.z < 0 || blockPos.x >= depth) return;﻿
		GameObject newBlock = null;

		if (selected == 1)
		{
			if (create) {
				newBlock = (GameObject)Instantiate (blueBlock, blockPos, Quaternion.identity);
				newBlock.layer = LayerMask.NameToLayer ("Objects");
			}

			worldBlocks[(int)blockPos.x,(int)blockPos.y,(int)blockPos.z] = new Block(1, create, newBlock);
		}
		else if (selected == 2)
		{
			if (create) {
				newBlock = (GameObject)Instantiate (whiteBlock, blockPos, Quaternion.identity);
				newBlock.layer = LayerMask.NameToLayer ("Objects");
			}

			worldBlocks[(int)blockPos.x,(int)blockPos.y,(int)blockPos.z] = new Block(2, create, newBlock);
		}
		else if (selected == 3)
		{
			if (create) {
				newBlock = (GameObject)Instantiate (redBlock, blockPos, Quaternion.identity);
				newBlock.layer = LayerMask.NameToLayer ("Objects");
			}

			worldBlocks[(int)blockPos.x,(int)blockPos.y,(int)blockPos.z] = new Block(3, create, newBlock);
		}
		else
		{
			if (create) {
				newBlock = (GameObject)Instantiate (invisibleBlock, blockPos, Quaternion.identity);
				newBlock.layer = LayerMask.NameToLayer ("Objects");
			}

			worldBlocks[(int)blockPos.x, (int)blockPos.y, (int)blockPos.z] = new Block(1, create, newBlock);
		}


	}

    // Update is called once per frame
    void Update () {
        /*
        if (Controller.GetPress(SteamVR_Controller.ButtonMask.Touchpad))
        {
            RaycastHit hit;
            // 2
            if (Physics.Raycast(trackedObj.transform.position, transform.forward, out hit, 100))
            {
                hitPoint = hit.point;
            }
        }
        // 1
        if (Controller.GetHairTriggerDown())
        {
            if (collidingObject)
            {
                GrabObject();
            }
        }

        // 2
        if (Controller.GetHairTriggerUp())
        {
            if (objectInHand)
            {
                ReleaseObject();
            }
        }
        */
    }
    
}
