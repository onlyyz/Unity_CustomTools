using System;
using GameCreator.Runtime.Common;
using UnityEngine;

namespace GameCreator.Runtime.Variables
{
    [Serializable]
    public class FieldInterObjectSetLocalName : TFieldSetVariable
    {
        [SerializeReference]
        protected GameObject m_Variable = new GameObject();
        
        [SerializeField] protected IdPathString m_Name;
        
        public GameObject m_State 
            => m_Variable.GetComponent<InterObject>().GetStateGameObject();
        // CONSTRUCTORS: --------------------------------------------------------------------------

        public FieldInterObjectSetLocalName(IdString typeID)
        {
            this.m_TypeID = typeID;
        }
        
        // PUBLIC METHODS: ------------------------------------------------------------------------
        public override void Set(object value, Args _)
        {}

        public void SetState(object value, Args _)
        {
            LocalNameVariables instance = this.m_Variable.GetComponent<InterObject>().GetStateLocalVar();
            instance.Set(this.m_Name.String, value);
        }
        public void SetData(object value, Args _)
        {
            LocalNameVariables instance = this.m_Variable.GetComponent<InterObject>().GetDataLocalVar();
            instance.Set(this.m_Name.String, value);
        }

        public override object Get(Args _) => null;
        public object GetData(Args _)
        {
            LocalNameVariables instance = this.m_Variable.GetComponent<InterObject>().GetDataLocalVar();
            return instance != null ? instance.Get(this.m_Name.String) : null;
        }
        public object GetState(Args _)
        {
            LocalNameVariables instance = this.m_Variable.GetComponent<InterObject>().GetStateLocalVar();
            return instance != null ? instance.Get(this.m_Name.String) : null;
        }

        public override string ToString()
        {
            return string.Format(
                "{0}{1}",
                this.m_Variable,
                string.IsNullOrEmpty(this.m_Name.String) ? string.Empty : $"[{this.m_Name.String}]" 
            );
        }
    }
}