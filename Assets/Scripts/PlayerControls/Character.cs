using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Character : MonoBehaviour
{
    [SerializeField] private CharacterConfig _config;
    [SerializeField] private Transform _cameraTransform;
    
    private PlayerInput _input;
    private InteractionStateMachine _stateMachine;
    public PlayerInput Input => _input;
    public CharacterConfig Config => _config;
    public Vector3 CameraPosition => _cameraTransform.position;
    public Vector3 CameraForward => _cameraTransform.forward;

    private void Awake()
    {
        _input = new PlayerInput();
        _stateMachine = new InteractionStateMachine(this);
    }

    private void Update()
    {
        _stateMachine.HandleInput();
        _stateMachine.Update();        
    }

    private void OnEnable() => _input.Enable();

    private void OnDisable() => _input.Disable();
}
