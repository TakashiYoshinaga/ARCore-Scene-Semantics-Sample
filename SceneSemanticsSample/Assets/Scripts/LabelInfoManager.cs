using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[System.Serializable]
public class LabelInfoManager
{
    [SerializeField] LabelInfo _unlabeledInfo=new LabelInfo(new Color(0,0,0,0),false);
    [SerializeField] LabelInfo _skyInfo = new LabelInfo(new Color(0.3803921f,0.545098f,0.7490196f,0.4f),false);
    [SerializeField] LabelInfo _buildingInfo= new LabelInfo(new Color(0.3098038f,0.3137254f,0.3098038f,0.4f),true);
    [SerializeField] LabelInfo _treeInfo = new LabelInfo(new Color(0.3098039f,0.572549f,0.2392156f,0.4f),true);
    [SerializeField] LabelInfo _roadInfo = new LabelInfo(new Color(0.5490196f,0.2156862f,0.8745099f,0.4f),true);
    [SerializeField] LabelInfo _sidewalkInfo = new LabelInfo(new Color(0.8941177f,0.2274509f,0.8862746f,0.4f),true);
    [SerializeField] LabelInfo _terrainInfo = new LabelInfo(new Color(0.7176471f,0.972549f,0.6705883f,0.4f),true);
    [SerializeField] LabelInfo _structureInfo = new LabelInfo(new Color(0.8313726f,0.7450981f,0.6078432f,0.4f),true);
    [SerializeField] LabelInfo _objectInfo = new LabelInfo(new Color(0.8862746f,0.8745099f,0.2862745f,0.4f),true);
    [SerializeField] LabelInfo _vehicleInfo = new LabelInfo(new Color(0.04313721f,0.06274506f,0.8784314f,0.4f),true);
    [SerializeField] LabelInfo _personInfo = new LabelInfo(new Color(0.9215687f,0.2f,0.1411764f,0.4f),true);
    [SerializeField] LabelInfo _waterInfo = new LabelInfo(new Color(0.454902f,0.909804f,0.854902f,0.4f),true);
    [SerializeField] LabelInfo _defaultInfo = new LabelInfo(new Color(1,1,1,0.4f),false);
    LabelInfo[] _labelInfoArray;
   

    public LabelInfoManager(){
        _labelInfoArray=new LabelInfo[]{
            _unlabeledInfo,
            _skyInfo,
            _buildingInfo,
            _treeInfo,
            _roadInfo,
            _sidewalkInfo,
            _terrainInfo,
            _structureInfo,
            _objectInfo,
            _vehicleInfo,
            _personInfo,
            _waterInfo,
            _defaultInfo
        };
    }
    
    public List<Color> GetColorArray(){
        List<Color> _ColorList = new List<Color>();
        foreach(LabelInfo _labelInfo in _labelInfoArray){
            _ColorList.Add(_labelInfo._color);
        }
        return _ColorList;
    }
    public List<float> GetMaskArray(){
        List<float> _MaskList = new List<float>();
        foreach(LabelInfo _labelInfo in _labelInfoArray){
            if(_labelInfo._isMask){
                _MaskList.Add(1);
            }else{
                _MaskList.Add(0);
            }
        }
        return _MaskList;
    }
    public void SetMaskFlag(LabelName _labelType, bool _isMask){
        _labelInfoArray[(int)_labelType]._isMask=_isMask;
    } 
    public void SetLabelColor(LabelName _labelType, Color _color){
        _labelInfoArray[(int)_labelType]._color=_color;
    }
}  
[Serializable]
public class LabelInfo{
        public Color _color;
        public bool _isMask;
       public LabelInfo(Color color, bool isMask){
           _color = color;
           _isMask = isMask;
       }
}

public enum LabelName{
    Unlabeled,
    Sky,
    Building,
    Tree,
    Road,
    Sidewalk,
    Terrain,
    Structure,
    Object,
    Vehicle,
    Person,
    Water,
    Default
}