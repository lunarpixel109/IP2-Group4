using TMPro;
using UnityEngine;

public class LeaderboardRowUI : MonoBehaviour
{
    public TextMeshProUGUI posText;
    public TextMeshProUGUI totalText;
    public TextMeshProUGUI s1Text;
    public TextMeshProUGUI s2Text;
    public TextMeshProUGUI s3Text;

    public void SetRow(int position, LapData lap)
    {
        posText.text = position.ToString();
        totalText.text = FormatTime(lap.totalTime);
        s1Text.text = FormatTime(lap.sectorTimes[0]);
        s2Text.text = FormatTime(lap.sectorTimes[1]);
        s3Text.text = FormatTime(lap.sectorTimes[2]);
    }

    private string FormatTime(float time)
    {
        int minutes = (int)time / 60;
        float seconds = time % 60;
        return time.ToString("0.00");
    }
}