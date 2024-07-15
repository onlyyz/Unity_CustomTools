using UnityEditor;
using UnityEngine;

namespace CustomHierarchyUI
{
    [InitializeOnLoad]
    public class StyleHierarchy
    {
        //Find ColorPalette GUID
        static string[] dataArray;
        //Get ColorPalette(ScriptableObject) path
        static string path;
        static HierachyUiSettings colorPalette;

        static StyleHierarchy()
        {
            //过滤泛型 ColorPalette
            // dataArray = AssetDatabase.FindAssets("t:ColorPalette");
            dataArray = AssetDatabase.FindAssets("t:HierachyUiSettings");
            if (dataArray.Length >= 1)
            {    
                //We have only one color palette, so we use dataArray[0] to get the path of the file
                //根据资源选择,默认第一个
                path = AssetDatabase.GUIDToAssetPath(dataArray[0]);
                colorPalette = AssetDatabase.LoadAssetAtPath<HierachyUiSettings>(path);
                EditorApplication.hierarchyWindowItemOnGUI += OnHierarchyWindow;
            }
        }

        private static void OnHierarchyWindow(int instanceID, Rect selectionRect)
        {
            if (dataArray.Length == 0) return;
            UnityEngine.Object instance = EditorUtility.InstanceIDToObject(instanceID);
            if (instance != null)
            {
                for (int i = 0; i < colorPalette.colorDesigns.Count; i++)
                {
                    var design = colorPalette.colorDesigns[i];

                    //Check if the name of each gameObject is begin with keyChar in colorDesigns list.
                    if (instance.name.StartsWith(design.keyChar))
                    {
                        //Remove the symbol(keyChar) from the name.
                        string newName = instance.name.Substring(design.keyChar.Length);
                        //Draw a rectangle as a background, and set the color.
                        EditorGUI.DrawRect(selectionRect, design.backgroundColor);

                        //Create a new GUIStyle to match the desing in colorDesigns list.
                        GUIStyle newStyle = new GUIStyle
                        {
                            alignment = design.textAlignment,
                            fontStyle = design.fontStyle,
                            normal = new GUIStyleState()
                            {
                                textColor = design.textColor,
                            }
                        };

                        //Draw a label to show the name in upper letters and newStyle.
                        //If you don't like all capital latter, you can remove ".ToUpper()".
                        //colorPalette.toUpper?newName.ToUpper():newName
                        EditorGUI.LabelField(selectionRect, colorPalette.toUpper?newName.ToUpper():newName, newStyle);
                    }
                }
            }
        }
    }
}