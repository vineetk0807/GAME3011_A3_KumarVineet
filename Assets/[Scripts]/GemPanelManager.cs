using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class GemPanelManager : MonoBehaviour
{

    public GameObject CheckImage;
    public TextMeshProUGUI Count;
    public GameObject GemIconParent;

    private bool executeOnce = false;

    // Start is called before the first frame update
    void Start()
    {
        CheckImage.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        
    }


    /// <summary>
    /// Add Gem function
    /// </summary>
    /// <param name="gem"></param>
    /// <param name="count"></param>
    public void AddGem(GameObject _gem, int count)
    {
        GameObject gem = Instantiate(_gem);
        gem.transform.SetParent(GemIconParent.transform, false);
        Count.text = count.ToString();
    }


    /// <summary>
    /// All gems collected will mark it as done
    /// </summary>
    public void AllGemsCollected()
    {
        if (!executeOnce)
        {
            Count.gameObject.SetActive(false);
            CheckImage.SetActive(true);
            executeOnce = true;
        }
        
    }
}
