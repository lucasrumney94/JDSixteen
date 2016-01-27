using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class buttonMethods : MonoBehaviour
{

    public Canvas pauseMenu;
    public Canvas youLostMenu;
    public bool paused = false;
    public bool gameLost = false;

    private scoreHandler myScoreHandler;

    // Use this for initialization
    void Start()
    {

    }
    void OnLevelWasLoaded(int level)
    {
        //myScoreHandler = GameObject.FindGameObjectWithTag("ScoreHandler").GetComponent<scoreHandler>();
    }
    // Update is called once per frame
    void Update()
    {
        myScoreHandler = GameObject.FindGameObjectWithTag("ScoreHandler").GetComponent<scoreHandler>();
        if (gameLost)
        {
            //Time.timeScale = 0.0f;
            youLostMenu.enabled = true;
        }
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
    public void togglePause()
    {
        paused = !paused;
        pauseMenu.enabled = paused;
        if (paused)
        {
            Time.timeScale = 0.0f;
        }
        else
        {
            Time.timeScale = 1.0f;
        }

    }

    public void restart()
    {
        gameLost = false;
        Time.timeScale = 1.0f;
        myScoreHandler.score = 0;
        SceneManager.LoadScene(SceneManager.GetActiveScene().name, LoadSceneMode.Single);
    }


}
