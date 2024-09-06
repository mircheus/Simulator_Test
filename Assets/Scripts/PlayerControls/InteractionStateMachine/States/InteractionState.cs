using UnityEngine.InputSystem;

public class InteractionState : IState
{
    protected readonly IStateSwitcher StateSwitcher;
    protected readonly StateMachineData Data;
    protected readonly Character _character;

    public InteractionState(IStateSwitcher stateSwitcher, StateMachineData data, Character character)
    {
        StateSwitcher = stateSwitcher;
        Data = data; 
        _character = character;
    }
    
    protected PlayerInput Input => _character.Input;
    
    public virtual void Enter()
    {
        AddInputActionsCallbacks();
    }

    public virtual void Exit()
    {
        RemoveInputActionsCallbacks();
    }

    public virtual void HandleInput()
    {
    }

    public virtual void Update()
    {
    }
    
    protected virtual void AddInputActionsCallbacks()
    {
        Input.Player.Interact.performed += OnInteracted;
    }
    
    protected virtual void RemoveInputActionsCallbacks()
    {
        Input.Player.Interact.performed -= OnInteracted;
    }
    
    protected virtual void OnInteracted(InputAction.CallbackContext obj) { }
}
