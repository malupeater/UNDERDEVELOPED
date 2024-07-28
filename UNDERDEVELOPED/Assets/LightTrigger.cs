using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.Universal;

public class LightTrigger : MonoBehaviour
{
    [SerializeField]
    Light2D playerLight;
    
    [SerializeField]
    float innerRadius, outerRadius, fallOff;

    void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            playerLight.pointLightInnerRadius = innerRadius;
            playerLight.pointLightOuterRadius = outerRadius;
            playerLight.falloffIntensity = fallOff;
        }    
    }
}
