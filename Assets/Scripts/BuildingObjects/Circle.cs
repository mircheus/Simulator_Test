using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Circle : BuildingObject
{
    private void OnTriggerEnter(Collider collider)
    {
        if (collider.TryGetComponent(out Circle circle))
        {
            _isCollidingAny = true;
        }
    }
    
    private void OnTriggerExit(Collider collider)
    {
        if (collider.TryGetComponent(out Circle circle))
        {
            _isCollidingAny = false;
        }
    }
}
