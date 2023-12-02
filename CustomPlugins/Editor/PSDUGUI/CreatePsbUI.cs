using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using ParadoxNotion.Serialization.FullSerializer;
using UnityEngine.UI;

public class CreatePsbUI : EditorWindow {

    [MenuItem("Tools/创建PsbUI",false,24)]
    private static void ShowWindow() {
        var window = GetWindow<CreatePsbUI>();
        window.titleContent = new GUIContent("创建PsbUI");
        window.Show();
    }

    private string preName;
    private void OnGUI() {
        preName = GUILayout.TextField(preName,GUILayout.Height(20));
       if (GUILayout.Button("生成UI") && !string.IsNullOrEmpty(preName)){
        RectTransform par = GameObject.Find(preName) .AddComponent<RectTransform>();
        for (int i = 0; i < par.childCount; i++){
            RectTransform rect = par.GetChild(i) .gameObject.AddComponent<RectTransform>();
            SpriteRenderer spriteRenderer=par.GetChild(i).GetComponent<SpriteRenderer>();
            par.GetChild(i).gameObject.AddComponent<Image>().sprite = spriteRenderer.sprite;
            GameObject.DestroyImmediate(spriteRenderer);
            rect.sizeDelta = rect.sizeDelta * 100;
            rect.anchoredPosition = rect.anchoredPosition * 100;
        }
       }
    }
}
