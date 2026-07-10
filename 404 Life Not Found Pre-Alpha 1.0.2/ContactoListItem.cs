using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ContactoListItem : MonoBehaviour
{
    [SerializeField] private Image _imagenContacto;
    [SerializeField] private TextMeshProUGUI _textoNombreContacto;
    [SerializeField] private TextMeshProUGUI _textoDescripcionCorta;
    [SerializeField] private TextMeshProUGUI _textoEstadoMision;
    [SerializeField] private Button _botonAbrir;

    private MisionesContacto _misionContacto;
    private ContactosUI _contactosUI;

    public void Configurar(MisionesContacto misionContacto, ContactosUI contactosUI)
    {
        _misionContacto = misionContacto;
        _contactosUI = contactosUI;

        if (_textoNombreContacto != null)
        {
            _textoNombreContacto.text = misionContacto.NombreContacto;
        }

        if (_textoDescripcionCorta != null)
        {
            _textoDescripcionCorta.text = misionContacto.MensajeContacto;
        }

        if (_textoEstadoMision != null)
        {
            _textoEstadoMision.text = ObtenerEstadoTexto(misionContacto);
        }

        if (_imagenContacto != null)
        {
            _imagenContacto.sprite = misionContacto.FotoContacto;
            _imagenContacto.enabled = misionContacto.FotoContacto != null;
        }

        if (_botonAbrir != null)
        {
            _botonAbrir.onClick.RemoveAllListeners();
            _botonAbrir.onClick.AddListener(AbrirDetalle);
        }
    }

    private string ObtenerEstadoTexto(MisionesContacto misionContacto)
    {
        if (misionContacto == null)
        {
            return "Desconocida";
        }

        if (misionContacto.Completada)
        {
            return "Completada";
        }

        if (misionContacto.Aceptada)
        {
            return "Aceptada";
        }

        return "Nueva";
    }

    private void AbrirDetalle()
    {
        if (_contactosUI == null)
        {
            Debug.LogWarning("No existe ContactosUI.");
            return;
        }

        _contactosUI.MostrarDetalleMision(_misionContacto);
    }
}