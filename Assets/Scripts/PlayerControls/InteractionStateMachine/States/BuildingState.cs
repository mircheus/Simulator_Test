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

    public BuildingState(IStateSwitcher stateSwitcher, StateMachineData data, Character character) : base(stateSwitcher,
        data, character)
    {
        _groundLayer = _character.Config.InteractionConfig.GroundLayer;
        _rayDistance = _character.Config.InteractionConfig.RayDistance;
    }

    protected PlayerInput Input => _character.Input;

    public override void Enter()
    {
        base.Enter();
        _interactedObject = Data.ObjectToHold;
    }

    public void Exit()
    {
        base.Exit();
        _interactedObject = null;
    }

    public override void Update()
    {
        if (Physics.Raycast(_character.CameraPosition, _character.CameraForward, out var hit, _rayDistance, 3))
        {
            PlaceObjectOnGround(_interactedObject, hit);
        }
        else
        {
            HoldObject();
        }
    }

    protected override void OnInteracted(InputAction.CallbackContext obj)
    {

    }

    private void HoldObject()
    {
        Vector3 targetPosition = _character.CameraPosition + _character.CameraForward * 2f;
        var interactedObjectTransform = _interactedObject.transform;
        // _objectToHold.transform.position = Vector3.Slerp(_objectToHold.transform.position, targetPosition, Time.deltaTime * 10f);
        interactedObjectTransform.rotation = Quaternion.LookRotation(_character.transform.forward, Vector3.up);
        interactedObjectTransform.position = Vector3.Lerp(interactedObjectTransform.position, targetPosition, Time.deltaTime * 10f); // less smooth
    }

    private void PlaceObjectOnGround(BuildingObject objectToPlace, RaycastHit hit)
    {
        Vector3 anchorPointPosition = objectToPlace.AnchorPoint.transform.position;
        Vector3 objectPosition = objectToPlace.transform.position;
        Vector3 delta = objectPosition - anchorPointPosition;
        objectToPlace.transform.position = Vector3.Lerp(objectPosition, hit.point + delta, Time.deltaTime * 10f);
        Vector3 normal = hit.normal;
        Vector3 right = _character.CameraRight;
        Vector3 forward = Vector3.Cross(right, normal);
        objectToPlace.transform.rotation = Quaternion.LookRotation(forward, normal);
    }
}