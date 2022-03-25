using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIManager : MonoBehaviour
{

    //STAR,
    //HEART,
    //QUAD,
    //PENTA,
    //OCTA,

    [Header("Gem UI Prefabs")] 
    public GameObject[] ListOfUIObjects;

    [Header("Gem Panel")] 
    public GameObject GemPanel;

    [Header("Gems To Collect Prefab")] 
    public GameObject GemsToCollectPrefab;

    private static UIManager _instance;

    public int objectiveEasyNormal = 2;
    public int objectiveHard = 3;

    public List<GemPanelManager> GemsCollected;
    public static UIManager GetInstance()
    {
        return _instance;
    }

    private void Awake()
    {
        _instance = this;
        GemsCollected = new List<GemPanelManager>();
        for (int i = 0; i < 5; i++)
        {
            GemsCollected.Add(null);
        }
    }

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }


    /// <summary>
    /// Initialize UI Manager
    /// </summary>
    public void InitializeUIManager()
    {
        switch (GameManager.GetInstance().difficulty)
        {
            case Difficulty.EASY:
            case Difficulty.NORMAL:
                for (int i = 0; i < objectiveEasyNormal; i++)
                {
                    GameObject gemsToCollect = Instantiate(GemsToCollectPrefab);
                    gemsToCollect.GetComponent<GemPanelManager>().AddGem(ListOfUIObjects[GameManager.GetInstance().GemsObjective[i]], GameManager.GetInstance().totalGemsToCollectEasyNormal);
                    gemsToCollect.
                        transform.SetParent(GemPanel.transform,false);

                    GemsCollected[GameManager.GetInstance().GemsObjective[i]] = gemsToCollect.GetComponent<GemPanelManager>();
                }

                break;

            case Difficulty.HARD:
                for (int i = 0; i < objectiveHard; i++)
                {
                    GameObject gemsToCollect = Instantiate(GemsToCollectPrefab);
                    gemsToCollect.GetComponent<GemPanelManager>().AddGem(ListOfUIObjects[GameManager.GetInstance().GemsObjective[i]], GameManager.GetInstance().totalGemsToCollectHard);
                    gemsToCollect.transform.SetParent(GemPanel.transform,false);

                    GemsCollected[GameManager.GetInstance().GemsObjective[i]] = gemsToCollect.GetComponent<GemPanelManager>();
                }
                break;
        }
    }

}
