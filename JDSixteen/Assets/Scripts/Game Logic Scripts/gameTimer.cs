using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class gameTimer : MonoBehaviour
{
    //All times are in Seconds
    public float startTime = 15.0f;         //This sets the inital timer time
    //public float changeStartTimeBy = -5.0f;      //when changeStartTime() is called, this is the default amount it will subtract
    public float offsetForSlider = 5.0f;

    private Slider timerSlider;
    private float actualTime;
    private float newStartTime;

    private GridController grid;

    // Use this for initialization
    void Start()
    {
        grid = GameObject.FindGameObjectWithTag("Grid").GetComponent<GridController>();

        actualTime = startTime + offsetForSlider;
        newStartTime = startTime + offsetForSlider;

        timerSlider = GameObject.FindGameObjectWithTag("Timer").GetComponent<Slider>();
        timerSlider.maxValue = startTime + offsetForSlider;
        timerSlider.value = actualTime + offsetForSlider;
    }

    // Update is called once per frame
    void Update()
    {
        if (!GameObject.FindGameObjectWithTag("UI").GetComponent<buttonMethods>().gameLost)
        {
            actualTime -= Time.deltaTime;
            timerSlider.value = actualTime;
            timerSlider.maxValue = startTime + offsetForSlider;
            if (actualTime <= offsetForSlider)
            {
                grid.CreateNewBlockRow();

                startTime = newStartTime;
                actualTime = startTime;
            }
        }
    }

    public void SetStartTime(float time)
    {
        newStartTime = time;
    }

}
