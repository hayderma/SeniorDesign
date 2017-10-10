using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class Block
{
	public int type;
	public bool vis;
	public GameObject block;

	public Block(int t, bool v, GameObject b)
	{
		type = t;
		vis = v;
		block = b;
	}
}

public class GenerateLandscape : MonoBehaviour {

	public static int width = 64;
	public static int depth = 64;
	public static int height = 128;
	public int heightScale = 20;
	public int heightOffset = 100;
	public float detailScale = 25.0f;

	public GameObject grassBlock;
	public GameObject dirtBlock;
	public GameObject stoneBlock;

	public int selected =1;

	public int inventory_slots = 3;
	public Image inventory1;
	public Image inventory2;
	public Image inventory3;

	Color originalColor = Color.white;
	Color selectedColor = Color.green;

	Block[,,] worldBlocks = new Block[width,height,depth];

	// Use this for initialization
	void Start () 
	{
		inventory1.color = Color.green;
		inventory2.color = Color.white;
		inventory3.color = Color.white;

		int seed = (int) Network.time * 10;
		for(int z = 0; z < depth; z++)
		{
			for(int x = 0; x < width; x++)
			{
				int y = (int) (Mathf.PerlinNoise((x+seed)/detailScale, (z+seed)/detailScale) * heightScale) 
					+ heightOffset;
				Vector3 blockPos = new Vector3(x,y,z);

				CreateBlock(y, blockPos, true);
				while(y > 0)
				{
					y--;
					blockPos = new Vector3(x,y,z);
					CreateBlock(y, blockPos, false);
				}

			}
		}

	}
		

	void CreateBlock(int y, Vector3 blockPos, bool create)
	{
		if(blockPos.x < 0 || blockPos.x >= width || blockPos.y < 0 || blockPos.y >= height || blockPos.z < 0 || blockPos.x >= depth) return;﻿
		GameObject newBlock = null;
		//if(y > 115)
		if (selected == 1)
		{
			if(create)
				newBlock = (GameObject) Instantiate(dirtBlock,blockPos, Quaternion.identity);

			worldBlocks[(int)blockPos.x,(int)blockPos.y,(int)blockPos.z] = new Block(1, create, newBlock);
		}
		//else if(y > 105)
		else if (selected == 2)
		{
			if(create)
				newBlock = (GameObject) Instantiate(grassBlock,blockPos, Quaternion.identity);

			worldBlocks[(int)blockPos.x,(int)blockPos.y,(int)blockPos.z] = new Block(2, create, newBlock);
		}
		else
		{
			if(create)
				newBlock = (GameObject) Instantiate(stoneBlock,blockPos, Quaternion.identity);

			worldBlocks[(int)blockPos.x,(int)blockPos.y,(int)blockPos.z] = new Block(3, create, newBlock);
		}
			


	}

	void DrawBlock(Vector3 blockPos)
	{
		//if it is outside the map
		if(blockPos.x < 0 || blockPos.x > width-1 ||
			blockPos.y < 0 || blockPos.y > height-1 ||
			blockPos.z < 0 || blockPos.z > depth-1)	return;

		if(worldBlocks[(int)blockPos.x,(int)blockPos.y,(int)blockPos.z] == null) return;

		if(!worldBlocks[(int)blockPos.x,(int)blockPos.y,(int)blockPos.z].vis)
		{
			GameObject newBlock = null;
			worldBlocks[(int)blockPos.x,(int)blockPos.y,(int)blockPos.z].vis = true;
			if(worldBlocks[(int)blockPos.x,(int)blockPos.y,(int)blockPos.z].type == 1)
				newBlock = (GameObject) Instantiate(dirtBlock,blockPos, Quaternion.identity);

			else if(worldBlocks[(int)blockPos.x,(int)blockPos.y,(int)blockPos.z].type == 2)
				newBlock = (GameObject) Instantiate(grassBlock,blockPos, Quaternion.identity);

			else if(worldBlocks[(int)blockPos.x,(int)blockPos.y,(int)blockPos.z].type == 3)
				newBlock = (GameObject) Instantiate(stoneBlock,blockPos, Quaternion.identity);

			else
				worldBlocks[(int)blockPos.x,(int)blockPos.y,(int)blockPos.z].vis = false;

			if(newBlock != null)
				worldBlocks[(int)blockPos.x,(int)blockPos.y,(int)blockPos.z].block = newBlock;
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
						if(worldBlocks[(int)blockPos.x+x,(int)blockPos.y+y,(int)blockPos.z+z] != null)
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
						Vector3 neighbour = new Vector3(blockPos.x+x, blockPos.y+y, blockPos.z+z);

						//if it is outside the map
						if(neighbour.x < 1 || neighbour.x > width-2 ||
							neighbour.y < 1 || neighbour.y > height-2 ||
							neighbour.z < 1 || neighbour.z > depth-2)	continue;


						if(worldBlocks[(int)neighbour.x,(int)neighbour.y,(int)neighbour.z] != null)
						{
							if(NeighbourCount(neighbour) == 26)
							{
								Destroy(worldBlocks[(int)neighbour.x,(int)neighbour.y,(int)neighbour.z].block);
								worldBlocks[(int)neighbour.x,(int)neighbour.y,(int)neighbour.z] = null;
							}
						}
					}
				}
	}

	// Update is called once per frame
	void Update () 
	{

		if (Input.GetKey ("1")) {
			selected = 1;
			inventory1.GetComponent<Image> ().color = Color.green;
			inventory2.GetComponent<Image> ().color = Color.white;
			inventory3.GetComponent<Image> ().color = Color.white;
		}
		if (Input.GetKey ("2")) {
			selected = 2;
			inventory1.GetComponent<Image> ().color = Color.white;
			inventory2.GetComponent<Image> ().color = Color.green;
			inventory3.GetComponent<Image> ().color = Color.white;
		}
		if (Input.GetKey ("3")) {
			selected = 3;
			inventory1.GetComponent<Image> ().color = Color.white;
			inventory2.GetComponent<Image> ().color = Color.white;
			inventory3.GetComponent<Image> ().color = Color.green;

		}
		if(Input.GetMouseButtonDown(0))
		{
			RaycastHit hit;
			Ray ray = Camera.main.ScreenPointToRay(new Vector3(Screen.width/2.0f, Screen.height/2.0f, 0));
			if(Physics.Raycast(ray, out hit, 1000.0f))
			{
				Vector3 blockPos = hit.transform.position;

				//this is the bottom block.  Don't delete it.
				if((int)blockPos.y == 0) return;

				worldBlocks[(int)blockPos.x,(int)blockPos.y,(int)blockPos.z] = null;
				Destroy(hit.transform.gameObject);

				for(int x = -1; x <= 1; x++)
					for(int y = -1; y <=1; y++)
						for(int z = -1; z <= 1; z++)
						{
							if(!(x == 0 && y == 0 && z == 0))
							{
								Vector3 neighbour = new Vector3(blockPos.x+x, blockPos.y+y, blockPos.z+z);
								DrawBlock(neighbour);
							}
						}
			}
		}
		else if(Input.GetMouseButtonDown(1))
		{
			RaycastHit hit;
			Ray ray = Camera.main.ScreenPointToRay(new Vector3(Screen.width/2.0f, Screen.height/2.0f, 0));
			if(Physics.Raycast(ray, out hit, 1000.0f))
			{
				Vector3 blockPos = hit.transform.position;
				Vector3 hitVector = blockPos - hit.point;

				hitVector.x = Mathf.Abs(hitVector.x);
				hitVector.y = Mathf.Abs(hitVector.y);
				hitVector.z = Mathf.Abs(hitVector.z);

				if(hitVector.x > hitVector.z && hitVector.x > hitVector.y)
					blockPos.x -= (int) Mathf.RoundToInt(ray.direction.x);
				else if(hitVector.y > hitVector.x && hitVector.y > hitVector.z)
					blockPos.y -= (int) Mathf.RoundToInt(ray.direction.y);
				else
					blockPos.z -= (int) Mathf.RoundToInt(ray.direction.z);

				CreateBlock((int) blockPos.y, blockPos, true);
				CheckObscuredNeighbours(blockPos);

			}
		}


	}
}