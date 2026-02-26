using UnityEngine;
using TMPro;
using System.Collections.Generic;
using UnityEngine.UI;

public class LeaderBoardManager : MonoBehaviour
{
    public static LeaderBoardManager instance;

    private const int MAX_ENTRIES = 10;                             // max number of entries 
    private const string LEADERBOARD_KEY = "BestLapTimes";          // used to save laptime data 

    public List<float> bestLapTimes = new List<float>();            //holds the best lap times top 10

    public TextMeshProUGUI leaderboardText;                         // leaderboard text ui 

    // ui elements for menu navigation 
    public GameObject leaderBoardButton;                            
    public GameObject backButton;
    public GameObject clear;

    


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
        
        leaderBoardButton.GetComponent<Button>().onClick.AddListener(UpdateLeaderboardUI);
        backButton.GetComponent<Button>().onClick.AddListener(back);
        clear.GetComponent<Button>().onClick.AddListener(ClearLeaderboards);
    }
    // called when a new lap is submitted 
    public void AddLapTime(float lapTime)
    {

        
        // add the new lap time
        bestLapTimes.Add(lapTime);

        // Sort best to worst
        bestLapTimes.Sort();

        // Keep only top 10
        if (bestLapTimes.Count > MAX_ENTRIES)
            bestLapTimes.RemoveRange(MAX_ENTRIES, bestLapTimes.Count - MAX_ENTRIES);

        // save updated leaderboard
        SaveLeaderboard();
        //UpdateLeaderboardUI();
    }
    // saves the leaderboard
    private void SaveLeaderboard()
    {
        string data = string.Join(",", bestLapTimes);
        PlayerPrefs.SetString(LEADERBOARD_KEY, data);
        PlayerPrefs.Save();
    }

    // loads the leaderboard
    private void LoadLeaderboard()
    {
        bestLapTimes.Clear();

        // if no saved data the exit early 
        if (!PlayerPrefs.HasKey(LEADERBOARD_KEY))
            return;

        // retrived saved data and split into individual times
        string data = PlayerPrefs.GetString(LEADERBOARD_KEY);
        string[] times = data.Split(',');

        foreach (string t in times)
        {
            if (float.TryParse(t, out float value))
                bestLapTimes.Add(value);
        }
    }
    // display leaderbaord on screen
    public void UpdateLeaderboardUI()
    {
        leaderboardText.text = "";

        for (int i = 0; i < bestLapTimes.Count; i++)
        {
            leaderboardText.text += $"{i + 1}. {FormatTime(bestLapTimes[i])}\n";
        }
        //enable leaderboard ui and disable the leaderboard button
        leaderboardText.gameObject.SetActive(true);
        leaderBoardButton.SetActive(false);
        clear.SetActive(true);
        backButton.SetActive(true);
        Time.timeScale = 0;
        
        
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
        bestLapTimes.Clear();
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
        Time.timeScale = 1;
    }

}

    


