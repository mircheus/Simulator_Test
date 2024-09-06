using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class ChoosingState : InteractionState
{
    private float _rayDistance;
    
    public ChoosingState(IStateSwitcher stateSwitcher, StateMachineData data, Character character) : base(stateSwitcher, data, character)
    {
    }
    
    public override void Enter()
    {
        base.Enter();
        _rayDistance = _character.Config.InteractionConfig.ChoosingRayDistance;
    }

    protected override void OnInteracted(InputAction.CallbackContext obj)
    {
        if (Physics.Raycast(_character.CameraPosition, _character.CameraForward, out var hit, _rayDistance))
        {
            if(hit.collider.gameObject.TryGetComponent(out BuildingObject buildingObject))
            {
                Data.ObjectToHold = buildingObject;
                StateSwitcher.SwitchState<BuildingState>();
            }
        }
    }
}
