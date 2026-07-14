using UnityEngine;
using UnityEngine.SceneManagement;

public class SalidaEscena : MonoBehaviour
{
    [Header("Transición de escena")]
    [SerializeField] private string escenaDestino;
    [SerializeField] private string spawnDestino;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!other.CompareTag("Player"))
        {
            return;
        }

        SpawnManager.spawnObjetivo = spawnDestino;
        SceneManager.LoadScene(escenaDestino);
    }
}
