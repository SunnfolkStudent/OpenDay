using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class InputManager : MonoBehaviour
{
    private InputSystem_Actions _inputSystem;
   // public float horizontal;

   public Vector2 moveVector { get; private set; }

    public float jumpValue;
    public bool jumpPressed;
    public bool jumpHeld;
    public bool jumpReleased;

    public InputAction Jump { get; private set; }

    private void Awake()
    {
        _inputSystem = new InputSystem_Actions();
    }

    private void Update()
    {
        // = _inputSystem.Player.Move.ReadValue<Vector2>().x;
        moveVector = _inputSystem.Player.Move.ReadValue<Vector2>();
        jumpValue = _inputSystem.Player.Jump.ReadValue<float>();
        jumpPressed = _inputSystem.Player.Jump.triggered;
        jumpHeld = _inputSystem.Player.Jump.inProgress;

        jumpReleased = _inputSystem.Player.Jump.WasReleasedThisFrame();
        
        Jump =  _inputSystem.Player.Jump;
    }

    private void OnEnable()
    {
        _inputSystem.Enable();
    }
    private void OnDisable()
    {
        _inputSystem.Disable();
    }
    
}
