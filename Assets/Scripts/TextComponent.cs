using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TextComponent : Component
{
    public string type { get; set; } = "Text";
    public string name { get => throw new System.NotImplementedException(); set => throw new System.NotImplementedException(); }
    public string text { get => throw new System.NotImplementedException(); set => throw new System.NotImplementedException(); }
    public int font_size { get => throw new System.NotImplementedException(); set => throw new System.NotImplementedException(); }
    public string font_color { get => throw new System.NotImplementedException(); set => throw new System.NotImplementedException(); }
    public string action { get => throw new System.NotImplementedException(); set => throw new System.NotImplementedException(); }
    public string image_url { get => throw new System.NotImplementedException(); set => throw new System.NotImplementedException(); }
    public Vector3 position { get => throw new System.NotImplementedException(); set => throw new System.NotImplementedException(); }
    public Vector3 rotation { get => throw new System.NotImplementedException(); set => throw new System.NotImplementedException(); }
    public Vector3 scale { get => throw new System.NotImplementedException(); set => throw new System.NotImplementedException(); }
    public List<ComponentData> child { get => throw new System.NotImplementedException(); set => throw new System.NotImplementedException(); }
}
