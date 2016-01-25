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
        if (!GameObject.FindGameObjectWithTag("UI").GetComponent<buttonMethods>().paused && !GameObject.FindGameObjectWithTag("UI").GetComponent<buttonMethods>().gameLost)
        {
            gameObject.transform.Translate(new Vector3(-speed, 0.0f, 0.0f));
            if (gameObject.transform.position.x < -20.0f)
            {
                Destroy(gameObject);
            }
        }
	}
}
