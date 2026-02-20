using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class GameSignal
{
    public class OnShapePlaced
    {
        public GameObject Shape;
        public OnShapePlaced(GameObject shape) => Shape = shape;
    }
    public class OnMatchesFound 
    {
        public int RowMatchCount;
        public int ColMatchCount;

        public OnMatchesFound(int rowMatchCount, int colMatchCount)
        {
            RowMatchCount = rowMatchCount;
            ColMatchCount = colMatchCount;
        }
    }
    public class OnSlotCleared { }
    public class OnSpawnedNewBlocks { }
    public class OnAllShapePlaced { }
    public class OnGameLevelPassed { }

}
