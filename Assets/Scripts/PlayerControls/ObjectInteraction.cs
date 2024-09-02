using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class ObjectInteraction : MonoBehaviour
{
    [SerializeField] private InputActionAsset _playerControls;
    [SerializeField] private float _interactionDistance = 5f;
    
    private InputAction _interactAction;

    private void Awake()
    {
        var playerActionMap = _playerControls.FindActionMap(Constants.PlayerActionMap);
        _interactAction = playerActionMap.FindAction(Constants.InteractAction);
    }
    
    private void OnEnable()
    {
        _interactAction.Enable();
    }
    
    private void OnDisable()
    {
        _interactAction.Disable();
    }
    
    private void Update()
    {
        if (_interactAction.IsPressed())
        {
            Interact();
        }
    }

    private void Interact()
    {
        RaycastHit hit;
        if (Physics.Raycast(transform.position, transform.forward, out hit, _interactionDistance))
        {
            Debug.Log(hit.collider.gameObject.name);
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.magenta;
        Gizmos.DrawRay(transform.position, transform.forward * _interactionDistance);
    }
}
