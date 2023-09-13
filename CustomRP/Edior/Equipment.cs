using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

 
[ExecuteAlways]
public class Equipment : MonoBehaviour
{
    //指定输出文本框
    public UnityEngine.UI.Text messageText;
//存储临时字符串
    System.Text.StringBuilder info = new System.Text.StringBuilder();

    
    //FPS
    // 记录帧数
    private int _frame;
    // 上一次计算帧率的时间
    private float _lastTime;
    // 平均每一帧的时间
    private float _frameDeltaTime;
    // 间隔多长时间(秒)计算一次帧率
    private float _Fps;
    private const float _timeInterval = 0.5f;

     
    private void Awake()
    {  
        //FPS
       
        //将输出文本框置空
        messageText.text = "";
    }

    void DebugEquipment()
    {
        //FPS
        GetMessage("FPS",  "<color=red>" + "FPS" + "</color>");
        //CPU类型
        GetMessage("CPU",  "<color=red>" + SystemInfo.processorType.ToString() + "</color>");
        //显卡类型
        GetMessage("显卡类型","<color=#66ff66>"+SystemInfo.graphicsDeviceType.ToString() + "</color>");
        //显卡名称
        GetMessage("显卡名称","<color=blue>"+SystemInfo.graphicsDeviceName+"</color>");
        //显卡是否支持多线程渲染
        GetMessage("显卡是否支持多线程渲染", "<color=#66ff66>"+ SystemInfo.graphicsMultiThreaded.ToString()+"</color>");
        //支持的渲染目标数量
        GetMessage("支持的渲染目标数量", "<color=#66ff66>" + SystemInfo.supportedRenderTargetCount.ToString()+"</color>");
        
        //操作系统
        GetMessage("操作系统", "<color=red>" + SystemInfo.operatingSystem+"</color>");
        //系统内存大小
        GetMessage("系统内存大小MB","<color=blue>"+ SystemInfo.systemMemorySize.ToString()+"</color>");
        
        //输出
        messageText.text = info.ToString();

    }


    
    void Update()
    {

        //退出
        if (Input.GetKeyUp("escape"))
        {
            if (Input.GetKeyUp("escape"))
            {
                Application.Quit();
            }
        }
        DebugEquipment();
    }
    
    void GetMessage(params string[] str)
    {
        if(str.Length==2)
        {
            info.AppendLine(str[0]+":"+str[1]);
        }

    }  

}