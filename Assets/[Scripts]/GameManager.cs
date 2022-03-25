using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


/// <summary>
/// This Match-3 game was referred from an online tutorial video and was as I saw fit at places that were requirements for this assignment
/// Reference Link: https://www.youtube.com/watch?v=i7jTb-dEpqM
/// </summary>


// Difficulty variations
public enum Difficulty
{
    EASY,
    NORMAL,
    HARD
}

//STAR,
//HEART,
//QUAD,
//PENTA,
//OCTA,

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
        LoadBlockedCoordinates();
    }

    private int points = 0;

    public List<int> GemsTaken;

    public int numberOfMovesRemaining = 10;
    public int easyMoveCount;
    public int normalMoveCount;
    public int hardMoveCount;

    // difficulty to set
    public Difficulty difficulty;

    // Time
    private float currentTime = 0f;
    [Header("Timer")]
    public int timeRemaining;
    public List<int> difficultyBasedTime;


    // Start is called before the first frame update
    void Start()
    {
        GemsTaken = new List<int>();
        InitializeGemList();
        SetNumberOfMoves();

        timeRemaining = difficultyBasedTime[(int)difficulty];
    }


    // Update is called once per frame
    void Update()
    {
        UpdateTimer();
    }

    /// <summary>
    /// Updates timer
    /// </summary>
    private void UpdateTimer()
    {
        if (Time.time - currentTime >= 1)
        {
            currentTime = Time.time;
            timeRemaining -= 1;
        }
    }

    /// <summary>
    /// Set number of moves based on difficulty
    /// </summary>
    private void SetNumberOfMoves()
    {
        switch (difficulty)
        {
            case Difficulty.EASY:
                numberOfMovesRemaining = easyMoveCount;
                break;

            case Difficulty.NORMAL:
                numberOfMovesRemaining = normalMoveCount;
                break;

            case Difficulty.HARD:
                numberOfMovesRemaining = hardMoveCount;
                break;

            default:
                numberOfMovesRemaining = normalMoveCount;
                break;
        }
    }

    /// <summary>
    /// Initialize the gem list
    /// </summary>
    public void InitializeGemList()
    {
        for (int i = 0; i < (int)GemColor.ColorType.TOTAL_COLOR_TYPES - 1; i++)
        {
            GemsTaken.Add(0);
        }
       
    }

    /// <summary>
    /// Update points
    /// </summary>
    /// <param name="points"></param>
    public void UpdatePoints(int points)
    {
        this.points += points;
    }

    /// <summary>
    /// Resets game
    /// </summary>
    public void ResetGame()
    {
        // Points
        points = 0;

        // Gems
        for (int i = 0; i < GemsTaken.Count; i++)
        {
            GemsTaken[i] = 0;
        }
    }

    /// <summary>
    /// Loads blocked coordinates
    /// </summary>
    private void LoadBlockedCoordinates()
    {
        blockedCoordinates.Add(new Vector2(1, 5));
        blockedCoordinates.Add(new Vector2(2, 1));
        blockedCoordinates.Add(new Vector2(3, 8));
        blockedCoordinates.Add(new Vector2(4, 9));
        blockedCoordinates.Add(new Vector2(5, 5));
        blockedCoordinates.Add(new Vector2(6, 6));
        blockedCoordinates.Add(new Vector2(7, 2));
        blockedCoordinates.Add(new Vector2(8, 4));
        blockedCoordinates.Add(new Vector2(9, 3));
    }
}
