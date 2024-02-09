using GameCreator.Runtime.VisualScripting;
using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.UIElements;


[CustomPropertyDrawer(typeof(InterObjectSendEventOrAction))]
public class InterObjectSendEventOrActionDrawer : PropertyDrawerAction
{
    
    public override VisualElement CreatePropertyGUI(SerializedProperty property)
    {
       
        var container = new VisualElement();
        
        SerializedProperty signalType = property.FindPropertyRelative("signalType");
        SerializedProperty actionType = property.FindPropertyRelative("actionType");
        SerializedProperty m_WaitToFinish = property.FindPropertyRelative("m_WaitToFinish");

        var interObject = new PropertyField(property.FindPropertyRelative("m_interObject"), "Inter 物体");
        var signalActions = new PropertyField(property.FindPropertyRelative("signalActions"));

        var msignal = new PropertyField(property.FindPropertyRelative("m_Signal"), "事件名称");
        var data  = new PropertyField(property.FindPropertyRelative("m_Data"),"携带数据");
        

        var signal = new EnumField("目标数量", (SignalType)signalType.enumValueIndex);
        signal.bindingPath = signalType.propertyPath;
        
        var action = new EnumField("Action 类型", (ActionType)actionType.enumValueIndex);
        action.bindingPath = actionType.propertyPath;
        
        var wait = new Toggle("等待Action运行结束");
        wait.value = m_WaitToFinish.boolValue;
        
        container.Add(interObject);
        
        container.Add(signal);
        container.Add(action);
        container.Add(signalActions);
        container.Add(wait);
        
        container.Add(msignal);
        container.Add(data);
        
    
        void UpdateFieldVisibility(SignalType currentOption)
        {
            var play = currentOption.ToString().Equals(SignalType.单目标.ToString());
            action.style.display = play ? DisplayStyle.Flex : DisplayStyle.None;
            wait.style.display = play ? DisplayStyle.Flex : DisplayStyle.None;
            
            //多数据
            msignal.style.display = !play ? DisplayStyle.Flex : DisplayStyle.None;
            data.style.display = !play ? DisplayStyle.Flex : DisplayStyle.None;
            
        }
        // 初始化字段的可见性类似于构造函数
        UpdateFieldVisibility((SignalType)signalType.enumValueIndex);
     
        signal.RegisterValueChangedCallback(evt =>
        {
            UpdateFieldVisibility((SignalType)evt.newValue);
        });
        
        void UpdateFieldVisibilityActionType(ActionType currentOption)
        {
            signalActions.style.display = 
                currentOption.ToString().Equals(ActionType.自定义.ToString())? 
                    DisplayStyle.Flex : DisplayStyle.None;
        }
        // 初始化字段的可见性类似于构造函数
        UpdateFieldVisibilityActionType((ActionType)actionType.enumValueIndex);
        action.RegisterValueChangedCallback(evt =>
        {
            UpdateFieldVisibilityActionType((ActionType)evt.newValue);
        });
        
        return container;
    }
}