using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tracker : MonoBehaviour
{
   public ComponentData componentData;
    private void Update()
    {
        componentData.position = this.transform.localPosition;
        componentData.scale = this.transform.localScale;
        componentData.rotation = this.transform.localEulerAngles;
    }
}
