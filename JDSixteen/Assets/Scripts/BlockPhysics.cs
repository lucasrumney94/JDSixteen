using UnityEngine;
using System.Collections;

public class BlockPhysics : MonoBehaviour {

    public Vector3 currentMousePos = Vector3.zero;

    public int blockRank = 1;

    public float mouseSnapForce = 5f;
    public float maxSnapForce = 50f;
    public float anchorSnapSpeed = 0.2f;
    public float minInstantSnapDistance = 0.01f;

    public bool beingDragged;

    //public Vector3 anchorPoint;
    private GridController grid;

    void Start()
    {
        grid = GameObject.FindGameObjectWithTag("Grid").GetComponent<GridController>();
    }

    void FixedUpdate()
    {
    }

    void OnMouseDown()
    {
        beingDragged = true;
        GetComponent<Rigidbody2D>().isKinematic = false;

        //Announce position to GridController
        Vector2 gridPos = GridPosition();
        grid.SetBlockGridPosition((int)gridPos.x, (int)gridPos.y, null);
    }

    void OnMouseDrag()
    {
        currentMousePos = Input.mousePosition;
        Vector2 dragPosition = WorldMousePos(currentMousePos);
        float deltaX = dragPosition.x - transform.position.x;
        float deltaY = dragPosition.y - transform.position.y;
        float distanceSquared = Mathf.Pow(Vector3.Distance(dragPosition, transform.position), 2f);
        Vector2 toDragPos = new Vector2(deltaX, deltaY).normalized * mouseSnapForce * distanceSquared;
        toDragPos.x = Mathf.Clamp(toDragPos.x, -maxSnapForce, maxSnapForce);
        toDragPos.y = Mathf.Clamp(toDragPos.y, -maxSnapForce, maxSnapForce);
        GetComponent<Rigidbody2D>().velocity = toDragPos;

        //Check for combination
        Vector2 gridPos = GridPosition();
        grid.CheckBlockGridCombination((int)gridPos.x, (int)gridPos.y, this);
    }

    void OnMouseUp()
    {
        beingDragged = false;
        GetComponent<Rigidbody2D>().isKinematic = true;
        GetComponent<Rigidbody2D>().velocity = Vector2.zero;

        //Announce position to GridController
        Vector2 gridPos = GridPosition();
        grid.SetBlockGridPosition((int)gridPos.x, (int)gridPos.y, this);
    }

    private Vector2 GridPosition()
    {
        int gridX = (int)Mathf.Floor(transform.position.x);
        int gridY = (int)Mathf.Floor(transform.position.y);
        gridX = gridX + (grid.width / 2);
        gridY = gridY + (grid.height / 2);
        return new Vector2(gridX, gridY);
    }

    //Mouse world position on the near clipping plane of the camera
    private Vector2 WorldMousePos(Vector3 mousePos)
    {
        Vector3 worldPos = Camera.main.ScreenToWorldPoint(new Vector3(mousePos.x, mousePos.y, 0f));
        return new Vector2(worldPos.x, worldPos.y);
    }
}
