using System;
using System.Collections.Generic;
using Sirenix.OdinInspector;

namespace CustomPlugins.Zone.SO
{
    using Data;
    [Serializable]
    public class ZoneDataSO: SerializedScriptableObject
    {
        [DictionaryDrawerSettings]
        public Dictionary<string, ZoneData> DictZoneData = new Dictionary<string, ZoneData>();
    }
}