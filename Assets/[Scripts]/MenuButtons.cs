using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MenuButtons : MonoBehaviour
{
    public Slider difficultySlider;

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

    }
}
