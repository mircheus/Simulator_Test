﻿using UnityEditor;
using UnityEngine;
using UnityEngine.InputSystem;

public class FirstPersonController : MonoBehaviour
{
    [Header("Config: ")]
    [SerializeField] private CharacterConfig _characterConfig;
        
    [Header("References: ")]
    [SerializeField] private InputActionAsset _playerControls;
    [SerializeField] private Transform _cameraTransform;
        
    private float _moveSpeed; 
    private float _mouseSensitivity; 
    private InputAction _moveAction;
    private InputAction _lookAction;
    private float _verticalRotation;
    private Vector2 _moveInput;
    private Vector2 _lookInput;
    private Vector3 _lookDirectionNoY;
    private Vector3 _resultVector;
    private float _rayCollisionLength;

    private void Awake()
    {
        var playerActionMap = _playerControls.FindActionMap(Constants.PlayerActionMap);
        _moveAction = playerActionMap.FindAction(Constants.MoveAction);
        _lookAction = playerActionMap.FindAction(Constants.LookAction);
        _rayCollisionLength = _characterConfig.MovementConfig.CollisionRayLength;
        _moveSpeed = _characterConfig.MovementConfig.MoveSpeed;
        _mouseSensitivity = _characterConfig.MovementConfig.MouseSensitivity;
        LockCursor();
    }

    private void OnEnable()
    {
        _moveAction.Enable();
        _lookAction.Enable();
    }

    private void OnDisable()
    {
        _moveAction.Disable();
        _lookAction.Disable();
    }
        
    private void Update()
    {
        HandleMovement();
        HandleLook();
    }

    private void HandleLook()
    {
        _lookInput = _lookAction.ReadValue<Vector2>();
        float mouseXRotation = _lookInput.x * _mouseSensitivity;
        transform.Rotate(0, mouseXRotation, 0);
        _verticalRotation -= _lookInput.y * _mouseSensitivity;
        _verticalRotation = Mathf.Clamp(_verticalRotation, -90, 90);
        _cameraTransform.localRotation = Quaternion.Euler(_verticalRotation, 0, 0);
    }

    private void HandleMovement()
    {
        _moveInput = _moveAction.ReadValue<Vector2>();
        var moveDirection = new Vector3(_moveInput.x, 0, _moveInput.y);
        var moveSpeed = _moveSpeed;
        var rayDirection = GetResultDirectionVector();
            
        if(Physics.Raycast(transform.position, rayDirection, out var hit, _rayCollisionLength))
        {
            if(hit.collider.TryGetComponent(out Wall wall) || hit.collider.TryGetComponent(out BuildingObject buildingObject))
            {
                moveSpeed = 0;
            }
        }
            
        transform.Translate(moveDirection * (moveSpeed * Time.deltaTime));
    }

    private void LockCursor()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    private Vector3 GetResultDirectionVector()
    {
        var resultVector = Vector3.zero;
            
        if (_moveInput != Vector2.zero)
        {
            var moveDirection = new Vector3(_moveInput.x, 0, _moveInput.y);
            resultVector = transform.TransformDirection(moveDirection);
        }
            
        return resultVector;
    }
}
