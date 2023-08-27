
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class TemplateObject
{
    public List<ComponentData> components;
}

[System.Serializable]
public class ComponentData
{
    public string type;
    public string name;
    public string text;
    public int font_size;
    public string font_color;
    public string action;
    public string image_url;
    public Vector3 position;
    public Vector3 rotation;
    public Vector3 scale;
    public List<ComponentData> child;
}