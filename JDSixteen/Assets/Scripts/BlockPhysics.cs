using UnityEngine;
using System.Collections;

public class BlockPhysics : MonoBehaviour {

    public Vector3 currentMousePos = Vector3.zero;

    public int blockRank = 1;

    void Start()
    {

    }

    void OnMouseDrag()
    {
        currentMousePos = Input.mousePosition;
        Vector3 dragPosition = WorldMousePos(currentMousePos);
        dragPosition.z = 0f;
        transform.position = dragPosition;
    }

    //Mouse world position on the near clipping plane of the camera
    public Vector3 WorldMousePos(Vector3 mousePos, float distance = 0f)
    {
        return Camera.main.ScreenToWorldPoint(new Vector3(mousePos.x, mousePos.y, distance));
    }
}
