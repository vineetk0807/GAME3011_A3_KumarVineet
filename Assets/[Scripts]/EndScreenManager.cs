using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class EndScreenManager : MonoBehaviour
{

    public TextMeshProUGUI EndScreenText;
    public TextMeshProUGUI ScoreText_TMP;

    // Start is called before the first frame update
    void Start()
    {
        ScoreText_TMP.text = Data.Score.ToString();
    }

    private void OnEnable()
    {
        UIManager.GetInstance().GemPanel.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    /// <summary>
    /// Change scene to main menu
    /// </summary>
    public void GoToMainMenu()
    {
        SceneManager.LoadScene(0);
    }
}
