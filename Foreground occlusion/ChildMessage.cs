using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChildMessage : MonoBehaviour
{
    void OnTriggerEnter(Collider other)
    {
        SendMessageUpwards("TriggerEnterChild", other);
    }

    void OnTriggerExit(Collider other)
    {
        SendMessageUpwards("TriggerExitChild", other);
    }
}
