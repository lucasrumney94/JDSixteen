using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class scoreHandler : MonoBehaviour {

    public int highScore = 0;
    public int score = 0;
    private Text highScoreText;
    private Text scoreText;

    private static scoreHandler scoreH;
    public static scoreHandler SCORE
    {
        get
        {
            if (scoreH == null)
            {
                GameObject UIPrefab = new GameObject("scoreHandler");
                UIPrefab.AddComponent<scoreHandler>();
            }
            return scoreH;
        }
    }




    void Awake()                                                        
    {
        DontDestroyOnLoad(gameObject);
        //highScoreText = GameObject.FindGameObjectWithTag("highScore").GetComponent<Text>();
        //scoreText = GameObject.FindGameObjectWithTag("score").GetComponent<Text>();
        if (scoreH == null)
        {
            //DontDestroyOnLoad(gameObject);							
            scoreH = this;                                                  
        }
        else if (scoreH != this)
        {
            Destroy(gameObject);                                        
        }
    }


    // Use this for initialization
    void Start ()
    {
        
    }
	
	// Update is called once per frame
	void Update ()
    {
        highScoreText = GameObject.FindGameObjectWithTag("highScore").GetComponent<Text>();
        scoreText = GameObject.FindGameObjectWithTag("score").GetComponent<Text>();
        updateText();
        checkHighScore();
	}


    public void resetScore()
    {
        score = 0;
    }

    public void addToScore(int scoreToBeAdded)
    {
        score += scoreToBeAdded;
    }

    void updateText()
    {
        highScoreText.text = "High: " + highScore;
        scoreText.text = "Score: " + score;
    }
    void checkHighScore()
    {
        if (score > highScore)
        {
            highScore = score;
        }
    }


}
