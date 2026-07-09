using UnityEngine;
using UnityEngine.InputSystem;

public class ProbadorInventario : MonoBehaviour
{
    [Header("Consumibles de prueba")]
    [SerializeField] private Consumible _byteWater;
    [SerializeField] private Consumible _debugChips;
    [SerializeField] private Consumible _stackSandwich;

    private void Update()
    {
        if (Keyboard.current == null)
        {
            return;
        }

        if (Inventario.Instancia == null)
        {
            return;
        }

        if (Keyboard.current.digit1Key.wasPressedThisFrame)
        {
            Inventario.Instancia.AgregarItem(_byteWater, 1);
            Debug.Log("Agregado: " + _byteWater.Nombre);
        }

        if (Keyboard.current.digit2Key.wasPressedThisFrame)
        {
            Inventario.Instancia.AgregarItem(_debugChips, 1);
            Debug.Log("Agregado: " + _debugChips.Nombre);
        }

        if (Keyboard.current.digit3Key.wasPressedThisFrame)
        {
            Inventario.Instancia.AgregarItem(_stackSandwich, 1);
            Debug.Log("Agregado: " + _stackSandwich.Nombre);
        }
    }
}