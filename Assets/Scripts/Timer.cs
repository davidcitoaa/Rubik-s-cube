using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Timer : MonoBehaviour
{
    public float Seconds = 0;
    public int Minutes = 0;
    public TMP_Text error;
    public TMP_Text timerText;
    public float GameTime;
    private float timer = 1;
    int limit = 60;
    public bool timerRunning = false;
    private bool stillTimeToSolve;
    int nSecondsPerMinute = 60;
    public CubeManager cubeManager;


    void Start()
    {
        timerText.text = "0";

    }

    void PrintTime()
    {

        if (timerRunning == true)
        {
            Seconds = GameTime % nSecondsPerMinute;
            Minutes = (int)GameTime / nSecondsPerMinute;
            if (GameTime == 0)
                timerText.text = "0";
            GameTime += Time.deltaTime;
            timer += Time.deltaTime;
            if (timer >= 1f)
            {
                timer = 0;
                if (Minutes != 0)
                    timerText.text = Minutes.ToString() + ":" + Seconds.ToString("F0");
                else
                    timerText.text = Seconds.ToString("F0");
            }
        }
        else if (Minutes == 0)
            timerText.text = Seconds.ToString("F2");
        else
            timerText.text = Minutes.ToString() + ":" + Seconds.ToString("00.00");
    }

    void Update()
    {
        StartFinish();
        PrintTime();
    }

    void StartFinish()
    {
        /*if (Input.GetKeyDown(KeyCode.Semicolon) ||
            Input.GetKeyDown(KeyCode.A) ||
            Input.GetKeyDown(KeyCode.T) ||
            Input.GetKeyDown(KeyCode.B) ||
            Input.GetKeyDown(KeyCode.Q) ||
            Input.GetKeyDown(KeyCode.P))
            return;*/
        if (cubeManager.isComplete())
        {
            timerRunning = false;
            
        }
        else if (Input.GetKeyDown(KeyCode.Space))
        {
            timerRunning = true;
            if (!stillTimeToSolve)
                return;
        }

        /*if (Input.GetKeyDown(KeyCode.Space))
        {
            timerRunning = !timerRunning;
            if (!stillTimeToSolve)
                return;
        }*/

        // если количество прошедших минут меньше 60, значит, время для сборки еще осталось, иначе- нет.
        stillTimeToSolve = Minutes < limit;
        if (!timerRunning)
            GameTime = 0;
        else if (!stillTimeToSolve)
        {
            error.text = "Лимит по сборке - 60 минут. К сожалению, вы в него не уложились. Попробуйте еще раз.";
            timerRunning = false;
            cubeManager.canRotate = false;
        }
        else 
           error.text = "";
    }
 }
