using UnityEngine;
using UnityEngine.InputSystem;

public class PersonajeController : MonoBehaviour
{
    public static PersonajeController Instancia { get; private set; }

    private Rigidbody2D _rb;
    private Animator _animator;

    [SerializeField] private float _velocidad = 3f;
    [SerializeField] private float _multiplicadorCorrer = 1.8f;

    private float _ultimoDirX = 0f;
    private float _ultimoDirY = -1f;

    private void Awake()
    {
        if (Instancia != null && Instancia != this)
        {
            Destroy(gameObject);
            return;
        }

        Instancia = this;
        DontDestroyOnLoad(gameObject);
    }

    void Start()
    {
        _rb = GetComponent<Rigidbody2D>();
        _animator = GetComponent<Animator>();
    }

    void Update()
    {
        float movX = 0f;
        float movY = 0f;

        if (Keyboard.current.dKey.isPressed || Keyboard.current.rightArrowKey.isPressed) movX = 1f;
        else if (Keyboard.current.aKey.isPressed || Keyboard.current.leftArrowKey.isPressed) movX = -1f;

        if (Keyboard.current.wKey.isPressed || Keyboard.current.upArrowKey.isPressed) movY = 1f;
        else if (Keyboard.current.sKey.isPressed || Keyboard.current.downArrowKey.isPressed) movY = -1f;

        bool corriendo = Keyboard.current.leftShiftKey.isPressed;
        float velocidadActual = corriendo ? _velocidad * _multiplicadorCorrer : _velocidad;

        _rb.linearVelocity = new Vector2(movX * velocidadActual, movY * velocidadActual);

        bool moviendo = movX != 0 || movY != 0;

        if (moviendo)
        {
            _ultimoDirX = movX;
            _ultimoDirY = movY;
        }

        _animator.SetFloat("velocidadX", _ultimoDirX);
        _animator.SetFloat("velocidadY", _ultimoDirY);
        _animator.SetBool("nMovimiento", moviendo);
    }
}