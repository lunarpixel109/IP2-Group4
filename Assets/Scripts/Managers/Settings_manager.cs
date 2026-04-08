using UnityEditor;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Settings_manager : MonoBehaviour
{
    public static Settings_manager instance;
    public AudioMixer main_mixer;
    public Slider vol_Slider;
    public GenericFloatSO volumeSO;
    public Button button;
    private void Awake()
    {
        instance = this;
        
    }

    private void Start()
    {
        button.onClick.AddListener(Back);
        vol_Slider.Select();
        float volume = PlayerPrefs.GetFloat("Volume", 0f);
    }

    public void SetVolume(float volume)
    {
        PlayerPrefs.SetFloat("Volume", volume);
        main_mixer.SetFloat("Master Volume", volume);
    }

    void Back()
    {
        FindFirstObjectByType<MainMenu>()?.EnterButtons();
        SceneManager.UnloadSceneAsync("settings menu");
         
    }
    
    
}
