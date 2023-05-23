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
    [Header("[For Using Scene Semantics]")]
    [SerializeField]
    ARSemanticManager _semanticManager;
    [SerializeField]
    int _maxWaitFrameCount=150;
    [SerializeField]
    GameObject _semanticQuad;

    [SerializeField]
    LabelColorManager _labelColorManager=new LabelColorManager();

    [Header("[For Debug]")]
    [SerializeField]
    Text _debugText;
    [SerializeField]
    bool _showDebugText=true;

    //Private variables
    bool _isSemanticModeSupported = false;
    MeshRenderer _semanticMeshRenderer;
    Texture2D _semanticTexture;
    bool _isQuadSizeInitialized=false;

    // Start is called before the first frame update
    void Start()
    {
        _semanticMeshRenderer = _semanticQuad.GetComponent<MeshRenderer>();
        _semanticMeshRenderer.material.SetColorArray("_LabelColorArray", _labelColorManager.GetColorArray());
        //Check whether SemanticMode is supported.
        StartCoroutine(CheckSemanticModeSupportedCoroutine());
    }

    IEnumerator CheckSemanticModeSupportedCoroutine(){
        int count=0;
        while(!_isSemanticModeSupported && count<_maxWaitFrameCount){
            //https://developers.google.com/ar/reference/unity-arf/class/Google/XR/ARCoreExtensions/ARSemanticManager
            FeatureSupported featureSupported = _semanticManager.IsSemanticModeSupported(SemanticMode.Enabled);
            if (featureSupported == FeatureSupported.Supported)
            {
                _isSemanticModeSupported=true;
            }
            count++;
            SetDebugText("SemanticModeSupported:" + _isSemanticModeSupported);
            yield return new WaitForSeconds(0.1f);
        }    
    }
    void SetDebugText(string text){
        if(_debugText==null || !_showDebugText){ return;}
        _debugText.text = text;
    }
    void ResizeQuadToScreen(int imgWidth, int imgHeight) 
    {
        //Aspect ratio of screen (height / width of Landscape image)
        float imageAspectRatio=(float)imgHeight/(float)imgWidth;
        //Convert camera's field of view to radian.
        float cameraFovRad = Camera.main.fieldOfView * Mathf.Deg2Rad;
        // Calculate the height of the Quad based on the distance from the camera to the Quad in the vertical direction.
        float quadHeightAtDistance = 2.0f *_semanticQuad.transform.localPosition.z * Mathf.Tan(cameraFovRad / 2.0f);
        //Calculate the width of the Quad from the height of the Quad using the aspect ratio.
        float quadWidthAtDistance = quadHeightAtDistance * imageAspectRatio;
        //Apply the calculated width and height to the Quad.
        _semanticQuad.transform.localScale = new Vector3(quadWidthAtDistance, quadHeightAtDistance, 1);
    }
    // Update is called once per frame
    void Update()
    {
        //If SemanticMode is not supported, do nothing.
        if(!_isSemanticModeSupported){return ;}
        //If SemanticTexture is not ready, do nothing.
        if(!_semanticManager.TryGetSemanticTexture(ref _semanticTexture)){ return;}
        //Set SemanticTexture to MeshRenderer.
        _semanticMeshRenderer.material.mainTexture = _semanticTexture;
        
        //Resize Quad to fit the screen.
        if(!_isQuadSizeInitialized){
            _isQuadSizeInitialized=true;
            ResizeQuadToScreen(_semanticTexture.width, _semanticTexture.height);
        }

        //Note:
        //Semantic label is stored in SemanticTexture as R8 format.
        //To show the label, conversion from R8 to RGBA32 is required.
        //Also, image is given as landscape and flipped. So, swapping pixel order is required.
        //In order to avoid cpu load, the conversion is done in GPU.
        //Please see the shader file(Assets/Materials/SemanticLabels.shader) for details.
    }
}
