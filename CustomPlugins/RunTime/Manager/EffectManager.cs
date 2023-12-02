using UnityEngine;
using System;
using System.Collections.Generic;

public class FilterMonoBehavioursByName : MonoBehaviour
{
    // 作为条件的脚本
    public MonoBehaviour filterScript;

    // 存储过滤后的脚本组件的数组
    public List<MonoBehaviour> filteredScripts = new List<MonoBehaviour>();

    // 保存 filterScript 的名字作为全局量
    private string filterScriptName;

    void Awake()
    {
        if (filterScript != null)
        {
            // 保存 filterScript 的名字作为全局量
            filterScriptName = filterScript.name;

            // 将 filterScript 的类型改为你的脚本的类型
            filterScript = GetComponent(filterScript.GetType()) as MonoBehaviour;
            
            Debug.Log(filterScript.GetType());
        }
    }

    void Start()
    {
        if (!string.IsNullOrEmpty(filterScriptName))
        {
            // 获取子物体的所有 MonoBehaviour 类型的脚本组件
            MonoBehaviour[] allScriptsOnChildren = GetComponentsInChildren<MonoBehaviour>(true);

            // 过滤出满足条件的脚本组件
            foreach (MonoBehaviour script in allScriptsOnChildren)
            {
                if (IsMyMonoBehaviour(script) && script.name == filterScriptName)
                {
                    filteredScripts.Add(script);
                }
            }

            // 在这里执行你希望执行的操作，使用 filteredScripts 数组
            OperateOnFilteredScripts();
        }
        else
        {
            Debug.LogError("请在Inspector窗口中选择有效的脚本类型！");
        }
    }

    void OperateOnFilteredScripts()
    {
        // 在这里可以执行你希望执行的操作，使用 filteredScripts 数组
        foreach (MonoBehaviour script in filteredScripts)
        {
            Debug.Log($"操作满足条件的脚本：{script.name}");
            // 在这里执行你的操作，例如：script.DoSomething();
        }
    }

    bool IsMyMonoBehaviour(MonoBehaviour script)
    {
        // 在这里定义你自己的逻辑来判断是否是你的 MonoBehaviour 类型
        // 例如，检查命名空间或者其他标识符，这里示例使用 filterScriptName
        return script.GetType().Namespace == filterScriptName;
    }
}