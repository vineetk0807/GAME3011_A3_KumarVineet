using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SFXManager : MonoBehaviour
{
    public AudioClip SFX_gemMoveNotAllowed;
    public AudioClip SFX_match;
    public AudioClip SFX_explode;
    public AudioClip SFX_Won;
    public AudioClip SFX_Lost;

    public AudioSource audioSrc;
    public AudioSource endSceneAudioSource;

    private static SFXManager _instance;

    public static SFXManager GetInstance()
    {
        return _instance;
    }

    private void Awake()
    {
        _instance = this;
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
