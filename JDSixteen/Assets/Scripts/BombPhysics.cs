using UnityEngine;
using System.Collections;

public class BombPhysics : BlockPhysics {

    public int explosionRange = 2;
    
    public void Explode()
    {
        int[] gridPos = GridPosition();
        int xMin = Mathf.Clamp(gridPos[0] - explosionRange, 0, grid.width);
        int xMax = Mathf.Clamp(gridPos[0] + explosionRange, 0, grid.width);
        int yMin = Mathf.Clamp(gridPos[1] - explosionRange, 0, grid.height);
        int yMax = Mathf.Clamp(gridPos[1] + explosionRange, 0, grid.height);
        for (int i = xMin; i <= xMax; i++)
        {
            for(int j = yMin; j <= yMax; j++)
            {
                //Check if index is too far from gridpos
                int orthoDistance = Mathf.Abs(i - gridPos[0]) + Mathf.Abs(j - gridPos[1]);
                if(orthoDistance != 0 && orthoDistance <= explosionRange)
                {
                    if(grid.gridPoints[i, j] != null)
                    {
                        Destroy(grid.gridPoints[i, j].gameObject);
                        grid.gridPoints[i, j] = null;
                    }
                }
            }
        }
    }

    void OnDestroy()
    {
        //Play special bomb animation
    }
}
