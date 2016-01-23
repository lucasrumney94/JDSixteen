using UnityEngine;
using System.Collections;

public class GridController : MonoBehaviour {

    public int width = 6;
    public int height = 6;
    public float scale = 1f;
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
                    if(gridPoints[i, j] != null)
                    {
                        Gizmos.DrawCube(gridPoints[i, j].transform.position, Vector3.one * 0.1f);
                    }
                }
            }
        }
    }
}
