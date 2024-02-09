using Cinemachine;
using UnityEditor;
using UnityEngine;
using Sirenix.OdinInspector.Editor;

namespace CustomPlugins
{
    using ForegroundShelter;
    using Zone;
    using Zone.Cinemachine;
    public class MenuItemTool : OdinEditorWindow
    {

        [MenuItem("GameObject/yz/测距工具")]
        public static void EditorSceneScale()
        {
            GameObject obj = new GameObject("Measuring Tool");
            GameObject obj2 = new GameObject("ScalePoint");
            obj2.transform.parent = obj.transform;
            obj.AddComponent<EditorSceneScaleTool>();
            EditorSceneScaleTool sceneScaleTool = obj.GetComponent<EditorSceneScaleTool>();

            sceneScaleTool.xAxis = true;
            sceneScaleTool.yAxis = true;
            sceneScaleTool.zAxis = true;
            sceneScaleTool.showScale = false;
            // sceneScaleTool.sizeType = DistanceType.MeterPoint;

            // sceneScaleTool.scaleObjectScaleColor = Color.white;
        }

        #region 区域相机
    
        [MenuItem("GameObject/yz/区域相机管理器", false, 5)]
        public static void RegionCam(MenuCommand menuCommand)
        {
       
            GameObject go = new GameObject("Zone Manager");
            GameObjectUtility.SetParentAndAlign(go, menuCommand.context as GameObject);
            Undo.RegisterCreatedObjectUndo(go, "Create " + go.name);
            Selection.activeObject = go;
            go.AddComponent<ZoneManager>();

        }
    
        [MenuItem("GameObject/yz/区域相机", false, 5)]
        public static void LimitRegion(MenuCommand menuCommand)
        {
            // Create a custom game object
            GameObject go = new GameObject("Trigger Zone");
            GameObjectUtility.SetParentAndAlign(go, menuCommand.context as GameObject);
            Undo.RegisterCreatedObjectUndo(go, "Create " + go.name);
            Selection.activeObject = go;

            //Trigger
            go.AddComponent<BoxCollider>();
            var TriggerZone = go.GetComponent<BoxCollider>();
            TriggerZone.size = new Vector3(4,4,2);
            TriggerZone.center = new Vector3(0,0,0);
            TriggerZone.isTrigger = true;
        
        
            GameObject camera = new GameObject("Camera");
       
            camera.SetActive(false);
            // camera.gameObject.SetActive(false);
       
            camera.GetComponent<Transform>().position = new Vector3(0.0f,6.0f,-20.0f);
        
            //Cinemachine
        
            var proper =  camera.AddComponent<CinemachineVirtualCamera>();
            CinemachineFramingTransposer body = proper.AddCinemachineComponent<CinemachineFramingTransposer>();
            camera.AddComponent<CinemachineConfiner>();
        
            // proper.enabled = false;
            proper.m_Lens.FieldOfView = 30.0f;
        
            body.m_XDamping = 0.5f;
            body.m_YDamping = 1.0f;
            body.m_ZDamping = 1.0f;
        
            body.m_CameraDistance = 15.0f;
            body.m_DeadZoneWidth = 0.2f;
            body.m_DeadZoneHeight = 0.1f;
        
        
            go.AddComponent<ZoneCinemachine>().VirtualCamera = camera.GetComponent<CinemachineVirtualCamera>();
            // go.GetComponent<ZoneCinemachine>().TriggerMask = LayerMask.GetMask("DefaultController");
            camera.transform.parent = go.transform;
        
        
        
            GameObject Limit = new GameObject("Limit");
            Limit.GetComponent<Transform>().position = new Vector3(0,0,-25);
            var LimitZone =  Limit.AddComponent<BoxCollider>();
            LimitZone.size = new Vector3(2,2,45);
            LimitZone.center = new Vector3(0,0,0);
            LimitZone.isTrigger = true;

            CinemachineConfiner confiner = camera.GetComponent<CinemachineConfiner>();
            confiner.m_ConfineMode = CinemachineConfiner.Mode.Confine3D;
            confiner.m_BoundingVolume = Limit.GetComponent<BoxCollider>();
            Limit.transform.parent = go.transform;
        }
    
        #endregion

        #region 前景遮挡

        //[MenuItem("GameObject/yz/前景遮挡管理器")]
        public static void CreateNearOcclusionManager(MenuCommand menuCommand)
        {
            GameObject go = new GameObject("NearOcclusion Manager");
            GameObjectUtility.SetParentAndAlign(go, menuCommand.context as GameObject);
            Undo.RegisterCreatedObjectUndo(go, "Create " + go.name);
            Selection.activeObject = go;
        
        
        
            GameObject NearOcclusionZone = new GameObject("NearOcclusion Zone");
        
            NearOcclusionZone.AddComponent<SheltePublisher>();
       
        
            GameObject NearOcclusionBox = new GameObject("NearOcclusion Box");
            GameObject NearOcclusionMesh = new GameObject("NearOcclusion Mesh");
            NearOcclusionBox.transform.parent = NearOcclusionZone.transform;
            NearOcclusionMesh.transform.parent = NearOcclusionZone.transform;
        
        }
        [MenuItem("GameObject/yz/前景遮挡")]
        public static void CreateNearOcclusion(MenuCommand menuCommand)
        {
            GameObject NearOcclusionZone = new GameObject("NearOcclusion Zone");
            GameObjectUtility.SetParentAndAlign(NearOcclusionZone, menuCommand.context as GameObject);
            Undo.RegisterCreatedObjectUndo(NearOcclusionZone, "Create " + NearOcclusionZone.name);
            Selection.activeObject = NearOcclusionZone;
        
            NearOcclusionZone.AddComponent<SheltePublisher>();
      
        
            GameObject NearOcclusionBox = new GameObject("NearOcclusion Box");
            GameObject NearOcclusionMesh = new GameObject("NearOcclusion Mesh");
            NearOcclusionBox.transform.parent = NearOcclusionZone.transform;
            NearOcclusionMesh.transform.parent = NearOcclusionZone.transform;
       
        }

        #endregion
    }

}