using UnityEngine;

public class CamaraController : MonoBehaviour
{
    [SerializeField] private Transform _personaje;
    [SerializeField] private float _suavidad = 0.1f;

    private Vector3 _velocidadActual = Vector3.zero;

    void Start()
    {
        if (_personaje == null)
        {
            GameObject player = GameObject.FindGameObjectWithTag("Player");
            if (player != null)
            {
                _personaje = player.transform;
            }
        }
    }

    void LateUpdate()
    {
        if (_personaje == null)
        {
            GameObject player = GameObject.FindGameObjectWithTag("Player");

            if (player != null)
            {
                _personaje = player.transform;
            }
            else
            {
                return;
            }
        }

        Vector3 posObjetivo = new Vector3(
            _personaje.position.x,
            _personaje.position.y,
            transform.position.z
        );

        transform.position = Vector3.SmoothDamp(
            transform.position,
            posObjetivo,
            ref _velocidadActual,
            _suavidad
        );
    }
}

