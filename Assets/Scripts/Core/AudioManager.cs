using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance;

    [Header("Audio Source")]
    public AudioSource voiceSource;

    [Header("Scene 1 Voice Overs")]
    public AudioClip introClip;
    public AudioClip natureOfLightClip;
    public AudioClip lighterClick;
    public AudioClip rotateObj;
    public AudioClip metalClick;
    public AudioClip lampLightSource;
    public AudioClip placeDoubleSlit;
    public AudioClip interferencePattern;
    public AudioClip labTransition;
    public AudioClip LightAsWave;
    public AudioClip TwoSlitsTwoWaves;
    public AudioClip InterferenceIntro;
    public AudioClip constructiveInterference;
    public AudioClip destructiveInterference;
    public AudioClip followHologram;





    void Awake()
    {
        // Singleton
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);
    }

    // =========================
    // SCENE 1 FUNCTIONS
    // =========================

    public void PlayIntro()
    {
        PlayVO(introClip);
    }

    public void PlayNatureOfLight()
    {
        PlayVO(natureOfLightClip);
    }

    public void PlaylighterClick()
    {
        PlayVO(lighterClick);
    }
    public void PlayRotateTheObj()
    {
        PlayVO(rotateObj);
    }
    public void PlayMetalClick()
    {
        PlayVO(metalClick);
    }
    public void PlayTurnOnTheLamp()
    {
        PlayVO(lampLightSource);
    }
    public void PlayInsertTheSlit()
    {
        PlayVO(placeDoubleSlit);
    }
    public void PlayObserveThePattern()
    {
        PlayVO(interferencePattern);
    }
    public void PlayLabTransition()
    {
        PlayVO(labTransition);
    }
    public void PlayLightAsWave()
    {
        PlayVO(LightAsWave);
    }
    public void PlayTwoSlitsTwoWaves()
    {
        PlayVO(TwoSlitsTwoWaves);
    }
    public void PlayWaveInterferenceIntro()
    {
        PlayVO(InterferenceIntro);
    }
    public void PlayConstructiveInterference()
    {
        PlayVO(constructiveInterference);
    }
    public void PlayDestructiveInterference()
    {
        PlayVO(destructiveInterference);
    }
    public void PlayFollowHologram()
    {
        PlayVO(followHologram);
    }



    // =========================
    // CORE VO PLAYER
    // =========================

    void PlayVO(AudioClip clip)
    {
        if (!clip || !voiceSource) return;

        voiceSource.Stop();
        voiceSource.clip = clip;
        voiceSource.Play();
    }

    public void StopVO()
    {
        if (!voiceSource) return;
        voiceSource.Stop();
    }

    public bool IsPlaying()
    {
        return voiceSource && voiceSource.isPlaying;
    }
}
