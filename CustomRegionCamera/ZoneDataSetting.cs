using System;
using System.Collections.Generic;
using UnityEngine;

namespace Zone.SO
{
    [Serializable,CreateAssetMenu(menuName = "yz/Zone Data Setting")]
    public class ZoneDataSetting: ScriptableObject
    {
        public Dictionary<string, ZoneData> DictZoneData = new Dictionary<string, ZoneData>();
    }
}