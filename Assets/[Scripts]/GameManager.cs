using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


// Difficulty variations
public enum Difficulty
{
    EASY,
    NORMAL,
    HARD
}

public class GameManager : MonoBehaviour
{
    private static GameManager _instance;

    public static GameManager GetInstance()
    {
        if (_instance != null)
        {
            return _instance;
        }

        return _instance;
    }

    public List<Vector2> blockedCoordinates;

    private void Awake()
    {
        _instance = this;
        blockedCoordinates = new List<Vector2>();

        blockedCoordinates.Add(new Vector2(1,5));
        blockedCoordinates.Add(new Vector2(2,1));
        blockedCoordinates.Add(new Vector2(3,8));
        blockedCoordinates.Add(new Vector2(4,9));
        blockedCoordinates.Add(new Vector2(5,0));
        blockedCoordinates.Add(new Vector2(6,6));
        blockedCoordinates.Add(new Vector2(7,2));
        blockedCoordinates.Add(new Vector2(8,4));
        blockedCoordinates.Add(new Vector2(9,9));
    }

    

    // difficulty to set
    public Difficulty difficulty;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
