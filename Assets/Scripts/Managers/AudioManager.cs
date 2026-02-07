using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Rendering;
using System;

[Serializable]
public struct Sound_list // used to create multi dimensional arrays in the inspector to allow for ease of use
{
    public AudioClip[] Sounds { get => sounds; }
    [HideInInspector] public string name;
    [SerializeField] AudioClip[] sounds;
}

public enum Sound_types // used to store each type of audio case, automatically adds type section to inspector 
{
    ACCELERATING,
    CRASHING,
    POWERUP
}

[RequireComponent(typeof(AudioSource)), ExecuteInEditMode] // manager object must have an audio source
public class AudioManager : MonoBehaviour
{
    [SerializeField] private Sound_list[] sound_list; // stores audioclip file refrences in inspector
    private static AudioManager instance;
    private AudioSource audio_source;
    

    private void Awake()
    {
        instance = this;
    }

    private void Start()
    {
        audio_source = GetComponent<AudioSource>();
    }

    public static void PlaySound(Sound_types sound, int index, float volume = 1) // used to play a specific sound
    {
        // use AudioManager.PlaySound(Sound_types.enum_ref, array_index); to play a specific sound and type
        AudioClip[] clips = instance.sound_list[(int)sound].Sounds;
        AudioClip clip_to_play = clips[index];
       instance.audio_source.PlayOneShot(clip_to_play, volume);
    }

    public static void PlayRandomSound(Sound_types sound, float volume = 1) // used to play a random sound of a specific sound type
    {
        // use AudioManager.PlayRandomSound(Sound_types.enum_ref); to play a random souund of a specific type
        AudioClip[] clips = instance.sound_list[(int)sound].Sounds;
        AudioClip clip_to_play = clips[UnityEngine.Random.Range(0, clips.Length)];
        instance.audio_source.PlayOneShot(clip_to_play, volume);
    }

#if UNITY_EDITOR // used to set the index name's in the inspector to the enum refrences
    private void OnEnable()
    {
        string[] index_names = Enum.GetNames(typeof(Sound_types)); // gets the names of all the index's in the enum and stores them
        Array.Resize(ref sound_list, index_names.Length); // sets the sound_list array size to the maount of indexs in the 

        for (int i = 0; i < sound_list.Length; i++) // for every index in the array
        {
            sound_list[i].name = index_names[i]; // set the array name to the enum index refrence
        }
    }
#endif
}


