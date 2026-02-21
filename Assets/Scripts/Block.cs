using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Block : MonoBehaviour
{
    [Header("Local Coordinates")]
    [Tooltip("Bu küpün Prefab içindeki yerel X konumu")]
    [SerializeField] private int _localX;

    [Tooltip("Bu küpün Prefab içindeki yerel Y konumu")]
    [SerializeField] private int _localY;
    [Header("Visual References")]
    [SerializeField] private MeshRenderer _meshRenderer;
    public int LocalX => _localX;
    public int LocalY => _localY;
    public void SetColor(Color color)
    {
        if (_meshRenderer != null)
        {
            _meshRenderer.material.color = color;
        }
    }
}
