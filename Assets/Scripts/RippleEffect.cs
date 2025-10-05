using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RippleEffect : MonoBehaviour
{
    [Header("Ripple Material")]
    public Material rippleMaterial;

    [Header("Ripple Settings")]
    public Vector2 rippleOrigin = new Vector2(0.5f, 0.5f);
    public float rippleStrength = 0f;
    public float rippleDiameter = 1f;
    public float rippleSpeed = 10f;
    public float fadeSpeed = 2f;

    private void OnRenderImage(RenderTexture src, RenderTexture dest)
    {
        rippleMaterial.SetVector("_RippleOrigin", rippleOrigin);
        rippleMaterial.SetFloat("_RippleStrength", rippleStrength);
        rippleMaterial.SetFloat("_RippleDiameter", rippleDiameter);
        rippleMaterial.SetFloat("_RippleSpeed", rippleSpeed);

        Graphics.Blit(src, dest, rippleMaterial);
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
            rippleStrength = Mathf.MoveTowards(rippleStrength, 0, Time.deltaTime * fadeSpeed);
    }
}
