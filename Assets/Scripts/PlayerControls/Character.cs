using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Character : MonoBehaviour
{
    [SerializeField] private CharacterConfig _config;

    private PlayerInput _input;
    private CharacterStateMachine _stateMachine;
    public PlayerInput Input => _input;
    public CharacterConfig Config => _config;
    
    private void Awake()
    {
        _input = new PlayerInput();
        _stateMachine = new CharacterStateMachine(this);
    }

    private void Update()
    {
        // _stateMachine.HandleInput();
        // _stateMachine.Update();        
    }

    private void OnEnable() => _input.Enable();

    private void OnDisable() => _input.Disable();
}
