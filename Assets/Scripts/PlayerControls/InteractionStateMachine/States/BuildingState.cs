using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class BuildingState : InteractionState
{
    private GameObject _objectToHold;

    public BuildingState(IStateSwitcher stateSwitcher, StateMachineData data, Character character) : base(stateSwitcher, data, character)
    {
    }
    
    protected PlayerInput Input => _character.Input;
    
    public override void Enter()
    {
        base.Enter();
        _objectToHold = Data.ObjectToHold;
    }

    public void Exit()
    {
        base.Exit();
        _objectToHold = null;
    }

    public override void Update()
    {
        HoldObject();
    }
    
    protected override void OnInteracted(InputAction.CallbackContext obj)
    {
        
    }

    private void HoldObject()
    {
        Vector3 targetPosition = _character.CameraPosition + _character.CameraForward * 2f;
        _objectToHold.transform.position = Vector3.Slerp(_objectToHold.transform.position, targetPosition, Time.deltaTime * 10f);
        // _objectToHold.transform.position = Vector3.Lerp(_objectToHold.transform.position, targetPosition, Time.deltaTime * 10f); // less smooth
    }
}
