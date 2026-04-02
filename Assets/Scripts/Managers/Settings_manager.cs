using UnityEditor;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class Settings_manager : MonoBehaviour
{
    public static Settings_manager instance;
    public AudioMixer main_mixer;
    public Slider vol_Slider;
    public GenericFloatSO volumeSO;

    private void Awake()
    {
        instance = this;
        
    }

    private void Start()
    {
        vol_Slider.value = volumeSO.volume;
        SetVolume(vol_Slider.value);
    }

    public void SetVolume(float volume)
    {
        main_mixer.SetFloat("Master Volume", volume);
        volumeSO.volume = volume;
        EditorUtility.SetDirty(volumeSO);
    }

}
