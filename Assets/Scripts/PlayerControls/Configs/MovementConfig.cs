using System;
using UnityEngine;

[Serializable]
public class MovementConfig
{
    [field: SerializeField, Range(0, 10)] public float MoveSpeed { get; private set; }
    [field: SerializeField, Range(0, 1)] public float MouseSensitivity { get; private set; }
    [field: SerializeField, Range(0, 3)] public float CollisionRayLength { get; private set; }
}