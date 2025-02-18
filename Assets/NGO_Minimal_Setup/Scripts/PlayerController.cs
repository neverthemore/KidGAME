using Unity.Netcode;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : NetworkBehaviour
{
    [SerializeField] private float _speed = 5f;
    [SerializeField] private float _lookSensitivity = 2f;
    private CharacterController _controller;
    private PlayerInput _playerInput;
    private Transform _cameraTransform;

    private void Awake()
    {
        _controller = GetComponent<CharacterController>();
        _playerInput = GetComponent<PlayerInput>();
        _cameraTransform = GetComponentInChildren<Camera>().transform;
    }

    private void Update()
    {
        // Работает только у владельца объекта
        if (!IsOwner) return;

        // Движение
        Vector2 moveInput = _playerInput.actions["Move"].ReadValue<Vector2>();
        Vector3 move = transform.forward * moveInput.y + transform.right * moveInput.x;
        _controller.SimpleMove(move * _speed);

        // Вращение камеры
        Vector2 lookInput = _playerInput.actions["Look"].ReadValue<Vector2>();
        RotateCamera(lookInput);
    }

    private void RotateCamera(Vector2 lookInput)
    {
        // Вертикальный поворот камеры
        float verticalRotation = lookInput.y * _lookSensitivity;
        float newXRotation = _cameraTransform.localEulerAngles.x - verticalRotation;
        newXRotation = Mathf.Clamp(newXRotation, -90f, 90f);
        _cameraTransform.localEulerAngles = new Vector3(newXRotation, 0, 0);

        // Горизонтальный поворот всего игрока
        float horizontalRotation = lookInput.x * _lookSensitivity;
        transform.Rotate(0, horizontalRotation, 0);
    }
}