using UnityEngine;

public class PuntoDeTransicion : MonoBehaviour
{
    [Header("Destino")]
    [SerializeField] private Transform destino;

    private bool _transicionando;

    private void OnTriggerEnter2D(Collider2D otro)
    {
        if (_transicionando)
        {
            return;
        }

        if (!otro.CompareTag("Player"))
        {
            return;
        }

        if (destino == null)
        {
            Debug.LogWarning("No hay destino asignado en PuntoDeTransicion.");
            return;
        }

        _transicionando = true;

        Vector3 posicionAnterior = otro.transform.position;
        Vector3 posicionNueva = destino.position;
        Vector3 diferencia = posicionNueva - posicionAnterior;

        otro.transform.position = posicionNueva;

        if (Camera.main != null)
        {
            Camera.main.transform.position += diferencia;
        }

        _transicionando = false;
    }
}
}
