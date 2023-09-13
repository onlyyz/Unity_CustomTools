using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CustomHierarchyUI
{
    [System.Serializable]
    public class HierarchyFontColorDesign
    {
        // [Tooltip("Rename gameObject begin with this keychar")]
        [Tooltip("关键词识别")]
        public string keyChar;
        // [Tooltip("Don't forget to change alpha to 255")]
        [Tooltip("字体颜色")]
        public Color textColor;
        // [Tooltip("Don't forget to change alpha to 255")]
        [Tooltip("背景颜色")]
        public Color backgroundColor;
        [Tooltip("字体位置")]
        public TextAnchor textAlignment;
        [Tooltip("字体粗细")]
        public FontStyle fontStyle;
    }
    
    [CreateAssetMenu(menuName = "Custom/UI")]
    public class HierachyUiSettings : ScriptableObject
    {
        [Tooltip("英语大写")]
        public bool toUpper;
        public List<HierarchyFontColorDesign> colorDesigns = new List<HierarchyFontColorDesign>();
    }
}