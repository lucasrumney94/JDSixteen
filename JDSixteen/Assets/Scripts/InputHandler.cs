using UnityEngine;
using System.Collections;

public class InputHandler : MonoBehaviour {

    public Vector3 currentMousePos = Vector3.zero;

    public GameObject target; //Object that was hit when the mouse was pressed
    public GameObject heldObject; //Last pickable object to be selected

    public bool holdingObject = false;

    void Update()
    {
        currentMousePos = Input.mousePosition;

        CheckSelectInput();

        if (holdingObject)
        {
            Vector3 heldPosition = WorldMousePos(currentMousePos, Mathf.Abs(Camera.main.transform.position.z - heldObject.transform.position.z));
            heldPosition.z = heldObject.transform.position.z;
            heldObject.transform.position = heldPosition;
        }
    }

    private void CheckSelectInput()
    {
        if (Input.GetMouseButtonDown(0))
        {
            RaycastHit clickHit;

            if (Physics.Raycast(ClickRay(currentMousePos), out clickHit))
            {
                target = clickHit.transform.gameObject;

                if (target.tag == "Block")
                {
                    Debug.Log("Clicked a block!");
                    heldObject = target.transform.gameObject;
                    holdingObject = true;
                }
            }
        }
    }

    //Ray from camera position to mouse world position on the near clipping plane of the camera
    public Ray ClickRay(Vector3 mousePos)
    {
        RaycastHit clickHit;
        Vector3 worldMousePos = WorldMousePos(mousePos);
        Ray clickCast = new Ray(worldMousePos, Vector3.forward);

        //Depreciated
        bool hit = Physics.Raycast(clickCast, out clickHit);
        if (hit)
        {
            Debug.DrawLine(clickCast.origin, clickHit.point);
            Debug.Log(clickHit.transform.gameObject.name);
        }

        return clickCast;
    }

    //Mouse world position on the near clipping plane of the camera
    public Vector3 WorldMousePos(Vector3 mousePos, float distance = 0f)
    {
        return Camera.main.ScreenToWorldPoint(new Vector3(mousePos.x, mousePos.y, distance));
    }
}
