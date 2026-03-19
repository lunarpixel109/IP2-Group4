using UnityEngine;

public class LapData
{
    public float totalTime;
    public float[] sectorTimes;

    
    public LapData(int sectorCount)
    {
        sectorTimes = new float[sectorCount];
    }
    
    
}
