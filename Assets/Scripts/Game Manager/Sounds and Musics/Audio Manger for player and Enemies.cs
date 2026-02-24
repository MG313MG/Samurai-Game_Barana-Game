using System.Collections;
using UnityEngine;

public enum Sounds {jump, evosion, attak_by_sword_1, attak_by_sword_2, attak_by_sword_3, load_bow, attak_by_bow, defend, hit, dead }

public class Audio_Manger_for_player_and_Enemies : MonoBehaviour
{
    public AudioSource audio_source;

    [Header("Sounds")]
    public AudioClip Jump;
    public AudioClip Evosion;
    public AudioClip Attak_by_Sword_1;
    public AudioClip Attak_by_Sword_2;
    public AudioClip Attak_by_Sword_3;
    public AudioClip Attak_by_Bow;
    public AudioClip Load_Bow;
    public AudioClip Defend;
    public AudioClip hit;
    public AudioClip dead;
    [Space(15)]
    [Range(0f, 1f)]
    public float Valume;

    private bool Hurt_sound_palyed;
    private bool Dead_sound_palyed;


    private void Start()
    {
        audio_source.volume = Valume;
    }

    // این متد رو از Animation Event صدا بزن (با پارامتر string)
    public void PlaySound(string soundName)
    {
        // تبدیل string به enum
        if (System.Enum.TryParse(soundName, true, out Sounds sound))
        {
            PlaySound(sound); // صدا زدن متد enum
        }
        else
        {
            Debug.LogWarning($"صدای {soundName} پیدا نشد!");
        }
    }

    // متد اصلی با enum
    private void PlaySound(Sounds sound)
    {
        switch (sound)
        {
            case Sounds.jump:
                audio_source.clip = Jump;
                audio_source.Play();
                break;
            case Sounds.evosion:
                audio_source.PlayOneShot(Evosion);
                break;
            case Sounds.attak_by_sword_1:
                audio_source.PlayOneShot(Attak_by_Sword_1);
                break;
            case Sounds.attak_by_sword_2:
                audio_source.PlayOneShot(Attak_by_Sword_2);
                break;
            case Sounds.attak_by_sword_3:
                audio_source.PlayOneShot(Attak_by_Sword_3);
                break;
            case Sounds.attak_by_bow:
                audio_source.PlayOneShot(Attak_by_Bow);
                break;
            case Sounds.load_bow:
                audio_source.PlayOneShot(Load_Bow);
                break;
            case Sounds.defend:
                audio_source.PlayOneShot(Defend);
                break;
            case Sounds.hit:
                if (!Hurt_sound_palyed)
                    audio_source.PlayOneShot(hit);
                Hurt_sound_palyed = true;
                StartCoroutine(Hurt_sound_played_Set_True());
                break;
            case Sounds.dead:
                if (!Dead_sound_palyed)
                    audio_source.PlayOneShot(dead);
                Dead_sound_palyed = true;
                break;
        }
    }
    public void Stop_Sounds()
    {
        audio_source.Stop();
    }
    private IEnumerator Hurt_sound_played_Set_True()
    {
        yield return new WaitForSeconds(2);
        Hurt_sound_palyed = false;
    }
}