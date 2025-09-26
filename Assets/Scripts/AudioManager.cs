using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    [SerializeField] AudioSource musicSource;
    [SerializeField] AudioSource SFXSource;
    [SerializeField] AudioSource bubbleSource;
    

    public AudioClip[] BubblePops;
    public AudioClip ButtonHighlight;
    public AudioClip ButtonClicked;
    public AudioClip Music;
    public AudioClip RotateSound;
    public AudioClip MoveSound;
    public AudioClip BlockLock;

    private Queue<AudioClip> popQueue = new Queue<AudioClip>();
    private bool isPlaying = false;
    public float staggerDelay = 1f;
    public float pitchLowEnd = 0.8f;
    public float pitchHighEnd = 1.9f;
    public float volumeLowEnd = 0.7f;
    public float volumeHighEnd = 1.0f;

    private void Start()
    {
        GameObject[] audioPlayer = GameObject.FindGameObjectsWithTag("Audio");

        if(audioPlayer.Length > 1)
        {
            Destroy(audioPlayer[1]);
        }

        musicSource.clip = Music;
        

        if (!musicSource.isPlaying)
        {
            musicSource.Play();
            GameObject.DontDestroyOnLoad(audioPlayer[0]);
        }

        //set ambience here too somehow
        //ambienceSource.clip = SFXSource;
        /*if (!ambienceSource.isPlaying)
        {
            ambienceSource.volume = 0f;
            ambienceSource.Play();
            ambienceSource.DOFade(0.2f, 0.5f); // fade in to gentle 20% volume
        }*/

    }

    public void PlaySFX(AudioClip clip)
    {
        SFXSource.PlayOneShot(clip);
    }

    // Call this for each bubble
    public void EnqueuePop()
    {
        popQueue.Enqueue(BubblePops[Random.Range(0, BubblePops.Length)]);
        if (!isPlaying)
        {
            StartCoroutine(ProcessQueue());
        }
    }

    private IEnumerator<System.Object> ProcessQueue()
    {
        isPlaying = true;

        while (popQueue.Count > 0)
        {
            AudioClip clip = popQueue.Dequeue();
            PlayPopSound(clip);

            yield return new WaitForSeconds(staggerDelay);
        }

        isPlaying = false;
    }

    public void PlayPopSound(AudioClip clip)
    {
        bubbleSource.pitch = Random.Range(pitchLowEnd, pitchHighEnd);
        bubbleSource.volume = Random.Range(volumeLowEnd, volumeHighEnd);
        bubbleSource.PlayOneShot(clip);
        //bubbleSource.PlayOneShot(BubblePops[Random.Range(0, BubblePops.Length)]);
    }
}
