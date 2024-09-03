using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "CharacterConfig", menuName = "Configs/CharacterConfig")]
public class CharacterConfig : ScriptableObject
{
    [SerializeField] private MovementConfig _movementConfig;
    [SerializeField] private InteractionConfig _interactionConfig;
    
    public MovementConfig MovementConfig => _movementConfig;
    public InteractionConfig InteractionConfig => _interactionConfig;
}
