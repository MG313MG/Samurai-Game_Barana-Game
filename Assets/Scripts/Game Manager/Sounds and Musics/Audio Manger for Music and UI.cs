using UnityEngine;

public class AudioManger : MonoBehaviour
{
    public static AudioManger Play_Audio;

    public AudioSource UI_Source;
    public AudioSource Music_Source;

    [Header("UI Sounds")]
    public AudioClip Click_On_Buttons;
    [Header("Music_Source Sounds")]
    public AudioClip Win_Sound;
    public AudioClip Lose_Sound;
    public AudioClip BG_Music_Sound;
    public AudioClip Pause_Sound;

    void Start()
    {
        if (Music_Source != null)
        {
            Music_Source.clip = BG_Music_Sound;
            Music_Source.loop = true;
            Music_Source.Play();
        }
    }
}
