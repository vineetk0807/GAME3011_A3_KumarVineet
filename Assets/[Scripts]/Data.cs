using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Data
{
    public static Difficulty difficulty = Difficulty.NORMAL;
    public static int Score = 0;

    public static void ResetData()
    {
        difficulty = Difficulty.NORMAL;
        Score = 0;
    }
}
