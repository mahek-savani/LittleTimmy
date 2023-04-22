using System.Collections.Generic;
using UnityEditor.Build.Content;
using UnityEngine;
using UnityEngine.SceneManagement;

public class AudioManager : MonoBehaviour
{
    public Sound[] sounds;
    public AudioSource channel1;
    public AudioSource channel2;
    public AudioSource channel3;

    private Dictionary<int, AudioSource> channelMap;

    private static AudioManager instance;

    private void defineChannelMap()
    {
        if (channelMap != null)
        {
            return;
        }
        else
        {
            channelMap = new Dictionary<int, AudioSource> { { 1, channel1 }, { 2, channel2 }, { 3, channel3 } };
        }
    }

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            if (SceneManager.GetActiveScene().buildIndex == 0)
            {
                instance.Play("Title Music", 3);
            }
            else if (instance.channel3.clip.name == "Title Screen Music")
            {
                instance.Play("New Background Music", 3);
            }
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        if (SceneManager.GetActiveScene().buildIndex == 0)
        {
            Play("Title Music", 3);
        }
        else
        {
            Play("New Background Music", 3);
        }
    }

    //void Awake()
    //{

    //       if (instance==null)
    //       {
    //           instance=this;
    //       }


    //   foreach (Sound s in sounds)
    //   {
    //       s.source = gameObject.AddComponent<AudioSource>();
    //       s.source.clip = s.clip;
    //       s.source.volume = s.volume;
    //       s.source.pitch = s.pitch;
    //       s.source.loop = s.loop;
    //       s.source.playOnAwake = s.playOnAwake;
    //       s.source.spatialBlend = s.spatialBlend;
    //       s.source.rolloffMode = s.rolloffMode;
    //       s.source.maxDistance = s.maxDistance;
    //       s.source.spread = s.spread;
    //       s.source.dopplerLevel = s.dopplerLevel;


    //   }
    //}

    //void Start()
    //{
    //    DontDestroyOnLoad(gameObject);
    //    Play("BackgroundMusic");
    //}

    //private void Awake()
    //{
    //    foreach (Sound s in sounds)
    //    {
    //        s.initialize(channel1);
    //    }
    //}

    public void Play (string name, int channel)
     {
        defineChannelMap();
        Sound s = findSound(name);

        if (s != null)
        {
            s.changeChannel(channelMap[channel]);
            s.setDefault();
            s.source.Play();
        }
        else
        {
            Debug.Log("AudioManager Error: Play (Default values): Null sound played (Sound not found)");
        }

        //if(s.name != "PlayerFootSteps")
        //{
        //print("Sound Played");
        //print(s.name);
        //}
     }

    public void Play(string name, int channel, float volume = 0.5f, float pitch = 1f,
                     float spatialBlend = 0f, bool loop = true, AudioRolloffMode rollOffMode = AudioRolloffMode.Custom,
                     float maxDistance = Mathf.Infinity, float spread = 0f, float dopplerLevel = 0f)
    {
        defineChannelMap();
        Sound s = findSound(name);

        if (s != null)
        {
            s.changeChannel(channelMap[channel]);
            s.setCustom(volume, pitch, spatialBlend, loop, rollOffMode, maxDistance, spread, dopplerLevel);
            s.source.Play();
        }
        else
        {
            Debug.Log("AudioManager Error: Play (Custom values): Null sound played (Sound not found)");
        }

        //if(s.name != "PlayerFootSteps")
        //{
        //print("Sound Played");
        //print(s.name);
        //}
    }


    public void Stop (int channel)
     {
        channelMap[channel].Stop();

     }

    public void StopAll ()
    {
        channel1.Stop();
        channel2.Stop();
        channel3.Stop();
    }

    public Sound findSound(string name)
    {
        Sound s = null;
        foreach (Sound sound in sounds)
        {
            if (sound.name == name)
            {
                s = sound;
                break;
            }
        }

        return s;
    }
}
