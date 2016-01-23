using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class gameTimer : MonoBehaviour
{
    //All times are in Seconds
    public float startTime = 15.0f;         //This sets the inital timer time
    public float changeStartTimeBy = -5.0f;      //when changeStartTime() is called, this is the default amount it will subtract
    public float offsetForSlider = 5.0f;

    private Slider timerSlider;
    private float actualTime;
    private float newStartTime;

    // Use this for initialization
    void Start()
    {
        actualTime = startTime + offsetForSlider;
        newStartTime = startTime + offsetForSlider;

        timerSlider = GameObject.FindGameObjectWithTag("Timer").GetComponent<Slider>();
        timerSlider.maxValue = startTime + offsetForSlider;
        timerSlider.value = actualTime + offsetForSlider;
    }

    // Update is called once per frame
    void Update()
    {

        actualTime -= Time.deltaTime;
        timerSlider.value = actualTime;
        if (actualTime <= offsetForSlider)
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
