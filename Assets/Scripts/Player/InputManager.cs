using System;
using UnityEngine;

public class InputManager : MonoBehaviour
{
    private InputSystem_Actions _inputSystem;
    public float horizontal;
    public bool jump;

    private void Awake()
    {
        _inputSystem = new InputSystem_Actions();
    }

    private void Update()
    {
        horizontal = _inputSystem.Player.Move.ReadValue<Vector2>().x;
        jump = _inputSystem.Player.Jump.IsPressed();
    }
    
}
