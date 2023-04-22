    using UnityEngine.Audio;
using UnityEngine;
using System;
using System.Collections.Generic;

public class LocalAudioManager : MonoBehaviour
{
    public AudioManager manager;
    public AudioSource channel1;
    public AudioSource channel2;
    public AudioSource channel3;

    public Dictionary<int, AudioSource> channelMap;


    private void Start()
    {
        manager = GameObject.Find("AudioManager").GetComponent<AudioManager>();
    }
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

    //public void Play (string name, int channel)
    // {
    //    defineChannelMap();
    //    Sound s = findSound(name);

    //    if (s != null)
    //    {
    //        s.changeChannel(channelMap[channel]);
    //        s.setDefault();
    //        s.source.Play();
    //    }
    //    else
    //    {
    //        Debug.Log("LocalAudioManager Error: Play (Default values): Null sound played (Sound not found)");
    //    }
    // }

    public void Play(string name, int channel, float volume = 0.3f, float pitch = 1f,
                     float spatialBlend = 1f, bool loop = true, AudioRolloffMode rollOffMode = AudioRolloffMode.Linear,
                     float maxDistance = 20f, float spread = 0f, float dopplerLevel = 1f)
    {
        defineChannelMap();
        Sound s = findSound(name);


        if (name == "NoiseTrapSound" || name == "NPCDeath" || name == "FinishSound")
        {
            volume = 0.1f;
        }

        if (s != null)
        {
            s.changeChannel(channelMap[channel]);
            s.setCustom(volume, pitch, spatialBlend, loop, rollOffMode, maxDistance, spread, dopplerLevel);
            s.source.Play();
        }
        else
        {
            Debug.Log("LocalAudioManager Error: Play (Custom values): Sound name " + name + " is null");
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
        return manager.findSound(name);
    }
}
