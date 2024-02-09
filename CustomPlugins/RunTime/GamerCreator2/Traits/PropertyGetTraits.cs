using System;
using System.Globalization;
using Doozy.Runtime.UIManager.Components;
using UnityEngine;
using GameCreator.Runtime.Common;

namespace GameCreator.Runtime.Stats
{
    [Title("Attribute Value")]
    [Category("Stats/Attribute Value")]

    [Image(typeof(IconAttr), ColorTheme.Type.Blue)]
    [Description("Returns the current value of an Attribute")]
    
    [Serializable] [HideLabelsInEditor]
    public class PropertyGetTraits : PropertyTypeGetTraits
    {
        [SerializeField,GCLabel("物体")] public PropertyGetGameObject m_TraitsGameObject;
        [SerializeField,GCLabel("ATT")] public Attribute m_Attribute;
       
        public override string String => string.Format(
            "获取{0}上的{1}",
            this.m_TraitsGameObject,
            this.m_Attribute
        );

        public GameObject GetTraitsGameObject(Args args)
        {
            GameObject traits = this.m_TraitsGameObject.Get(args);
            if (traits == null)
            {
                traits = traits.GetComponent<InterObject>().GetTraitsGameObject();
            }
            if (traits == null)
            {
                traits = traits.GetComponentInParent<InterObject>().GetTraitsGameObject();
            }
            return traits;
        }
        
        
        public Attribute GetAttribute()
        {
            return this.m_Attribute;
        }
        public IdString GetAttributeID()
        {
            return this.m_Attribute.ID;
        }
        public Traits GetTraits(Args args)
        {
            return m_TraitsGameObject.Get(args).GetComponent<Traits>();
        }

        public int GetFluid(Args args)
        {
            Traits traits = GetTraits(args);
            return (int)traits.RuntimeAttributes.Get(GetAttributeID()).Value;
        }
        public void SetFluid(Args args,int value)
        {
            Traits traits = GetTraits(args);
            traits.RuntimeAttributes.Get(GetAttributeID()).Value = value;
        }
        
        public int GetType(Args args)
        {
            Traits traits = GetTraits(args);
            return (int)traits.RuntimeAttributes.Get(GetAttributeID()).Value;
        }
        
        // public void SetType<T>(Args args,string type, T value)
        // {
        //   GetTraits(args).RuntimeAttributes.Get(type)<T>.Value = value;
        // }
    }
}