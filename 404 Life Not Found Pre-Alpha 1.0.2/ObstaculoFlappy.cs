using UnityEngine;

public class ObstaculoFlappy : MonoBehaviour
{
    [SerializeField] private float _limiteIzquierdo = -12f;

    private float _velocidad;
    private bool _activo;

    public void Inicializar(float velocidad)
    {
        _velocidad = velocidad;
        _activo = true;
    }

    public void Detener()
    {
        _activo = false;
    }

    private void Update()
    {
        if (!_activo)
        {
            return;
        }

        transform.position += Vector3.left * _velocidad * Time.deltaTime;

        if (transform.position.x <= _limiteIzquierdo)
        {
            Destroy(gameObject);
        }
    }
}