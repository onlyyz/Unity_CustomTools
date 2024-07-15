using System;
using UnityEngine;
using Random = UnityEngine.Random;

public class HomingMissile : MonoBehaviour
{
    public Transform target; // 目标敌人的Transform
    public float speed = 5f; // 子弹的速度
    public float rotateSpeed = 200f; // 子弹基本转向速度
    
   

    private Vector3 initialDirection; // 初始方向
    private float timeSinceStart; // 从开始到现在的时间
    public float curveDuration = 0.5f; // 曲线运动持续时间（秒）
    
    public void Init(Transform target, float speed, float rotateSpeed,float curveDuration,float initialAngleRange )
    {
        this.target = target;
        this.speed = speed;
        this.rotateSpeed = rotateSpeed;
        this.curveDuration = curveDuration;
        
        // 计算一个随机的初始偏移角度
        float initialAngle = Random.Range(-initialAngleRange, initialAngleRange);
        // 获取从当前位置到目标的方向
        Vector3 targetDir = (target.position - transform.position).normalized;
        // 旋转这个方向
        initialDirection = Quaternion.Euler(0, 0, initialAngle) * targetDir;
        
        // 初始化计时器
        timeSinceStart = 0f;
    }
    void Update()
    {
        if (target == null) return;

        timeSinceStart += Time.deltaTime;

        Vector3 targetDirection = (target.position - transform.position).normalized;
        Vector3 currentDirection;

        if (timeSinceStart < curveDuration)
        {
            currentDirection = Vector3.RotateTowards(
                initialDirection,
                targetDirection,
                rotateSpeed * Mathf.Deg2Rad * Time.deltaTime,
                0.0f
            );
            initialDirection = currentDirection;
        }
        else
        {
            currentDirection = targetDirection;
        }

        // 平滑旋转到新方向
        if (currentDirection != Vector3.zero)
        {
            Quaternion targetRotation = Quaternion.LookRotation(Vector3.forward, currentDirection);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime * rotateSpeed);
        }

        transform.position += currentDirection * speed * Time.deltaTime;
    }
}
