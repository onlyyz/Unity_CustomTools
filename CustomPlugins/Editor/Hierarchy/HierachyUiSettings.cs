using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

namespace CustomHierarchyUI
{
    [System.Serializable]
    public class HierarchyFontColorDesign
    {
        [LabelText("关键词识别")] public string keyChar;
        [LabelText("字体颜色")] public Color textColor;
        [LabelText("背景颜色")] public Color backgroundColor;
        [LabelText("字体位置")] public TextAnchor textAlignment;
        [LabelText("字体粗细")] public FontStyle fontStyle;
    }

    [CreateAssetMenu(menuName = "Custom/UI")]
    public class HierachyUiSettings : ScriptableObject
    {
        [Tooltip("英语大写")] public bool toUpper;
        public List<HierarchyFontColorDesign> colorDesigns = new List<HierarchyFontColorDesign>();
    }
}