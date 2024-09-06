using System;
using UnityEngine;

[Serializable]
public class InteractionConfig
{
    [field: SerializeField] public float BuildingRayLength { get; private set; }
    [field: SerializeField] public float ChoosingRayLength { get; private set; }
    [field: SerializeField, Range(0, 90)] public float RotationAngleDelta { get; private set; }
}
