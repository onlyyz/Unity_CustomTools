using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

namespace CustomPlugins.ForegroundShelter.SO
{
    using Data;
    public class ForegroundShelterSO : SerializedScriptableObject
    {
        [DictionaryDrawerSettings]
        public Dictionary<string, ForegroundShelterData> DictNearOcclusionData = new Dictionary<string, ForegroundShelterData>();
    }
}