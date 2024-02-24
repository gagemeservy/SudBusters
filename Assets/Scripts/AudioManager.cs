using Unity.VisualScripting;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    [SerializeField] AudioSource musicSource;
    [SerializeField] AudioSource SFXSource;

    public AudioClip[] BubblePops;
    public AudioClip ButtonHighlight;
    public AudioClip ButtonClicked;
    public AudioClip Music;
    public AudioClip RotateSound;
    public AudioClip MoveSound;
    public AudioClip BlockLock;


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
    }

    public void PlaySFX(AudioClip clip)
    {
        SFXSource.PlayOneShot(clip);
    }
}
