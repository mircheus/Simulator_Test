using System;
using UnityEngine;

[Serializable]
public class InteractionConfig
{
    [field: SerializeField] public LayerMask GroundLayer { get; private set; }
    [field: SerializeField] public float RayDistance { get; private set; }
}
