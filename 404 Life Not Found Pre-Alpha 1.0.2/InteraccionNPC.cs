using UnityEngine;
using TMPro;
using UnityEngine.InputSystem;
using System.Collections;

public class InteraccionNPC : MonoBehaviour
{
    [Header("Icono de interacción")]
    [SerializeField] private GameObject _iconoInteraccion;

    [Header("UI de diálogo")]
    [SerializeField] private GameObject _panelMensaje;
    [SerializeField] private TMP_Text _textoMensaje;

    [Header("Diálogos del NPC")]
    [TextArea(2, 8)]
    [SerializeField] private string[] _dialogos;

    [Header("Efecto de texto")]
    [SerializeField] private float _velocidadTexto = 0.04f;

    private bool _jugadorCerca = false;
    private bool _dialogoActivo = false;
    private bool _escribiendo = false;

    private int _indiceDialogo = 0;
    private string _textoActual = "";

    private Coroutine _corrutinaTexto;

    public bool DialogoActivo { get { return _dialogoActivo; } }
    public bool Escribiendo { get { return _escribiendo; } }
    public int IndiceDialogo { get { return _indiceDialogo; } }

    private void Start()
    {
        if (_iconoInteraccion != null)
        {
            _iconoInteraccion.SetActive(false);
        }

        if (_panelMensaje != null)
        {
            _panelMensaje.SetActive(false);
        }

        if (_textoMensaje != null)
        {
            _textoMensaje.text = "";
        }
    }

    private void Update()
    {
        if (!_jugadorCerca)
        {
            return;
        }

        if (Keyboard.current == null)
        {
            return;
        }

        if (Keyboard.current.eKey.wasPressedThisFrame)
        {
            ManejarInteraccion();
        }
    }

    private void ManejarInteraccion()
    {
        if (!_dialogoActivo)
        {
            IniciarDialogo();
            return;
        }

        if (_escribiendo)
        {
            CompletarTextoActual();
            return;
        }

        SiguienteDialogo();
    }

    private void IniciarDialogo()
    {
        if (_dialogos == null || _dialogos.Length == 0)
        {
            Debug.LogWarning("No hay diálogos asignados en el NPC.", this);
            return;
        }

        _dialogoActivo = true;
        _indiceDialogo = 0;

        if (_iconoInteraccion != null)
        {
            _iconoInteraccion.SetActive(false);
        }

        if (_panelMensaje != null)
        {
            _panelMensaje.SetActive(true);
        }

        MostrarDialogoActual();
    }

    private void MostrarDialogoActual()
    {
        if (_textoMensaje == null)
        {
            Debug.LogWarning("No asignaste el textoMensaje en el Inspector.", this);
            return;
        }

        _textoActual = _dialogos[_indiceDialogo];

        if (_corrutinaTexto != null)
        {
            StopCoroutine(_corrutinaTexto);
        }

        _corrutinaTexto = StartCoroutine(EscribirTexto(_textoActual));
    }

    private IEnumerator EscribirTexto(string texto)
    {
        _escribiendo = true;
        _textoMensaje.text = "";

        foreach (char letra in texto)
        {
            _textoMensaje.text += letra;
            yield return new WaitForSeconds(_velocidadTexto);
        }

        _escribiendo = false;
        _corrutinaTexto = null;
    }

    private void CompletarTextoActual()
    {
        if (_corrutinaTexto != null)
        {
            StopCoroutine(_corrutinaTexto);
            _corrutinaTexto = null;
        }

        _textoMensaje.text = _textoActual;
        _escribiendo = false;
    }

    private void SiguienteDialogo()
    {
        _indiceDialogo++;

        if (_indiceDialogo >= _dialogos.Length)
        {
            TerminarDialogo();
            return;
        }

        MostrarDialogoActual();
    }

    private void TerminarDialogo()
    {
        _dialogoActivo = false;
        _indiceDialogo = 0;

        if (_panelMensaje != null)
        {
            _panelMensaje.SetActive(false);
        }

        if (_textoMensaje != null)
        {
            _textoMensaje.text = "";
        }

        if (_jugadorCerca && _iconoInteraccion != null)
        {
            _iconoInteraccion.SetActive(true);
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            _jugadorCerca = true;

            if (!_dialogoActivo && _iconoInteraccion != null)
            {
                _iconoInteraccion.SetActive(true);
            }
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            _jugadorCerca = false;

            if (_iconoInteraccion != null)
            {
                _iconoInteraccion.SetActive(false);
            }

            CerrarTodo();
        }
    }

    private void CerrarTodo()
    {
        _dialogoActivo = false;
        _escribiendo = false;
        _indiceDialogo = 0;

        if (_corrutinaTexto != null)
        {
            StopCoroutine(_corrutinaTexto);
            _corrutinaTexto = null;
        }

        if (_panelMensaje != null)
        {
            _panelMensaje.SetActive(false);
        }

        if (_textoMensaje != null)
        {
            _textoMensaje.text = "";
        }
    }
}
