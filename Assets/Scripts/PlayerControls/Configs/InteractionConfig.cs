using System;
using UnityEngine;

[Serializable]
public class InteractionConfig
{
    [field: SerializeField] public LayerMask GroundLayer { get; private set; }
    [field: SerializeField] public float RayDistance { get; private set; }
    [field: SerializeField, Range(0, 90)] public float RotationAngleDelta { get; private set; }
    
}
