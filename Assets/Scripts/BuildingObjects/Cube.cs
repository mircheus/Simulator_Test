using System;
using System.Collections;
using System.Collections.Generic;
using System.Numerics;
using UnityEngine;
using UnityEngine.tvOS;
using UnityEngine.UIElements;
using Quaternion = UnityEngine.Quaternion;
using Vector3 = UnityEngine.Vector3;

public class Cube : BuildingObject, IJoinable
{
    [SerializeField] private Transform _joinPoint;
    
    private Vector3 _deltaVector;

    public Transform JoinPoint => _joinPoint;
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

    private Vector3 CalculateDeltaVector()
    {
        Vector3 anchorPointPosition = AnchorPoint.transform.position;
        Vector3 objectPosition = transform.position;
        Vector3 delta = objectPosition - anchorPointPosition;
        return delta;
    }

    public bool IsAbleToJoin(BuildingObject buildingObject)
    {
        bool isBuildingObjectCube = buildingObject is Cube;
        bool result = isBuildingObjectCube;
        return result;
    }

    public Vector3 GetJoinPoint(BuildingObject buildingObject)
    {
        Cube cube = (Cube)buildingObject;
        return JoinPoint.position + cube.DeltaVector;
    }

    public Quaternion GetJoinRotation()
    {
        return gameObject.transform.rotation;
    }
}
