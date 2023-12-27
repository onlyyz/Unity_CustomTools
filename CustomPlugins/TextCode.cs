using System;
using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using GameCreator.Runtime.Variables;
using UnityEngine;

public class TextCode : MonoBehaviour
{
    // public Collider textCol;
    // public CinemachineVirtualCamera VirtualCamera;
    
    public string keyString;
    
    
    // private string boolKey;
    // private LocalNameVariables NameVar;
    void Start()
    {
        // textCol = GetComponentInChildren<BoxCollider>();
        // VirtualCamera = GetComponentInChildren<CinemachineVirtualCamera>();
        
        // NameVar =  transform.GetComponent<LocalNameVariables>();
        // boolKey = (string)NameVar.Get(keyString);
        // Debug.Log("<color=#4db8ff> 测试 ： " + NameVar.Get(keyString) + "  " + boolKey + "</color>" );
    }

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log(other.name);
    }
}
