using UnityEngine;
using UnityEngine.Events;

public class GameObjectFilter : MonoBehaviour
{
    [System.Serializable]
    public class ScriptFilterEvent : UnityEvent<GameObject>
    {
    }

    [SerializeField]
    private ScriptFilterEvent onFilterEvent;

    void Start()
    {
        // 在这里执行过滤操作，你可以在Start方法中调用，或者在编辑器中手动触发
        FilterGameObjects();
    }

    void FilterGameObjects()
    {
        foreach (Transform child in transform)
        {
            // 触发事件，将子物体传递给订阅者
            onFilterEvent.Invoke(child.gameObject);
        }
    }
}