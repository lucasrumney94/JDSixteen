using UnityEngine;
using System.Collections;

public class GridController : MonoBehaviour {

    public bool spawnRow = false;

    public int width = 6;
    public int height = 6;
    public float scale = 1f;
    public int maxRank = 3;
    [Range(0f, 1f)]
    public float specialBlockChance = 0.1f;

    public float mouseSnapForce = 5f;
    public float maxSnapForce = 40f;
    public float anchorSnapSpeed = 0.2f;
    public float minInstantSnapDistance = 0.01f;

    //These should be ordered correctly, block rank increases are based on the order of this list
    public BlockPhysics[] blockPrefabs;
    public BlockPhysics[] specialBlockPrefabs;
    public BlockPhysics[,] gridPoints;
    private Vector3[,] anchorPoints;

    private scoreHandler scoreBoard;
    private gameTimer timer;

    void Start()
    {
        scoreBoard = GameObject.FindGameObjectWithTag("ScoreHandler").GetComponent<scoreHandler>();
        timer = GameObject.FindGameObjectWithTag("NotUITimer").GetComponent<gameTimer>();

        gridPoints = new BlockPhysics[width, height];
        anchorPoints = new Vector3[width, height];
        InitializeGrid();
        FillAnchorPoints();

        CreateNewBlockRow();
        CreateNewBlockRow();
        CreateNewBlockRow();
    }

    void Update()
    {
        if (spawnRow)
        {
            CreateNewBlockRow();
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
                        block.transform.position = Vector3.Lerp(block.transform.position, anchor, anchorSnapSpeed);
                        if (Vector3.Distance(block.transform.position, anchor) < minInstantSnapDistance)
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
        else CheckBlockGridCombination(x, y, block);
    }

    //Instantiates a new block from the combination of a and b, removes a and b from the scene, and returns the created object
    private BlockPhysics CombineBlocks(BlockPhysics a, BlockPhysics b)
    {
        BombPhysics bombA;
        BombPhysics bombB;
        if ((bombA = a as BombPhysics) && (bombB = b as BombPhysics))
        {
            bombA.Explode();
            Destroy(bombA.gameObject);
            Destroy(bombB.gameObject);
            return null;
        }
        else if(a.blockRank == b.blockRank && a.blockRank < blockPrefabs.Length)
        {
            BlockPhysics newBlock = Instantiate(blockPrefabs[a.blockRank]) as BlockPhysics;
            if(newBlock.blockRank > maxRank)
            {
                maxRank = newBlock.blockRank;
                SetTimerBasedOnMaxRank();
            }
            if(newBlock.blockRank == blockPrefabs.Length)
            {
                AddHighestBlockToScore(newBlock);
            }
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

    private void SetTimerBasedOnMaxRank()
    {
        if(maxRank < 8)
        {
            timer.SetStartTime(15f);
        }
        else if(maxRank < 12)
        {
            timer.SetStartTime(11f);
        }
        else
        {
            timer.SetStartTime(7f);
        }
    }

    private void AddHighestBlockToScore(BlockPhysics block)
    {
        //Call ScoreHandler
        scoreBoard.addToScore(100);

        //Play some kind of particle effect


        //Remove the block from the game and clear the grid space
        Destroy(block.gameObject, 0.5f);
    }

    public void CreateNewBlockRow()
    {
        if (CheckForGameOver())
        {
            return;
        }
        else
        {
            for (int i = 0; i < width; i++)
            {
                for (int j = height - 1; j > 0; j--)
                {
                    if (gridPoints[i, j - 1] != null && gridPoints[i, j - 1].beingDragged)
                    {
                        gridPoints[i, j - 1].initialGridPosition = new int[2] { i, j };
                        //gridPoints[i, j - 1].clearedInitialPosition = true;
                        //gridPoints[i, j - 1] = null;
                    }
                    gridPoints[i, j] = gridPoints[i, j - 1];
                }

                int newIndex = Random.Range(0, HighestRank());
                BlockPhysics newBlock;
                //Check if the block to be spawned would match the rank of the one above it
                //If so, decrease its rank by one, or if the rank is one already, increase it
                if (Random.value < specialBlockChance && (gridPoints[i, 0] == null || (gridPoints[i, 0] != null && gridPoints[i, 0].blockRank != 0)))
                {
                    newBlock = Instantiate(specialBlockPrefabs[Random.Range(0, specialBlockPrefabs.Length)]);
                }
                else
                {
                    if (gridPoints[i, 0] != null && gridPoints[i, 0].blockRank == newIndex + 1)
                    {
                        if (newIndex == 0) newIndex = 1;
                        else newIndex--;
                    }
                    newBlock = Instantiate(blockPrefabs[newIndex]);
                }
                float xPos = (float)i - (((float)width - 1f) / 2f);
                float yPos = (-(float)height / 2f) - 1f;
                newBlock.transform.position = new Vector3(xPos, yPos);
                gridPoints[i, 0] = newBlock;
            }
        }
    }

    private bool CheckForGameOver()
    {
        for(int i = 0; i < width; i++)
        {
            if(gridPoints[i, height - 1] != null)
            {
                Debug.Log("Checked for game over");
                StartCoroutine(TotalBoardScore());
                //Triggers Bool in UI that activates the Game Over UI
                GameObject.FindGameObjectWithTag("UI").GetComponent<buttonMethods>().gameLost = true;
                return true;
            }
        }
        return false;
    }

    IEnumerator TotalBoardScore()
    {
        Debug.Log("Totaling board score...");
        for(int j = height - 1; j >= 0; j--)
        {
            for(int i = 0; i < width; i++)
            {
                if(gridPoints[i, j] != null)
                {
                    yield return new WaitForSeconds(0.15f);
                    //Debug.Log(gridPoints[i, j].blockRank, gridPoints[i, j].gameObject);
                    Destroy(gridPoints[i, j].gameObject);
                    scoreBoard.addToScore(gridPoints[i, j].blockRank);
                }
            }
        }
    }

    public void AddBombedBlocksToScore(int score)
    {
        scoreBoard.addToScore(score);
    }

    //Check score handler to see what the highest spawned in a new block row can be
    private int HighestRank()
    {
        if(maxRank < 6)
        {
            return 3;
        }
        else if (maxRank < 9)
        {
            return 5;
        }
        else if (maxRank < 12)
        {
            return 7;
        }
        else
        {
            return 9;
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
                float xPos = (float)i - ((float)(width - 1) / 2f);
                float yPos = (float)j - ((float)(height - 1) / 2f);
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
