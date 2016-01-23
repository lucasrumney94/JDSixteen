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
        for (int i = 0; i < width; i++)
        {
            for (int j = 1; j < height; j++)
            {
                float xPos = (float)i - ((float)(width - 1) / 2);
                float yPos = (float)j - ((float)(height - 1) / 2);
                if(gridPoints[i,j - 1] == null)
                {
                    gridPoints[i, j].anchorPoint.y -= 1f;
                    gridPoints[i, j - 1] = gridPoints[i, j];
                    gridPoints[i, j] = null;
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

    //void OnDrawGizmos()
    //{
    //    if(gridPoints != null)
    //    {
    //        for (int i = 0; i < width; i++)
    //        {
    //            for (int j = 0; j < height; j++)
    //            {
    //                Gizmos.DrawCube(gridPoints[i, j], Vector3.one * 0.05f);
    //            }
    //        }
    //    }
    //}
}
