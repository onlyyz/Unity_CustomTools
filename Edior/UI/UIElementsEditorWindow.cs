using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;
using UnityEditor.UIElements;

public class UIElementsEditorWindow : EditorWindow
{
    [MenuItem("Tool/Learn/UIElementsEditorWindow")]
    public static void ShowExample()
    {
        UIElementsEditorWindow wnd = GetWindow<UIElementsEditorWindow>();
        wnd.titleContent = new GUIContent("UIElementsEditorWindow");
    }

    public void CreateGUI()
    {
        VisualElement container = new VisualElement();

        rootVisualElement.Add(container);

        StyleSheet styleSheet = (StyleSheet)EditorGUIUtility.Load("UIElementsStyles.uss");
        rootVisualElement.styleSheets.Add(styleSheet);

        Label title = new Label("Color Picker");
        //Editor 窗口修改颜色为红色
        // title.style.color = Color.red;

        ColorField colorField = new ColorField();
        {
            name = "color-picker";
        }
 
        container.Add(title);
        container.Add(colorField);

        VisualElement buttonsContainer = new VisualElement();

        Button randomColorButton = (Button) CreateButton("Random Color");
        Button resetColorButton = (Button) CreateButton("Reset Color");
        Button copyColorButton = (Button) CreateButton("Copy Color");
        Button pasteColorButton = (Button) CreateButton("Paste Color");

        buttonsContainer.Add(randomColorButton);
        buttonsContainer.Add(resetColorButton);
        buttonsContainer.Add(copyColorButton);
        buttonsContainer.Add(pasteColorButton);

        container.Add(buttonsContainer);
        
        buttonsContainer.AddToClassList("horizontal-container");
        randomColorButton.AddToClassList("dark-button");
        resetColorButton.AddToClassList("dark-button");
        copyColorButton.AddToClassList("dark-button");
        pasteColorButton.AddToClassList("dark-button");
        
        
    }

    private VisualElement CreateButton(string text)
    {
        return new Button() { text = text };
    }
}