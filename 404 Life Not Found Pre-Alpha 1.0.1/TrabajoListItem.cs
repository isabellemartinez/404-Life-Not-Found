using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class TrabajoListItem : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _textoNombre;
    [SerializeField] private TextMeshProUGUI _textoPago;
    [SerializeField] private TextMeshProUGUI _textoNivelRequerido;
    [SerializeField] private Button _botonAceptar;
    [SerializeField] private GameObject _candado;

    private Trabajo _trabajo;

    public void Configurar(Trabajo trabajo, bool desbloqueado)
    {
        _trabajo = trabajo;

        _textoNombre.text = trabajo.Nombre;
        _textoPago.text = trabajo.PagoBits + " Bits";
        _textoNivelRequerido.text = "Nivel " + trabajo.NivelRequerido;

        _botonAceptar.interactable = desbloqueado;

        if (_candado != null)
        {
            _candado.SetActive(!desbloqueado);
        }

        _botonAceptar.onClick.RemoveAllListeners();
        _botonAceptar.onClick.AddListener(AceptarTrabajo);
    }

    private void AceptarTrabajo()
    {
        if (GestorTrabajos.Instancia == null)
        {
            Debug.LogWarning("No existe GestorTrabajos.");
            return;
        }

        bool aceptado = GestorTrabajos.Instancia.AceptarTrabajo(_trabajo);

        if (!aceptado)
        {
            Debug.Log("No se pudo aceptar el trabajo.");
            return;
        }

        _botonAceptar.interactable = false;

        if (_candado != null)
        {
            _candado.SetActive(true);
        }

        Debug.Log("Trabajo aceptado desde la lista: " + _trabajo.Nombre);
    }
}