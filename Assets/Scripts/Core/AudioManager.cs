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
    public AudioClip pickUpDoubleSlit;
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
    public AudioClip followHologram2;

    public AudioClip scene3to4VO;

    public AudioClip scene4Intro;
    public AudioClip waveLengthIntro;
    public AudioClip waveLengthExplain;
    public AudioClip distanceIntro;
    public AudioClip distanceExplain;
    public AudioClip slitseperationIntro;
    public AudioClip slitSeperationExplaine;
    public AudioClip summeryIntro;
    public AudioClip formulaExplain;
    public AudioClip formulaConslusion;
    public AudioClip beginExperiment;
    public AudioClip exploreInterefence;
    public void PlayFollowHologram2()
    {
        PlayVO(followHologram2);
    }

    public void PlayExploreInterefence()
    {
        PlayVO(exploreInterefence);
    }
    public void Playscene3to4VO()
    {
        PlayVO(scene3to4VO);
    }
    public void PlayFormulaConslusion()
    {
        PlayVO(formulaConslusion);
    }
    public void PlayPickUpDoubleSlit()
    {
        PlayVO(pickUpDoubleSlit);
    }
    public void Playscene4Intro()
    {
        PlayVO(scene4Intro);
    }
    public void PlaywaveLengthIntro()
    {
        PlayVO(waveLengthIntro);
    }
    public void PlaywaveLengthExplain()
    {
        PlayVO(waveLengthExplain);
    }
    public void PlaydistanceIntro()
    {
        PlayVO(distanceIntro);
    }
    public void PlaydistanceExplain()
    {
        PlayVO(distanceExplain);
    }
    public void PlayslitseperationIntro()
    {
        PlayVO(slitseperationIntro);
    }
    public void PlayslitSeperationExplaine()
    {
        PlayVO(slitSeperationExplaine);
    }
    public void PlaysummeryIntro()
    {
        PlayVO(summeryIntro);
    }
    public void PlayFormulaExplain()
    {
        PlayVO(formulaExplain);
    }
    public void PlayBeginExperiment()
    {
        PlayVO(beginExperiment);
    }


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
