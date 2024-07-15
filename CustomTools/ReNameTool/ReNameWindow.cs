
using UnityEngine;
using UnityEditor;
using UnityEngine.UIElements;

namespace CustomPlugins.ToolWindow
{
    using ToolEditor;
    public class ReNameWindow : EditorWindow
    {
        private TextField prefixTextField;
        private TextField postfixTextField;
        private TextField removeStringTextField;
        private TextField matchingTextField;
        private TextField modifyTextField;
        private TextField reNameTextField;
        private IntegerField startCountIntegerField;
        private IntegerField reNameCountIntegerField;

        [MenuItem("Window/命名工具")]
        public static void ShowWindow()
        {
            ReNameWindow wnd = GetWindow<ReNameWindow>();
            wnd.titleContent = new GUIContent("命名工具");
            wnd.minSize = new Vector2(100, 350);
            wnd.maxSize = new Vector2(350, 350);
        }

        public void OnEnable()
        {
            // Create UI
            prefixTextField = new TextField("前缀");
            postfixTextField = new TextField("后缀");
            removeStringTextField = new TextField("移除的字符串");
            matchingTextField = new TextField("匹配字符串");
            modifyTextField = new TextField("替换字符串");
            reNameTextField = new TextField("批量重命名");
            startCountIntegerField = new IntegerField("后缀添加的起始序号");
            reNameCountIntegerField = new IntegerField("起始序号");

            // Add to the root
            rootVisualElement.Add(CreateSection(prefixTextField, () => ReNameTool.AddCustomPrefix(prefixTextField.value)));
            rootVisualElement.Add(CreateSection(postfixTextField, () => ReNameTool.AddCustomPostfix(postfixTextField.value)));
            rootVisualElement.Add(CreateSection(startCountIntegerField, () => ReNameTool.AddCustomPostfixNumber(startCountIntegerField.value)));
            rootVisualElement.Add(CreateSection(removeStringTextField, () => ReNameTool.Remove(removeStringTextField.value)));
            rootVisualElement.Add(CreateMatchingSection());
            rootVisualElement.Add(CreateRenameSection());
        }

        private VisualElement CreateSection(VisualElement field, System.Action action)
        {
            var container = new VisualElement();
            container.style.marginBottom = 10;
            container.Add(field);

            var button = new Button(action) { text = "确定更改" };
            container.Add(button);

            return container;
        }

        private VisualElement CreateMatchingSection()
        {
            var container = new VisualElement();
            container.style.marginBottom = 10;
            container.Add(matchingTextField);
            container.Add(modifyTextField);

            var button = new Button(() => ReNameTool.MatchingModifyString(matchingTextField.value, modifyTextField.value))
            {
                text = "确定更改"
            };
            container.Add(button);

            return container;
        }

        private VisualElement CreateRenameSection()
        {
            var container = new VisualElement();
            container.style.marginBottom = 10;
            container.Add(reNameTextField);
            container.Add(reNameCountIntegerField);

            var button = new Button(() => ReNameTool.ReDefine(reNameTextField.value, reNameCountIntegerField.value))
            {
                text = "确定更改"
            };
            container.Add(button);

            return container;
        }
    }
}
