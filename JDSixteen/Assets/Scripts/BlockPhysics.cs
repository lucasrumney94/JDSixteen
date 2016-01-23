using UnityEngine;
using System.Collections;

public class BlockPhysics : MonoBehaviour {

    public Vector3 currentMousePos = Vector3.zero;

    public int blockRank = 1;

    public float mouseSnapForce = 5f;
    public float anchorSnapSpeed = 0.2f;
    public float minInstantSnapDistance = 0.01f;

    public bool beingDragged;

    private GameObject grid;

    void Start()
    {
        grid = GameObject.FindGameObjectWithTag("Grid");
    }

    void FixedUpdate()
    {
        if (!beingDragged)
        {
            //Find closest vector3 in grid.gridPoints[,]
            float anchorX = Mathf.Floor(transform.position.x) + 0.5f;
            float anchorY = Mathf.Floor(transform.position.y) + 0.5f;
            Vector3 anchorPoint = new Vector3(anchorX, anchorY);

            //Apply a translation to keep the block stuck to that point
            transform.position = Vector3.Lerp(transform.position, anchorPoint, anchorSnapSpeed);
            if(Vector3.Distance(transform.position, anchorPoint) < minInstantSnapDistance)
            {
                transform.position = anchorPoint;
            }
            //GetComponent<Rigidbody2D>().AddForce(toAnchor * anchorForce);
        }
    }

    void OnMouseDrag()
    {
        beingDragged = true;
        currentMousePos = Input.mousePosition;
        Vector2 dragPosition = WorldMousePos(currentMousePos);
        float deltaX = dragPosition.x - transform.position.x;
        float deltaY = dragPosition.y - transform.position.y;
        float distanceSquared = Mathf.Pow(Vector3.Distance(dragPosition, transform.position), 2f);
        Vector2 toDragPos = new Vector2(deltaX, deltaY).normalized * mouseSnapForce * distanceSquared;
        GetComponent<Rigidbody2D>().velocity = toDragPos;
        //transform.position = dragPosition;
    }

    void OnMouseUp()
    {
        beingDragged = false;
        GetComponent<Rigidbody2D>().velocity = Vector2.zero;
    }

    //Mouse world position on the near clipping plane of the camera
    public Vector2 WorldMousePos(Vector3 mousePos)
    {
        Vector3 worldPos = Camera.main.ScreenToWorldPoint(new Vector3(mousePos.x, mousePos.y, 0f));
        return new Vector2(worldPos.x, worldPos.y);
    }
}
