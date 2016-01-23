using UnityEngine;
using System.Collections;

public class GridController : MonoBehaviour {

    public int width = 6;
    public int height = 6;
    public float scale = 1f;
    //These should be ordered correctly, block rank increases are based on the order of this list
    public BlockPhysics[] blockPrefabs;
    public BlockPhysics[,] gridPoints;

    void Start()
    {
        gridPoints = new BlockPhysics[width, height];
        InitializeGrid();
    }

    void LateUpdate()
    {
        //After position of all blocks in the scene has been established
        //Working left to right, bottom to top
        for (int i = 0; i < width; i++)
        {
            for (int j = 0; j < height; j++)
            {
                if (gridPoints[i,j] != null)
                {
                    BlockPhysics block = gridPoints[i, j];
                    //Find all blocks that have an empty space below them
                    if (j > 0 && gridPoints[i, j - 1] == null)
                    {
                        //Set their anchor points to the space below
                        block.anchorPoint.y -= 1f;
                        //and reassign their position in the grid
                        gridPoints[i, j - 1] = gridPoints[i, j];
                        gridPoints[i, j] = null;
                    }
                    
                    if(block.beingDragged == false)
                    {
                        //Apply a translation to keep the block stuck to anchor point
                        block.transform.position = Vector3.Lerp(block.transform.position, block.anchorPoint, block.anchorSnapSpeed);
                        if (Vector3.Distance(block.transform.position, block.anchorPoint) < block.minInstantSnapDistance)
                        {
                            block.transform.position = block.anchorPoint;
                        }
                    }
                }
            }
        }

        InitializeGrid();
    }

    //Checks if there is already another block in the current position and tries to combine them if there is
    public void ReportBlockGridPosition(int x, int y, BlockPhysics block)
    {
        if(gridPoints[x,y] != null && gridPoints[x, y] != block)
        {
            if(gridPoints[x,y].blockRank == block.blockRank)
            {
                BlockPhysics newBlock = Instantiate(CombineBlocks(gridPoints[x, y], block)) as BlockPhysics;
                newBlock.transform.position = gridPoints[x, y].anchorPoint;
                Destroy(block.gameObject);
                Destroy(gridPoints[x, y].gameObject);
                gridPoints[x, y] = newBlock;
            }
        }
        else
        {
            gridPoints[x, y] = block;
        }
    }

    //Returns the prefab that would result from A and B combining
    private BlockPhysics CombineBlocks(BlockPhysics a, BlockPhysics b)
    {
        if(a.blockRank == b.blockRank && a.blockRank < blockPrefabs.Length)
        {
            return blockPrefabs[a.blockRank];
        }
        else
        {
            Debug.Log("can't combine, block ranks should match!");
            return null;
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

    void OnDrawGizmos()
    {
        if (gridPoints != null)
        {
            for (int i = 0; i < width; i++)
            {
                for (int j = 0; j < height; j++)
                {
                    if (gridPoints[i, j] != null)
                    {
                        Gizmos.DrawCube(gridPoints[i, j].anchorPoint, Vector3.one * 0.1f);
                    }
                }
            }
        }
    }
}
