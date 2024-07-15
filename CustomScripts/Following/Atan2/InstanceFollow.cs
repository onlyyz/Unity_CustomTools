using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DS
{
    public class InstanceFollow : MonoBehaviour
    {
        public GameObject InstanceGo;
        public bool isInstacne;

        public float awaitTime;

        public GameObject Target;
        public float speed = 5f; // 子弹的速度
        public float rotateSpeed = 200f; // 子弹基本转向速度
        
        public float curveDuration = 30f; // 旋转速度的随机范围
        public float initialAngleRange;
        private void Awake()
        {
            Invoke("FireInstance",awaitTime);
        }

        public void FireInstance()
        {
            StartCoroutine(InstanceOBJ());
        }
        IEnumerator InstanceOBJ()
        {
            while (isInstacne)
            {
                IntanceObj();
                yield return new WaitForSeconds(awaitTime);
            }
        }
        
        public void IntanceObj()
        {
            GameObject intance = GameObject.Instantiate(InstanceGo, transform.position, Quaternion.identity);
            HomingMissile code =  intance.AddComponent<HomingMissile>();
            code.Init(Target.transform, speed, rotateSpeed, curveDuration,initialAngleRange);
        }
    }
}
