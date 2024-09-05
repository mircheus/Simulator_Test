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
    [SerializeField] private Transform _topPoint;
    
    private Vector3 _deltaVector;

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

    private Vector3 CalculateDeltaVector()
    {
        Vector3 anchorPointPosition = AnchorPoint.transform.position;
        Vector3 objectPosition = transform.position;
        Vector3 delta = objectPosition - anchorPointPosition;
        return delta;
    }

    public Vector3 GetJoinPoint()
    {
        return TopPoint.position;
    }

    public void Join(BuildingObject buildingObject)
    {
        Vector3 objectPosition = buildingObject.transform.position;
        Cube cube = (Cube)buildingObject;
        // buildingObject.transform.rotation = Quaternion.identity * Quaternion.Euler(0, rotationAroundYAxis, 0);
        buildingObject.transform.position = Vector3.Lerp(objectPosition, TopPoint.position + cube.DeltaVector, Time.deltaTime * 10f);
    }
}
