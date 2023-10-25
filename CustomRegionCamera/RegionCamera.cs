using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using UnityEditor;
using UnityEngine;
using Zone;


public class RegionCamera : MonoBehaviour
{
    
   

    [MenuItem("GameObject/yz/Zone", false, 5)]
    public static void RegionCam(MenuCommand menuCommand)
    {
       
        GameObject go = new GameObject("Zone");
        // GameObjectUtility.SetParentAndAlign(go, menuCommand.context as GameObject);
        Undo.RegisterCreatedObjectUndo(go, "Create " + go.name);
        Selection.activeObject = go;
        go.AddComponent<ZoneSerialize>();

    }
    
    [MenuItem("GameObject/yz/Limit Region", false, 5)]
    public static void LimitRegion(MenuCommand menuCommand)
    {
        // Create a custom game object
        GameObject go = new GameObject("Limit Region");
        // GameObjectUtility.SetParentAndAlign(go, menuCommand.context as GameObject);
        Undo.RegisterCreatedObjectUndo(go, "Create " + go.name);
        Selection.activeObject = go;

        //Trigger
        go.AddComponent<BoxCollider>();
        go.GetComponent<BoxCollider>().size = new Vector3(4,4,2);
        go.GetComponent<BoxCollider>().center = new Vector3(0,0,0);
      
        
        
        GameObject camera = new GameObject("Virtual Camera");
        var proper =  camera.AddComponent<CinemachineVirtualCamera>();
        proper.m_Lens.FieldOfView = 30.0f;
            
        camera.AddComponent<CinemachineConfiner>();
        camera.GetComponent<Transform>().position = new Vector3(0.0f,6.0f,-20.0f);
        
        //Cinemachine
        var cmvc = camera.GetComponent<CinemachineVirtualCamera>();
        CinemachineFramingTransposer body = cmvc.AddCinemachineComponent<CinemachineFramingTransposer>();
        
        body.m_XDamping = 0.5f;
        body.m_YDamping = 0.0f;
        body.m_ZDamping = 1.0f;
        
        body.m_CameraDistance = 15.0f;
        body.m_DeadZoneWidth = 0.2f;
        body.m_DeadZoneHeight = 0.1f;
        
        camera.transform.parent = go.transform;
        
        
        
        GameObject Limit = new GameObject("Limit Region");
        Limit.AddComponent<BoxCollider>();
        Limit.GetComponent<Transform>().position = new Vector3(0,0,-25);
        Limit.GetComponent<BoxCollider>().size = new Vector3(2,2,45);
        Limit.GetComponent<BoxCollider>().center = new Vector3(0,0,0);


        CinemachineConfiner confiner = camera.GetComponent<CinemachineConfiner>();
        confiner.m_ConfineMode = CinemachineConfiner.Mode.Confine3D;
        confiner.m_BoundingVolume = Limit.GetComponent<BoxCollider>();
        Limit.transform.parent = go.transform;
    }
}
