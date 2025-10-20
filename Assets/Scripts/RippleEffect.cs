using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Video;

[System.Serializable]
public struct Ripple
{
    public Vector2 origin;
    public float strength;
    public float diameter;
    public float speed;
    public float time;
}

public class RippleEffect : MonoBehaviour
{
    [Header("Ripple Material")]
    public Material rippleMaterial;

    [Header("Ripple Settings")]
    public Vector2 rippleOrigin = new Vector2(0.5f, 0.5f);
    public VideoPlayer backgroundVideo; // assign in Inspector
    public float rippleStrength = 0f;
    public float rippleDiameter = 1f;
    public float rippleSpeed = 10f;
    public float fadeSpeed = 2f;

    private bool isFadingOut = false;
    private static bool hasPlayedOnce = false;
    public bool isMainMenu = false;

    private void Start()
    {   //this makes sure the main menu ripple doesn't happen twice and screw up everything
        if (isMainMenu)
        {
            // Only run ripple logic the first time
            if (hasPlayedOnce)
        {
            rippleStrength = 0f;
            rippleMaterial.SetFloat("_RippleStrength", 0f);
            return;
        }

        hasPlayedOnce = true;

        if (backgroundVideo != null)
        {
            backgroundVideo.prepareCompleted += OnVideoReady;
        }
        else
        {
            Debug.LogWarning("No VideoPlayer assigned to RippleEffect.");
        }
        }
        else
        {
            // For non-main-menu scenes, don’t affect hasPlayedOnce
            if (backgroundVideo != null)
            {
                backgroundVideo.prepareCompleted += OnVideoReady;
            }
        }
    }

    private void OnRenderImage(RenderTexture src, RenderTexture dest)
    {
        rippleMaterial.SetVector("_RippleOrigin", rippleOrigin);
        rippleMaterial.SetFloat("_RippleStrength", rippleStrength);
        rippleMaterial.SetFloat("_RippleDiameter", rippleDiameter);
        rippleMaterial.SetFloat("_RippleSpeed", rippleSpeed);

        Graphics.Blit(src, dest, rippleMaterial);
    }

    private void OnVideoReady(VideoPlayer source)
    {
        // Fade out ripple once video is ready to play
        StartCoroutine(FadeOutRipple());
    }

    private IEnumerator FadeOutRipple()
    {
        if (isFadingOut) yield break;
        isFadingOut = true;

        float duration = 1.5f;
        float startStrength = rippleStrength;
        float t = 0f;

        while (t < duration)
        {
            rippleStrength = Mathf.Lerp(startStrength, 0f, t / duration);
            rippleMaterial.SetFloat("_RippleStrength", rippleStrength);
            t += Time.deltaTime;
            yield return null;
        }

        rippleStrength = 0f;
        rippleMaterial.SetFloat("_RippleStrength", 0f);
    }

    // --- Trigger the ripple effect ---
    public void TriggerRipple(Vector2 origin, float strength, float diameter = 1f, float speed = 10f)
    {
        rippleOrigin = origin;
        rippleStrength = strength;
        rippleDiameter = diameter;
        rippleSpeed = speed;

        Debug.Log($"Ripple triggered at {origin} (strength {strength}, diameter {diameter}, speed {speed})");
    }

    private void Update()
    {
        if (rippleStrength > 0)
        {
            rippleStrength = Mathf.MoveTowards(rippleStrength, 0, Time.deltaTime * fadeSpeed);
        }
    }
}
