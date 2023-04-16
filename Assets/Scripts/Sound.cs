using UnityEngine.Audio;
using UnityEngine;


// [System.Serializable]
[System.Serializable]
public class Sound
{

    public string name;
    public AudioClip clip;

    [Range(0f, 5f)]
    public float volume;
    [Range(0f, 5f)]
    public float pitch;


    public float spatialBlend = 1;


    public bool loop;
    //public bool playOnAwake;

    public AudioRolloffMode rolloffMode;

    [Range(0f, 10f)]
    public float maxDistance;

    public float spread;
    public float dopplerLevel;


    [HideInInspector]
    public AudioSource source;

    public void changeChannel(AudioSource channel)
    {
        this.source = channel;
    }

    //public void initialize(AudioSource audioSource)
    //{
    //    source = audioSource;
    //}

    //private void Start()
    //{
    //    source = new AudioSource();
    //    source.clip = clip;
    //    source.playOnAwake = false;
    //}

    //void Awake()
    //{
    //    source = new AudioSource();
    //    source.clip = clip;
    //    source.volume = volume;
    //    source.pitch = pitch;
    //    source.loop = loop;
    //    source.playOnAwake = playOnAwake;
    //    source.spatialBlend = spatialBlend;
    //    source.rolloffMode = rolloffMode;
    //    source.maxDistance = maxDistance;
    //    source.spread = spread;
    //    source.dopplerLevel = dopplerLevel;
    //}

    public void setDefault()
    {
        source.clip = this.clip;
        source.volume = this.volume;
        source.pitch = this.pitch;
        source.loop = this.loop;
        source.spatialBlend = this.spatialBlend;
        source.rolloffMode = this.rolloffMode;
        source.maxDistance = this.maxDistance;
        source.spread = this.spread;
        source.dopplerLevel = this.dopplerLevel;
    }

    public void setCustom(float volume = 0.5f, float pitch = 1f,
                 float spatialBlend = 1f, bool loop = true, AudioRolloffMode rolloffMode = AudioRolloffMode.Custom,
                 float maxDistance = 0f, float spread = 0f, float dopplerLevel = 0f)
    {
        {
            source.volume = volume;
            source.pitch = pitch;
            source.loop = loop;
            source.spatialBlend = spatialBlend;
            source.rolloffMode = rolloffMode;
            source.maxDistance = maxDistance;
            source.spread = spread;
            source.dopplerLevel = dopplerLevel;
            source.clip = this.clip;
        }
    }
}
