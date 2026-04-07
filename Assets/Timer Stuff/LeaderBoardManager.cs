using UnityEngine;
using TMPro;
using System.Collections.Generic;
using UnityEngine.EventSystems;

public class LeaderBoardManager : MonoBehaviour
{
    public static LeaderBoardManager instance;

    private const int MAX_ENTRIES = 10;                             // max number of entries 
             

    [SerializeField] private string trackID = "Track1";
    private string LeaderboradKey => "BestLapTimes_" + trackID;


    public List<LapData> bestLaps = new List<LapData>();            //holds the best lap times top 10

    public TextMeshProUGUI leaderboardText;                         // leaderboard text ui 

    

    public Transform entriesContainer;
    public GameObject leaderboardRowPrefab;
    public GameObject firstButton;


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
        EventSystem.current.SetSelectedGameObject(null);
        EventSystem.current.SetSelectedGameObject(firstButton);
        

    }


    private void Start()
    {
        LoadLeaderboard();
        UpdateLeaderboardUI();
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

        PlayerPrefs.SetString(LeaderboradKey, string.Join(",", entries));
        PlayerPrefs.Save();

    }

    // loads the leaderboard
    private void LoadLeaderboard()
    {
        bestLaps.Clear();

        if (!PlayerPrefs.HasKey(LeaderboradKey)) return;

        string data = PlayerPrefs.GetString(LeaderboradKey);
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
        foreach (Transform child in entriesContainer)
        {
            Destroy(child.gameObject);
        }

        if (bestLaps.Count == 0)
        {
            GameObject row = Instantiate(leaderboardRowPrefab, entriesContainer);
            LeaderboardRowUI rowUI = row.GetComponent<LeaderboardRowUI>();

            rowUI.posText.text = "-";
            rowUI.totalText.text = "No times";
            rowUI.s1Text.text = "-";
            rowUI.s2Text.text = "-";
            rowUI.s3Text.text = "-";
            return;
        }

        for (int i = 0; i < bestLaps.Count; i++)
        {
            GameObject row = Instantiate(leaderboardRowPrefab, entriesContainer);
            LeaderboardRowUI rowUI = row.GetComponent<LeaderboardRowUI>();
            rowUI.SetRow(i + 1, bestLaps[i]);
        }
    }

    private string FormatTime(float time)
    {
        int minutes = (int)time / 60;
        float seconds = time % 60;
        return string.Format("{0:00}:{1:00.00}", minutes, seconds);
    }
    // clears leaderboard data
    public void ClearCurrentLeaderboards()
    {
        bestLaps.Clear();
        PlayerPrefs.DeleteKey(LeaderboradKey);
        PlayerPrefs.Save();
        UpdateLeaderboardUI();
    }
    // return from leaderboard view back to normal view
    public void Back()
    {
        //pauseCanvas.SetActive(false);

        Time.timeScale = 1;
       
    }


    public void ShowTrack1()
    {
        ShowTrack("Track1");
    }
   

    public void ShowTrack2()
    {
        ShowTrack("Track2");
    }


    public void ShowTrack3()
    {
        ShowTrack("Track3");
    }


    public void ShowTrack(string newTrackID)
    {
        trackID = newTrackID;
        LoadLeaderboard();
        UpdateLeaderboardUI();
    }

}




