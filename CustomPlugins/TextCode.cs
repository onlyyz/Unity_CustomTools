using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using UnityEngine;

public class TextCode : MonoBehaviour
{
    public Collider textCol;
    public CinemachineVirtualCamera VirtualCamera;
    void Start()
    {
        textCol = GetComponentInChildren<BoxCollider>();
        VirtualCamera = GetComponentInChildren<CinemachineVirtualCamera>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
