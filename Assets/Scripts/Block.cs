using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Block : MonoBehaviour
{
    private BlockAnimationController _animationController;

    [Header("Local Coordinates")]
    [Tooltip("Bu küpün Prefab içindeki yerel X konumu")]
    [SerializeField] private int _localX;

    [Tooltip("Bu küpün Prefab içindeki yerel Y konumu")]
    [SerializeField] private int _localY;
    [Header("Visual References")]
    [SerializeField] private MeshRenderer _meshRenderer;
    public int LocalX => _localX;
    public int LocalY => _localY;
    private void Awake() => _animationController = GetComponent<BlockAnimationController>();
    
    public void SetScale(Vector3 newScale) => transform.localScale = newScale;
    public void PlayMatchAnimation() => _animationController.PlayPulseDissolve();
    public void SetColor(Color color)
    {
        if (_meshRenderer != null)
        {
            _meshRenderer.material.color = color;
        }
    }
}
