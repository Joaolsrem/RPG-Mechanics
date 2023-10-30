using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.PostProcessing;

public class EfeitosDeCamera : MonoBehaviour
{
    public PostProcessVolume postProcessing;

    // EFEITOS DE CAMERA
    public PlayerMovement p1;

    [Header("Efeito morreu")]
    public Vignette vignette;

    void Start()
    {
        vignette = postProcessing.profile.GetSetting<Vignette>();
        vignette.intensity.value = 0;
    }

    
    void Update()
    {
        if (p1.estaMorto)
        {
            vignette.intensity.value = 0.4f;
            vignette.color.value = Color.red;
        }
    }
}
