﻿using UnityEngine;
using System.Collections;

public class buttonMethods : MonoBehaviour {

    public Canvas pauseMenu;
    public bool paused = false;

	// Use this for initialization
	void Start ()
    {
	    
	}
	
	// Update is called once per frame
	void Update ()
    {
	
	}

    public void pause()
    {
        Time.timeScale = 0.0f;
        pauseMenu.enabled = true;
    }
    public void unpause()
    {
        Time.timeScale = 1.0f;
        pauseMenu.enabled = false;
    }


}
