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
    public class OnSlotCleared { }
    public class OnMatchesFound { }

}
