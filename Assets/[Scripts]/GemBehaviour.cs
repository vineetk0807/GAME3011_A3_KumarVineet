using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GemBehaviour : MonoBehaviour
{
    // Position in the grid
    private float x;
    private float y;

    // Type
    public GemType type;

    // Grid Manager Reference
    public GridManager gridRef;

    // Movement behaviour comp
    public GemMovementBehaviour movableComponent;

    // Color component of gem
    public GemColor colorComponent;

    // Getters and Setters
    public float X
    {
        get { return x; }
        set
        {
            if (IsMovable())
            {
                x = value;
            }
        }
    }

    public float Y
    {
        get { return y; }
        set
        {
            if (IsMovable())
            {
                y = value;
            }
        }
    }

    private void Awake()
    {
        movableComponent = GetComponent<GemMovementBehaviour>();
        colorComponent = GetComponent<GemColor>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    /// <summary>
    /// Initialize on Instantiation of the Gem
    /// </summary>
    public void Initialize(float x, float y, GridManager grid, GemType gemType)
    {
        this.x = x;
        this.y = y;
        type = gemType;
        gridRef = grid;
    }

    /// <summary>
    /// If movement behaviour exists, then return true
    /// </summary>
    /// <returns></returns>
    public bool IsMovable()
    {
        if (movableComponent)
        {
            return true;
        }
        else
        {
            return false;
        }
    }


    /// <summary>
    /// If GemColor component exists, then return true
    /// </summary>
    /// <returns></returns>
    public bool IsColored()
    {
        if (colorComponent)
        {
            return true;
        }
        else
        {
            return false;
        }
    }
}
