using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

public class ZonaInteraccion : MonoBehaviour
{
    private enum TipoInteraccion
    {
        Mision,
        Trabajo
    }

    [Header("Tipo de interacción")]
    [SerializeField] private TipoInteraccion _tipoInteraccion;

    [Header("Datos de misión")]
    [SerializeField] private string _idPuntoMision;

    [Header("UI de misión")]
    [SerializeField] private ProgresoMisionUI _progresoMisionUI;

    private bool _jugadorDentro;
    private bool _realizandoMision;
    private Coroutine _rutinaMision;

    private void Update()
    {
        if (!_jugadorDentro)
        {
            return;
        }

        if (_realizandoMision)
        {
            return;
        }

        if (Keyboard.current != null && Keyboard.current.eKey.wasPressedThisFrame)
        {
            Interactuar();
        }
    }

    private void Interactuar()
    {
        if (_tipoInteraccion == TipoInteraccion.Mision)
        {
            IntentarRealizarMision();
            return;
        }

        if (_tipoInteraccion == TipoInteraccion.Trabajo)
        {
            IntentarIniciarTrabajo();
            return;
        }
    }

    private void IntentarRealizarMision()
    {
        if (GestorMisiones.Instancia == null)
        {
            Debug.LogWarning("No existe GestorMisiones en la escena.");
            MostrarMensaje("No existe GestorMisiones.");
            return;
        }

        MisionesContacto mision = GestorMisiones.Instancia.ObtenerMisionAceptadaPorPunto(_idPuntoMision);

        if (mision == null)
        {
            MostrarMensaje("No tienes ninguna misión activa en esta zona.");
            return;
        }

        if (mision.Completada)
        {
            MostrarMensaje("Esta misión ya fue completada.");
            return;
        }

        _rutinaMision = StartCoroutine(RealizarMision(mision));
    }

    private IEnumerator RealizarMision(MisionesContacto mision)
    {
        _realizandoMision = true;

        float tiempoActual = 0f;
        float tiempoTotal = mision.TiempoParaCompletar;

        if (tiempoTotal <= 0f)
        {
            tiempoTotal = 1f;
        }

        if (_progresoMisionUI != null)
        {
            _progresoMisionUI.Mostrar("Realizando misión: " + mision.NombreMision);
        }

        while (tiempoActual < tiempoTotal)
        {
            tiempoActual += Time.deltaTime;

            float progreso = tiempoActual / tiempoTotal;

            if (_progresoMisionUI != null)
            {
                _progresoMisionUI.Actualizar(progreso);
            }

            yield return null;
        }

        if (_progresoMisionUI != null)
        {
            _progresoMisionUI.Ocultar();
        }

        GestorMisiones.Instancia.CompletarMision(mision.IdMision);

        _realizandoMision = false;
        _rutinaMision = null;
    }

    private void IntentarIniciarTrabajo()
    {
        if (GestorTrabajos.Instancia == null)
        {
            Debug.LogWarning("No existe GestorTrabajos.");
            MostrarMensaje("No existe GestorTrabajos.");
            return;
        }

        if (GestorTrabajos.Instancia.TrabajoActivo == null)
        {
            MostrarMensaje("No tienes ningún trabajo activo.");
            return;
        }

        GestorTrabajos.Instancia.IniciarMinijuegoTrabajoActivo();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            _jugadorDentro = true;
            MostrarMensaje("Presiona E para interactuar.");
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            _jugadorDentro = false;

            if (_realizandoMision && _rutinaMision != null)
            {
                StopCoroutine(_rutinaMision);
                _rutinaMision = null;
                _realizandoMision = false;

                if (_progresoMisionUI != null)
                {
                    _progresoMisionUI.Ocultar();
                }

                MostrarMensaje("Misión cancelada.");
            }
        }
    }

    private void MostrarMensaje(string mensaje)
    {
        MensajesManager mensajesManager = FindAnyObjectByType<MensajesManager>();

        if (mensajesManager != null)
        {
            mensajesManager.MostrarMensaje(mensaje);
        }
        else
        {
            Debug.Log(mensaje);
        }
    }
}