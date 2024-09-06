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
    private string _initialLayer;
    
    public LayerMask TargetSurfaceLayer => _targetSurfaceLayer;
    public Transform AnchorPoint => _anchorPoint;
    public bool IsCollidingAny => _isCollidingAny;
    public string InitialLayer => _initialLayer;

    private void Start()
    {
        _renderer = GetComponent<Renderer>();
        _collider = GetComponent<Collider>();
        _initialLayer = LayerMask.LayerToName(gameObject.layer);
    }

    public void ActivateTrigger()
    {
        _collider.isTrigger = true;
        gameObject.layer = LayerMask.NameToLayer(Constants.InteractiveLayer);
    }

    public void DeactivateTrigger()
    {
        _collider.isTrigger = false;
        gameObject.layer = LayerMask.NameToLayer(_initialLayer);
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
