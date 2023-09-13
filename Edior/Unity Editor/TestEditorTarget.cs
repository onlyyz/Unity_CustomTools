using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

//这个属性会让Editor与TestGUILayout绑定起来
[CustomEditor(typeof(TestGUILayout))]  
//注意要加上这个属性，才能批量操作子物体
[CanEditMultipleObjects] 
public class TestEditorTarget : Editor
{
    TestGUILayout testGUILayout;
    
/*
 target 是一个UnityEngine.Object ，就是绑定到具体物体上，Mono的实例化对象
    因为我们绑定了TestGUILayout，因此这个target就是TestGUILayout实例
    通过强制类型转换，即可获取 TestGUILayout实例，在拓展编辑时对这个实例进行操作
    也别忘了 .gameobject .transform 可以获取到绑定 TestGUILayout 实例的物体，及其上面的其他组件
    */
    private void OnEnable() {
        testGUILayout = target as TestGUILayout;
    
    }


    //注意访问修饰是 public
    //这个方法就是 TestGUILayout 在Inspector中序列化展开面板的定义
    public override void OnInspectorGUI() {
        //先绘制默认的实例化展开
        base.OnInspectorGUI();

        //再在下方绘制我们自己的实例化展开
        if (GUILayout.Button("改名")) {
            testGUILayout.transform.name = "AAA";
            if (testGUILayout.GetComponent<MeshRenderer>() != null)
                DestroyImmediate(testGUILayout.GetComponent<MeshRenderer>());
        }

        if (GUILayout.Button("改名子物体")) {

            if (testGUILayout.gameObject.transform.childCount > 0)
                testGUILayout.gameObject.transform.GetChild(0).name = "BBB";
        }

        if (GUILayout.Button("批量改名子物体")) {

            for (int i = 0; i < testGUILayout.gameObject.transform.childCount; ++i)
                testGUILayout.gameObject.transform.GetChild(i).name = "CCC";
        }
      
        
        if(GUILayout.Button("打印选中物体名称",GUILayout.MaxWidth(200))) {
            var sobjs = Selection.gameObjects;
            for (int i = 0; i < sobjs.Length; ++i)
                Debug.Log(sobjs[i].name);

        }
        //serializedObject.ApplyModifiedProperties();//最后记得调一下这个方法才能保存序列化结果
    }
}