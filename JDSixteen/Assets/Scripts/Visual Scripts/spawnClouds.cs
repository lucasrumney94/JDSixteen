using UnityEngine;
using System.Collections;

public class spawnClouds : MonoBehaviour {

    public GameObject cloud;
    public float ApproximatePeriod = 3.0f;
    public float speed = 5.0f;
    public float randomScaleSpread = 0.2f;
    public float randomSpeedSpread = 0.2f;
    public float ySpread = 0.5f;

	// Use this for initialization
	void Start ()
    {
        StartCoroutine("SpawnClouds");
	}
	
	// Update is called once per frame
	void Update ()
    {
	    
	}

    IEnumerator SpawnClouds()
    {
        for (;;)
        {
            GameObject temp = Instantiate(cloud, new Vector3(20, 3, 0), Quaternion.identity) as GameObject;
            temp.GetComponent<moveMeLeft>().speed = speed + Random.Range(-randomSpeedSpread, randomSpeedSpread);
            float ScaleFactor = temp.transform.localScale.x + Random.Range(-randomScaleSpread, randomScaleSpread);
            temp.transform.localScale =new Vector3(ScaleFactor, ScaleFactor, ScaleFactor);
            temp.transform.position = new Vector3(temp.transform.position.x, temp.transform.position.y + Random.Range(-ySpread,ySpread), 0.0f);

            yield return new WaitForSeconds(ApproximatePeriod + Random.Range(-ApproximatePeriod / 4, ApproximatePeriod / 4));
        }
    }
}
