using UnityEngine;

public class SoundManager : MonoBehaviour
{
    public AudioSource efxSource;                    //Drag a reference to the audio source which will play the sound effects.
    public AudioSource musicSource;                    //Drag a reference to the audio source which will play the music.
    public static SoundManager instance = null;        //Allows other scripts to call functions from SoundManager.                
    public float lowPitchRange = .95f;                //The lowest a sound effect will be randomly pitched.
    public float highPitchRange = 1.05f;            //The highest a sound effect will be randomly pitched.
    [SerializeField] private AudioClip coinSound;
    [SerializeField] private AudioClip hitSound;

    void Awake()
    {
        //Check if there is already an instance of SoundManager
        if (instance == null)
            //if not, set it to this.
            instance = this;
        //If instance already exists:
        else if (instance != this)
            //Destroy this, this enforces our singleton pattern so there can only be one instance of SoundManager.
            Destroy(gameObject);

        //Set SoundManager to DontDestroyOnLoad so that it won't be destroyed when reloading our scene.
        DontDestroyOnLoad(gameObject);
    }
    public float EffectSoundVolume
    {
        get
        {
            return PlayerPrefs.GetFloat("EffectSoundVolume", 1);
        }
        set
        {
            if(value <=1 && value >=0)
            {
                PlayerPrefs.SetFloat("EffectSoundVolume", value);
                PlayerPrefs.Save();
            }
            else
            {
                PlayerPrefs.SetFloat("EffectSoundVolume", 1);
                PlayerPrefs.Save();
            }
        }
    }
    public float MusicSoundVolume
    {
        get
        {
            return PlayerPrefs.GetFloat("MusicSoundVolume", 1);
        }
        set
        {
            if (value <= 1 && value >= 0)
            {
                PlayerPrefs.SetFloat("MusicSoundVolume", value);
                PlayerPrefs.Save();
            }
            else
            {
                PlayerPrefs.SetFloat("MusicSoundVolume", 1);
                PlayerPrefs.Save();
            }
        }
    }

    //Used to play single sound clips.
    public void PlaySingleEffect(AudioClip clip)
    {
        //Set the clip of our efxSource audio source to the clip passed in as a parameter.
        efxSource.clip = clip;

        // set volume
        efxSource.volume = EffectSoundVolume;

        //Play the clip.
        efxSource.Play();
    }


    //RandomizeSfx chooses randomly between various audio clips and slightly changes their pitch.
    public void RandomizeSfxEffect(params AudioClip[] clips)
    {
        //Generate a random number between 0 and the length of our array of clips passed in.
        int randomIndex = Random.Range(0, clips.Length);

        //Choose a random pitch to play back our clip at between our high and low pitch ranges.
        float randomPitch = Random.Range(lowPitchRange, highPitchRange);

        //Set the pitch of the audio source to the randomly chosen pitch.
        efxSource.pitch = randomPitch;

        //Set the clip to the clip at our randomly chosen index.
        efxSource.clip = clips[randomIndex];

        //Play the clip.
        efxSource.Play();
    }


    public void SoundOnEatCoin()
    {
        PlaySingleEffect(coinSound);
    }

    public void SoundOnPlayerGetHit()
    {
        PlaySingleEffect(hitSound);
    }
}