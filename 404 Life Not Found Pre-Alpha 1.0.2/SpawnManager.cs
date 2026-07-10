using UnityEngine;
using UnityEngine.SceneManagement;

public class SpawnManager : MonoBehaviour
{
    public static string spawnObjetivo;

    private void OnEnable()
    {
        SceneManager.sceneLoaded += ColocarEnSpawn;
    }

    private void OnDisable()
    {
        SceneManager.sceneLoaded -= ColocarEnSpawn;
    }

    private void Start()
    {
        ColocarEnSpawn(SceneManager.GetActiveScene(), LoadSceneMode.Single);
    }

    private void ColocarEnSpawn(Scene scene, LoadSceneMode mode)
    {
        if (string.IsNullOrEmpty(spawnObjetivo))
        {
            Debug.Log("No hay spawn objetivo todavía.");
            return;
        }

        SpawnPoint[] spawns = Object.FindObjectsByType<SpawnPoint>(FindObjectsInactive.Exclude);

        foreach (SpawnPoint spawn in spawns)
        {
            Debug.Log("Spawn encontrado: " + spawn.spawnID);

            if (spawn.spawnID == spawnObjetivo)
            {
                transform.position = spawn.transform.position;
                Debug.Log("Player colocado en spawn: " + spawn.spawnID);
                return;
            }
        }

        Debug.LogWarning("No se encontró ningún SpawnPoint con ID: " + spawnObjetivo);
    }
}