using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class gameTimer : MonoBehaviour
{
    //All times are in Seconds
    public float startTime = 15.0f;         //This sets the inital timer time
    public float changeStartTimeBy = -5.0f;      //when changeStartTime() is called, this is the default amount it will subtract
    

    private Slider timerSlider;
    private float actualTime;
    private float newStartTime;

    // Use this for initialization
    void Start()
    {
        actualTime = startTime;
        newStartTime = startTime;

        timerSlider = GameObject.FindGameObjectWithTag("Timer").GetComponent<Slider>();
        timerSlider.maxValue = startTime;
        timerSlider.value = actualTime;
    }

    // Update is called once per frame
    void Update()
    {

        actualTime -= Time.deltaTime;
        timerSlider.value = actualTime;
        if (actualTime <= 0)
        {
            //TODO: call method that spawns another line

            startTime = newStartTime;
            actualTime = startTime;
        }
    }

    void changeStartTime()
    {
        newStartTime += changeStartTimeBy;
    }

}
