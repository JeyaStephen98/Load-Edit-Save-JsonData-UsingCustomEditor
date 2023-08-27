using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public interface Component
{
    string type { get; set; }
    string name { get; set; }
    Vector3 position { get; set; }
    Vector3 rotation { get; set; }
    Vector3 scale { get; set; }
}
