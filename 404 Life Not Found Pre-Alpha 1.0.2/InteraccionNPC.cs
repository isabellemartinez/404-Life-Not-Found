using UnityEngine;
using TMPro;
using UnityEngine.InputSystem;
using System.Collections;

public class InteraccionNPC : MonoBehaviour
{
    [Header("Icono de interacción")]
    [SerializeField] private GameObject iconoInteraccion;

    [Header("UI de diálogo")]
    [SerializeField] private GameObject panelMensaje;
    [SerializeField] private TMP_Text textoMensaje;

    [Header("Diálogos del NPC")]
    [TextArea(2, 8)]
    [SerializeField] private string[] dialogos;

    [Header("Efecto de texto")]
    [SerializeField] private float velocidadTexto = 0.04f;

    private bool jugadorCerca = false;
    private bool dialogoActivo = false;
    private bool escribiendo = false;

    private int indiceDialogo = 0;
    private string textoActual = "";

    private Coroutine corrutinaTexto;

    private void Start()
    {
        if (iconoInteraccion != null)
        {
            iconoInteraccion.SetActive(false);
        }

        if (panelMensaje != null)
        {
            panelMensaje.SetActive(false);
        }

        if (textoMensaje != null)
        {
            textoMensaje.text = "";
        }
    }

    private void Update()
    {
        if (jugadorCerca == false)
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
        if (dialogoActivo == false)
        {
            IniciarDialogo();
            return;
        }

        if (escribiendo)
        {
            CompletarTextoActual();
            return;
        }

        SiguienteDialogo();
    }

    private void IniciarDialogo()
    {
        if (dialogos == null || dialogos.Length == 0)
        {
            Debug.LogWarning("No hay diálogos asignados en el NPC.", this);
            return;
        }

        if (panelMensaje == null)
        {
            Debug.LogWarning("No asignaste el panelMensaje en el Inspector.", this);
            return;
        }

        if (textoMensaje == null)
        {
            Debug.LogWarning("No asignaste el textoMensaje en el Inspector.", this);
            return;
        }

        dialogoActivo = true;
        indiceDialogo = 0;

        if (iconoInteraccion != null)
        {
            iconoInteraccion.SetActive(false);
        }

        panelMensaje.SetActive(true);

        MostrarDialogoActual();
    }

    private void MostrarDialogoActual()
    {
        if (textoMensaje == null)
        {
            Debug.LogWarning("No asignaste el textoMensaje en el Inspector.", this);
            return;
        }

        textoActual = dialogos[indiceDialogo];

        if (corrutinaTexto != null)
        {
            StopCoroutine(corrutinaTexto);
        }

        corrutinaTexto = StartCoroutine(EscribirTexto(textoActual));
    }

    private IEnumerator EscribirTexto(string texto)
    {
        escribiendo = true;
        textoMensaje.text = "";

        foreach (char letra in texto)
        {
            textoMensaje.text += letra;
            yield return new WaitForSeconds(velocidadTexto);
        }

        escribiendo = false;
        corrutinaTexto = null;
    }

    private void CompletarTextoActual()
    {
        if (corrutinaTexto != null)
        {
            StopCoroutine(corrutinaTexto);
            corrutinaTexto = null;
        }

        if (textoMensaje != null)
        {
            textoMensaje.text = textoActual;
        }

        escribiendo = false;
    }

    private void SiguienteDialogo()
    {
        indiceDialogo++;

        if (indiceDialogo >= dialogos.Length)
        {
            TerminarDialogo();
            return;
        }

        MostrarDialogoActual();
    }

    private void TerminarDialogo()
    {
        dialogoActivo = false;
        escribiendo = false;
        indiceDialogo = 0;

        if (corrutinaTexto != null)
        {
            StopCoroutine(corrutinaTexto);
            corrutinaTexto = null;
        }

        if (panelMensaje != null)
        {
            panelMensaje.SetActive(false);
        }

        if (textoMensaje != null)
        {
            textoMensaje.text = "";
        }

        if (jugadorCerca && iconoInteraccion != null)
        {
            iconoInteraccion.SetActive(true);
        }
    }

    private void CerrarTodo()
    {
        dialogoActivo = false;
        escribiendo = false;
        indiceDialogo = 0;

        if (corrutinaTexto != null)
        {
            StopCoroutine(corrutinaTexto);
            corrutinaTexto = null;
        }

        if (panelMensaje != null)
        {
            panelMensaje.SetActive(false);
        }

        if (textoMensaje != null)
        {
            textoMensaje.text = "";
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player") == false)
        {
            return;
        }

        jugadorCerca = true;

        if (dialogoActivo == false && iconoInteraccion != null)
        {
            iconoInteraccion.SetActive(true);
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player") == false)
        {
            return;
        }

        jugadorCerca = false;

        if (iconoInteraccion != null)
        {
            iconoInteraccion.SetActive(false);
        }

        CerrarTodo();
    }
}