/*
    Created by Takashi Yoshinaga
    Copyright (C) 2023 Takashi Yoshinaga. All Rights Reserved.
*/

using System.Collections;
using System.Collections.Generic;
using Google.XR.ARCoreExtensions;
using UnityEngine;
using UnityEngine.UI;

public class Sample02 : MonoBehaviour
{
    [Header("Required for Using SemanticMode")]
    [SerializeField]
    ARSemanticManager _semanticManager;
    [SerializeField]
    int _waitFrameCount=150;
    [SerializeField]
    GameObject _semanticQuad;
    [Header("MaskSetting")]
    [SerializeField]
    bool _unlabeldPixels=false;
    [SerializeField]
    bool _skyPixels=false;
    [SerializeField]
    bool _buildingPixels=false;
    [SerializeField]
    bool _treePixels=true;
    [SerializeField]
    bool _roadPixels=true;
    [SerializeField]
    bool _sideWalkPixels=true;
    [SerializeField]
    bool _rerrainPixels=true;
    [SerializeField]
    bool _structurePixels=true;
    [SerializeField]
    bool _objectPixels=true;
    [SerializeField]
    bool _vehiclePixels=true;
    [SerializeField]
    bool _personPixels=true;
    [SerializeField]
    bool _waterPixels=true;
    [SerializeField]
    [Range (0.0f, 1.0f)]
    float _maskAreaVisibility=1.0f;
    
    [Header("For Debug")]
    [SerializeField]
    Text _debugText;
    [SerializeField]
    bool _showDebugText=true;
    [SerializeField]
    bool _testingInsideRoom=false; //If true, the building pixels are not shown.

    //Private variables
    bool _isSemanticModeSupported = false;
    MeshRenderer _semanticMeshRenderer;
    Texture2D _semanticTexture;
    bool _isQuadSizeInitialized=false;

    // Start is called before the first frame update
    void Start()
    {
        _semanticMeshRenderer = _semanticQuad.GetComponent<MeshRenderer>();
        //Set StencilFlags
        SetStencilFlags();
        //Check whether SemanticMode is supported.
        StartCoroutine(CheckSemanticModeSupportedCoroutine());
    }
    void SetStencilFlags(){
        if(!_skyPixels){ 
            Color c= _semanticMeshRenderer.material.GetColor("_Sky");
            c.a=0.0f;
            _semanticMeshRenderer.material.SetColor("_Sky", c);
       }
        if(!_buildingPixels || _testingInsideRoom){ 
            Color c= _semanticMeshRenderer.material.GetColor("_Building");
            c.a=0.0f;
            _semanticMeshRenderer.material.SetColor("_Building", c);
       }
        if(!_treePixels){ 
            Color c= _semanticMeshRenderer.material.GetColor("_Tree");
            c.a=0.0f;
            _semanticMeshRenderer.material.SetColor("_Tree", c);
       }
        if(!_roadPixels){ 
            Color c= _semanticMeshRenderer.material.GetColor("_Road");
            c.a=0.0f;
            _semanticMeshRenderer.material.SetColor("_Road", c);
       }
        if(!_sideWalkPixels){ 
            Color c= _semanticMeshRenderer.material.GetColor("_SideWalk");
            c.a=0.0f;
            _semanticMeshRenderer.material.SetColor("_SideWalk", c);
       }
        if(!_rerrainPixels){ 
            Color c= _semanticMeshRenderer.material.GetColor("_Rerrain");
            c.a=0.0f;
            _semanticMeshRenderer.material.SetColor("_Rerrain", c);
       }
        if(!_structurePixels){ 
            Color c= _semanticMeshRenderer.material.GetColor("_Structure");
            c.a=0.0f;
            _semanticMeshRenderer.material.SetColor("_Structure", c);
       }
        if(!_objectPixels){ 
            Color c= _semanticMeshRenderer.material.GetColor("_Object");
            c.a=0.0f;
            _semanticMeshRenderer.material.SetColor("_Object", c);
       }
        if(!_vehiclePixels){ 
            Color c= _semanticMeshRenderer.material.GetColor("_Vehicle");
            c.a=0.0f;
            _semanticMeshRenderer.material.SetColor("_Vehicle", c);
       }
        if(!_personPixels){ 
            Color c= _semanticMeshRenderer.material.GetColor("_Person");
            c.a=0.0f;
            _semanticMeshRenderer.material.SetColor("_Person", c);
       }
        if(!_waterPixels){ 
            Color c= _semanticMeshRenderer.material.GetColor("_Water");
            c.a=0.0f;
            _semanticMeshRenderer.material.SetColor("_Water", c);
       }   
       _semanticMeshRenderer.material.SetFloat("_MaskAreaVisibility", _maskAreaVisibility);
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
        //If SemanticTexture is not assigned, do nothing.
        if(_semanticTexture==null){ SetDebugText("Render object isn't assigned"); return;}

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
