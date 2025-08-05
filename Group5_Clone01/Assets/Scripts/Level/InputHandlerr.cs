using UnityEngine;
using UnityEngine.InputSystem;
using Cinemachine;

public class InputHandlerr : MonoBehaviour, AxisState.IInputAxisProvider
{
    [HideInInspector]
    public InputAction hori;
    [HideInInspector]
    public InputAction verti;


    public float GetAxisValue(int axis)
    {
        switch (axis)
        {
            case 0: return hori.ReadValue<Vector2>().x;
            case 1: return hori.ReadValue<Vector2>().y;
            case 2: return verti.ReadValue<float>();
        }

        return 0;
    }
}