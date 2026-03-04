using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public static class GameSignal
{
    public class OnShapePlaced
    {
        public BaseShape Shape;
        public OnShapePlaced(BaseShape shape) => Shape = shape;
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
    public class OnGameStateChanged
    {
        public GameState NewState;
        public OnGameStateChanged(GameState newState)  => NewState = newState;
    }

    //Butonlar için signaller
    public class OnClickLevelButton
    {
        public LevelButton LevelButton;
        public OnClickLevelButton(LevelButton levelButton) { LevelButton = levelButton; }
    }
    public class OnClickPlayButton { }
    public class OnClickExitButton { }
    public class OnClickBackButton { }

}
