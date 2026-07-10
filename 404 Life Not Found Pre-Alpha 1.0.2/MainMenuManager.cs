using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenuManager : MonoBehaviour
{
    [Header("Nombre de la primera escena jugable")]
    [SerializeField] private string _firstGameSceneName = "GarageDistrict";

    [Header("Botones")]
    [SerializeField] private Button _continueButton;

    private const string HAS_SAVE_KEY = "HasSave";

    private void Start()
    {
        if (_continueButton != null)
        {
            _continueButton.interactable = HasSave();
        }
    }

    public void NewGame()
    {
        BorrarDatosGuardados();

        PlayerPrefs.SetInt(HAS_SAVE_KEY, 1);
        PlayerPrefs.Save();

        SceneManager.LoadScene(_firstGameSceneName);
    }

    public void ContinueGame()
    {
        if (!HasSave())
        {
            Debug.Log("No hay partida guardada.");
            return;
        }

        SceneManager.LoadScene(_firstGameSceneName);
    }

    public void OpenOptions()
    {
        Debug.Log("Abrir opciones");
    }

    public void QuitGame()
    {
        Application.Quit();

#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#endif
    }

    private bool HasSave()
    {
        return PlayerPrefs.GetInt(HAS_SAVE_KEY, 0) == 1;
    }

    private void BorrarDatosGuardados()
    {
        PlayerPrefs.DeleteKey("Bits");
        PlayerPrefs.DeleteKey("Experiencia");

        PlayerPrefs.DeleteKey("MisionesAceptadas");
        PlayerPrefs.DeleteKey("MisionesCompletadas");

        PlayerPrefs.DeleteKey("TrabajoActivo");
        PlayerPrefs.DeleteKey("CooldownTrabajoFinTicks");

        PlayerPrefs.Save();

        Debug.Log("Datos guardados borrados para nueva partida.");
    }
}