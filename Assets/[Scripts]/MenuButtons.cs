using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MenuButtons : MonoBehaviour
{
    public Slider difficultySlider;
    public TextMeshProUGUI Difficulty_TMP;
    public TextMeshProUGUI DifficultyDescription_TMP;


    public string EasyDescription = "Something";
    public string NormalDescription = "Something";
    public string HardDescription = "Something";

    // Start is called before the first frame update
    void Start()
    {
        difficultySlider.value = (int)Data.difficulty;
    }

    // Update is called once per frame
    void Update()
    {
        
    }


    /// <summary>
    /// Start button pressed
    /// </summary>
    public void OnStartButtonPressed()
    {
        SceneManager.LoadScene(1);
    }



    /// <summary>
    /// Difficulty Value changed
    /// </summary>
    public void OnDifficultyChanged()
    {
        switch ((int)difficultySlider.value)
        {
            case 0:
                Data.difficulty = Difficulty.EASY;
                Difficulty_TMP.text = "EASY";
                DifficultyDescription_TMP.text = EasyDescription;
                break;


            case 1:
                Data.difficulty = Difficulty.NORMAL;
                Difficulty_TMP.text = "NORMAL";
                DifficultyDescription_TMP.text = NormalDescription;
                break;

            case 2:
                Data.difficulty = Difficulty.HARD;
                Difficulty_TMP.text = "HARD";
                DifficultyDescription_TMP.text = HardDescription;
                break;

            default:
                Data.difficulty = Difficulty.NORMAL;
                Difficulty_TMP.text = "NORMAL";
                DifficultyDescription_TMP.text = NormalDescription;
                break;
        }
    }
}
