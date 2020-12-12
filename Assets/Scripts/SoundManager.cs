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
    public AudioClip[] dragonSoundList;
    public AudioClip[] mageSoundList;
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
    public void ArcherSoundPlay(int num1, int num2)
    {
        audioSource.clip = archerSoundList[num1];
        audioSource.Play();
        audioSource.clip = archerSoundList[num2];
        audioSource.Play();
    }

    public void DragoonSoundPlay(int num)
    {
        audioSource.clip = dragoonSoundList[num];
        audioSource.Play();
    }
    public void DragonSoundPlay(int num)
    {
        audioSource.clip = dragonSoundList[num];
        audioSource.Play();
    }
    public void MageSoundPlay(int num)
    {
        audioSource.clip = mageSoundList[num];
        audioSource.Play();
    }
    public void HitSoundPlay(int num)
    {
        audioSource.clip = hitSoundList[num];
        audioSource.Play();
    }
    public void FootstepsSoundPlay(int num)
    {
        audioSource.clip = footStepsSoundList[num];
        audioSource.Play();
    }
   
}
