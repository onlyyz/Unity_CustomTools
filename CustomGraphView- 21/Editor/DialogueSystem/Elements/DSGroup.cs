using System.Collections;
using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

namespace DS.Elements
{
    using Enumerations;
    using Utilities;
    using Winndos;
    public class DSGroup : Group
    {
        private Color defaultBackgroundColor;
        private float defaultBorderWidth;
        public string oldTitle;
        public DSGroup(string groupTitle, Vector2 position)
        {
            defaultBackgroundColor = contentContainer.style.borderBottomColor.value;
            defaultBorderWidth = contentContainer.style.borderBottomWidth.value;

            title = groupTitle;
            oldTitle = groupTitle;
            SetPosition(new Rect(position,Vector2.zero));
        }
        
        #region Style
        public void SetErrorStyle(Color color)
        {
            contentContainer.style.backgroundColor = color;
            contentContainer.style.borderBottomWidth = 2.0f;
        }
        public void ResetStyle()
        {
            contentContainer.style.backgroundColor = defaultBackgroundColor;
            contentContainer.style.borderBottomWidth = defaultBorderWidth;
        }
        #endregion
    }
}
