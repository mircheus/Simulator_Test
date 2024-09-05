using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEditor.SearchService;
using UnityEngine;
using UnityEngine.InputSystem;

public class BuildingState : InteractionState
{
    private readonly float _rayDistance;
    
    private LayerMask _targetLayerMask;
    private BuildingObject _interactedObject;
    private float _mouseScrollY;
    private float _rotateAngleDelta;
    private float _rotationAroundYAxis = 0f;
    // private int _targetLayerInt;
    private bool _isTouchingTargetSurface;
    // private Vector3 _delta;

    public BuildingState(IStateSwitcher stateSwitcher, StateMachineData data, Character character) : base(stateSwitcher,
        data, character)
    {
        // _targetLayerMask = _character.Config.InteractionConfig.GroundLayer;
        _rayDistance = _character.Config.InteractionConfig.RayDistance;
        _rotateAngleDelta = _character.Config.InteractionConfig.RotationAngleDelta;
    }

    public override void Enter()
    {
        base.Enter();
        _interactedObject = Data.ObjectToHold;
        _interactedObject.ActivateTrigger();
        // _targetLayerInt = _interactedObject.TargetSurfaceLayerValue;
        _targetLayerMask = _interactedObject.TargetSurfaceLayer;
        // _delta = CalculateDeltaVector();
        // Debug.Log("Delta on enter" + _delta);
    }

    public override void Exit()
    {
        base.Exit();
        _interactedObject.DeactivateTrigger();
        _interactedObject.SetInitialMaterial();
        // _delta = Vector3.zero;
        _interactedObject = null;
    }

    public override void HandleInput()
    {
        HandleRotation();
    }

    public override void Update()
    {
        if (Physics.Raycast(_character.CameraPosition, _character.CameraForward, out var hit, _rayDistance, _targetLayerMask))
        {
            if (hit.collider.gameObject.layer == LayerMask.NameToLayer(_interactedObject.InitialLayer))
            {
                PlaceObjectOnBox(_interactedObject, hit);
            }
            
            // PlaceObjectOnGround(_interactedObject, hit);
            // KeepObjectRotation(_interactedObject, hit);
            
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
        if (IsAllowedToPlaceObject())
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
    
    private void PlaceObjectOnBox(BuildingObject objectToPlace, RaycastHit hit)
    {
        _isTouchingTargetSurface = true;
        var neighbourCube = hit.collider.gameObject.GetComponent<Cube>();
        Vector3 objectPosition = objectToPlace.transform.position;
        Cube cube = (Cube)objectToPlace;
        objectToPlace.transform.rotation = Quaternion.identity * Quaternion.Euler(0, _rotationAroundYAxis, 0);
        objectToPlace.transform.position = Vector3.Lerp(objectPosition, neighbourCube.TopPoint.position + cube.DeltaVector, Time.deltaTime * 10f);
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

    private bool IsAllowedToPlaceObject()
    {
        return _interactedObject.IsCollidingAny == false && _isTouchingTargetSurface;
    }
}