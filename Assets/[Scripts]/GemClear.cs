using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GemClear : MonoBehaviour
{
    // Animator Controller key
    public readonly int isMatched = Animator.StringToHash("IsMatched");
    public readonly int isRotating = Animator.StringToHash("IsRotating");
    public readonly int isExploding = Animator.StringToHash("IsExploding");

    // Animator component
    private Animator _animator;
    public AnimationClip Sample;

    private bool isBeingCleared = false;

    // Getter
    public bool IsBeingCleared
    {
        get { return isBeingCleared; }
    }

    protected GemBehaviour gem;

    public float AnimationDuration = 1.2f;

    private void Awake()
    {
        gem = GetComponent<GemBehaviour>();
    }

    // Start is called before the first frame update
    void Start()
    {
        _animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }


    // Coroutine for animation
    private IEnumerator ClearGemCoroutine()
    {
        if (_animator)
        {
            _animator.SetBool(isMatched,true);

            yield return new WaitForSeconds(AnimationDuration);

            Destroy(gameObject);
        }
    }


    /// <summary>
    /// Clear the gem
    /// </summary>
    public virtual void ClearGem()
    {
        isBeingCleared = true;
        StartCoroutine(ClearGemCoroutine());

        if (!GameManager.GetInstance().isGameOver)
        {
            // Gem list
            GameManager.GetInstance().GemsTakenUpdate(gem.colorComponent.Color);
            GameManager.GetInstance().UpdatePoints(10);
        }
    }

    private IEnumerator ExplodeGemCoroutine()
    {
        if (_animator)
        {
            _animator.SetBool(isExploding, true);

            yield return new WaitForSeconds(AnimationDuration);

            Destroy(gameObject);
        }
    }

    public void ExplodeGem()
    {
        isBeingCleared = true;
        StartCoroutine(ExplodeGemCoroutine());

        // Gem list
        GameManager.GetInstance().GemsTakenUpdate(gem.colorComponent.Color);
    }
}
