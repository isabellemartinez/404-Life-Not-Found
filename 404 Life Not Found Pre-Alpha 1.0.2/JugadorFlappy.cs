using UnityEngine;

public class JugadorFlappy : MonoBehaviour
{
    [SerializeField] private JuegoFlappy _juegoFlappy;

    [Header("Limites de perdida")]
    [SerializeField] private float _limiteInferiorY = -6f;
    [SerializeField] private float _limiteSuperiorY = 6f;

    private void Awake()
    {
        if (_juegoFlappy == null)
        {
            _juegoFlappy = Object.FindAnyObjectByType<JuegoFlappy>();
        }
    }

    private void Update()
    {
        if (_juegoFlappy == null)
        {
            return;
        }

        if (transform.position.y < _limiteInferiorY || transform.position.y > _limiteSuperiorY)
        {
            Debug.Log("Jugador salió de los límites.");
            _juegoFlappy.Perder();
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        Debug.Log("Choque con: " + collision.gameObject.name + " Tag: " + collision.gameObject.tag);

        if (_juegoFlappy == null)
        {
            return;
        }

        ObstaculoFlappy obstaculo = collision.gameObject.GetComponentInParent<ObstaculoFlappy>();

        if (obstaculo != null)
        {
            Debug.Log("Choque con obstáculo. Perdiste.");
            _juegoFlappy.Perder();
            return;
        }

        if (collision.gameObject.CompareTag("SueloFlappy"))
        {
            Debug.Log("Choque con suelo. Perdiste.");
            _juegoFlappy.Perder();
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        Debug.Log("Trigger con: " + other.gameObject.name + " Tag: " + other.gameObject.tag);

        if (_juegoFlappy == null)
        {
            return;
        }

        if (other.CompareTag("PuntoFlappy"))
        {
            other.enabled = false;
            _juegoFlappy.SumarPunto();
        }
    }
}