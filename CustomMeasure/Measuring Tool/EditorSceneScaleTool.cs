using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using Unity.Mathematics;
using UnityEditor;
using UnityEditor.Searcher;
using UnityEngine;
using Color = UnityEngine.Color;

/// <summary>
/// scale megaring type
/// </summary>
public enum DistanceType
{
    MeterPoint ,
    MeterLine,
    MeterAll,
}

/// <summary>
/// Scale pevot type
/// </summary>
public enum WorldType
{
    Global = 0,
    Local = 1
}

/// <summary>
/// This script is for create a scaling tool in editor 3D space
/// 
/// </summary>
public  class EditorSceneScaleTool : MonoBehaviour
{
    [HideInInspector]
    public float CamDistance;
    [SerializeField] WorldType worldType; // see enum
    
    // [Header("覆盖范围"),Tooltip("地圖範圍")]
    [HideInInspector]
    [SerializeField] int scaleSize = 10; // scale size

    // [Range(1, 200),Tooltip("間隔")]
    [HideInInspector]
    [SerializeField] int unit = 1; // Show scale per Unit

    [Header("显示轴"),Tooltip("显示X轴")]
    public bool xAxis = false; // enable or disable axis
    [Tooltip("显示Y轴")]
    public bool yAxis = false; // enable or disable axis
    // [Tooltip("显示Z轴")]
    [HideInInspector]
    public bool zAxis = false; // enable or disable axis
    
    
    
    public bool showScale = false; // enable or disable scale
    public Color scaleObjectScaleColor = Color.magenta; // Castom resizable scale color
    [Tooltip("单位选择")]
    public DistanceType sizeType; // see enum
    
    public GUIStyle textStyle; // scale text style
    
    //优化
    Camera SVCam;
    Vector3[] nearPos;
    Vector3[] farPos;
    
#if UNITY_EDITOR

    /// <summary>
    /// This function always call in editor update
    /// </summary>
    public void OnDrawGizmos()
    {
        
        SVCam = SceneView.lastActiveSceneView.camera;
        nearPos = new Vector3[4];
        farPos = new Vector3[4];

        var IsflagH = math.abs(Vector3.Dot( SVCam.transform.forward,Vector3.right));
        if(IsflagH>0.5)
            return;
        var IsflagV = math.abs(Vector3.Dot( SVCam.transform.forward,Vector3.up));
        if(IsflagV>0.3)
            return;
        
        //计算视椎体near
        // CalcCameraFrustumCorners(Camera.main);
        CalcCameraFrustumCorners(SVCam);
       
        
        nearPos = new Vector3[4];
        
        CamDistance =new Vector3(0.0f,SVCam.transform.position.z,0.0f).magnitude;
        // CamDistance = Vector3.Distance(currenPos,transform.position);
        //计算绘制数量

        setUnit();
        
        if (worldType == WorldType.Global) transform.position = Vector3.zero;
        transform.eulerAngles = Vector3.zero;
        transform.localScale = Vector3.one;
        
        XAxisLine();
        YAxisLine();
        // ZAxisLine();
        
        
    }

    void setUnit()
    {
        if (CamDistance > 30 && CamDistance < 60)
            unit = 10;
        else if (CamDistance > 60 && CamDistance < 100)
            unit = 20;
        else if (CamDistance > 100 && CamDistance < 150)
            unit = 50;
        else if (CamDistance > 200 && CamDistance < 600)
            unit = 100;
        else if (CamDistance > 600 && CamDistance < 800)
            unit = 200;
        else if (CamDistance > 800 && CamDistance < 1000)
            unit = 500;
        else if (CamDistance > 1000 && CamDistance < 1500)
            unit = 1000;
        else if (CamDistance > 1500)
            unit = 0;
        
        // scaleSize = 20;
    }
    
    void CalcCameraFrustumCorners(Camera cam)
    {
        float nearWidth = cam.orthographicSize * 2 * cam.aspect;
        float nearHeight = cam.orthographicSize * 2;
        
        nearPos[0] = cam.transform.position + cam.transform.forward * cam.nearClipPlane - cam.transform.right * nearWidth / 2 + cam.transform.up * nearHeight / 2;
        nearPos[1] = cam.transform.position + cam.transform.forward * cam.nearClipPlane + cam.transform.right * nearWidth / 2 + cam.transform.up * nearHeight / 2;
        nearPos[2] = cam.transform.position + cam.transform.forward * cam.nearClipPlane - cam.transform.right * nearWidth / 2 - cam.transform.up * nearHeight / 2;
        nearPos[3] = cam.transform.position + cam.transform.forward * cam.nearClipPlane + cam.transform.right * nearWidth / 2 - cam.transform.up * nearHeight / 2;
         
       
        
        for (int i = 0; i < 4; i++)
        {
            //真实位置
            farPos[i] = nearPos[i] + new Vector3(0,0 ,(cam.farClipPlane - cam.nearClipPlane) );
            //卡死XY平面
            farPos[i].z = 0.0f;
        }
        // DrewIndex(nearPos);
        // DrewIndex(farPos);

        DrewIndex(nearPos, farPos);
    }

    //相机投影矩阵绘制
    public void OnDrawGizmos1(Camera cam)
    {
        //相机投影矩阵绘制
        Matrix4x4 start = Gizmos.matrix;
        Gizmos.matrix = Matrix4x4.TRS(cam.transform.position, cam.transform.rotation, Vector3.one);
        Gizmos.color = Color.yellow;
        Gizmos.DrawFrustum(Vector3.zero, cam.fieldOfView, cam.farClipPlane, 0, cam.aspect);
        Gizmos.color = Color.red;
        Gizmos.DrawFrustum(Vector3.zero, cam.fieldOfView, cam.farClipPlane, cam.nearClipPlane, cam.aspect);      
        Gizmos.matrix = start;

        //坐标辅助线绘制
        Gizmos.color = Color.red;
        Gizmos.DrawLine(cam.transform.position, cam.transform.position + cam.transform.right * 10);
        Gizmos.color = Color.green;
        Gizmos.DrawLine(cam.transform.position, cam.transform.position + cam.transform.up * 10);
        Gizmos.color = Color.blue;
        Gizmos.DrawLine(cam.transform.position, cam.transform.position + cam.transform.forward * 10);
    }
    
    //绘制使用相机
    void DreaPoinLine(Vector3[] poses)
    {
        Color originColor = Gizmos.color;
        Gizmos.color = Color.red;
        if( poses!=null && poses.Length>0 )
        {
            //Draw Sphere
            for (int i = 0; i < poses.Length; i++)
            {
                Gizmos.DrawSphere( poses[i], 0.2f );
            }
            //Draw Line
            Gizmos.color = Color.yellow;
            Vector3 lastPos = Vector3.zero;
            for (int i = 0; i < poses.Length; i++)
            {
                if( i > 0 )
                {
                    Gizmos.DrawLine( lastPos, poses[i] );
                }
                lastPos = poses[i];
            }
        }
        Gizmos.color = originColor;
    }
    
    //绘制顺序
    void DrewIndex(Vector3[] nearPos)
    {
        Color originColor = Gizmos.color;
        Gizmos.color = Color.red;
        Gizmos.DrawSphere( nearPos[0], 0.2f );
        
        Gizmos.color = Color.yellow;
        Gizmos.DrawSphere( nearPos[1], 0.2f );
        
        Gizmos.color = Color.blue;
        Gizmos.DrawSphere( nearPos[2], 0.2f );
        
        Gizmos.color = Color.green;
        Gizmos.DrawSphere( nearPos[3], 0.2f );
    }
    
    void DrewIndex(Vector3[] nearPos,Vector3[] farPos)
    {
        Color originColor = Gizmos.color;
        Gizmos.color = Color.red;
        Gizmos.DrawSphere( nearPos[0], 0.2f );
        Gizmos.DrawSphere( farPos[0], 0.2f );
        
        
        Gizmos.color = Color.yellow;
        Gizmos.DrawSphere( nearPos[1], 0.2f );
        Gizmos.DrawSphere( farPos[1], 0.2f );
        
        Gizmos.color = Color.blue;
        Gizmos.DrawSphere( nearPos[2], 0.2f );
        Gizmos.DrawSphere( farPos[2], 0.2f );
        
        Gizmos.color = Color.green;
        Gizmos.DrawSphere( nearPos[3], 0.2f );
        Gizmos.DrawSphere( farPos[3], 0.2f );

        for (int i = 0; i < 4; i++)
        {
            Gizmos.color = Color.magenta;
            Gizmos.DrawLine(nearPos[i],farPos[i]);
        }
    }
    /// <summary>
    /// Scale Z Axis
    /// </summary>
    private void ZAxisLine()
    {
        
        if (zAxis)
        {
            Gizmos.color = Color.blue;
            Line(Vector3.forward, Vector3.back, transform.position);
            // LineScaleLabel(Vector3.forward, Vector3.back, transform.position);
            
            
            Gizmos.color = Color.yellow;
            // Gizmos.DrawLine(new Vector3(0,0,0),new Vector3(0,5,5));
            // Gizmos.DrawLine(new Vector3(0,5,5),new Vector3(0,0,10));
        }
    }
    
    /// <summary>
    /// Scale Y Axis
    /// </summary>
    private void YAxisLine()
    {
        // if (yAxis)
        // {
        //     Gizmos.color = Color.green;
        //     Line(Vector3.up, Vector3.down, transform.position);
        //     LineScaleLabel(Vector3.up, Vector3.down, transform.position);
        //     
        //     var startPos = transform.position - new Vector3(Mathf.FloorToInt(scaleSize / unit) * unit, 0, 0);
        //     for (int i = 0; i < Mathf.FloorToInt(scaleSize/unit) * 2; i++)
        //     {
        //         
        //         if(sizeType != DistanceType.MeterPoint)
        //             Line(Vector3.up, Vector3.down, startPos + new Vector3(i * unit,0,0));
        //         LineScaleLabel(Vector3.up, Vector3.down, startPos + new Vector3(i * unit,0,0));
        //     }
        // }
        
        // if (yAxis)
        // {
        //     farPos[0] = new Vector3(0, farPos[0].y, 0);
        //     farPos[2] = new Vector3(0, farPos[2].y, 0);
        //     Gizmos.color = Color.green;
        //     Line(farPos[0], farPos[2], transform.position);
        //     LineScaleLabel(farPos[0], farPos[2], transform.position);
        //     
        //     var startPos = transform.position - new Vector3(Mathf.FloorToInt(scaleSize / unit) * unit, 0, 0);
        //     for (int i = 0; i < Mathf.FloorToInt(scaleSize/unit) * 2; i++)
        //     {
        //         
        //         if(sizeType != DistanceType.MeterPoint)
        //             Line(farPos[0], farPos[2], startPos + new Vector3(i * unit,0,0));
        //         LineScaleLabel(farPos[0], farPos[2], startPos + new Vector3(i * unit,0,0));
        //     }
        // }
        
        //Left
        Gizmos.color = Color.blue;
        var amendX1 = (math.floor(farPos[0].x/unit) * unit);
        var start0 = new Vector3(amendX1,(Mathf.Floor(farPos[0].y/unit) + 1)* unit,     farPos[0].z);
        var start2 = new Vector3(amendX1,(Mathf.Ceil(farPos[2].y/unit) - 1)* (unit),    farPos[2].z);
       
        //Right
        var amendX2 = (math.ceil(farPos[1].x/ unit) * unit);
        var start1 = new Vector3(amendX2,farPos[1].y,farPos[1].z);
        var start3 = new Vector3(amendX2,farPos[3].y,farPos[3].z);
       
        
        Gizmos.DrawLine(start0, start2);
        // Gizmos.DrawLine(start1, start3);
        
        //Camera Distance
        var HorizontalDistance =  math.abs(start0.x- start1.x);
        var verticalDistance =  math.abs(start0.y- start2.y);
       
        for (int i = 0; i < Mathf.FloorToInt(HorizontalDistance/unit); i++)
        {
            start0 += new Vector3(unit , 0, 0);
            start2 += new Vector3(unit , 0, 0);
            var startPos =new Vector3(start0.x, 0, 0);
            // start2 += new Vector3(unit , 0, 0);
            // if(sizeType != DistanceType.MeterPoint)
            
            if(sizeType != DistanceType.MeterPoint)
                Gizmos.DrawLine(start0, start2);
           
            
            
            // Line(Vector3.up, Vector3.down, startPos + new Vector3(i * unit,0,0));
            // LineScaleLabel(Vector3.up, Vector3.down, startPos + new Vector3(i * unit,0,0));

            
            LineScaleLabel(start0, start1);
            // Label(start0, "Test");
            // Meter(Vector3.up, Vector3.down, Vector3.zero);
        }
        

    }
    
    /// <summary>
    /// Scale X Axis
    /// </summary>
    private void XAxisLine()
    {
        if (xAxis)
        {
            Gizmos.color = Color.red;
            // Line(farPos[0], farPos[0], transform.position);
            Line(Vector3.right, Vector3.left, transform.position);
            // LineScaleLabel(Vector3.right, Vector3.left, transform.position);
        }
        
    }

    /// <summary>
    /// Generate Gizmoz Line
    /// </summary>
    /// <param name="v1">Direction 1</param>
    /// <param name="v2">Direction 2</param>
    /// <param name="start">Start point</param>
    private void Line(Vector3 v1, Vector3 v2, Vector3 start)
    {
       
        float tempDis = scaleSize;
        Gizmos.DrawLine(start, start+ (v1 * tempDis)) ;
        Gizmos.DrawLine(start, start + (v2 * tempDis));

    }
    
    private void LineScaleLabel(Vector3 v1, Vector3 start)
    {
        if (!showScale) return;
        if (sizeType == DistanceType.MeterPoint) Meter(v1, start);
        else if (sizeType == DistanceType.MeterLine) LineMeter(v1, start);
        else if (sizeType == DistanceType.MeterAll) LineMeterAll(v1,  start);
       
    }
    private void LineMeter(Vector3 v1, Vector3 v2, Vector3 start)
    {
        return;
        
    }
    
    private void LineMeter(Vector3 v1, Vector3 start)
    {
        return;
        
    }
    private void Meter(Vector3 v1, Vector3 v2, Vector3 start)
    {
        for (int i = 0; i <= scaleSize; i+=(1* unit))
        {
            Label(start + v1 * i, "( "+start.x +", " + i + " )");
            Label(start + v2 * i, "( "+ start.x +", " + (-i)+ " )" );
        }
    }
    
    private void Meter(Vector3 v1,  Vector3 v2)
    {
        var start = v1 - new Vector3(0.0f, v1.y, 0.0f);
        Vector3 dir = (v1 - start).normalized;
        var distance = Mathf.Abs(v1.x - v2.x);
        
        for (int i = 0; i <= distance; i+=(1* unit))
        {
            Label(start + dir * i, "( "+start.x +", " + i + " )");
            Label(start + (-dir) * i, "( "+ start.x +", " + (-i)+ " )" );
        }
    }
    private void LineMeterAll(Vector3 v1, Vector3 v2, Vector3 start)
    {
        for (int i = 0; i <= scaleSize; i+=(1* unit))
        {
            Label(start + v1 * i, i + "m");
            Label(start + v2 * i, -i + "m");
        }
    }
    private void LineMeterAll(Vector3 v1,  Vector3 v2)
    {
        var start = v1 - new Vector3(0.0f, v1.y, 0.0f);
        Vector3 dir = (v1 - start).normalized;
        var distance = Mathf.Abs(v1.x - v2.x);

        for (int i = 0; i <= scaleSize; i+=(1* unit))
        {
            Label(start + dir * i, i + "m");
            Label(start + (-dir) * i, -i + "m");
        }
    }
    GUIStyle gUI = new GUIStyle();
    /// <summary>
    /// Drow Gizmoz Text
    /// </summary>
    /// <param name="point">Point</param>
    /// <param name="str">scale number string</param>
    private void Label(Vector3 point, object str)
    {
        gUI.alignment = TextAnchor.MiddleCenter;
        gUI.normal = textStyle.normal;
        gUI.fontSize = 20;

        //相机空间
        // float4 camPos = SVCam.worldToCameraMatrix * new Vector4(SVCam.transform.position.x, SVCam.transform.position.y,SVCam.transform.position.z, 1.0f);
        //裁剪空间
        // float4 proPos = SVCam.projectionMatrix * camPos;
        // Debug.Log(GL.GetGPUProjectionMatrix(SVCam.projectionMatrix, false) * new Vector4(point.x,point.y,point.z, 1.0f));
      
        Handles.Label(point, "▪", gUI);
        Handles.Label(point, "   "+str.ToString(), textStyle);
    }


    //perspective
    bool atperCamera(Vector3 position)
    {
        Vector3 cameraPosition = SVCam.transform.position;
        Vector3 objectPosition = position;
        
        Vector3 cameraToObject = objectPosition - cameraPosition;
        cameraToObject.Normalize();
        
        Vector3 cameraForward = SVCam.transform.forward;
        
        float angle = Mathf.Acos(Vector3.Dot(cameraForward, cameraToObject)) * Mathf.Rad2Deg;
        float halfFOV = SVCam.fieldOfView * 0.5f;
        
        if (angle < halfFOV)
        {
            return true;
        }
        return false;
    }
    //正交
    bool atOrthCamera(Vector3 position)
    {
        Vector3 cameraPosition = SVCam.transform.position;
        Vector3 objectPosition = position;
        
        Vector3 cameraToObject = objectPosition - cameraPosition;
       
        float projection = Vector3.Dot(cameraToObject, SVCam.transform.forward);
        
        float halfHeight = SVCam.orthographicSize;
        float halfWidth = halfHeight * SVCam.aspect;
        
        if (Mathf.Abs(projection) < halfWidth)
        {
            return true;
        }
        return false;
    }
    

#endif
}