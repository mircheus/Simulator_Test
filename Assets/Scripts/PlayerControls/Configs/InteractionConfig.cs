using System;
using UnityEngine;

[Serializable]
public class InteractionConfig
{
    [field: SerializeField] public float BuildingRayDistance { get; private set; }
    [field: SerializeField] public float ChoosingRayDistance { get; private set; }
    [field: SerializeField, Range(0, 90)] public float RotationAngleDelta { get; private set; }
}
