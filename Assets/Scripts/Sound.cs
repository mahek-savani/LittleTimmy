using UnityEngine.Audio;
using UnityEngine;


// [System.Serializable]
[System.Serializable]
public class Sound 
{
    
   public string name;
   public AudioClip clip;

    [Range(0f,5f)]
   public float volume;
   [Range(0f,5f)]
   public float pitch;
   

    public bool loop;
    public bool playOnAwake;

    [HideInInspector]
    public AudioSource source;
}
