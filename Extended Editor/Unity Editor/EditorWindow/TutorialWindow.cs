using UnityEngine;
using UnityEditor;
public class TutorialWindow : EditorWindow
{
    private static TutorialWindow window;
    [MenuItem("Tool/LearnEditor/02-TutorialWindow")]
    private static void CreatWindow() {
        //通过顶部菜单中添加按钮，来创建出窗口实例
        window = GetWindow<TutorialWindow>(false, "TutorialWindow", true);
        window.Show();
    }
    
    private GameObject obj;
    //成为窗口实例的一个字段
    //可通过 Selection 获取到选中对象上的某个字段进行序列化
    //或是结合 Editor OnInspector 中target上某个脚本的字段进行序列化
    
   
    private void OnGUI() {
        obj = EditorGUILayout.ObjectField("带刚体的GameObj的序列化", obj, typeof(GameObject),true) as GameObject;
    
        
        // ------------------------ValueTypeField
        // isValueTypeField();
        // ------------------------DelayedValueTypeField
        // isDelayedValueTypeField();
        //------------------------ColorField and CurveField
        isColororCurve();
        //------------------------Popup/IntPopup/EnumPopup
        isPopuporIntPopuporEnumPopup();
        //------------------------MaskField 
        isMaskField();
        //------------------------EnumFlagsField 
        isEnumFlagsField();
        //------------------------DropdownButtonDropdownButton 
        isDropdownButton();
        //------------------------Slider/IntSlider
        isSlider();
    }
    
    
    
    // ------------------------ValueTypeField
    private int i;
    private long l;
    private float f;
    private double d;

    string str_Field;

    string str_Area = new string("default Str"); //注意这里必须初始化，不能起手用空字符串否则无法展开 TextArea

    private Vector2 v2;
    private Vector2Int v2int; //这个可能不常见，分量由int组成不是float

    private Vector3 v3;
    private Vector3Int v3int;

    private Vector4 v4;

    private Rect rec;
    private RectInt recint;

    private Bounds b;//Unity自带的碰撞盒（Collider组件里有用到），通过 centerPos中心点 + extentsSize半尺向量 定义
    private BoundsInt bint;//创建后可以通过属性获取到 max min Point，设置max minPoint，并通过方法判断碰撞/包含

    private Quaternion quaternion;//四元数可以通过 V4 转换得到，或者自己起一堆 floadField 来录入
    private Vector4 qv4;

    private Vector2 scrollViewRoot;
    void isValueTypeField()
    {
        //如果东西多，一定要用 ScrollView 包一下，否则必须拉伸窗口才能看见所有控件
        scrollViewRoot = EditorGUILayout.BeginScrollView(scrollViewRoot);

        i = EditorGUILayout.IntField("整数域", i);
        l = EditorGUILayout.LongField("长整数域", l);
        f = EditorGUILayout.FloatField("单精度浮点域", f);
        d = EditorGUILayout.DoubleField("双精度浮点域", d);

        str_Field = EditorGUILayout.TextField("字符串域（单行）", str_Field);

        str_Area = EditorGUILayout.TextArea(str_Area); //【注意】这里不能加string标题

        v2 = EditorGUILayout.Vector2Field("Vector2域", v2);
        v2int = EditorGUILayout.Vector2IntField("Vector2Int域", v2int);

        v3 = EditorGUILayout.Vector3Field("Vector3域", v3);
        v3int = EditorGUILayout.Vector3IntField("Vector3Int域", v3int);

        v4 = EditorGUILayout.Vector4Field("Vector4域", v4);

        rec = EditorGUILayout.RectField("Rect域", rec);
        recint = EditorGUILayout.RectIntField("RectInt域", recint);

        b = EditorGUILayout.BoundsField("Bounds域", b);
        bint = EditorGUILayout.BoundsIntField("BoundsInt域", bint);

        qv4 = EditorGUILayout.Vector4Field("Quaternion域", qv4); //四元数用Vector4Field 加个转换就好了
        quaternion = new Quaternion(qv4.x, qv4.y, qv4.z, qv4.w);

        EditorGUILayout.EndScrollView();
    }
    
    //------------------------isDelayedValueTypeField
    private string str;
    void isDelayedValueTypeField()
    {
         i = EditorGUILayout.DelayedIntField("整数域", i);
                f = EditorGUILayout.DelayedFloatField("单精度浮点域", f);
                d = EditorGUILayout.DelayedDoubleField("双精度浮点域", d);
                str = EditorGUILayout.DelayedTextField("字符串域", str);
    }
    
    //------------------------ColorField and CurveField
    //特别注意这里起手的初始化 否则编辑时会报错
    private Color color;
    public AnimationCurve curve = new AnimationCurve();
    private Gradient gradient = new Gradient();
    
    //标签是一个string（CompareTag传参就是String）
    private string tag;
    //layer则是一个int，是通过二进制掩码的方式运作的
    private int layer = 0; 
    void isColororCurve()
    {
        color = EditorGUILayout.ColorField("颜色Field", color);
        curve = EditorGUILayout.CurveField("动画曲线Field", curve);
        gradient = EditorGUILayout.GradientField("Gradient渐变Field", gradient);
        
        //特别注意这个别写反了！！！
        tag = EditorGUILayout.TagField("Tag标签Field",tag);
        //可用于Phy各种碰撞Cast的Mask中
        layer = EditorGUILayout.LayerField("Layer标签Field",layer);
    }
    
    
    
    //------------------------Popup/IntPopup/EnumPopup
    private int popup;
    private string[] popupSelection = new string[] { "选项1", "选项2", "选项3" };
    private int intPopup;
//这里是定义展示在Editor那边的选项名称
    private string[] intPopupSelection = new string[] { "选项1", "选项2", "选项3" };
//这里是定义选项对应的数值
    private int[] PopupValue = new int[] { 857, 114514, 2022 };
//枚举选项可以通过 ToString() 拿到代码里的名称，也可以通过强转 (int) 拿到其数值
    public enum Choise
    {
        选项一 = 857, 
        选项二 = 114514,
        选项三 = 2022
    }
    private Choise cs;
    void isPopuporIntPopuporEnumPopup()
    {
        popup = EditorGUILayout.Popup("Popup", popup, popupSelection);
        //输出 [0,1,2...] Index 下标值
        //Debug.Log("popup：" + popup);
        intPopup = EditorGUILayout.IntPopup("IntPopup", intPopup, intPopupSelection, PopupValue);
        //输出与 PopupValue 对应
        //Debug.Log("intPopup：" + intPopup);
        //通常需要加强制类型转换
        cs = (Choise)EditorGUILayout.EnumPopup("EnumPopup", cs); 
        //输出枚举选项，通过ToString可获取选项名称，(int)获取指定值
        //Debug.Log("EnumPopup：" + cs.ToString() + " " + (int)cs);
    }
    
    //------------------------MaskField 
    private int mask;
    static string[] options = new string[] { "CanJump", "CanShoot", "CanSwim" };
    void isMaskField()
    {
        mask = EditorGUILayout.MaskField("Mask Field", mask,options);
        //Debug.Log(mask);
    }
    
    //------------------------EnumFlagsField 
    public enum WorkDay
    {
        None=0,                 //【特别注意】 这里顶上必须加一个 None = 0 
        星期一=1,               // 并且按照二进制掩码的位次组织 Enum 的数值对应
        星期二=2,               // 否则使用会显得不正常
        星期三=4,
        星期四=8,
        星期五=16,
        星期六=32,
        星期日=64,
        NormalWorkDay=1+2+4+8+16     //我们可以加一些特定的组合项，数值指定为对应的掩码组合
    };
    WorkDay workDayMask;
    int Daymask;
        void isEnumFlagsField (){workDayMask = (WorkDay)EditorGUILayout.EnumFlagsField("EnumFlagsField", workDayMask);
            Daymask = (int)workDayMask;//虽然接收是用Enum，但二进制掩码应当转成int使用
            //Debug.Log(Daymask);
            
        }
    //------------------------DropdownButtonDropdownButton 
    void isDropdownButton()
    {
        bool isShow = false;

        if (EditorGUILayout.DropdownButton(new GUIContent("显示组件"), FocusType.Passive,GUILayout.MaxWidth(300)))
            isShow = !isShow;
        
        if(isShow) {

            GUILayout.Label("加油！", GUILayout.MaxWidth(200));
            GUILayout.Label("加油！！", GUILayout.MaxWidth(200));
            GUILayout.Label("加油！！！", GUILayout.MaxWidth(200));

            if (GUILayout.Button("加油", GUILayout.MaxWidth(200)))
                Debug.Log("加油！！！");
        }
    }
        
    
    //------------------------Slider/IntSlider
    private float sliderValue;
    private int intSliderValue;
    //MinMaxSlider
    private float minValue;
    private float maxValue;
    void isSlider()
    {
        //通过Label来展示数值
        EditorGUILayout.LabelField("Float Value：",sliderValue.ToString()); 
        sliderValue = EditorGUILayout.Slider("Float Slider", sliderValue,0,100);
//通过Label来展示数值
        EditorGUILayout.LabelField("Int Value：",intSliderValue.ToString()); 
        intSliderValue = EditorGUILayout.IntSlider("Int Slider", intSliderValue, 0, 100);

        
        
        //MinMaxSlider
        
        EditorGUILayout.MinMaxSlider("float [0,100]",ref minValue, ref maxValue, 0, 100);
        //最好用带延迟的，输入过程中不会改变上面滑动条的显示
        minValue = EditorGUILayout.DelayedFloatField("区间左端：", minValue);
        //这里如果用户 Focus On，Focus Out 或是 摁下Enter之前，滑动条的控制将失效
        maxValue = EditorGUILayout.DelayedFloatField("区间右端：", maxValue);
    }
}

