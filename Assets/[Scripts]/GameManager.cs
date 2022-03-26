using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using Random = UnityEngine.Random;


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
    public List<int> GemsObjective;

    [Header("Total GemsTypes as Objectives")]
    public int objectiveEasyNormal = 2;
    public int objectiveHard = 3;
    private int objectivesRemaining;

    [Header("Total Gems to Collect")]
    public int totalGemsToCollectEasyNormal = 25;
    public int totalGemsToCollectHard = 40;

    [Header("Moves Remaining")]
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

    [Header("End Screen")] 
    public GameObject EndScreenPanel;
    public bool hasWon = false;
    public bool isGameOver = false;


    [Header("UI")] 
    public TextMeshProUGUI points_TMP;
    public TextMeshProUGUI timer_TMP;
    public TextMeshProUGUI difficulty_TMP;
    public TextMeshProUGUI moves_TMP;

    [Header("Colors")] 
    public Color lessTimeMovesRedColor;
    public Color scoreColor;
    public Color bonusScore;

    // Start is called before the first frame update
    void Start()
    {
        GemsTaken = new List<int>();
        GemsObjective = new List<int>();

        // Set difficulty
        difficulty = Data.difficulty;

        InitializeGemList();
        SetNumberOfMoves();

        timeRemaining = difficultyBasedTime[(int)difficulty];

        if (difficulty == Difficulty.EASY || difficulty == Difficulty.NORMAL)
        {
            objectivesRemaining = objectiveEasyNormal;
        }
        else
        {
            objectivesRemaining = objectiveHard;
        }

        // disable end screen panel
        EndScreenPanel.SetActive(false);
        isGameOver = false;
        hasWon = false;

        switch (difficulty)
        {
            case Difficulty.EASY:
                difficulty_TMP.text = "Easy";
                break;

            case Difficulty.NORMAL:
                difficulty_TMP.text = "Normal";
                break;

            case Difficulty.HARD:
                difficulty_TMP.text = "Hard";
                break;
        }

        // Setting TMPs
        points_TMP.text = points.ToString();
        moves_TMP.text = numberOfMovesRemaining.ToString();

        if (difficulty == Difficulty.EASY)
        {
            moves_TMP.text = "LOL";
        }
        

        timer_TMP.text = timeRemaining.ToString();


        // Set Color
        points_TMP.color = scoreColor;

    }


    // Update is called once per frame
    void Update()
    {
        if (!isGameOver)
        {
            UpdateTimer();
        }
        
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

            if (timeRemaining < 15)
            {
                timer_TMP.color = lessTimeMovesRedColor;
            }

            // Update text
            timer_TMP.text = timeRemaining.ToString();

            // based on timer - game over
            if (timeRemaining <= 0)
            {
                isGameOver = true;
                EndScreenPanel.SetActive(true);
                EndScreenPanel.GetComponent<EndScreenManager>().EndScreenText.text = "You ran out of time !!";
                SFXManager.GetInstance().endSceneAudioSource.clip = SFXManager.GetInstance().SFX_Lost;
                SFXManager.GetInstance().endSceneAudioSource.Play();
            }
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
            GemsObjective.Add(i);
        }


        // Add Objective - 2 gems to collect for easy and normal, 3 for hard
        
        switch (difficulty)
        {
            case Difficulty.EASY:
            case Difficulty.NORMAL:
                for (int i = 0; i < (int)GemColor.ColorType.TOTAL_COLOR_TYPES - 1 - objectiveEasyNormal; i++)
                {
                    GemsObjective.RemoveAt(Random.Range(0, GemsObjective.Count));
                }
                break;


            case Difficulty.HARD:
                for (int i = 0; i < (int)GemColor.ColorType.TOTAL_COLOR_TYPES - 1 - objectiveHard; i++)
                {
                    GemsObjective.RemoveAt(Random.Range(0, GemsObjective.Count));
                }
                break;
        }

        UIManager.GetInstance().InitializeUIManager();

    }


    /// <summary>
    /// Gems Take increment
    /// </summary>
    /// <param name="color"></param>
    public void GemsTakenUpdate(GemColor.ColorType color)
    {
        GemsTaken[(int)color]++;
        
        // Update UI Text
        bool toReturn = true;
        
        foreach (var gem in GemsObjective)
        {
            if ((int)color == gem)
            {
                toReturn = false;
            }
        }

        if (toReturn)
        {
            return;
        }

        int remaining = 0;
        switch (difficulty)
        {
            case Difficulty.EASY:
            case Difficulty.NORMAL:
                remaining = totalGemsToCollectEasyNormal - GemsTaken[(int)color];
                if (remaining <= 0 && GemsObjective.Contains(((int)color)))
                {
                    UIManager.GetInstance().GemsCollected[(int)color].AllGemsCollected();
                    GemsObjective.Remove((int)color);
                }
                else
                {
                    UIManager.GetInstance().GemsCollected[(int)color].Count.text = remaining.ToString();
                }
                
                break;

            case Difficulty.HARD:
                remaining = totalGemsToCollectHard - GemsTaken[(int)color];

                if (remaining <= 0 && GemsObjective.Contains(((int)color)))
                {
                    UIManager.GetInstance().GemsCollected[(int)color].AllGemsCollected();
                    GemsObjective.Remove((int)color);
                }
                else
                {
                    UIManager.GetInstance().GemsCollected[(int)color].Count.text = remaining.ToString();
                }
                
                break;
        }

        // Game done
        if (GemsObjective.Count <= 0)
        {
            Debug.Log("Done !");
            hasWon = true;
            isGameOver = true;
            EndScreenPanel.GetComponent<EndScreenManager>().EndScreenText.text = "You have won!";
            StartCoroutine(EndScreenWithDelay(2f));
        }
    }

    IEnumerator EndScreenWithDelay(float delay)
    {
        yield return new WaitForSeconds(delay);

        if (hasWon)
        {
            SFXManager.GetInstance().endSceneAudioSource.clip = SFXManager.GetInstance().SFX_Won;
        }
        else
        {
            SFXManager.GetInstance().endSceneAudioSource.clip = SFXManager.GetInstance().SFX_Lost;
        }

        
        SFXManager.GetInstance().endSceneAudioSource.Play();
        EndScreenPanel.SetActive(true);
    }


    /// <summary>
    /// Updates moves
    /// </summary>
    public void MovesUpdate()
    {
        numberOfMovesRemaining--;

        if (numberOfMovesRemaining < 5)
        {
            moves_TMP.color = lessTimeMovesRedColor;
        }

        moves_TMP.text = numberOfMovesRemaining.ToString();
        if (numberOfMovesRemaining <= 0)
        {
            isGameOver = true;
            hasWon = false;
            EndScreenPanel.GetComponent<EndScreenManager>().EndScreenText.text = "You ran out of moves !!";
            EndScreenPanel.SetActive(true);
            SFXManager.GetInstance().endSceneAudioSource.clip = SFXManager.GetInstance().SFX_Lost;
            SFXManager.GetInstance().endSceneAudioSource.Play();
        }
    }

    /// <summary>
    /// Update points
    /// </summary>
    /// <param name="points"></param>
    public void UpdatePoints(int points)
    {

        if (points > 200)
        {
            StartCoroutine(BonusPoints(1));
        }

        this.points += points;
        Data.Score = this.points;
        points_TMP.text = this.points.ToString();
    }


    IEnumerator BonusPoints(float interval)
    {
        points_TMP.color = bonusScore;
        float timer = 0.0f;
        float scaleValue = 4f;
        points_TMP.rectTransform.localScale = new Vector3(scaleValue, scaleValue,1f);
        // Perform Fade-Out using the provided delay
        while (timer < interval)
        {
            timer += Time.deltaTime;

            // Smooth step function to lerp smoothly
            scaleValue = Mathf.SmoothStep(scaleValue, 1.0f, timer / interval);
            points_TMP.rectTransform.localScale = new Vector3(scaleValue, scaleValue, 1f);
            yield return new WaitForEndOfFrame();
        }

        points_TMP.rectTransform.localScale = new Vector3(1f, 1f, 1f);
        points_TMP.color = scoreColor;
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

        // End Screen
        EndScreenPanel.SetActive(false);
        hasWon = false;
        isGameOver = false;
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
