using UnityEngine;
using TMPro;
using System.Collections.Generic;

public class LeaderBoardManager : MonoBehaviour
{
    public static LeaderBoardManager instance;

    private const int MAX_ENTRIES = 10;
    private const string LEADERBOARD_KEY = "BestLapTimes";

    public List<float> bestLapTimes = new List<float>();

    public TextMeshProUGUI leaderboardText;

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

        LoadLeaderboard();
        //UpdateLeaderboardUI();
        leaderboardText.gameObject.SetActive(false);
        backButton.SetActive(false);
        clear.SetActive(false);
        

    }

    public void AddLapTime(float lapTime)
    {

        

        bestLapTimes.Add(lapTime);

        // Sort best to worst
        bestLapTimes.Sort();

        // Keep only top 10
        if (bestLapTimes.Count > MAX_ENTRIES)
            bestLapTimes.RemoveRange(MAX_ENTRIES, bestLapTimes.Count - MAX_ENTRIES);

        SaveLeaderboard();
        //UpdateLeaderboardUI();
    }

    private void SaveLeaderboard()
    {
        string data = string.Join(",", bestLapTimes);
        PlayerPrefs.SetString(LEADERBOARD_KEY, data);
        PlayerPrefs.Save();
    }

    private void LoadLeaderboard()
    {
        bestLapTimes.Clear();

        if (!PlayerPrefs.HasKey(LEADERBOARD_KEY))
            return;

        string data = PlayerPrefs.GetString(LEADERBOARD_KEY);
        string[] times = data.Split(',');

        foreach (string t in times)
        {
            if (float.TryParse(t, out float value))
                bestLapTimes.Add(value);
        }
    }

    public void UpdateLeaderboardUI()
    {
        leaderboardText.text = "";

        for (int i = 0; i < bestLapTimes.Count; i++)
        {
            leaderboardText.text += $"{i + 1}. {FormatTime(bestLapTimes[i])}\n";
        }

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

    public void ClearLeaderboards()
    {
        bestLapTimes.Clear();
        PlayerPrefs.DeleteKey(LEADERBOARD_KEY);
        PlayerPrefs.Save();
        UpdateLeaderboardUI();
    }

    public void back()
    {
        backButton.gameObject.SetActive(false);
        leaderBoardButton.gameObject.SetActive(true);
        leaderboardText.gameObject.SetActive(false);
        clear.SetActive(false);
        Time.timeScale = 1;
    }

}

    


