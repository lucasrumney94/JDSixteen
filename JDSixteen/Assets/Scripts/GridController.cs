using UnityEngine;
using System.Collections;

public class GridController : MonoBehaviour {

    public int width = 6;
    public int height = 6;
    public float scale = 1f;
    public Vector3[,] gridPoints;

    void Start()
    {
        gridPoints = new Vector3[width, height];
        GenerateGrid();
    }

    private void GenerateGrid()
    {
        for(int i = 0; i < width; i++)
        {
            for (int j = 0; j < height; j++)
            {
                float xPos = (float)i - ((float)(width - 1) / 2);
                float yPos = (float)j - ((float)(height - 1) / 2);
                gridPoints[i, j] = new Vector3(xPos, yPos, 0f);
                //GameObject testCube = GameObject.CreatePrimitive(PrimitiveType.Cube);
                //testCube.name = i.ToString() + ", " + j.ToString();
                //testCube.transform.position = gridPoints[i, j];
            }
        }
    }

    void OnDrawGizmos()
    {
        if(gridPoints != null)
        {
            for (int i = 0; i < width; i++)
            {
                for (int j = 0; j < height; j++)
                {
                    Gizmos.DrawCube(gridPoints[i, j], Vector3.one * 0.05f);
                }
            }
        }
    }
}
