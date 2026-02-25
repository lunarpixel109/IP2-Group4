using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class LapTimerManager : MonoBehaviour
{
    public static LapTimerManager instance;

    private float currentLapTime;                               // tracks current lap time 
    private float bestLapTime = Mathf.Infinity;                 // stores the fastest lap time
    public List<float> lapTimes = new List<float>();            // list of all recorded lap times 
    public int laps = 0;                                        // current lap counter

    public bool raceStarted = false;                            
    public bool lapRunning = false;

    // ui refrences 
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
        lapTimes = new  List<float>();
        
    }

    private void Update()
    {
        // start lap timer if lap is running 
        if (lapRunning)
        {
            currentLapTime += Time.deltaTime;
        }
        // update the text ui 
        UpdateUi();

        // after at least 2 laps show best lap time
        if (laps >= 2)
        {
            BestLapTimeText.gameObject.SetActive(true);
            BeforeBestLapText.gameObject.SetActive(false);
        }

    }

    // called when the player crosses the start/finish line
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

            // if the lap is faster than best lap update best lap
            if (currentLapTime < bestLapTime)
            {
                bestLapTime = currentLapTime;
            }


            // store lap time locally
            lapTimes.Add(currentLapTime);

            // send lap time to leaderboard manager
            LeaderBoardManager.instance.AddLapTime(currentLapTime);

            // reset timer for next lap and move to next lap
            currentLapTime = 0f;
            laps += 1;
            



            //Debug.Log("Total laps recorded: " + lapTimes.Count);

        }
    }

    // update all ui elemets 
    private void UpdateUi()
    {
        currentLapTimeText.text = FormatTime(currentLapTime);
        BestLapTimeText.text = FormatTime(bestLapTime);
        // placeholder text before best lap is available
        BeforeBestLapText.text = "--:--";
        lapText.text = "Lap:" + laps + "/10";
    }
    // converts time to minutes and seconds
    private string FormatTime(float time)
    {
        int minutes = (int)time / 60;
        float seconds = time % 60;
        return string.Format("{0:00}:{1:00}", minutes, seconds);
    }

}
