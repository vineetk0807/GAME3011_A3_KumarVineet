using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlockScript : MonoBehaviour
{
    // Animator Controller key
    public readonly int isExploding = Animator.StringToHash("IsExploding");

    // Animator component
    private Animator _animator;

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
            Debug.Log("block name: " + gameObject.name);

            _animator.SetBool(isExploding,true);

            yield return new WaitForSeconds(AnimationDuration);

            Destroy(gameObject);
        }
    }


    /// <summary>
    /// Clear the gem
    /// </summary>
    public void ClearGem()
    {
        isBeingCleared = true;
        StartCoroutine(ClearGemCoroutine());
    }
}
