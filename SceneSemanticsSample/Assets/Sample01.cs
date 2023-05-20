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
        

    }
}
