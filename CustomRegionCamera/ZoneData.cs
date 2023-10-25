using System;

using UnityEngine;

namespace Zone.SO
{
  
    [Serializable]
    public class ZoneData
    {
        [SerializeField]
        public string ZoneName;

        [SerializeField]
        public Vector3 ZonePos;

        [SerializeField]
        public Vector3 ZoneCenter;

        [SerializeField]
        public Vector3 ZoneSize;

        /// <summary>
        /// 
        /// </summary>
       
        public Vector3 LimitPos;

        [SerializeField]
        public Vector3 LimitCenter;

        [SerializeField]
        public Vector3 LimitSize;

        
        /// <summary>
        /// 
        /// </summary>
     
        public float Fov;
        [SerializeField]
        public float Near;
        [SerializeField]
        public float Far;

     
        [SerializeField]
        public float XDamping;
        
        [SerializeField]
        public float YDamping;
        [SerializeField]
        public float ZDamping;
        
        
        
        [SerializeField]
        public float Distance;
        
        [SerializeField]
        public float DeadWidth;
        [SerializeField]
        public float DeadHeight;
        
    }
}