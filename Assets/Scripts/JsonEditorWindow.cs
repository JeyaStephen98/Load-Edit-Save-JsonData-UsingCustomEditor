using UnityEngine;
using UnityEditor;
using System.IO;
using System.Collections.Generic;
using TMPro;
using UnityEngine.UI;
using System;

public class JsonEditorWindow : EditorWindow
{
    #region variables
    private Vector2 buttonSize = new Vector2(100, 100);
    private Vector2 scrollPositiontext = Vector2.zero;
    private string jsonFilePath = Application.streamingAssetsPath + "/template.json";
    private string createJsonFilePath = Application.streamingAssetsPath + "/createTemplate.json";
    private string jsonText = "";
    TemplateObject loadedTemplate, selectedTemplate, createdTemplate;
    List<ComponentData> addTemplateObj = new List<ComponentData>();
    Canvas canvas;
    #endregion

    #region Menu
    [MenuItem("Window/JSON Editor")]
    public static void ShowWindow()
    {
        GetWindow<JsonEditorWindow>("JSON Editor");
    }
    #endregion

    #region OnGUI
    private void OnGUI()
    {
        GUILayout.Label("JSON Editor", EditorStyles.boldLabel);

        if (GUILayout.Button("Load Default JSON"))
        {
            selectedTemplate = loadedTemplate = LoadTemplateFromFile();
        }

        scrollPositiontext = EditorGUILayout.BeginScrollView(scrollPositiontext);

        jsonText = EditorGUILayout.TextArea(jsonText, GUILayout.Width(1000), GUILayout.ExpandHeight(true));

        EditorGUILayout.EndScrollView();

        if (GUILayout.Button("Save JSON"))
        {
            SaveTemplateToFile( jsonFilePath, "TemplateJsonDataCanvas");
        }

        if (GUILayout.Button("Instantiate Template"))
        {
            InstantiateSelectedTemplate("TemplateJsonDataCanvas");
        }

        GUILayout.Label("Instantiate Selected Template", EditorStyles.boldLabel);

        if (GUILayout.Button("Create Template"))
        {
            CreateNewTemplate();
        }

        GUILayout.Label("My Custom Editor Window", EditorStyles.boldLabel);

        GUILayout.BeginHorizontal();

        if (GUILayout.Button("Add Image", GUILayout.Width(buttonSize.x), GUILayout.Height(buttonSize.y)))
        {

            addTemplateObj.Add(CreateComponent(new ImageComponent()));
            updateJsonEditorText(addTemplateObj);

        }
        if (GUILayout.Button("Add Button", GUILayout.Width(buttonSize.x), GUILayout.Height(buttonSize.y)))
        {
            addTemplateObj.Add(CreateComponent(new ButtonComponent()));
            updateJsonEditorText(addTemplateObj);
        }
        if (GUILayout.Button("Add Text", GUILayout.Width(buttonSize.x), GUILayout.Height(buttonSize.y)))
        {
            addTemplateObj.Add(CreateComponent(new TextComponent()));
            updateJsonEditorText(addTemplateObj);
        }
        if (GUILayout.Button("Add RawImage", GUILayout.Width(buttonSize.x), GUILayout.Height(buttonSize.y)))
        {
            addTemplateObj.Add(CreateComponent(new RawImageComponent()));
            updateJsonEditorText(addTemplateObj);
        }

        GUILayout.EndHorizontal();

        if (GUILayout.Button("Save & Instantiate"))
        {
            selectedTemplate = createdTemplate;
            InstantiateSelectedTemplate("UserCreateCanvas");
            SaveTemplateToFile(createJsonFilePath, "UserCreateCanvas");

            File.WriteAllText(createJsonFilePath, JsonUtility.ToJson(selectedTemplate, true));
        }

    }
    #endregion

    #region UserDefinedMethods
   
    private void InstantiateSelectedTemplate(string canvasName)
    {
        if (selectedTemplate != null)
        {
            InstantiateUIObjects(selectedTemplate, canvasName);
        }
        else
        {
            Debug.LogWarning("No template selected.");
        }
    }
    private void CreateNewTemplate()
    {
        addTemplateObj.Clear();
        updateJsonEditorText(addTemplateObj);
    }
    private void InstantiateUIObjects(TemplateObject template, string canvasName)
    {
        if (GameObject.Find(canvasName))
        {
            DestroyImmediate(GameObject.Find(canvasName));
        }
        GameObject canvasObject = new GameObject(canvasName);
        canvas = canvasObject.AddComponent<Canvas>();
        canvas.renderMode = RenderMode.ScreenSpaceOverlay;

        foreach (ComponentData componentData in template.components)
        {
            CreateGO(componentData, canvas.transform);
        }
    }
    private void CreateGO(ComponentData componentData, Transform parent)
    {
        GameObject uiObject = new GameObject();
        uiObject.AddComponent<Tracker>().componentData = componentData;
        uiObject.name = componentData.name;
        RectTransform rectTransform = uiObject.AddComponent<RectTransform>();
        rectTransform.SetParent(parent);
        rectTransform.anchoredPosition = new Vector3(componentData.position.x, componentData.position.y, componentData.position.z);
        rectTransform.localRotation = Quaternion.Euler(componentData.rotation.x, componentData.rotation.y, componentData.rotation.z);
        rectTransform.localScale = new Vector3(componentData.scale.x, componentData.scale.y, componentData.scale.z);

        NeedToAddComponent(componentData, uiObject);

        if (componentData.child != null)
        {
            foreach (var item in componentData.child)
            {
                CreateGO(item, uiObject.transform);
            }
        }
    }

    private void SaveTemplateToFile(string path, string canvasName)
    {
        if(!GameObject.Find(canvasName))
        {
            return;
        }
        GameObject canvas = GameObject.Find(canvasName);
        TemplateObject template = new TemplateObject();
        template.components = new List<ComponentData>();
        if (canvas.transform.childCount != 0)
        {
            for (int i = 0; i < canvas.transform.childCount; i++)
            {
                template.components.Add(canvas.transform.GetChild(i).GetComponent<Tracker>().componentData);
            }
        }
        jsonText = JsonUtility.ToJson(template, true);
        File.WriteAllText(path, jsonText);
    }

    private void updateJsonEditorText(List<ComponentData> componentDatas)
    {
        createdTemplate = new TemplateObject();
        createdTemplate.components = new List<ComponentData>();
        if (componentDatas.Count != 0)
        {
            for (int i = 0; i < componentDatas.Count; i++)
            {

                createdTemplate.components.Add(componentDatas[i]);
            }
        }
        string json = JsonUtility.ToJson(createdTemplate, true);
        jsonText = json;
    }


    // Error handling and Deserialization
    private TemplateObject LoadTemplateFromFile()
    {
        if (File.Exists(jsonFilePath))
        {
            //..error handling..
            try
            {
                string json = File.ReadAllText(jsonFilePath);
                jsonText = JsonUtility.ToJson(JsonUtility.FromJson<TemplateObject>(json), true);
                return JsonUtility.FromJson<TemplateObject>(json); //..Deserialization..
            }
            catch (System.Exception e)
            {
                Debug.LogError("Error reading JSON: " + e.Message); 
                return null;
            }
        }
        else
        {
            Debug.LogWarning("JSON file not found.");
            return null;
        }
    }

    ComponentData CreateComponent<T>(T component) where T : Component
    {
        ComponentData componentData = new ComponentData();
        switch (component.type)
        {
            case "Text":
                componentData.type = "Text";
                componentData.name = "Text";
                break;
            case "Button":
                componentData.type = "Button";
                componentData.name = "Button";
                break;
            case "Image":
                componentData.type = "Image";
                componentData.name = "Image";
                break;
            case "RawImage":
                componentData.type = "RawImage";
                componentData.name = "RawImage";
                break;
        }
        return componentData;
    }
    private void NeedToAddComponent(ComponentData componentData, GameObject GO)
    {
        switch (componentData.type)
        {
            case "Text":
                GO.AddComponent<TextMeshProUGUI>().text = componentData.text;
                break;
            case "Button":
                Image img = GO.AddComponent<Image>();
                Button btn = GO.AddComponent<Button>();
                btn.targetGraphic = img;
                break;
            case "Image":
                GO.AddComponent<Image>();
                break;
            case "RawImage":
                GO.AddComponent<RawImage>();
                break;
        }
    }
    #endregion
}
