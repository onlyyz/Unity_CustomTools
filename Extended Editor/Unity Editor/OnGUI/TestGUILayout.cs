using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestGUILayout : MonoBehaviour
{
    [SerializeField]
    private Texture AreaTexture;

    private bool toggle;

    private int nowSelectionIdx = 0;
    private int lastSelectionIdx = -1; //保证启动时响应默认选择
    private int nowToggleIdx = 0;
    private int lastToggleIdx = -1;
    private string[] buttonNames = new string[] { "星期一", "星期二", "星期三", "星期四", "星期五", "星期六" };
    private int maxButtonPerLine = 3;

    private string areaText = "areaText";
    private string fieldText = "fieldText";

    private int heigth;//高度递推

    private float barHorizontalValue = 0;
    private float barVerticlaValue = 0;

    private Vector2 scrollViewRoot;

    [SerializeField]
    private GUIContent content;

    [SerializeField]
    private GUIStyle style;

    [SerializeField]
    private GUISkin skin;

    private void OnGUI()
    {

        //GUILayout.BeginArea(new Rect(350, heigth, 200, 250));
        //GUILayout.EndArea();
        
        //Part - 1
        isToggle();
        //Part - 2
        // isTextArea();
        //Part - 3
        // isSlider();

        //Part - 4
        // isHorizontal();
        //Part - 5
        // isAreaScrollView();
        //Part - 6
        //isRepeatButton();

    }
    ///Part - 1
    public void isToggle()
    {
        toggle = GUI.Toggle(new Rect(0, heigth, 150, 30), toggle, "是否展开Area");
        //一个重要的技巧就是使用Toggle来控制Layout的展开
        if (toggle)
        {

            //这个Area通过传入Rect定义位置大小，其实并没有接入到Layout中
            GUILayout.BeginArea(new Rect(0, heigth, 350, 200), AreaTexture);

            //Width Height传参可省略
            if (GUILayout.Button("测试按钮", GUILayout.Width(100), GUILayout.Height(25)))
            {
                Debug.Log("Click 按钮 !");
            }
            //分三一行
            nowSelectionIdx = GUILayout.SelectionGrid(nowSelectionIdx, buttonNames, maxButtonPerLine);
            if (nowSelectionIdx != lastSelectionIdx)
            {

                Debug.Log("SelectionGrid 选择了：" + buttonNames[nowSelectionIdx]);
                lastSelectionIdx = nowSelectionIdx;

            }

            //无法定义 maxButtonPerLine 只能展开为一行
            nowToggleIdx = GUILayout.Toolbar(nowToggleIdx, buttonNames);

            if (nowToggleIdx != lastToggleIdx)
            {

                Debug.Log("Toolbar 选择了：" + buttonNames[nowToggleIdx]);
                lastToggleIdx = nowToggleIdx;
            }


            GUILayout.Label("Hi 这是一个Label", style);
            GUILayout.Box("但是Label太不明显了所以可以用Box代替", GUILayout.Width(280), GUILayout.Height(25));

            GUILayout.EndArea();
            //一定记得上面Begin展开Area
            //下面就要End结束 ，不然会报错
        }
    }
    ///Part - 2
    public void isTextArea()
    {
        //使用GUI版本的 TextArea TextField 是定死的宽度
        //heigth = 0;
        //areaText = GUI.TextArea(new Rect(0, heigth, 100, 30), areaText);
        //heigth += 30;
        //fieldText = GUI.TextField(new Rect(0, heigth, 100, 30), fieldText);
        //heigth += 30;

        //使用GUILayout 下的 TextArea TextField
        //常需要结合 MinWidth Option 否则在输入字符过短时宽度可能收缩到无法使用
        areaText = GUILayout.TextArea(areaText, GUILayout.MinWidth(200), GUILayout.MaxHeight(30));
        fieldText = GUILayout.TextField(fieldText, GUILayout.MinWidth(200), GUILayout.MaxHeight(30));

    }
    ///Part - 3
    public void isSlider()
    {
        //这个横条不加MinWidth会出错
        //Slider的特点是，当我们拉到末端时一定会给出 min/max 读数
        barHorizontalValue = GUILayout.HorizontalSlider(barHorizontalValue, 0f, 500f, GUILayout.MinWidth(200));
        barVerticlaValue = GUILayout.VerticalSlider(barVerticlaValue, 0f, 500f);

        GUILayout.Box("横条读数：" + barHorizontalValue);
        GUILayout.Box("纵条读数：" + barVerticlaValue);

        //【注意】上下两块代码应注释掉其中一块，否则最终输出值按后方的 Scrollbar 影响

        //Scrollbar的特点是通过一个 blockSize 控制滑块的大小
        //由于滑块自身大小的限制，最终读数范围是 [min,max-blcokSize] 
        // barHorizontalValue = GUILayout.HorizontalScrollbar(barHorizontalValue, 25, 0f, 500f, GUILayout.MinWidth(200));
        // barVerticlaValue = GUILayout.VerticalScrollbar(barVerticlaValue, 25, 0f, 500f);

        // GUILayout.Box("横条读数：" + barHorizontalValue);
        // GUILayout.Box("纵条读数：" + barVerticlaValue);
    }
    ///Part - 4
    public void isHorizontal()
    {
        GUILayout.BeginHorizontal();

        GUILayout.BeginVertical();
        GUILayout.Box("1");
        GUILayout.Box("2");
        GUILayout.Box("3");
        GUILayout.EndVertical();

        GUILayout.BeginVertical();
        GUILayout.Box("4");
        GUILayout.Box("5");
        GUILayout.Box("6");
        GUILayout.EndVertical();

        GUILayout.BeginVertical();
        GUILayout.Box("7");
        GUILayout.Box("8");
        GUILayout.Box("9");
        GUILayout.EndVertical();

        GUILayout.EndHorizontal();

        //最终会输出一个数字九键排列
        // 1 4 7
        // 2 5 8
        // 3 6 9
    }
    ///Part - 5
    public void isAreaScrollView()
    {
        // 这里要套一个Area来定义控件的区域，基于这个区域展开ScrollView
        //否则将基于整个屏幕展开ScrollView
        GUILayout.BeginArea(new Rect(0, 0, 350, 300));

        //scrollViewRoot 是一个Vector2 Mask内互动区域当前的位置锚点
        scrollViewRoot = GUILayout.BeginScrollView(scrollViewRoot);


        //默认纵向布局
        //Heigth 总计600 >> 400 因此纵向就会展开滑动条
        //Width 总计300 << 400 因此横向不会展开滑动条
        GUILayout.Button("Buttons", GUILayout.Height(200), GUILayout.Width(300));
        GUILayout.Button("Buttons", GUILayout.Height(200), GUILayout.Width(300));
        GUILayout.Button("Buttons", GUILayout.Height(200), GUILayout.Width(300));

        GUILayout.EndScrollView();

        GUILayout.EndArea();
    }
    //Part - 6
    public void isRepeatButton()
    {
        //Button必须是完成一次Click，才会返回true（触发）
        //而ReapeatButton会在IPointDown的瞬间触发

        if (GUILayout.RepeatButton("啊", GUILayout.MaxWidth(200)))
            Debug.Log("啊啊啊");
    }
}
