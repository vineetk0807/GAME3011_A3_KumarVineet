using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BombClear : MonoBehaviour
{
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
        gem.gridRef.BombClearFunction(gem.X, gem.Y);
        isBeingCleared = true;
        StartCoroutine(ExplodeGemCoroutine());
    }
}
