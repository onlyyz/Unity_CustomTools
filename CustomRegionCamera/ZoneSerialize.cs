using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using Cinemachine;
using UnityEditor;
using UnityEngine.SceneManagement;

namespace Zone
{
    using SO;

    [ExecuteAlways]
    public class ZoneSerialize : MonoBehaviour
    {
        
        // public readonly string DefalutPath= "Assets/CustomPlugins/Resources";
        public ZoneDataSetting ZoneSetting;

        // [Button("更新数组"), ButtonGroup]
        // public void ShowValue1()
        // {
        //     
        // }
        // [Button("手动 - 同步写入"), ButtonGroup]
        // public void ShowValue3()
        // {
        //     Excel = false;
        //     useHand();
        // }
        // [Button("保存数据"), ButtonGroup]
        // public void ShowValue1()
        // {
        //     if (ZoneSetting==null)
        //         CreateField();
        //     forEachZone();
        //     SaveData();
        //     Debug.Log("数据已保存");
        // }
        //
        // [Button("加载数据"), ButtonGroup]
        // public void ShowValue2()
        // {
        //     useExcel();
        //     Debug.Log("数据已加载");
        // }

      
        public Dictionary<string, ZoneData> DictZoneData = new Dictionary<string, ZoneData>();
        
        // private void Update()
        // {
        //     if (Excel)
        //     {
        //         useExcel();
        //     }
        //     else
        //     {
        //         useHand();
        //     }
        // }


        public void forEachZone()
        {
            ClearSomeData();
            Transform father = GetComponent<Transform>();
            for (int i = 0; i < father.childCount; i++)
            {
                var zone = father.GetChild(i);
                ZoneData zoneData = new ZoneData();
                if (DictZoneData.ContainsKey(zone.name))
                {
                    EditorUtility.DisplayDialog("错误!", " 有同名区域: " + zone.name + " 请更改，保持唯一性", "OK");
                    return;
                }

                //Zone
                zoneData.ZoneName = zone.name;
                zoneData.ZonePos = zone.position;
                zoneData.ZoneCenter = zone.GetComponent<BoxCollider>().center;
                zoneData.ZoneSize = zone.GetComponent<BoxCollider>().size;

                //limit
                var limt = zone.Find("Limit Region");
                zoneData.LimitPos = limt.GetComponent<Transform>().position;
                zoneData.LimitSize = limt.GetComponent<BoxCollider>().size;
                zoneData.LimitCenter = limt.GetComponent<BoxCollider>().center;

                //Camera
                var Camera = zone.Find("Virtual Camera");
                var vCam = Camera.GetComponent<CinemachineVirtualCamera>();
                zoneData.Fov = vCam.m_Lens.FieldOfView;
                zoneData.Near = vCam.m_Lens.NearClipPlane;
                zoneData.Far = vCam.m_Lens.FarClipPlane;

                //Body
                CinemachineFramingTransposer body = vCam.GetCinemachineComponent<CinemachineFramingTransposer>();

                zoneData.XDamping = body.m_XDamping;
                zoneData.YDamping = body.m_YDamping;
                zoneData.ZDamping = body.m_ZDamping;

                zoneData.Distance = body.m_CameraDistance;
                zoneData.DeadWidth = body.m_DeadZoneWidth;
                zoneData.DeadHeight = body.m_DeadZoneHeight;


                DictZoneData.Add(zone.name, zoneData);
            }
        }

        public void ClearSomeData()
        {
            DictZoneData.Clear();
        }

        public void useHand()
        {
            Transform father = GetComponent<Transform>();
            for (int i = 0; i < father.childCount; i++)
            {
                var zone = father.GetChild(i);
                var SOValue = DictZoneData[zone.name];


                SOValue.ZonePos = zone.position;
                SOValue.ZoneCenter = zone.GetComponent<BoxCollider>().center;
                SOValue.ZoneSize = zone.GetComponent<BoxCollider>().size;

                //limit
                var limt = zone.Find("Limit Region");
                SOValue.LimitPos = limt.GetComponent<Transform>().position;
                SOValue.LimitSize = limt.GetComponent<BoxCollider>().size;
                SOValue.LimitCenter = limt.GetComponent<BoxCollider>().center;

                //Camera
                var Camera = zone.Find("Virtual Camera");
                var vCam = Camera.GetComponent<CinemachineVirtualCamera>();
                SOValue.Fov = vCam.m_Lens.FieldOfView;
                SOValue.Near = vCam.m_Lens.NearClipPlane;
                SOValue.Far = vCam.m_Lens.FarClipPlane;

                //Body
                CinemachineFramingTransposer body = vCam.GetCinemachineComponent<CinemachineFramingTransposer>();
                SOValue.Distance = body.m_CameraDistance;
                SOValue.DeadWidth = body.m_DeadZoneWidth;
                SOValue.DeadHeight = body.m_DeadZoneHeight;

                SOValue.XDamping = body.m_XDamping;
                SOValue.YDamping = body.m_YDamping;
                SOValue.ZDamping = body.m_ZDamping;
            }
        }

        public void useExcel()
        {
            Transform father = GetComponent<Transform>();
            for (int i = 0; i < father.childCount; i++)
            {
                var zone = father.GetChild(i);
                var SOValue = ZoneSetting.DictZoneData[zone.name];

                //Zone
                zone.position = SOValue.ZonePos;
                zone.GetComponent<BoxCollider>().center = SOValue.ZoneCenter;
                zone.GetComponent<BoxCollider>().size = SOValue.ZoneSize;

                //limit
                var limt = zone.Find("Limit Region");
                limt.GetComponent<Transform>().position = SOValue.LimitPos;
                limt.GetComponent<BoxCollider>().size = SOValue.LimitSize;
                limt.GetComponent<BoxCollider>().center = SOValue.LimitCenter;

                //Camera
                var Camera = zone.Find("Virtual Camera");
                var vCam = Camera.GetComponent<CinemachineVirtualCamera>();
                vCam.m_Lens.FieldOfView = SOValue.Fov;
                vCam.m_Lens.NearClipPlane = SOValue.Near;
                vCam.m_Lens.FarClipPlane = SOValue.Far;


                //Body
                CinemachineFramingTransposer body = vCam.AddCinemachineComponent<CinemachineFramingTransposer>();
                body.m_CameraDistance = SOValue.Distance;
                body.m_DeadZoneWidth = SOValue.DeadWidth;
                body.m_DeadZoneHeight = SOValue.DeadHeight;
            }
        }

        public void SaveData()
        {
            ZoneSetting.DictZoneData.Clear();
            foreach (var element in DictZoneData)
            {
                ZoneSetting.DictZoneData.Add(element.Key, element.Value);
            }

            Debug.Log("写入数据");
        }

        public void CreateField()
        {
            
            var ZoneDataFold = ScriptableObject.CreateInstance<ZoneDataSetting>();

            if (!AssetDatabase.IsValidFolder("Assets/CustomPlugins/Resources"))
                AssetDatabase.CreateFolder("Assets/CustomPlugins/Resources", SceneManager.GetActiveScene ().name);

            AssetDatabase.CreateAsset(ZoneDataFold, $"Assets/CustomPlugins/Resources/{SceneManager.GetActiveScene ().name}.asset");
            AssetDatabase.SaveAssets();
            ZoneSetting = ZoneDataFold;
            Debug.Log("创建文件：" +SceneManager.GetActiveScene ().name);
        }
    }
}