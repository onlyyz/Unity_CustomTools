using System;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;
using Cinemachine;
using MoreMountains.Tools;
using Unity.Transforms;

#if UNITY_EDITOR
using UnityEditor;
#endif

namespace CustomPlugins.Zone
{
    using SO;
    using Data;
    public class ZoneManager : SerializedMonoBehaviour
    {
        public bool isDebug;
        [Header("触发层：SystemTrigger")]
        private LayerMask _triggerMask;
        [Header("Non-触发层：Ignore Raycast")]
        private LayerMask _nonTriggerMask;
        

       
        
        [Button("保存数据"), ButtonGroup]
        public void ShowValue1()
        {
            _triggerMask = LayerMask.NameToLayer("SystemTrigger");
            _nonTriggerMask = LayerMask.NameToLayer("Ignore Raycast");
            if(isDebug) Debug.Log("触发层："+ LayerMask.LayerToName(_triggerMask) + "   " +
                                  "Non-触发层：" +  LayerMask.LayerToName(_nonTriggerMask));
            
#if UNITY_EDITOR           
            if (ZoneSetting==null)

                CreateField();
#endif
            ClearSomeData();
            SetDataArray();
      
            if(isDebug) Debug.Log("数据保存");
        }

        [Button("加载数据"), ButtonGroup]
        public void ShowValue2()
        {
            _triggerMask = LayerMask.NameToLayer("SystemTrigger");
            _nonTriggerMask = LayerMask.NameToLayer("Ignore Raycast");
            
            GetDataArray();
            if(isDebug) Debug.Log("数据加载");
        }
   
    
        
        public ZoneDataSO ZoneSetting;
        [Title("触发颜色Tag")]
        [HideInInspector]
        public string nameTag = "/ZoneCam/";
        string GoName;
        
        // [DictionaryDrawerSettings]
        [DictionaryDrawerSettings,HideInInspector]
        public Dictionary<string, ZoneData> DictZoneData = new Dictionary<string, ZoneData>();
    
        public void SetDataArray()
        {
            Transform father = GetComponent<Transform>();
        
            for (int i = 0; i < father.childCount; i++)
            {
                ZoneData zoneData = new ZoneData();
                if (ZoneSetting.DictZoneData.ContainsKey(father.GetChild(i).name))
                {
#if UNITY_EDITOR
                    EditorUtility.DisplayDialog("错误!", " 有同名区域: " + father.GetChild(i).name + " 请更改，保持唯一性", "OK");
#endif
                    return;
                }

                //TODO:  test the Tag name and the Trigger
                var zone = father.GetChild(i);
                var zone3D = zone.GetComponent<MMCinemachineZone3D>();
                GoName = zone.name;
                // if(isDebug) Debug.Log( " Name： " + GoName);
            
                //Zone
                zoneData.ZoneName = GoName;
                zoneData.ZonePos = zone.position;
                zoneData.ZoneCenter = zone.GetComponent<BoxCollider>().center;
                zoneData.ZoneSize = zone.GetComponent<BoxCollider>().size;

                for (int j = 0; j < zone.childCount; j++)
                {
                    zone.GetChild(j).gameObject.layer = _nonTriggerMask;
                    if (zone.GetChild(j).GetComponent<BoxCollider>()!=null)
                    {
                        SetLimitZone(zoneData, zone.GetChild(j).gameObject);
                        
                       // if(isDebug) Debug.Log(zone.GetChild(j).name);
                    }

                    if (zone.GetChild(j).GetComponent<CinemachineVirtualCamera>()!=null)
                    {
                        SetVirtualCamera(zoneData, zone.GetChild(j).gameObject);
                        
                        
                        //if(isDebug) Debug.Log(zone.GetChild(j).gameObject.name);
                    }
                }
            
                //Hierarchy Color
                // if(nameTag !=null||nameTag!=(""))
                //     zone3D.nameTag = nameTag;
               if(zone.gameObject.layer != _triggerMask)
                   zone.gameObject.layer = _triggerMask;
                // if(isDebug) Debug.Log(zone3D.gameObject.name + " " + LayerMask.LayerToName(_triggerMask));
                
                
                ZoneSetting.DictZoneData.Add(GoName, zoneData);
                //Test
                DictZoneData.Add(GoName, zoneData);
            }
        }

        public void GetDataArray()
        {
            Transform father = GetComponent<Transform>();
            for (int i = 0; i < father.childCount; i++)
            {
                if (!ZoneSetting.DictZoneData.ContainsKey(father.GetChild(i).name))
                {
                    if(isDebug) Debug.Log("数据：" + father.GetChild(i).name + "丢失！");
                    return;
                }
            
           
            
                var zone = father.GetChild(i);
                GoName = zone.name;
            
            
                var zoneData = ZoneSetting.DictZoneData[GoName];

                //Zone
                zone.position = zoneData.ZonePos;
                zone.GetComponent<BoxCollider>().center = zoneData.ZoneCenter;
                zone.GetComponent<BoxCollider>().size = zoneData.ZoneSize;

            
                for (int j = 0; j < zone.childCount; j++)
                {
                    if (zone.GetChild(j).GetComponent<BoxCollider>()!=null)
                    {
                        GetLimitZone(zoneData, zone.GetChild(j).gameObject);
                        if(isDebug) Debug.Log(zone.GetChild(j).name);
                    }

                    if (zone.GetChild(j).GetComponent<CinemachineVirtualCamera>()!=null)
                    {
                        GetVirtualCamera(zoneData, zone.GetChild(j).gameObject);
                        if(isDebug) Debug.Log(zone.GetChild(j).gameObject.name);
                    }
                }
                if(isDebug) Debug.Log("读取数据");
            }
        }
    
        #region Data Get Set
        /// <summary>
        /// Set data
        /// </summary>
        /// <param name="zoneData"></param>
        /// <param name="limit"></param>
        public void SetLimitZone(ZoneData zoneData,GameObject limit)
        {
            //limit
            zoneData.LimitPos = limit.GetComponent<Transform>().position;
            zoneData.LimitSize = limit.GetComponent<BoxCollider>().size;
            zoneData.LimitCenter = limit.GetComponent<BoxCollider>().center;
        }
    
        public void SetVirtualCamera(ZoneData zoneData,GameObject virtualCam)
        {
            //limit
            virtualCam.SetActive(false);
            var vCam = virtualCam.GetComponent<CinemachineVirtualCamera>();
            zoneData.Fov = vCam.m_Lens.FieldOfView;
            zoneData.Near = vCam.m_Lens.NearClipPlane;
            zoneData.Far = vCam.m_Lens.FarClipPlane;
        
            //Body
            if (vCam.GetCinemachineComponent<CinemachineFramingTransposer>())
            {
                CinemachineFramingTransposer body = vCam.GetCinemachineComponent<CinemachineFramingTransposer>();

                zoneData.XDamping = body.m_XDamping;
                zoneData.YDamping = body.m_YDamping;
                zoneData.ZDamping = body.m_ZDamping;

                zoneData.Distance = body.m_CameraDistance;
                zoneData.DeadWidth = body.m_DeadZoneWidth;
                zoneData.DeadHeight = body.m_DeadZoneHeight;
            }
        }
    
        /// <summary>
        /// Get Data
        /// </summary>
        public void GetLimitZone(ZoneData zoneData,GameObject limit)
        {
            limit.GetComponent<Transform>().position = zoneData.LimitPos;
            limit.GetComponent<BoxCollider>().size = zoneData.LimitSize;
            limit.GetComponent<BoxCollider>().center = zoneData.LimitCenter;
        }
    
        public void GetVirtualCamera(ZoneData zoneData,GameObject virtualCam)
        {
            //limit
            var vCam = virtualCam.GetComponent<CinemachineVirtualCamera>();
            vCam.m_Lens.FieldOfView = zoneData.Fov;
            vCam.m_Lens.NearClipPlane = zoneData.Near;
            vCam.m_Lens.FarClipPlane = zoneData.Far;
        
            //Body
            if (vCam.GetCinemachineComponent<CinemachineFramingTransposer>())
            {
                CinemachineFramingTransposer body = vCam.GetCinemachineComponent<CinemachineFramingTransposer>();

                body.m_CameraDistance = zoneData.Distance;
                body.m_DeadZoneWidth = zoneData.DeadWidth;
                body.m_DeadZoneHeight = zoneData.DeadHeight;

                body.m_XDamping = zoneData.XDamping;
                body.m_YDamping = zoneData.YDamping;
                body.m_ZDamping = zoneData.ZDamping;
            }
        }

        #endregion
        public void ClearSomeData()
        {
            ZoneSetting.DictZoneData.Clear();
            //Test
            DictZoneData.Clear();
        }
        public void SaveData()
        {
            ZoneSetting.DictZoneData.Clear();
            foreach (var element in ZoneSetting.DictZoneData)
            {
                ZoneSetting.DictZoneData.Add(element.Key, element.Value);
            }
            if(isDebug) Debug.Log("写入数据");
        }
        
        
#if UNITY_EDITOR
        public void CreateField()
        {
            var ZoneDataFold = ScriptableObject.CreateInstance<ZoneDataSO>();

            if (!AssetDatabase.IsValidFolder("Assets/CustomData/Resources"))
                AssetDatabase.CreateFolder("Assets/CustomData/Resources", gameObject.scene.name);

            AssetDatabase.CreateAsset(ZoneDataFold, $"Assets/CustomData/Editor/Resources/{"Zone-" + gameObject.scene.name}.asset");
            AssetDatabase.SaveAssets();
            ZoneSetting = ZoneDataFold;
            if(isDebug) Debug.Log("创建文件：" +gameObject.scene.name);
        }
#endif
    }
}