using UnityEngine;
public class ScriptA : MonoBehaviour
{
    public string KeyString;
    void Start()
    {
        // 订阅事件
        EventManager.OnMessageReceived += HandleMessage;
    }

    void HandleMessage(string message)
    {
        Debug.Log(KeyString + ": " + message);
    }
}