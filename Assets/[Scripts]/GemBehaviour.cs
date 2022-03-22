using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GemBehaviour : MonoBehaviour
{
    // Position in the grid
    private int x;
    private int y;

    // Type
    public GemType type;

    // Grid Manager Reference
    public GridManager gridRef;

    // Movement behaviour comp
    public GemMovementBehaviour movableComponent;

    // Color component of gem
    public GemColor colorComponent;

    // Getters and Setters
    public int X
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

    public int Y
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
    public void Initialize(int x, int y, GridManager grid, GemType gemType)
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


    /// <summary>
    /// On Mouse Press - Equivalent of PointerClick/PointerDown
    /// </summary>
    private void OnMouseDown()
    {
        gridRef.PressGem(this);
    }

    /// <summary>
    /// Mouse Enter is when cursor hovers on it
    /// highlight it
    /// </summary>
    private void OnMouseEnter()
    {
        //GetComponent<SpriteRenderer>().color = new Color(1f, 1f, 1f, 0f);
        gridRef.EnteredGem(this);
        gridRef.EnteredBorder(this);
    }


    /// <summary>
    /// On Mouse Exit, remove highlight
    /// </summary>

    private void OnMouseExit()
    {
        //GetComponent<SpriteRenderer>().color = new Color(1f, 1f, 1f, 1f);
        gridRef.ExitBorder(this);
    }

    /// <summary>
    /// Mouse Up is when Click is released/ PointerUp
    /// </summary>
    private void OnMouseUp()
    {
        gridRef.ReleaseGem();
    }
}
