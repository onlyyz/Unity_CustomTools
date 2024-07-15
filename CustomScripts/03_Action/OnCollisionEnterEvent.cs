using UnityEngine;
using System;
using UnityEngine.Events;

public class OnCollisionEnterEvent : MonoBehaviour
{
   
    //
    // private void OnCollisionEnter(Collision collision)
    // {
    //     // action が持っている各関数の引数に collision を代入して、各関数を実行
    //     Action.Invoke(collision.gameObject.name);
    // }

    public event Action<Collision> Action;
    [SerializeField] public UnityEvent<Collision> Event;

    private void OnCollisionEnter(Collision collision)
    {
        // Event が受け取った各関数の引数に collisionを代入して、各関数を実行
        Event.Invoke(collision);
    }
}