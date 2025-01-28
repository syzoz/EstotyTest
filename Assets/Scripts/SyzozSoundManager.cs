using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SyzozSoundManager : SyzozRect
{
    [SerializeField] private AudioSource mainSource;
    [SerializeField] private AudioSource effectSource;
    [SerializeField] private Transform   Holder;

    [SerializeField] private statsSound BgMusic;
    [SerializeField] private statsSound[] Sounds;

    private void Start()
    {
        mainSource.clip = BgMusic.clip;
        mainSource.loop = true;
        mainSource.Play();
    }
    public void PlaySound(SoundType type)
    {
        foreach(var sound in Sounds)
        {
            if(sound.Type == type)
            {
                AudioSource source = (new GameObject(type.ToString())).AddComponent<AudioSource>();
                source.transform.SetParent(Holder);
                source.clip = sound.clip;
                source.Play();
                Destroy(source.gameObject, sound.clip.length);
            }
        }
    }
}