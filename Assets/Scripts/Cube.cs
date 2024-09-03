using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.tvOS;
using UnityEngine.UIElements;

public class Cube : MonoBehaviour
{
    [Header("References: ")]
    [SerializeField] private Transform _anchorPoint;
    [SerializeField] private Transform _topPoint;
    [Header("Building color materials: ")]
    [SerializeField] private Material _allowMaterial;
    [SerializeField] private Material _notAllowMaterial;
    [SerializeField] private Material _defaultMaterial;
    [SerializeField] private Material _transparentMaterial;

    private Renderer _renderer;
    private LayerMask _layer;
    private BoxCollider _collider;
    private bool _isCollidingAny = false;
    
    public Transform AnchorPoint => _anchorPoint;
    public Transform TopPoint => _topPoint;
    public bool IsCollidingAny => _isCollidingAny;

    private void Start()
    {
        _renderer = GetComponent<Renderer>();
        _collider = GetComponent<BoxCollider>();
        // _layer = GetComponent<LayerMask>();
        int cubeLayer = LayerMask.NameToLayer("Cube");
    }

    public void ActivateTrigger()
    {
        _collider.isTrigger = true;
    }

    public void DeactivateTrigger()
    {
        _collider.isTrigger = false;
    }

    private void OnTriggerEnter(Collider collider)
    {
        if (collider.TryGetComponent(out Cube cube))
        {
            _isCollidingAny = true;
        }
    }
    
    private void OnTriggerExit(Collider collider)
    {
        if (collider.TryGetComponent(out Cube cube))
        {
            _isCollidingAny = false;
        }
    }

    // private void OnCollisionEnter(Collider collider)
    // {
    //     if (collider.TryGetComponent(out Cube cube))
    //     {
    //         Debug.Log("Collision Enter");
    //         _isCollidingAny = true;
    //     }
    // }
    //
    // private void OnCollisionExit(Collider collider)
    // {
    //     if (collider.TryGetComponent(out Cube cube))
    //     {
    //         Debug.Log("Collision Exit");
    //         _isCollidingAny = false;
    //     }
    // }

    public void SetGreenMaterial()
    {
        _renderer.material = _allowMaterial;
    }
    
    public void SetRedMaterial()
    {
        _renderer.material = _notAllowMaterial;
    }
    
    public void SetDefaultMaterial()
    {
        _renderer.material = _defaultMaterial;
    }
}
