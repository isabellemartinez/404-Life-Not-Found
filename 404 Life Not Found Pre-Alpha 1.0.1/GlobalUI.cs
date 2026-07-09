using UnityEngine;

[DefaultExecutionOrder(-1000)]
public class GlobalUI : MonoBehaviour
{
    public static GlobalUI Instancia { get; private set; }

    private void Awake()
    {
        if (Instancia != null && Instancia != this)
        {
            Debug.LogWarning("GlobalUI duplicado detectado. Se eliminó solo el componente, NO el Canvas: " + gameObject.name);
            Destroy(this);
            return;
        }

        Instancia = this;

        if (transform.parent != null)
        {
            transform.SetParent(null);
        }

        DontDestroyOnLoad(gameObject);
    }
}