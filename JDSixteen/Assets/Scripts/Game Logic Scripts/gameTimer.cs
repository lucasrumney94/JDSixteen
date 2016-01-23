using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class gameTimer : MonoBehaviour {

    private Slider timerSlider;

	// Use this for initialization
	void Start ()
    {
        timerSlider = GameObject.FindGameObjectWithTag("timer").GetComponent<Slider>();
	}
	
	// Update is called once per frame
	void Update ()
    {
	
	}
}
