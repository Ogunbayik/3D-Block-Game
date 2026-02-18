using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shape : MonoBehaviour
{
    public List<Vector2Int> BlockOffsets { get; private set; } = new List<Vector2Int>();

    private void Awake() => InitializeShape();
    private void InitializeShape()
    {
        BlockOffsets.Clear();

        foreach (Transform child in transform)
        {
            if (child.GetComponent<BlockItem>() == null) continue;

            int x = Mathf.RoundToInt(child.localPosition.x);
            int z = Mathf.RoundToInt(child.localPosition.z);

            BlockOffsets.Add(new Vector2Int(x, z));
        }
    }
}
