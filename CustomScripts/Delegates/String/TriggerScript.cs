using UnityEngine;

public class TriggerScript : MonoBehaviour
{
    private EventManager Publicer = new EventManager();
    void OnTriggerEnter(Collider other)
    {
        Debug.Log("测试");
        // 触发事件并发送字符串
        Publicer.TriggerEvent("Player entered trigger zone!");
        
    }
}