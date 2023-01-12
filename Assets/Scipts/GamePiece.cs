using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GamePiece : MonoBehaviour
{

    private int x;
    private int y;
    private PieceType type;
    private Grid grid;
    private MovablePiece movableComponent;
    private ColorPiece colorComponent;

    public int X
    {
        get { return x; }
        set { if (IsMovable()) x = value; }
    }

    public int Y
    {
        get { return y; }
        set { if (IsMovable()) y = value; }
    }

    public PieceType Type
    {
        get { return type; }
    }

    public Grid GridRef
    {
        get { return grid; }
    }

    public MovablePiece MovableComponent
    {
        get { return movableComponent; }
    }

    public ColorPiece ColorComponent
    {
        get { return colorComponent; }
    }

    void Awake()
    {
        movableComponent = GetComponent<MovablePiece>();
        colorComponent = GetComponent<ColorPiece>();
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Init(int _x, int _y, Grid _grid, PieceType _type)
    {
        x = _x;
        y = _y;
        grid = _grid;
        type = _type;
    }

    //нажали на элемент
    private void OnMouseDown()
    {
        grid.PressPiece(this);
    }

    //перетащили на элемент
    private void OnMouseEnter()
    {
        grid.EnterPiece(this);
    }

    //отпустили кнопку мыши
    private void OnMouseUp()
    {
        grid.ReleasePiece();
    }

    public bool IsMovable()
    {
        return movableComponent != null;
    }

    public bool IsColored()
    {
        return colorComponent != null;
    }
}
