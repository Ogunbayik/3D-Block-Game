using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cell : MonoBehaviour
{
    private MeshRenderer _meshRenderer;

    [Header("Visual References")]
    [SerializeField] private GameObject _visualPrefab;

    private void Awake() => _meshRenderer = _visualPrefab.GetComponent<MeshRenderer>();
    public void SetCellColor(Color color) => _meshRenderer.material.color = color;
    public void SetCellScale(Vector3 scale) => _visualPrefab.transform.localScale = scale;
}
