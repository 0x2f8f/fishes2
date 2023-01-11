using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovablePiece : MonoBehaviour
{
    private GamePiece piece;
    private IEnumerator moveCoroutine;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void Awake()
    {
        piece = GetComponent<GamePiece>();
    }

    public void Move(int newX, int newY, float timeAnimation)
    {
        //piece.X = newX;
        //piece.Y = newY;

        ////Debug.Log($"({newX},{newY})");
        //piece.transform.localPosition = piece.GridRef.GetWorldPosition(newX, newY);

        if (moveCoroutine != null) {
            StopCoroutine(moveCoroutine);
        }

        moveCoroutine = MoveCoroutine(newX, newY, timeAnimation);
        StartCoroutine(moveCoroutine);
    }

    private IEnumerator MoveCoroutine(int newX, int newY, float timeAnimation)
    {
        piece.X = newX;
        piece.Y = newY;

        Vector3 startPos = transform.position;
        Vector3 endPos = piece.GridRef.GetWorldPosition(newX, newY);

        for (float t = 0; t <= 1* timeAnimation; t+=Time.deltaTime) {
            piece.transform.position = Vector3.Lerp(startPos, endPos, t / timeAnimation);

            yield return 0;
        }

        piece.transform.position = endPos;
    }
}
