using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


/// <summary>
/// Gem Type
/// </summary>
public enum GemType
{
    NORMAL,
    EMPTY,
    TOTAL_TYPES
}

public class GemScript : MonoBehaviour
{
    private Animator _animator;

    public GemType GemType;

    /// <summary>
    /// Awake to make sure its loaded up
    /// </summary>
    private void Awake()
    {
        _animator = GetComponent<Animator>();
    }

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
