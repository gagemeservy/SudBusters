using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RippleTrigger : MonoBehaviour
{
    [Tooltip("Assign the Ripple Shader Material here.")]
    public Material rippleMaterial;

    // Call this when you want to trigger the ripple
    public void TriggerRipple(Vector2 screenPos)
    {
        if (rippleMaterial == null) return;

        // screenPos should be normalized (0–1) already
        rippleMaterial.SetVector("_RippleCenter", screenPos);
        rippleMaterial.SetFloat("_RippleStrength", 1.0f);
    }

    private void Update()
    {
        // Example: fade out ripple over time
        if (rippleMaterial != null)
        {
            //float current = rippleMaterial.GetFloat("_RippleStrength");
            float current = 1f;
            if (current > 0)
            {
                rippleMaterial.SetFloat("_RippleStrength", Mathf.Lerp(current, 0, Time.deltaTime * 2f));
            }
        }
    }
}
