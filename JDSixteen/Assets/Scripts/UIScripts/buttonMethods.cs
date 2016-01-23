using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class buttonMethods : MonoBehaviour
{

    public Canvas pauseMenu;
    public Canvas youLostMenu;
    public bool paused = false;
    public bool gameLost = false;

    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (gameLost)
        {
            Time.timeScale = 0.0f;
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
    public void restart()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name, LoadSceneMode.Single);
    }


}