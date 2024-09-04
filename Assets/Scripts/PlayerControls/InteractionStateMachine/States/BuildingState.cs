using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEditor.SearchService;
using UnityEngine;
using UnityEngine.InputSystem;

public class BuildingState : InteractionState
{
    private readonly LayerMask _groundLayer;
    private readonly float _rayDistance;
    private BuildingObject _interactedObject;
    private float _mouseScrollY;
    private float _rotateAngleDelta;
    private float _rotationAroundYAxis = 0f;
    private bool _isTouchingTargetSurface;
    
    public BuildingState(IStateSwitcher stateSwitcher, StateMachineData data, Character character) : base(stateSwitcher,
        data, character)
    {
        _groundLayer = _character.Config.InteractionConfig.GroundLayer;
        _rayDistance = _character.Config.InteractionConfig.RayDistance;
        _rotateAngleDelta = _character.Config.InteractionConfig.RotationAngleDelta;
    }

    public override void Enter()
    {
        base.Enter();
        _interactedObject = Data.ObjectToHold;
        _interactedObject.ActivateTrigger();
    }

    public override void Exit()
    {
        base.Exit();
        _interactedObject.DeactivateTrigger();
        _interactedObject.SetInitialMaterial();
        _interactedObject = null;
    }

    public override void HandleInput()
    {
        HandleRotation();
    }

    public override void Update()
    {
        if (Physics.Raycast(_character.CameraPosition, _character.CameraForward, out var hit, _rayDistance, 3))
        {
            PlaceObjectOnGround(_interactedObject, hit);
            KeepObjectRotation(_interactedObject, hit);
            
            if (_interactedObject.IsCollidingAny)
            {
                _interactedObject.SetRedMaterial();
            }
            else
            {
                _interactedObject.SetGreenMaterial();
            }
        }
        else
        {
            HoldObject();
            _interactedObject.SetTransparentMaterial();
        }
    }

    protected override void OnInteracted(InputAction.CallbackContext obj)
    {
        if(_interactedObject.IsCollidingAny == false && _isTouchingTargetSurface)
        {
            StateSwitcher.SwitchState<ChoosingState>();
        }
    }

    private void HoldObject()
    {
        _isTouchingTargetSurface = false;
        Vector3 targetPosition = _character.CameraPosition + _character.CameraForward * 2f;
        var interactedObjectTransform = _interactedObject.transform;
        var characterForward = _character.transform.forward;
        interactedObjectTransform.rotation = Quaternion.LookRotation(characterForward, Vector3.up) * Quaternion.Euler(0, _rotationAroundYAxis, 0);
        interactedObjectTransform.position = Vector3.Lerp(interactedObjectTransform.position, targetPosition, Time.deltaTime * 10f); // less smooth
    }
    
    private void PlaceObjectOnGround(BuildingObject objectToPlace, RaycastHit hit)
    {
        _isTouchingTargetSurface = true;
        Vector3 anchorPointPosition = objectToPlace.AnchorPoint.transform.position;
        Vector3 objectPosition = objectToPlace.transform.position;
        Vector3 delta = objectPosition - anchorPointPosition;
        objectToPlace.transform.position = Vector3.Lerp(objectPosition, hit.point + delta, Time.deltaTime * 10f);
    }

    private void KeepObjectRotation(BuildingObject objectToPlace, RaycastHit hit)
    {
        Vector3 normal = hit.normal;
        Vector3 right = _character.CameraRight;
        Vector3 forward = Vector3.Cross(right, normal);
        objectToPlace.transform.rotation = Quaternion.LookRotation(forward, normal) * Quaternion.Euler(0, _rotationAroundYAxis, 0);
    }

    private void HandleRotation()
    {
        Input.Player.Scroll.performed += x => _mouseScrollY = x.ReadValue<float>();
        
        if(_mouseScrollY > 0)
        {
            _rotationAroundYAxis += _rotateAngleDelta;
        }
        else if(_mouseScrollY < 0)
        {
            _rotationAroundYAxis -= _rotateAngleDelta;
        }
    }
}