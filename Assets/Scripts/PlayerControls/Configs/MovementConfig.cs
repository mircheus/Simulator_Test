using System;
using UnityEngine;

[Serializable]
public class MovementConfig
{
    [field: SerializeField, Range(0, 3)] public float MoveSpeed { get; private set; }
    [field: SerializeField, Range(0, 3)] public float RayCollisionLength { get; private set; }
}