using UnityEngine;
using System.Collections;

public class moveMeLeft : MonoBehaviour {

    public float speed = 5.0f;

	// Use this for initialization
	void Start ()
    {
	
	}
	
	// Update is called once per frame
	void Update ()
    {
        gameObject.transform.Translate(new Vector3(-speed,0.0f,0.0f));
        if (gameObject.transform.position.x < -100)
        {
            Destroy(gameObject);
        }
	}
}
