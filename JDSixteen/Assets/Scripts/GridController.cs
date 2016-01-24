using UnityEngine;
using System.Collections;

public class GridController : MonoBehaviour {

    public bool spawnRow = false;

    public int width = 6;
    public int height = 6;
    public float scale = 1f;
    //These should be ordered correctly, block rank increases are based on the order of this list
    public BlockPhysics[] blockPrefabs;
    public BlockPhysics[,] gridPoints;
    private Vector3[,] anchorPoints;

    void Start()
    {
        gridPoints = new BlockPhysics[width, height];
        anchorPoints = new Vector3[width, height];
        InitializeGrid();
        FillAnchorPoints();
    }

    void Update()
    {
        if (spawnRow)
        {
            CreateNewBlockRow(3);
            spawnRow = false;
        }
    }

    void LateUpdate()
    {
        //Working left to right, bottom to top
        for (int i = 0; i < width; i++)
        {
            for (int j = 0; j < height; j++)
            {
                if (gridPoints[i,j] != null)
                {
                    BlockPhysics block = gridPoints[i, j];
                    Vector3 anchor = anchorPoints[i, j] + transform.position;

                    //Find all blocks that have an empty space below them
                    if (j > 0 && gridPoints[i, j - 1] == null)
                    {
                        //and reassign their position in the grid
                        gridPoints[i, j - 1] = gridPoints[i, j];
                        gridPoints[i, j] = null;
                    }
                    //Find all blocks that have a block of the same rank below them
                    else if(j > 0 && (gridPoints[i, j - 1].blockRank == block.blockRank))
                    {
                        anchor = anchorPoints[i, j - 1] + transform.position;
                        //Check if the blocks are close enough together to combine
                        if(Vector3.Distance(gridPoints[i, j - 1].transform.position, block.transform.position) < scale / 2f)
                        {
                            gridPoints[i, j - 1] = CombineBlocks(gridPoints[i, j - 1], block);
                            gridPoints[i, j] = null;
                        }
                    }
                    
                    if(block.beingDragged == false)
                    {
                        //Apply a translation to keep the block stuck to anchor point
                        block.transform.position = Vector3.Lerp(block.transform.position, anchor, block.anchorSnapSpeed);
                        if (Vector3.Distance(block.transform.position, anchor) < block.minInstantSnapDistance)
                        {
                            block.transform.position = anchor;
                        }
                    }
                }
            }
        }

        //InitializeGrid();
    }

    //Called by the block that is currently being held by the user
    //Checks if there is already another block in the current position and tries to combine them if there is
    public void CheckBlockGridCombination(int x, int y, BlockPhysics block)
    {
        x = Mathf.Clamp(x, 0, width);
        y = Mathf.Clamp(y, 0, height);
        if (gridPoints[x, y] != null && gridPoints[x, y] != block)
        {
            if (gridPoints[x, y].blockRank == block.blockRank)
            {
                BlockPhysics newBlock = CombineBlocks(gridPoints[x, y], block);
                gridPoints[x, y] = newBlock;
            }
        }
    }

    //Called when the user moves a block with the mouse
    public void SetBlockGridPosition(int x, int y, BlockPhysics block)
    {
        x = Mathf.Clamp(x, 0, width);
        y = Mathf.Clamp(y, 0, height);
        if (block == null || gridPoints[x, y] == null)
        {
            gridPoints[x, y] = block;
        }
        else Debug.Log("Tried to place block on top of another block!", block);
    }

    //Instantiates a new block from the combination of a and b, removes a and b from the scene, and returns the created object
    private BlockPhysics CombineBlocks(BlockPhysics a, BlockPhysics b)
    {
        if(a.blockRank == b.blockRank && a.blockRank < blockPrefabs.Length)
        {
            BlockPhysics newBlock = Instantiate(blockPrefabs[a.blockRank]) as BlockPhysics;
            newBlock.transform.position = a.transform.position;
            Destroy(a.gameObject);
            Destroy(b.gameObject);
            return newBlock;
        }
        else
        {
            Debug.Log("can't combine, block ranks should match!");
            return null;
        }
    }

    public void CreateNewBlockRow(int highestRank)
    {
        for (int i = 0; i < width; i++)
        {
            for (int j = height - 1; j > 0; j--)
            {
                gridPoints[i, j] = gridPoints[i, j - 1];
            }

            int newIndex = (int)Random.Range(0f, highestRank);
            BlockPhysics newBlock = Instantiate(blockPrefabs[newIndex]);
            float xPos = (float)i - ((float)width / 2f);
            float yPos = (-(float)height / 2f) - 1f;
            newBlock.transform.position = new Vector3(xPos, yPos);
            gridPoints[i, 0] = newBlock;
        }
    }

    private void InitializeGrid()
    {
        for (int i = 0; i < width; i++)
        {
            for (int j = 0; j < height; j++)
            {
                float xPos = (float)i - ((float)(width - 1) / 2);
                float yPos = (float)j - ((float)(height - 1) / 2);
                gridPoints[i, j] = null;
                //GameObject testCube = GameObject.CreatePrimitive(PrimitiveType.Cube);
                //testCube.name = i.ToString() + ", " + j.ToString();
                //testCube.transform.position = gridPoints[i, j];
            }
        }
    }

    private void FillAnchorPoints()
    {
        for (int i = 0; i < width; i++)
        {
            for (int j = 0; j < height; j++)
            {
                float xPos = (float)i - ((float)(width - 1) / 2);
                float yPos = (float)j - ((float)(height - 1) / 2);
                anchorPoints[i, j] = new Vector3(xPos, yPos);
            }
        }
    }

    void OnDrawGizmos()
    {
        if (gridPoints != null)
        {
            for (int i = 0; i < width; i++)
            {
                for (int j = 0; j < height; j++)
                {
                    if(gridPoints[i, j] != null)
                    {
                        Gizmos.DrawCube(anchorPoints[i, j], Vector3.one * 0.15f);
                    }
                }
            }
        }
    }
}
