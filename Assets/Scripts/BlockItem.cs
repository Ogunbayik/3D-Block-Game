using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlockItem : MonoBehaviour
{
    public Node CurrentNode {  get; private set; }

    public void SetNode(Node node) => CurrentNode = node;
}
