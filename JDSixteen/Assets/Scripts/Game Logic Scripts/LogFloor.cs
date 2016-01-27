using UnityEngine;
using System.Collections;

public class LogFloor : MonoBehaviour {

    void Update()
    {
        float rand = Random.Range(0f, 100f);

        int x = (int)Mathf.Floor(rand);
        Debug.Log(x);
    }
}
