using UnityEngine;
using UnityEngine.InputSystem;

public class BuildingState : InteractionState
{
    private readonly float _rayLength;
    
    private LayerMask _targetLayerMask;
    private BuildingObject _interactedObject;
    private float _mouseScrollY;
    private float _rotateAngleDelta;
    private float _rotationAroundYAxis = 0f;
    private bool _isTouchingTargetSurface;
    private bool _isJoinMode;

    public BuildingState(IStateSwitcher stateSwitcher, StateMachineData data, Character character) : base(stateSwitcher,
        data, character)
    {
        _rayLength = _character.Config.InteractionConfig.BuildingRayLength;
        _rotateAngleDelta = _character.Config.InteractionConfig.RotationAngleDelta;
    }

    public override void Enter()
    {
        base.Enter();
        _interactedObject = Data.ObjectToHold;
        _interactedObject.ActivateTrigger();
        _targetLayerMask = _interactedObject.TargetSurfaceLayer;
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
        if (Physics.Raycast(_character.CameraPosition, _character.CameraForward, out var hit, _rayLength, _targetLayerMask))
        {
            _isJoinMode = false;
            
            if (hit.collider.TryGetComponent(out IJoinable joinObject) && joinObject.IsAbleToJoin(_interactedObject))
            {
                _isJoinMode = true;
                Vector3 joinPoint = joinObject.GetJoinPoint(_interactedObject);
                Quaternion joinRotation = joinObject.GetJoinRotation();
                JoinObject(_interactedObject, joinPoint, joinRotation);
            }
            else
            {
                PlaceObjectOnSurface(_interactedObject, hit);
                KeepForwardLookRotation(_interactedObject, hit);
            }
            
            SetObjectColor();
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
        interactedObjectTransform.position = Vector3.Lerp(interactedObjectTransform.position, targetPosition, Time.deltaTime * 10f); 
    }
    
    private void PlaceObjectOnSurface(BuildingObject objectToPlace, RaycastHit hit)
    {
        _isTouchingTargetSurface = true;
        Vector3 anchorPointPosition = objectToPlace.AnchorPoint.transform.position;
        Vector3 objectPosition = objectToPlace.transform.position;
        Vector3 delta = objectPosition - anchorPointPosition;
        objectToPlace.transform.position = Vector3.Lerp(objectPosition, hit.point + delta, Time.deltaTime * 10f);
    }

    private void JoinObject(BuildingObject objectToPlace, Vector3 joinPosition, Quaternion joinRotation)
    {
        _isTouchingTargetSurface = true;
        _isJoinMode = true;
        var transform = objectToPlace.transform;
        Vector3 objectPosition = transform.position;
        transform.rotation = joinRotation  * Quaternion.Euler(0, _rotationAroundYAxis, 0);
        objectToPlace.transform.position = Vector3.Lerp(objectPosition, joinPosition, Time.deltaTime * 10f);
    }

    private void KeepForwardLookRotation(BuildingObject objectToPlace, RaycastHit hit)
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
        if(_isJoinMode)
        {
            
            return true;
        }
        
        return _interactedObject.IsCollidingAny == false && _isTouchingTargetSurface;
    }
    
    private void SetObjectColor()
    {
        if (_isJoinMode)
        {
            _interactedObject.SetGreenMaterial();
        }
        else
        {
            if (_interactedObject.IsCollidingAny)
            {
                _interactedObject.SetRedMaterial();
            }
            else
            {
                _interactedObject.SetGreenMaterial();
            }
        }
    }
}