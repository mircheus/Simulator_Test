using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.tvOS;
using UnityEngine.UIElements;

public class Cube : BuildingObject
{
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
}
