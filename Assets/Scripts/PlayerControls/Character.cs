using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
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
    public Vector3 CameraRight => _cameraTransform.right;

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
    
    private void OnDrawGizmos()
    {
        int groundLayer = Config.InteractionConfig.GroundLayer.value;
        LayerMask targetLayer = 6;
        Vector3 viewDirection = CameraForward;
        Handles.color = Color.magenta;
        Handles.DrawLine(CameraPosition, CameraPosition + CameraForward, 6f);
        Ray ray = new Ray(CameraPosition, CameraForward);
        
        if (Physics.Raycast(ray, out RaycastHit hit, 500, targetLayer))
        {
            Handles.color = Color.white;
            Handles.DrawLine(CameraPosition, hit.point, 10f);
            DrawUnitVectorsFromHit(hit);
        }
    }
    
    private void DrawUnitVectorsFromHit(RaycastHit hit)
    {
        Vector3 right = Vector3.Cross(hit.normal, CameraForward).normalized;
        Vector3 forward = Vector3.Cross(right, hit.normal).normalized;
        Handles.color = Color.green;
        Handles.DrawLine(hit.point, hit.point + hit.normal, 4f);
        Handles.color = Color.red;
        Handles.DrawLine(hit.point, hit.point + right, 4f);
        Handles.color = Color.cyan;
        Handles.DrawLine(hit.point, hit.point + forward, 4f);
    }
}
