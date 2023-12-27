using System;
using Sirenix.OdinInspector;
using UnityEngine;

namespace CustomPlugins.Zone.Data
{
    [TableList,TableColumnWidth(20,Resizable =false)]
    [Serializable]
    public class ZoneData
    {
        [SerializeField, LabelText("区域"), GUIColor(1, 0.2f, 0, 1)]
        public string ZoneName;

        [SerializeField, LabelText("Pos")]
        public Vector3 ZonePos;

        [SerializeField, LabelText("Center"), GUIColor(.5f, 1, 0, 1)]
        public Vector3 ZoneCenter;

        [SerializeField, LabelText("Size"), GUIColor(.5f, 1, 0, 1)]
        public Vector3 ZoneSize;

        /// <summary>
        /// 
        /// </summary>
        [PropertySpace] [SerializeField, LabelText("Pos")]
        public Vector3 LimitPos;

        [SerializeField, LabelText("Center"), GUIColor(.5f, 1, 0, 1)]
        public Vector3 LimitCenter;

        [SerializeField, LabelText("Size"), GUIColor(.5f, 1, 0, 1)]
        public Vector3 LimitSize;

        
        /// <summary>
        /// 
        /// </summary>
        [PropertySpace] [SerializeField, LabelText("FOV")]
        public float Fov;
        [SerializeField, LabelText("near")]
        public float Near;
        [SerializeField, LabelText("far")]
        public float Far;

     
        [SerializeField, LabelText("Damp X")]
        public float XDamping;
        
        [SerializeField, LabelText("Damp Y")]
        public float YDamping;
        [SerializeField, LabelText("Damp Z")]
        public float ZDamping;
        
        
        
        [SerializeField, LabelText("Distance")]
        public float Distance;
        
        [SerializeField, LabelText("Dead width")]
        public float DeadWidth;
        [SerializeField, LabelText("Dead height")]
        public float DeadHeight;
        
    }
}