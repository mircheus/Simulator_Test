using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class ChoosingState : InteractionState
{
    public ChoosingState(IStateSwitcher stateSwitcher, StateMachineData data, Character character) : base(stateSwitcher, data, character)
    {
    }

    protected override void OnInteracted(InputAction.CallbackContext obj)
    {
        if (Physics.Raycast(_character.CameraPosition, _character.CameraForward, out var hit, 5f)) // TODO: remove magic number
        {
            if(hit.collider.gameObject.TryGetComponent(out Cube cube))
            {
                Data.ObjectToHold = cube;
                StateSwitcher.SwitchState<BuildingState>();
            }
        }
    }
}
