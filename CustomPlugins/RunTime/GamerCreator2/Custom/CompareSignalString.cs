using System;
using UnityEngine;
using GameCreator.Runtime.Variables;


namespace GameCreator.Runtime.Common
{
    [Title("比较Signal变量中的String值")]
    [Category("Signal变量/比较Signal")]
    
    [Image(typeof(IconSearch), ColorTheme.Type.Yellow)]

    [Serializable] [HideLabelsInEditor]
    public class CompareSignalString : PropertyTypeGetBool
    {
        [SerializeField,GCLabel("GameObject")] protected PropertyGetGameObject m_gameObject;
        [SerializeField,GCLabel("Signal String名")] protected String KeyString;
        public override bool Get(Args args)
        {
            GameObject gameObject = m_gameObject.Get(args);
            string localString = (string)gameObject.GetComponent<LocalNameVariables>().Get(KeyString);
            foreach(var nv in args.variables)
            {
                if(nv.Name == KeyString)
                {
                    // Debug.Log((string)nv.Value + " " + localString.Equals((string)nv.Value));
                    return localString.Equals((string)nv.Value);
                }
            }
            Debug.LogError($"{args.Self}的信号不包含{localString}");
            return false;
        }

        public override string String => "信号 比较 "+ this.KeyString+ "值";
    }
}