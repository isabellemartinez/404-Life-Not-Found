using UnityEngine;
using UnityEngine.InputSystem;

public class MensajesTester : MonoBehaviour
{
    private void Update()
    {
        if (Keyboard.current != null && Keyboard.current.tKey.wasPressedThisFrame)
        {
            if (MensajesManager.Instance != null)
            {
                MensajesManager.Instance.MostrarMensaje("Mensaje de prueba funcionando");
            }
            else
            {
                Debug.LogWarning("No hay MensajesManager en la escena.");
            }
        }
    }
}