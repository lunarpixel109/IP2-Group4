using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class LapTimerManager : MonoBehaviour
{
    public static LapTimerManager instance;

    private float currentLapTime;
    private float bestLapTime = Mathf.Infinity;
    public List<float> lapTimes = new List<float>();
    public int laps = 0;

    public bool raceStarted = false;
    public bool lapRunning = false;

    public TextMeshProUGUI currentLapTimeText;
    public TextMeshProUGUI lapText;
    public TextMeshProUGUI BestLapTimeText;
    public TextMeshProUGUI BeforeBestLapText;

    private void Awake()
    {
        if (instance == null)
            instance = this;
        else
            Destroy(gameObject);


        BestLapTimeText.gameObject.SetActive(false);

        
    }

    private void Update()
    {
        if (lapRunning)
        {
            currentLapTime += Time.deltaTime;
        }
        UpdateUi();
        if(laps >= 2)
        {
            BestLapTimeText.gameObject.SetActive(true);
            BeforeBestLapText.gameObject.SetActive(false);
        }

    }

    public void OnStartFinishCrossed()
    {
        // First time crossing the line start race

        Debug.Log("Start/Finish line crossed");
        if (!raceStarted)
        {
            Debug.Log("Race started!");
            raceStarted = true;
            lapRunning = true;
            currentLapTime = 0f;
            laps = 1;
            return;
        }

        // Lap finished record time and restart lap
        if (lapRunning)
        {
            Debug.Log("Lap finished! Time: " + currentLapTime);
            if (currentLapTime < bestLapTime)
            {
                bestLapTime = currentLapTime;
            }

            
            lapTimes.Add(currentLapTime);

            LeaderBoardManager.instance.AddLapTime(currentLapTime);

            currentLapTime = 0f;
            laps += 1;
            



            //Debug.Log("Total laps recorded: " + lapTimes.Count);

        }
    }


    private void UpdateUi()
    {
        currentLapTimeText.text = FormatTime(currentLapTime);
        BestLapTimeText.text = FormatTime(bestLapTime);
        BeforeBestLapText.text = "--:--";
        lapText.text = "Lap:" + laps + "/10";
    }

    private string FormatTime(float time)
    {
        int minutes = (int)time / 60;
        float seconds = time % 60;
        return string.Format("{0:00}:{1:00}", minutes, seconds);
    }

}
