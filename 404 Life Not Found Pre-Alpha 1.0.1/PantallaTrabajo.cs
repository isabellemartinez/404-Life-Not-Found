using UnityEngine;
using System.Collections.Generic;

public class PantallaTrabajo : MonoBehaviour
{
    [SerializeField] private GameObject _prefabListItem;
    [SerializeField] private Transform Content;

    private List<GameObject> _itemsInstanciados = new List<GameObject>();

    private void Start()
    {
        if (GestorTrabajos.Instancia == null)
        {
            Debug.LogWarning("GestorTrabajos no existe todavía");
            return;
        }

        ActualizarLista();
    }

    private void OnEnable()
    {
        if (GestorTrabajos.Instancia == null)
        {
            return;
        }

        ActualizarLista();
    }

    public void ActualizarLista()
    {
        LimpiarLista();

        List<Trabajo> trabajos = GestorTrabajos.Instancia.TrabajosDisponibles;

        foreach (Trabajo trabajo in trabajos)
        {
            GameObject item = Instantiate(_prefabListItem, Content);
            TrabajoListItem scriptItem = item.GetComponent<TrabajoListItem>();

            bool desbloqueado = GestorTrabajos.Instancia.TrabajoEstaDesbloqueado(trabajo)
                               && GestorTrabajos.Instancia.PuedeAceptarTrabajo();

            scriptItem.Configurar(trabajo, desbloqueado);

            _itemsInstanciados.Add(item);
        }
    }

    private void LimpiarLista()
    {
        foreach (GameObject item in _itemsInstanciados)
        {
            Destroy(item);
        }

        _itemsInstanciados.Clear();
    }
}