using System;
using UnityEngine;

[Serializable]
public class MovementConfig
{
    [field: SerializeField, Range(1, 3)] public float MoveSpeed { get; private set; }
}