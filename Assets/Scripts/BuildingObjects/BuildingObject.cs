using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Collider))]
public abstract class BuildingObject: MonoBehaviour
{
    [Header("References: ")]
    [SerializeField] protected Transform _anchorPoint;
    [SerializeField] protected LayerMask _targetSurfaceLayer;
    [Header("Building color materials: ")]
    [SerializeField] protected Material _initialMaterial;
    [SerializeField] protected Material _allowMaterial;
    [SerializeField] protected Material _notAllowMaterial;
    [SerializeField] protected Material _transparentMaterial;
    
    protected bool _isCollidingAny = false;
    
    private Renderer _renderer;
    private Collider _collider;
    
    public LayerMask TargetSurfaceLayer => _targetSurfaceLayer;
    public Transform AnchorPoint => _anchorPoint;
    public bool IsCollidingAny => _isCollidingAny;

    private void Start()
    {
        _renderer = GetComponent<Renderer>();
        _collider = GetComponent<Collider>();
    }

    public void ActivateTrigger()
    {
        _collider.isTrigger = true;
    }

    public void DeactivateTrigger()
    {
        _collider.isTrigger = false;
    }

    public void SetGreenMaterial()
    {
        _renderer.material = _allowMaterial;
    }
    
    public void SetRedMaterial()
    {
        _renderer.material = _notAllowMaterial;
    }
    
    public void SetInitialMaterial()
    {
        _renderer.material = _initialMaterial;
    }
    
    public void SetTransparentMaterial()
    {
        _renderer.material = _transparentMaterial;
    }
}
