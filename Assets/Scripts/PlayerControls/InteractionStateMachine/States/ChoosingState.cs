using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class ChoosingState : InteractionState
{
    private float _rayLength;
    
    public ChoosingState(IStateSwitcher stateSwitcher, StateMachineData data, Character character) : base(stateSwitcher, data, character)
    {
        _rayLength = _character.Config.InteractionConfig.ChoosingRayLength;
    }

    protected override void OnInteracted(InputAction.CallbackContext obj)
    {
        if (Physics.Raycast(_character.CameraPosition, _character.CameraForward, out var hit, _rayLength))
        {
            if(hit.collider.gameObject.TryGetComponent(out BuildingObject buildingObject))
            {
                Data.ObjectToHold = buildingObject;
                StateSwitcher.SwitchState<BuildingState>();
            }
        }
    }
}
