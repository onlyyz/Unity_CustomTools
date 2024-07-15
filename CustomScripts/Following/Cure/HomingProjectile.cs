using System;
using UnityEngine;
using Sirenix.OdinInspector;
using Random = UnityEngine.Random;

public class HomingProjectile : MonoBehaviour
{
    [ShowInInspector, ReadOnly, Tooltip("目标的Transform")]
    public Transform target;
    
    [ShowInInspector, Tooltip("基础速度")]
    public float baseSpeed = 5f;
    
    [ShowInInspector, Tooltip("子弹的旋转速度")]
    public float rotateSpeed = 200f;
    
    [ShowInInspector, Tooltip("初始角度范围")]
    public float initialAngleRange = 45f;
    
    [ShowInInspector, Tooltip("曲线运动持续时间（秒）")]
    public float curveDuration = 2f;
    
    [ShowInInspector, Tooltip("曲线阶段速度曲线")]
    public AnimationCurve curveSpeedCurve;
    
    [ShowInInspector, Tooltip("直线阶段速度曲线")]
    public AnimationCurve directSpeedCurve;

    private Vector3 initialDirection;
    private float timeSinceStart;
    private float totalDuration;

    void Start()
    {
        // 假设总持续时间为3秒
        totalDuration = 5f;
    }

    private void OnDestroy()
    {
        Destroy(gameObject,6f);
    }

    void Update()
    {
        if (target == null) return;

        timeSinceStart += Time.deltaTime;

        Vector3 targetDirection = (target.position - transform.position).normalized;
        Vector3 currentDirection;
        float currentSpeed;

        if (timeSinceStart < curveDuration)
        {
            currentDirection = Vector3.RotateTowards(
                initialDirection,
                targetDirection,
                rotateSpeed * Mathf.Deg2Rad * Time.deltaTime,
                0.0f
            );
            initialDirection = currentDirection;
            currentSpeed = baseSpeed * curveSpeedCurve.Evaluate(timeSinceStart / curveDuration);
        }
        else
        {
            float directPhaseTime = timeSinceStart - curveDuration;
            float directPhaseDuration = totalDuration - curveDuration;

            if (directPhaseDuration > 0)
            {
                currentDirection = targetDirection;
                currentSpeed = baseSpeed * directSpeedCurve.Evaluate(directPhaseTime / directPhaseDuration);
            }
            else
            {
                currentDirection = targetDirection;
                currentSpeed = baseSpeed;
            }
        }

        if (currentDirection != Vector3.zero)
        {
            Quaternion targetRotation = Quaternion.LookRotation(Vector3.forward, currentDirection);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime * rotateSpeed);
        }

        transform.position += currentDirection * currentSpeed * Time.deltaTime;
    }

    [Button("Initialize Homing Projectile")]
    public void Init(
        Transform target, 
        float baseSpeed, 
        float rotateSpeed,
        float curveDuration,
        float initialAngleRange,
        AnimationCurve curveSpeedCurve, 
        AnimationCurve directSpeedCurve)
    {
        this.target = target;
        this.baseSpeed = baseSpeed;
        this.rotateSpeed = rotateSpeed;
        this.curveDuration = curveDuration;
        this.initialAngleRange = initialAngleRange;
        this.curveSpeedCurve = curveSpeedCurve;
        this.directSpeedCurve = directSpeedCurve;

        float initialAngle = Random.Range(-initialAngleRange, initialAngleRange);
        Vector3 targetDir = (target.position - transform.position).normalized;
        initialDirection = Quaternion.Euler(0, 0, initialAngle) * targetDir;

        timeSinceStart = 0f;
    }
}
