using UnityEngine.Audio;
using UnityEngine;
using System;

public class AudioManager : MonoBehaviour
{
    public Sound[] sounds;
    
    public static AudioManager instance;
    
     void Awake()
     {

            if (instance==null)
            {
                instance=this;
            }

    
        foreach (Sound s in sounds)
        {
            s.source = gameObject.AddComponent<AudioSource>();
            s.source.clip = s.clip;
            s.source.volume = s.volume;
            s.source.pitch = s.pitch;
            s.source.loop = s.loop;
            s.source.playOnAwake = s.playOnAwake;
        }
     }

    void Start()
    {
         DontDestroyOnLoad(gameObject);
        Play("BackgroundMusic");
    }

    public void Play (string name)
     {
            Sound s = null;
        foreach (Sound sound in sounds) {
            if (sound.name == name) {
                s = sound;
                break;
            }
        }

        if (s!=null)
        {
        s.source.Play();
        }

        else
        {
            print("Sound Not Found");
        }

        if(s.name != "PlayerFootSteps")
        {
        print("Sound Played");
        print(s.name);
        }
     }

    
    public void Stop (string name)
     {
            Sound s = null;
        foreach (Sound sound in sounds) {
            if (sound.name == name) {
                s = sound;
                break;
            }
        }

        if (s!=null)
        {
        s.source.Stop();
        }

        else
        {
            print("Sound Not Found");
        }


     }
}
