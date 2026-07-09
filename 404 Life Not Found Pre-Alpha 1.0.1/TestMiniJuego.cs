using UnityEngine;

public class TestMinijuego : MonoBehaviour
{
    [SerializeField] private Minijuego _minijuego;

    [Header("Modo prueba")]
    [SerializeField] private bool _modoPrueba = true;
    [SerializeField] private int _nivelPrueba = 1;

    private void Start()
    {
        if (_minijuego == null)
        {
            Debug.LogWarning("No hay minijuego asignado en TestMinijuego.");
            return;
        }

        if (GestorTrabajos.Instancia == null)
        {
            if (_modoPrueba)
            {
                Debug.LogWarning("No existe GestorTrabajos. Iniciando minijuego en modo prueba.");
                _minijuego.IniciarJuego(_nivelPrueba);
            }
            else
            {
                Debug.LogWarning("No existe GestorTrabajos.");
            }

            return;
        }

        if (GestorTrabajos.Instancia.TrabajoActivo == null)
        {
            if (_modoPrueba)
            {
                Debug.LogWarning("No hay trabajo activo. Iniciando minijuego en modo prueba.");
                _minijuego.IniciarJuego(_nivelPrueba);
            }
            else
            {
                Debug.LogWarning("No hay trabajo activo.");
            }

            return;
        }

        int nivel = GestorTrabajos.Instancia.TrabajoActivo.NivelMinijuego;
        _minijuego.IniciarJuego(nivel);
    }

    private void OnEnable()
    {
        if (_minijuego != null)
        {
            _minijuego.OnJuegoTerminado += ManejarResultado;
        }
    }

    private void OnDisable()
    {
        if (_minijuego != null)
        {
            _minijuego.OnJuegoTerminado -= ManejarResultado;
        }
    }

    private void ManejarResultado(bool gano, int bits, int xp)
    {
        Debug.Log("ManejarResultado llamado, gano: " + gano + " bits: " + bits + " xp: " + xp);

        if (GestorTrabajos.Instancia == null)
        {
            Debug.Log("Modo prueba terminado. No se entregan recompensas ni se cambia de escena.");
            return;
        }

        if (gano)
        {
            Billetera.Instancia.Agregar(bits);
            Billetera.Instancia.AgregarExperiencia(xp);
        }

        GestorTrabajos.Instancia.FinalizarTrabajoActivo();
        GestorTrabajos.Instancia.VolverAEscenaPrincipal();
    }
}