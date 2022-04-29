using UnityEngine;
using UnityEngine.InputSystem;

public static class InputManager
{
    static InputManager()
    {
        playerInput = new PlayerInput();
    }

    public static InputAction ESCInput => playerInput.KeyboardInput.ESC;
    private static PlayerInput playerInput;
}
