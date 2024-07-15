using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    private PlayerAction playerActions;

    private void Awake()
    {
        playerActions = new PlayerAction();
    }

    private void OnEnable()
    {
        playerActions.Enable();
    }

    private void OnDisable()
    {
        playerActions.Disable();
    }

    private void Update()
    {
        float TextValue =  playerActions.Land.Move.ReadValue<float>();
        Debug.Log(TextValue);
    }
}