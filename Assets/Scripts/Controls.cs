using System;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Controls : MonoBehaviour
{
    private AudioManager audioPlayer;

    private void Awake()
    {
        this.audioPlayer = GameObject.FindGameObjectWithTag("Audio").GetComponent<AudioManager>();
    }
    
    public void Back()
    {
        ButtonPressSound();
        StartCoroutine(LoadAsyncScene("MainMenu"));
    }

    private void ButtonPressSound()
    {
        audioPlayer.PlaySFX(audioPlayer.ButtonClicked);
    }

    IEnumerator LoadAsyncScene(String sceneToSwitchTo)
    {
        // The Application loads the Scene in the background as the current Scene runs.
        // This is particularly good for creating loading screens.
        // You could also load the Scene by using sceneBuildIndex. In this case Scene2 has
        // a sceneBuildIndex of 1 as shown in Build Settings.

        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(sceneToSwitchTo);

        // Wait until the asynchronous scene fully loads
        while (!asyncLoad.isDone)
        {
            yield return null;
        }
    }
}
