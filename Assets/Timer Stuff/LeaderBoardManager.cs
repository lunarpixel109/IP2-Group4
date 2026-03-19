using UnityEngine;
using TMPro;
using System.Collections.Generic;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class LeaderBoardManager : MonoBehaviour
{
    public static LeaderBoardManager instance;

    private const int MAX_ENTRIES = 10;                             // max number of entries 
    private const string LEADERBOARD_KEY = "BestLapTimes";          // used to save laptime data 

    public List<LapData> bestLaps = new List<LapData>();            //holds the best lap times top 10

    public TextMeshProUGUI leaderboardText;                         // leaderboard text ui 

    // ui elements for menu navigation 
    public GameObject leaderBoardButton;                            
    public GameObject backButton;
    public GameObject clear;
    public GameObject resume;

    


    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }


        // load leaderboard data from previous sesions 
        LoadLeaderboard();
        //UpdateLeaderboardUI();

        // hide leaderboard ui at the start
        leaderboardText.gameObject.SetActive(false);
        backButton.SetActive(false);
        clear.SetActive(false);
        leaderBoardButton.SetActive(false);
        
        leaderBoardButton.GetComponent<Button>().onClick.AddListener(UpdateLeaderboardUI);
        backButton.GetComponent<Button>().onClick.AddListener(back);
        clear.GetComponent<Button>().onClick.AddListener(ClearLeaderboards);
    }
    
    public void AddLap(LapData lap)
    {
        bestLaps.Add(lap);

        bestLaps.Sort((a, b) => a.totalTime.CompareTo(b.totalTime));

        if (bestLaps.Count > MAX_ENTRIES)
        {
            bestLaps.RemoveRange(MAX_ENTRIES, bestLaps.Count - MAX_ENTRIES);
        }

        SaveLeaderboard();
    }


    
    // saves the leaderboard
    private void SaveLeaderboard()
    {
        List<string> entries = new List<string>();

        foreach (LapData lap in bestLaps)
        {
            string entry = lap.totalTime.ToString();

            foreach (float sector in lap.sectorTimes)
            {
                entry += "|" + sector;

            }

            entries.Add(entry);
        }

        PlayerPrefs.SetString(LEADERBOARD_KEY, string.Join(",", entries));
        PlayerPrefs.Save();

    }

    // loads the leaderboard
    private void LoadLeaderboard()
    {
        bestLaps.Clear();

        if (!PlayerPrefs.HasKey(LEADERBOARD_KEY)) return;

        string data = PlayerPrefs.GetString(LEADERBOARD_KEY);
        string[] entries = data.Split(',');

        foreach (string entry in entries)
        {
            string[] values = entry.Split('|');
            LapData lap = new LapData(values.Length - 1);

            lap.totalTime = float.Parse(values[0]);

            for (int i = 1; i < values.Length; i++)
            {
                lap.sectorTimes[i - 1] = float.Parse(values[i]);

            }

            bestLaps.Add(lap);
        }
    }
    // display leaderbaord on screen
    public void UpdateLeaderboardUI()
    {
        leaderboardText.text = "";

    if (bestLaps.Count == 0)
    {
        leaderboardText.text = "No lap times recorded";
    }
    else
    {
        for (int i = 0; i < bestLaps.Count; i++)
        {
            LapData lap = bestLaps[i];

            leaderboardText.text +=
                $"{i + 1}. {FormatTime(lap.totalTime)} | " +
                $"S1 {lap.sectorTimes[0]:0.00} | " +
                $"S2 {lap.sectorTimes[1]:0.00} | " +
                $"S3 {lap.sectorTimes[2]:0.00}\n";
        }
    }


        //enable leaderboard ui and disable the leaderboard button
        leaderboardText.gameObject.SetActive(true);
        leaderBoardButton.SetActive(false);
        clear.SetActive(true);
        backButton.SetActive(true);
        resume.SetActive(false);
        Time.timeScale = 0;
        EventSystem.current.SetSelectedGameObject(null);
        EventSystem.current.SetSelectedGameObject(clear);


    }

    private string FormatTime(float time)
    {
        int minutes = (int)time / 60;
        float seconds = time % 60;
        return string.Format("{0:00}:{1:00.00}", minutes, seconds);
    }
    // clears leaderboard data
    public void ClearLeaderboards()
    {
        bestLaps.Clear();
        PlayerPrefs.DeleteKey(LEADERBOARD_KEY);
        PlayerPrefs.Save();
        UpdateLeaderboardUI();
    }
    // return from leaderboard view back to normal view
    public void back()
    {
        backButton.gameObject.SetActive(false);
        leaderBoardButton.gameObject.SetActive(true);
        leaderboardText.gameObject.SetActive(false);
        clear.SetActive(false);
        resume.SetActive(true);
        
        EventSystem.current.SetSelectedGameObject(null);
        EventSystem.current.SetSelectedGameObject(leaderBoardButton);
    }



    public void Resume()
    {
        leaderBoardButton.SetActive(false);
        resume.SetActive(false);

        Time.timeScale = 1;
    }

}




