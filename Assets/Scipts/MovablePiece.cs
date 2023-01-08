using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovablePiece : MonoBehaviour
{
    private GamePiece piece;

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

    public void Move(int newX, int newY)
    {
        piece.X = newX;
        piece.Y = newY;

        //Debug.Log($"({newX},{newY})");
        piece.transform.localPosition = piece.GridRef.GetWorldPosition(newX, newY);
    }
}
