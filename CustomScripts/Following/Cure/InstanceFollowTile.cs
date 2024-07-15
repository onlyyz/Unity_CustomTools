using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DS
{
    public class InstanceFollowTile : MonoBehaviour
    {
        public GameObject Target;
        public GameObject InstanceGo;
        public bool isInstacne;

        public float awaitTime;

      
        public float speed = 5f; // 子弹的速度
        public float rotateSpeed = 200f; // 子弹基本转向速度
        
        public float curveDuration = 30f; // 旋转速度的随机范围
        public float initialAngleRange;
        
        
        public AnimationCurve curveSpeedCurve; // 曲线阶段速度曲线
        public AnimationCurve directSpeedCurve; // 直线阶段速度曲线
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
            GameObject intance = Instantiate(InstanceGo, transform.position, Quaternion.identity);
            HomingProjectile code =  intance.GetComponent<HomingProjectile>();
            code.Init(Target.transform, speed, rotateSpeed, curveDuration,initialAngleRange,curveSpeedCurve,directSpeedCurve);
        }
    }
}
