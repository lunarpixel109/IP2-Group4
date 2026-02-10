using UnityEngine;

public class AudioManagerDummyScript : MonoBehaviour
{
    


    // used to make sure audio files where being played correctly
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.W))
        {
            AudioManager.PlaySound(Sound_types.ACCELERATING, 0);
        } else if (Input.GetKeyDown(KeyCode.S))
        {
            AudioManager.PlaySound(Sound_types.CRASHING, 0);
        } else if (Input.GetKeyDown(KeyCode.Space))
        {
            AudioManager.PlaySound(Sound_types.POWERUP, 0);
        }
        else if (Input.GetKeyDown(KeyCode.X))
        {
            AudioManager.PlayRandomSound(Sound_types.CRASHING);
        }
    }
}
