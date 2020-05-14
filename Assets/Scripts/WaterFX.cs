using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.PostProcessing;

[ExecuteInEditMode, ImageEffectAllowedInSceneView]
public class WaterFX : MonoBehaviour {
    public Material material;

    void Start() {
        Debug.Log("begin post process");
    }

    void Update() {
        Debug.Log(" post process");
    }

    private void OnRenderImage(RenderTexture source, RenderTexture destination) {
        Graphics.Blit(source, destination, material);
    }
}
