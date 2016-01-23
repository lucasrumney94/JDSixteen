using UnityEngine;
using System.Collections;

public class BlockPhysics : MonoBehaviour {

    public Vector3 currentMousePos = Vector3.zero;

    public int blockRank = 1;

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
        Vector3 dragPosition = WorldMousePos(currentMousePos);
        dragPosition.z = 0f;
        transform.position = dragPosition;
    }

    void OnMouseUp()
    {
        beingDragged = false;
    }

    //Mouse world position on the near clipping plane of the camera
    public Vector3 WorldMousePos(Vector3 mousePos, float distance = 0f)
    {
        return Camera.main.ScreenToWorldPoint(new Vector3(mousePos.x, mousePos.y, distance));
    }
}
