using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    private static SoundManager instance;
    AudioSource audioSource;
    public AudioClip[] footStepsSoundList;
    public AudioClip[] knightSoundList;
    public AudioClip[] archerSoundList;
    public AudioClip[] dragoonSoundList;
    public AudioClip[] dragonSounList;
    public AudioClip[] hitSoundList;
    public static SoundManager Instance
    {
        get
        {
            if (instance == null)
            {
                var obj = FindObjectOfType<SoundManager>();

                if (obj != null)
                {
                    instance = obj;
                }

                else
                {
                    var newSingleton = new GameObject("Singleton Class").AddComponent<SoundManager>();

                    instance = newSingleton;
                }
            }
            return instance;
        }

        set
        {
            instance = value;
        }
    }


    private void Awake()
    {

        var objs = FindObjectsOfType<SoundManager>();

        if (objs.Length != 1)
        {
            Destroy(gameObject);

            return;
        }
        //DontDestroyOnLoad(gameObject);
    }
    private void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }

    public void KnightSoundPlay(int num)
    {
        audioSource.clip = knightSoundList[num];
        audioSource.Play();
    }
    public void ArcherSoundPlay(int num)
    {
        audioSource.clip = archerSoundList[num];
        audioSource.Play();
    }

    public void DragoonSoundPlay(int num)
    {
        audioSource.clip = dragoonSoundList[num];
        audioSource.Play();
    }
}
