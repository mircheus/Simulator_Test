using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cube : MonoBehaviour
{
    [SerializeField] private Transform _anchorPoint;
    
    public Transform AnchorPoint => _anchorPoint;
}
