using System;
using System.Collections;
using System.Collections.Generic;
using System.Numerics;
using UnityEngine;
using UnityEngine.tvOS;
using UnityEngine.UIElements;
using Vector3 = UnityEngine.Vector3;

public class Cube : BuildingObject
{
    [SerializeField] private Transform _topPoint;
    
    private Vector3 _deltaVector;
    private bool _isJoinCubeMode = false;
    
    public Transform TopPoint => _topPoint;
    public Vector3 DeltaVector => CalculateDeltaVector();
    
    private void OnTriggerEnter(Collider collider)
    {
        if (collider.TryGetComponent(out BuildingObject cube) || collider.TryGetComponent(out Wall wall))
        {
            _isCollidingAny = true;
        }
    }
    
    private void OnTriggerExit(Collider collider)
    {
        if (collider.TryGetComponent(out BuildingObject cube) || collider.TryGetComponent(out Wall wall))
        {
            _isCollidingAny = false;
        }
    }

    public void EnableJoinCubeMode()
    {
        _isJoinCubeMode = true;
    }
    
    public void DisableJoinCubeMode()
    {
        _isJoinCubeMode = false;
    }
    
    private Vector3 CalculateDeltaVector()
    {
        Vector3 anchorPointPosition = AnchorPoint.transform.position;
        Vector3 objectPosition = transform.position;
        Vector3 delta = objectPosition - anchorPointPosition;
        return delta;
    }
    
    private bool IsCollidedWithWall(Collider collider)
    {
        return collider.TryGetComponent(out Wall wall);
    }
    
    private bool IsCollidedWithCubeAndWall(Collider collider)
    {
        return collider.TryGetComponent(out BuildingObject cube) || collider.TryGetComponent(out Wall wall);
    }
}
