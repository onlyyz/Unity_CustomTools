using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;


//这个属性会让Editor与TestGUILayout绑定起来
[CustomEditor(typeof(GUILayoutTest))] 
[CanEditMultipleObjects] 
public class TestEditor : Editor
{

    //注意访问修饰是 public
    //这个方法就是 TestGUILayout 在Inspector中序列化展开面板的定义
    public override void OnInspectorGUI() {
        //先绘制默认的实例化展开
        base.OnInspectorGUI();

        //再在下方绘制我们自己的实例化展开
        if(GUILayout.Button("HelloWorld!")) {
            Debug.Log("HelloWorld!");
        }
        
        //最后记得调一下这个方法才能保存序列化结果
        serializedObject.ApplyModifiedProperties();
    }
    
}