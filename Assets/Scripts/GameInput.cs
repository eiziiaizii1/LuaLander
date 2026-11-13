using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class GameInput : MonoBehaviour
{
    public static GameInput Instance {get; private set;}
    private InputActions inputActions;

     public event EventHandler onMenuButtonPressed;

    private void Awake()
    {
        Instance = this;
        inputActions = new InputActions();
        inputActions.Enable();

        inputActions.Player.Menu.performed += MenuPerformed;
    }

    private void MenuPerformed(InputAction.CallbackContext context)
    {
        onMenuButtonPressed?.Invoke(this, EventArgs.Empty);
    }

    private void OnDestroy()
    {
        inputActions.Disable();    
    }

    public bool IsUpActionPressed()
    {
        return inputActions.Player.LanderUp.IsPressed();
    }
    public bool IsLeftActionPressed()
    {
        return inputActions.Player.LanderLeft.IsPressed();
    }
    public bool IsRightActionPressed()
    {
        return inputActions.Player.LanderRight.IsPressed();
    }
    public Vector2 GetMovementInputVector()
    {
        return inputActions.Player.Movement.ReadValue<Vector2>();
    }
}
