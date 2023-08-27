
using UnityEngine;
using UnityEditor;

[InitializeOnLoad]
public class RectTransformChangeTracker : Editor
{
    static RectTransformChangeTracker()
    {
        EditorApplication.update += OnEditorUpdate;
    }

    private static void OnEditorUpdate()
    {
        GameObject[] selectedObjects = Selection.gameObjects;

      
        for (int i = 0; i < selectedObjects.Length; i++)
        {
            RectTransform rectTransform = selectedObjects[i].GetComponent<RectTransform>();

            if (rectTransform != null)
            {
                try
                {
                    selectedObjects[i].GetComponent<Tracker>().componentData.position = rectTransform.localPosition;
                    selectedObjects[i].GetComponent<Tracker>().componentData.rotation = rectTransform.eulerAngles;
                    selectedObjects[i].GetComponent<Tracker>().componentData.scale = rectTransform.localScale;
                }
                catch
                {

                }
                
            }
        }
    }
}
