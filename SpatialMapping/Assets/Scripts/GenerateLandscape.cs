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

    private SteamVR_TrackedController _controller;

    public static int width = 128;
	public static int depth = 128;
	public static int height = 10;
	public int heightScale = 1;
	public int heightOffset = 1;
	public float detailScale = 25.0f;
    public float blockScale = 0.1f;

	public GameObject whiteBlock;
	public GameObject blueBlock;
	public GameObject redBlock;
    public GameObject invisibleBlock;

	public int selected =1;

	public int inventory_slots = 3;
	public Image inventory1;
	public Image inventory2;
	public Image inventory3;

	Color originalColor = Color.white;
	Color selectedColor = Color.green;

	Block[,,] worldBlocks = new Block[width,height,depth];

    private SteamVR_TrackedObject trackedObj;
    SteamVR_TrackedController controller;

    // Use this for initialization
    void Start () 
	{
        //instantiate items
        inventory1.color = Color.green;
		inventory2.color = Color.white;
		inventory3.color = Color.white;

        //Create the block world
		int seed = (int) Network.time * 10;
		for(int z = 0; z < depth; z++)
		{
			for(int x = 0; x < width; x++)
			{
				int y = (int) (Mathf.PerlinNoise((x+seed)/detailScale, (z+seed)/detailScale) * heightScale) 
					+ heightOffset;
				Vector3 blockPos = new Vector3(x, y, z);

                startCreate(y, blockPos * blockScale, true);
				while(y > 0)
				{
					y--;
					blockPos = new Vector3(x , y , z );
                    startCreate(y, blockPos * blockScale, false);
				}

			}
		}

	}

    void startCreate (int y, Vector3 blockPos, bool create)
    {
        if (blockPos.x < 0 || blockPos.x >= width || blockPos.y < 0 || blockPos.y >= height || blockPos.z < 0 || blockPos.x >= depth) return;
        GameObject newBlock = null;
		if (create) {
			newBlock = (GameObject)Instantiate (invisibleBlock, blockPos, Quaternion.identity);
			newBlock.layer = LayerMask.NameToLayer ("Objects");
		}

        worldBlocks[(int)(blockPos.x), (int)blockPos.y, (int)blockPos.z] = new Block(1, create, newBlock);

    }
	/*
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
				newBlock = (GameObject) Instantiate(blueBlock,blockPos, Quaternion.identity);

			else if(worldBlocks[(int)blockPos.x,(int)blockPos.y,(int)blockPos.z].type == 2)
				newBlock = (GameObject) Instantiate(whiteBlock,blockPos, Quaternion.identity);

			else if(worldBlocks[(int)blockPos.x,(int)blockPos.y,(int)blockPos.z].type == 3)
				newBlock = (GameObject) Instantiate(redBlock,blockPos, Quaternion.identity);

			else
				worldBlocks[(int)blockPos.x,(int)blockPos.y,(int)blockPos.z].vis = false;

			if(newBlock != null)
				worldBlocks[(int)blockPos.x,(int)blockPos.y,(int)blockPos.z].block = newBlock;
		}

	}
*/		
		

	// Update is called once per frame
	void Update () 
	{
		/*
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

        if (Input.GetMouseButtonDown(0))
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
*/

	}

	/*
    public void PlaceBlock (object sender, ClickedEventArgs e)
    {
        Debug.Log("Trigger Pressed");
        int layerMask = 9;
        RaycastHit hit;
        
        //if (Physics.Raycast(transform.position, transform.forward * 10, out hit, layerMask))
		if (Physics.Raycast(transform.position, transform.forward * 10, out hit, layerMask))
        {
            Debug.Log("Block Created");
            Vector3 blockPos = hit.transform.position;
            Vector3 hitVector = blockPos - hit.point;
            hitVector.x = Mathf.Abs(hitVector.x);
            hitVector.y = Mathf.Abs(hitVector.y);
            hitVector.z = Mathf.Abs(hitVector.z);
            //Ray ray = Camera.main.ScreenPointToRay(new Vector3(Screen.width / 2.0f, Screen.height / 2.0f, 0));
            Ray ray = Camera.main.ScreenPointToRay(new Vector3(hit.transform.position.x,hit.transform.position.y));
            Debug.Log(hit.transform.position);
			Debug.Log(hitVector);
            if (hitVector.x > hitVector.z && hitVector.x > hitVector.y)
                blockPos.x -= (int)Mathf.RoundToInt(ray.direction.x);
            else if (hitVector.y > hitVector.x && hitVector.y > hitVector.z)
                blockPos.y -= (int)Mathf.RoundToInt(ray.direction.y);
            else
                blockPos.z -= (int)Mathf.RoundToInt(ray.direction.z);
			//blockPos.x *= .1f;
			//blockPos.y *= .1f;
			//blockPos.z *= .1f;
            CreateBlock((int)blockPos.y, blockPos, true);
            CheckObscuredNeighbours(blockPos);
        }
    }
    */

}