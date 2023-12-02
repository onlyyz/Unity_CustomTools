using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CustomPlugins.Signal
{
    public class CustomSignal : MonoBehaviour
    {
        public static void SignalUpWard( GameObject gameObject,string function)
        {
            gameObject.SendMessageUpwards(function);
        }
    }

}