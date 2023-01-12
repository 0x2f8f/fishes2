using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Grid : MonoBehaviour
{
    public int xDim;
    public int yDim;
    public float fillTime;
    private bool inverse = false; //если элементы могут заполняться по диагонали (есть ли другие эл-ты, к-ые могут заполнить пустое пространство)

    public PiecePrefab[] piecePrefabs;
    public GameObject backgroundPrefab;
    private Dictionary<PieceType, GameObject> piecePrefabDict; //словарь возможных элементов на поле
    private GamePiece[,] pieces; //элементы на поле

    private GamePiece pressedPiece; //на какой элемент нажали
    private GamePiece enteredPiece; //на какой элемент хотят обменять

    // Start is called before the first frame update
    void Start()
    {
        //заполнение словаря элементов
        piecePrefabDict = new Dictionary<PieceType, GameObject>();
        for (int i = 0; i < piecePrefabs.Length; i++) {
            if (!piecePrefabDict.ContainsKey(piecePrefabs[i].type)) {
                piecePrefabDict.Add(piecePrefabs[i].type, piecePrefabs[i].prefab);
            }
        }

        //инициализация клеток бекграунда
        for (int x = 0; x < xDim; x++) {
            for (int y = 0; y < yDim; y++) {
                GameObject background = (GameObject)Instantiate(backgroundPrefab, GetWorldPosition(x, y), Quaternion.identity);
                background.transform.parent = transform;
            }
        }

        //инициализация игровых элементов на поле
        pieces = new GamePiece[xDim, yDim];
        for (int x = 0; x < xDim; x++) {
            for (int y = 0; y < yDim; y++) {
                SpawnNewPiece(x, y, PieceType.EMPTY);
            }
        }

        Destroy(pieces[4, 4].gameObject);
        SpawnNewPiece(4, 4, PieceType.BUBBLE);

        StartCoroutine(Fill());
    }

    // Update is called once per frame
    void Update()
    {

    }

    public Vector2 GetWorldPosition(int x, int y)
    {
        return new Vector2(transform.position.x - xDim/2.0f + x, transform.position.y + yDim/2.0f - y);
    }

    //создает объект на поле
    public GamePiece SpawnNewPiece(int x, int y, PieceType type)
    {
        GameObject newPiece = (GameObject)Instantiate(piecePrefabDict[type], GetWorldPosition(x, y), Quaternion.identity);
        newPiece.transform.parent = transform;

        pieces[x, y] = newPiece.AddComponent<GamePiece>();
        pieces[x, y].Init(x, y, this, type);

        return pieces[x, y];
    }

    //нажатие на элемент
    public void PressPiece(GamePiece piece)
    {
        pressedPiece = piece;
    }

    //перетащили на элемент
    public void EnterPiece(GamePiece piece)
    {
        enteredPiece = piece;
    }

    //отпустили мышь, меняем элементы местами
    public void ReleasePiece()
    {
        if (!IsAdjasent(pressedPiece, enteredPiece)) {
            return;
        }

        SwapPieces(pressedPiece, enteredPiece);
    }

    //проверяет, являются ли элнементы соседними
    public bool IsAdjasent(GamePiece piece1, GamePiece piece2)
    {
        if (piece1.X == piece2.X && (int)Mathf.Abs(piece1.Y-piece2.Y) == 1) {
            return true;
        }

        if (piece1.Y == piece2.Y && (int)Mathf.Abs(piece1.X - piece2.X) == 1) {
            return true;
        }

        return false;
    }

    //меняет местами элементы
    public void SwapPieces(GamePiece piece1, GamePiece piece2)
    {
        if (!piece1.IsMovable() || !piece2.IsMovable()) {
            return;
        }

        //Debug.Log($"x1={piece1.X}, y1={piece1.Y}, x2={piece2.X}, y2={piece2.Y}");

        pieces[piece1.X, piece1.Y] = piece2;
        pieces[piece2.X, piece2.Y] = piece1;

        int piece1X = piece1.X;
        int piece1Y = piece1.Y;

        piece1.MovableComponent.Move(piece2.X, piece2.Y, fillTime);
        piece2.MovableComponent.Move(piece1X, piece1Y, fillTime);
    }

    //Заполнение. Вызывает FillStep пока доска не будет заполнена
    public IEnumerator Fill()
    {
        while(FillStep()) {
            inverse = !inverse;
            yield return new WaitForSeconds(fillTime);
        }
    }

    //Шаг перемещения. Перемещает на одну позицию
    public bool FillStep()
    {
        bool movedPiece = false;
        for (int y = yDim - 2; y >= 0; y--) {
            for (int loopX = 0; loopX < xDim; loopX++) {
                int x = loopX;
                if (inverse) {
                    x = xDim - 1 - loopX;
                }

                GamePiece piece = pieces[x, y];
                if (piece.IsMovable()) {
                    GamePiece pieceBelow = pieces[x, y + 1];
                    if (pieceBelow.Type == PieceType.EMPTY) {
                        Destroy(pieceBelow.gameObject);
                        piece.MovableComponent.Move(x, y + 1, fillTime);
                        pieces[x, y + 1] = piece;
                        SpawnNewPiece(x, y, PieceType.EMPTY);
                        movedPiece = true;
                    } else {
                        for (int diag = -1; diag <= 1; diag++) {
                            if (diag == 0) continue;

                            int diagX = x + diag;

                            if (inverse) {
                                diagX = x - diag;
                            }

                            if (diagX < 0 || diagX >= xDim) continue;

                            GamePiece diagonalPiece = pieces[diagX, y + 1];

                            if (diagonalPiece.Type != PieceType.EMPTY)
                                continue;

                            bool hasPieceAbove = true;

                            for (int aboveY = y; aboveY >= 0; aboveY--) {
                                GamePiece pieceAbove = pieces[diagX, aboveY];

                                if (pieceAbove.IsMovable()) {
                                    break;
                                } else if (/*!pieceAbove.IsMovable() && */pieceAbove.Type != PieceType.EMPTY) {
                                    hasPieceAbove = false;
                                    break;
                                }
                            }

                            if (hasPieceAbove)
                                continue;

                            Destroy(diagonalPiece.gameObject);
                            piece.MovableComponent.Move(diagX, y + 1, fillTime);
                            pieces[diagX, y + 1] = piece;
                            SpawnNewPiece(x, y, PieceType.EMPTY);
                            movedPiece = true;
                            break;
                        }
                    }
                }
            }
        }

        for (int x = 0; x < xDim; x++) {
            GamePiece pieceBelow = pieces[x, 0];
            if (pieceBelow.Type == PieceType.EMPTY) {
                Destroy(pieceBelow.gameObject);
                GameObject newPiece = (GameObject)Instantiate(piecePrefabDict[PieceType.NORMAL], GetWorldPosition(x, -1), Quaternion.identity);
                newPiece.transform.parent = transform;

                pieces[x, 0] = newPiece.GetComponent<GamePiece>();
                pieces[x, 0].Init(x, -1, this, PieceType.NORMAL);
                pieces[x, 0].MovableComponent.Move(x, 0, fillTime);
                pieces[x, 0].ColorComponent.SetColor((ColorType)Random.Range(0, pieces[x, 0].ColorComponent.NumColors));
                movedPiece = true;
            }

        }

        return movedPiece;
    }
}
