using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;
public abstract  class ManagerBase<T> : SerializedMonoBehaviour  where T : SerializedMonoBehaviour 
{
     public  static T Self;
       
}
