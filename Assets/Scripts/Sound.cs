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

    
   public float spatialBlend =1;
   

    public bool loop;
    public bool playOnAwake;

    public AudioRolloffMode rolloffMode =AudioRolloffMode.Custom;

    [Range(0f,10f)]
    public float maxDistance;
    
    public float spread;
    public float dopplerLevel;


    [HideInInspector]
    public AudioSource source;
}
