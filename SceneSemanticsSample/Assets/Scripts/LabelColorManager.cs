using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[System.Serializable]
public class LabelColorManager 
{
    [SerializeField] Color _unlabeled= new Color(0,0,0,0);
    [SerializeField] Color _sky = new Color(0.3803921f,0.545098f,0.7490196f,1);
    [SerializeField] Color _building= new Color(0.3098038f,0.3137254f,0.3098038f,0.4f);
    [SerializeField] Color _tree = new Color(0.3098039f,0.572549f,0.2392156f,0.4f);
    [SerializeField] Color _road = new Color(0.5490196f,0.2156862f,0.8745099f,0.4f);
    [SerializeField] Color _sidewalk = new Color(0.8941177f,0.2274509f,0.8862746f,0.4f);
    [SerializeField] Color _terrain = new Color(0.7176471f,0.972549f,0.6705883f,0.4f);
    [SerializeField] Color _structure = new Color(0.8313726f,0.7450981f,0.6078432f,0.4f);
    [SerializeField] Color _object = new Color(0.8862746f,0.8745099f,0.2862745f,0.4f);
    [SerializeField] Color _vehicle = new Color(0.04313721f,0.06274506f,0.8784314f,0.4f);
    [SerializeField] Color _person = new Color(0.9215687f,0.2f,0.1411764f,0.4f);
    [SerializeField] Color _water = new Color(0.454902f,0.909804f,0.854902f,0.4f);
    [SerializeField] Color _default = new Color(1.0f,1.0f,1.0f,0.4f);
    List<Color> _colorList;

    public LabelColorManager(){
        _colorList = new List<Color>();
        _colorList.Add(_unlabeled);
        _colorList.Add(_sky);
        _colorList.Add(_building);
        _colorList.Add(_tree);
        _colorList.Add(_road);
        _colorList.Add(_sidewalk);
        _colorList.Add(_terrain);
        _colorList.Add(_structure);
        _colorList.Add(_object);
        _colorList.Add(_vehicle);
        _colorList.Add(_person);
        _colorList.Add(_water);
        _colorList.Add(_default);
    }
    
    public List<Color> GetColorArray(){
        return _colorList;
    }
}  
