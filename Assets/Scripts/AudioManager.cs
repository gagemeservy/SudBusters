using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    [SerializeField] AudioSource musicSource;
    [SerializeField] AudioSource SFXSource;
    [SerializeField] AudioSource bubbleSource;

    public AudioSource oceanSource;       // assign in Inspector
    public AudioClip[] oceanClips;        // drop all your wave sounds here


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

    //[Header("Fizz")]
    //[SerializeField] AudioSource fizzSource;
    //[SerializeField] float fizzFadeTime = 0.5f;
    //[SerializeField] AudioClip fizzClip;
    //private Coroutine fizzFadeRoutine;

    private void Start()
    {
        GameObject[] audioPlayer = GameObject.FindGameObjectsWithTag("Audio");

        if(audioPlayer.Length > 1)
        {
            Destroy(audioPlayer[1]);
        }

        PlayRandomOceanClip();

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

    void Update()
    {
        // When the current clip finishes, play another
        if (!oceanSource.isPlaying)
        {
            PlayRandomOceanClip();
        }
    }

    void PlayRandomOceanClip()
    {
        if (oceanClips.Length == 0) return;

        // Pick a random clip that's NOT the same as the one we just played
        AudioClip nextClip;
        do
        {
            nextClip = oceanClips[Random.Range(0, oceanClips.Length)];
        } while (nextClip == oceanSource.clip && oceanClips.Length > 1);

        oceanSource.clip = nextClip;
        oceanSource.Play();
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

        // Start fizz immediately
        //StartFizz();

        while (popQueue.Count > 0)
        {
            AudioClip clip = popQueue.Dequeue();
            PlayPopSound(clip);

            yield return new WaitForSeconds(staggerDelay);
        }

        // When queue is empty, fade fizz out
        //StopFizz();

        isPlaying = false;
    }

    public void PlayPopSound(AudioClip clip)
    {
        bubbleSource.pitch = Random.Range(pitchLowEnd, pitchHighEnd);
        bubbleSource.volume = Random.Range(volumeLowEnd, volumeHighEnd);
        bubbleSource.PlayOneShot(clip);
        //bubbleSource.PlayOneShot(BubblePops[Random.Range(0, BubblePops.Length)]);
    }

    // ===== Fizz Control =====
    /*private void StartFizz()
    {
        if (!fizzSource.isPlaying)
        {
            fizzSource.clip = fizzClip;
            fizzSource.volume = 0f;
            fizzSource.Play();
            FadeFizz(1f);   // fade to full volume
        }
        else
        {
            // If already playing but faded down, fade up again
            FadeFizz(1f);
        }
    }

    private void StopFizz()
    {
        FadeFizz(0f);       // fade to silence then stop
    }

    private void FadeFizz(float targetVolume)
    {
        if (fizzFadeRoutine != null)
            StopCoroutine(fizzFadeRoutine);
        fizzFadeRoutine = StartCoroutine(FadeCoroutine(targetVolume));
    }

    private IEnumerator FadeCoroutine(float target)
    {
        float start = fizzSource.volume;
        float t = 0f;
        while (t < fizzFadeTime)
        {
            t += Time.deltaTime;
            fizzSource.volume = Mathf.Lerp(start, target, t / fizzFadeTime);
            yield return null;
        }
        fizzSource.volume = target;
        if (Mathf.Approximately(target, 0f))
            fizzSource.Stop();
    }*/
}
