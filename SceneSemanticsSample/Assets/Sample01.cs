/*
    Created by Takashi Yoshinaga
    Copyright (C) 2023 Takashi Yoshinaga. All Rights Reserved.
*/

using System.Collections;
using System.Collections.Generic;
using Google.XR.ARCoreExtensions;
using UnityEngine;
using UnityEngine.UI;

public class Sample01 : MonoBehaviour
{
    [SerializeField]
    ARSemanticManager _semanticManager;
    [SerializeField]
    Text _debugText;
    [SerializeField]
    bool _showDebugText=true;
    bool _isSemanticModeSupported = false;
    [SerializeField]
    int _waitFrameCount=150;
    [SerializeField]
    MeshRenderer _semanticMeshRenderer;
    Texture2D _semanticTexture;
    bool _textureInitialized=false;

    // Start is called before the first frame update
    void Start()
    {
        //Check whether SemanticMode is supported.
        StartCoroutine(CheckSemanticModeSupportedCoroutine());
    }

    IEnumerator CheckSemanticModeSupportedCoroutine(){
        int count=0;
        while(!_isSemanticModeSupported && count<_waitFrameCount){
            //https://developers.google.com/ar/reference/unity-arf/class/Google/XR/ARCoreExtensions/ARSemanticManager
            FeatureSupported featureSupported = _semanticManager.IsSemanticModeSupported(SemanticMode.Enabled);
            if (featureSupported == FeatureSupported.Supported)
            {
                _isSemanticModeSupported=true;
            }
            count++;
            SetDebugText("SemanticModeSupported:" + _isSemanticModeSupported+" count:"+count);
            yield return new WaitForSeconds(0.1f);
        }    
    }
    void SetDebugText(string text){
        if(_debugText==null || !_showDebugText){ return;}
        _debugText.text = text;
    }
    // Update is called once per frame
    void Update()
    {
        //If SemanticMode is not supported, do nothing.
        if(!_isSemanticModeSupported){return ;}
        //If SemanticTexture is not ready, do nothing.
        if(!_semanticManager.TryGetSemanticTexture(ref _semanticTexture)){ return;}
        //If texture of MeshRenderer is not initialized, set SemanticTexture to MeshRenderer.
        if(!_textureInitialized){
            _textureInitialized=true;
            //Set SemanticTexture to MeshRenderer.
            _semanticMeshRenderer.material.mainTexture = _semanticTexture;
        }
    }
}
