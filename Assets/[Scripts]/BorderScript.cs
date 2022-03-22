using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum LayerOrder
{
    NORMAL,
    HIGHLIGHTED
}

public class BorderScript : MonoBehaviour
{

    // Position in the grid
    private int x;
    private int y;

    // Grid Manager Reference
    public GridManager gridRef;

    // Getters and Setters
    public int X
    {
        get { return x; }
        set
        {
            x = value;
        }
    }
    public int Y
    {
        get { return y; }
        set
        { 
            y = value;
        }
    }

    public Sprite normalBorder;
    public Sprite highlightedBorder;

    private SpriteRenderer spriteRenderer;

    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        spriteRenderer.sprite = normalBorder;
        spriteRenderer.sortingOrder = (int)LayerOrder.NORMAL;
    }

    // Update is called once per frame
    void Update()
    {
        
    }


    // Initalize the border
    public void Initialize(int x, int y, GridManager grid)
    {
        this.x = x;
        this.y = y;
        gridRef = grid;
    }

    /// <summary>
    /// When mouse enters border, highlight it
    /// </summary>
    public void BorderEnter()
    {
        spriteRenderer.sprite = highlightedBorder;
        spriteRenderer.sortingOrder = (int)LayerOrder.HIGHLIGHTED;
    }

    /// <summary>
    /// When mouse exits border put it back to normal border
    /// </summary>
    public void BorderExit()
    {
        spriteRenderer.sprite = normalBorder;
        spriteRenderer.sortingOrder = (int)LayerOrder.NORMAL;
    }
}
