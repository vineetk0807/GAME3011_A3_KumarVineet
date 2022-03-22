using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GemMovementBehaviour : MonoBehaviour
{
    private GemBehaviour gem;
    private IEnumerator moveCoroutine;

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
    public void Move(int newX, int newY, float time)
    {
        // stop coroutine if the move coroutine doesn't exist
        if (moveCoroutine != null)
        {
            StopCoroutine(moveCoroutine);
        }

        // Set coroutine
        moveCoroutine = MoveSmoothly(newX, newY, time);

        // start coroutine
        StartCoroutine(moveCoroutine);
    }


    // Coroutine to move smoothly with "time" interval
    private IEnumerator MoveSmoothly(int newX, int newY, float time)
    {
        gem.X = newX;
        gem.Y = newY;

        Vector2 start = transform.position;
        Vector2 end = gem.gridRef.GetWorldPosition(newX,newY);

        // Lerp till just the end position
        for (float t = 0; t <= 1 * time; t+= Time.deltaTime)
        {
            gem.transform.position = Vector2.Lerp(start, end, t / time);
            yield return 0;
        }

        // Set the actual position as lerp doesn't reach the actual end value
        gem.transform.position = end;
    }
}
