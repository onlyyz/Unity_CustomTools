// EventManager.cs
using UnityEngine;

public class EventManager : MonoBehaviour
{
    // 定义一个委托类型，该委托接受一个字符串参数
    public delegate void StringEventHandler(string message);

    // 定义一个事件，使用上述委托类型
    public static event StringEventHandler OnMessageReceived;
     public void TriggerEvent(string message)
    {
        // 触发事件，通知所有订阅者
        OnMessageReceived?.Invoke(message);
    }
    
    // 单例实例
    // private static EventManager instance;

    // 公共的静态属性，用于访问单例实例
    // public static EventManager Instance
    // {
    //     get
    //     {
    //         // 如果仍然没有找到实例，创建一个新实例
    //         if (instance == null)
    //         {
    //             GameObject obj = new GameObject("EventManager");
    //             instance = obj.AddComponent<EventManager>();
    //         }
    //
    //         return instance;
    //     }
    // }
    // 触发事件的方法
   
}