using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GemMovementBehaviour : MonoBehaviour
{
    private GemBehaviour gem;

    // Awake function for avoid null
    private void Awake()
    {
        gem = GetComponent<GemBehaviour>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    // Move to given position
    public void Move(float newX, float newY)
    {
        gem.X = newX;
        gem.X = newY;

        gem.transform.localPosition = new Vector2(newX, newY);
    }
}
