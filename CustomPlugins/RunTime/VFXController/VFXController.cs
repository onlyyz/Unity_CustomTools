using UnityEngine;
using System.Collections;
using MoreMountains.Tools;
using TAMA.Framework.Actions;
using Dreamteck.Splines.Primitives;
using RootMotion;
using System;


public class VFXController : MonoBehaviour
{



    // public curve bulletCurve = 90f; // 特效轨迹曲线（例如，90表示直线，45表示抛物线）  

    [Header("特效销毁时间")]
    [Range(0, 5)]
    public float LifeTime = 2f; // 特效生命周期，用于追踪特效销毁时间  

    [Header("最大速度值")]
    [Range(1, 10)]
    public float ProgressSpeed;


    [Header("是否追踪目标")]
    public bool isTrackingBullet = false; // 是否追踪目标

    [Header("追踪强度")]
    [Range(1, 200)]
    public int TrackingInt = 1; // 追踪强度
    private int curTrackInt = 0;
    
     [Header("追踪角速度")]
    [Range(1, 10)]
    public float angularSpeed = 1; // 角速度  


    [Header("是否使用抛物线")]
    public bool isCurveBullet = false; // 是否抛物
                                       // public GameObject hitEffectPrefab; // 命中效果Prefab  

    // private Rigidbody rb;

    [Header("速度曲线")]
    public AnimationCurve curve; // 在Inspector中赋值  

    private float curProgressSpeed;

    [Header("旋转速度")]

    [Range(1, 10)]
    public float forwardSpeed = 1; // 转头速度  

    

    [Header("抛物线高度")]
    public float CurveHeight = 3f;

    [Header("抛物线长度")]
    [Range(0.1f, 1f)]
    public float CurveUpProgress = 0.5f;

    public class line
    {
        public Vector3 startPos;
        public Vector3 endPos;

//bCross 是否穿透EndPos;
        public Vector3 GetPosByProgress(float progress,bool bCross)
        {
            var posDir = endPos - startPos;
            
            if (!bCross && progress > 1)
                progress = 1;

            return startPos + posDir * progress;
        }

        public line(Vector3 _startPos, Vector3 _endPos)
        {
            startPos = _startPos;
            endPos = _endPos;
        }

        public float getLenght()
        {
            return Vector3.Distance(startPos, endPos);
        }

    }

    ///------------内部参数------------------------- 
    private line linePath;
    private Vector3 targetPosition;
    private float PathProgress = 0;

    void OnEnable()
    {

        targetPosition =  GetCurTargerPosition(); // 当前位置作为初始目标点
        //如果追踪角色
        if(isTrackingBullet)  
            linePath = new line(transform.position, targetPosition);
        else //不追踪角色则按初始朝向方向
             linePath = new line(transform.position, transform.position + transform.forward*5);
        
        lastPosition = transform.position;
        // ShootLinear(); // 发射特效
        bShoot = true;
        if (LifeTime > 0)
            Destroy(gameObject, LifeTime); // 在特效生命周期结束时销毁特效实例，以实现追踪效果。

        initY = transform.position.y;
        
    }
    float initY;

    bool bShoot = false;

    Vector3 lastPosition = Vector3.zero;
    float TrackingTime = 0.0f;
    float forwardTime = 0.0f;

    bool bUp = true;
    bool bProgress2 = false;
    Vector3 nextPos;

    int LineStep = 0;

    float lineTime = 0.0f;
    
    void Update()
    {
        // if (PlayerManager.Self.charPro.Input.GetButton(MoreMountains.CorgiEngine.ButtonType.Jump).State.CurrentState == MMInput.ButtonStates.ButtonDown && isReadyToShoot()) // 检查射击按键是否按下并且准备就绪  
        // {

        // }


        if (bShoot)
        {
            lineTime += Time.deltaTime;
            //抛物线的速度曲线
            curProgressSpeed =  ProgressSpeed * curve.Evaluate(lineTime);
         
            PathProgress += Time.deltaTime * curProgressSpeed * 3f / linePath.getLenght() ;//+  curve.Evaluate(PathProgress);
            nextPos = linePath.GetPosByProgress(PathProgress,true);
            // if (PathProgress > 1 && !bProgress2)
            //     PathProgress = 1;
            transform.position = Vector3.Lerp(transform.position, nextPos, PathProgress);


            forwardTime += Time.deltaTime;
            var fgs = 1 / forwardSpeed;
            if (forwardTime >= fgs && (lastPosition - transform.position) != Vector3.zero)
            {
                forwardTime -= fgs;
                transform.forward = (lastPosition - transform.position).normalized;
            }
        }

        if (bShoot && isCurveBullet)
        {
            if (bUp)
            {
                var intY = Mathf.Lerp(initY, initY + CurveHeight, PathProgress);
                transform.position = new Vector3(transform.position.x, intY, transform.position.z);
                if (PathProgress > CurveUpProgress)
                {
                    bUp = false;
                    //重定向line的 起始点
                    linePath.startPos = transform.position;
                    linePath.endPos = GetCurTargerPosition();
                    PathProgress = 0;
                    bProgress2 = true;
                    LineStep = 1;
                }
            }
            else
            {
                // var itY =  Mathf.Lerp(transform.position.y,linePath.endPos.y,Time.deltaTime);
                // transform.position = new Vector3(transform.position.x,itY,transform.position.z);
            }
        }

        if (bShoot && isTrackingBullet && (!isCurveBullet || !bUp) )
        {
            TrackingTime += Time.deltaTime;
            var ags = 1 / angularSpeed;
            if (TrackingTime >= ags && curTrackInt < TrackingInt)
            {

                TrackingTime -= ags;
                curTrackInt++;
            
                linePath.startPos = transform.position;
                linePath.endPos = GetCurTargerPosition();
                PathProgress = 0.1f;
            }

            //Rigidbody rb = bulletInstance.GetComponent<Rigidbody>();
        }

        lastPosition = transform.position;
    }

   Vector3 GetCurTargerPosition(){
        return  PlayerManager.Self.playerObj.transform.position   + new Vector3(0,0.75f,0);
   }
//跟踪流程
    void Tracking(){

    }

    void OnDestroy()
    {
        // Debug.Log("PathProgress:" + PathProgress);
        // Debug.Log("nextPos:" + nextPos);
    }
}

    