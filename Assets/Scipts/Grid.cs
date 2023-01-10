using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Grid : MonoBehaviour
{
    public int xDim;
    public int yDim;
    public PiecePrefab[] piecePrefabs;
    public GameObject backgroundPrefab;
    private Dictionary<PieceType, GameObject> piecePrefabDict; //словарь возможных элементов на поле
    private GamePiece[,] pieces; //элементы на поле

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
                //GameObject piece = (GameObject)Instantiate(piecePrefabDict[PieceType.NORMAL], GetWorldPosition(x, y), Quaternion.identity);
                GameObject piece = (GameObject)Instantiate(piecePrefabDict[PieceType.NORMAL], Vector3.zero, Quaternion.identity);
                piece.name = $"Piece ({x},{y})";
                piece.transform.parent = transform;
                pieces[x, y] = piece.GetComponent<GamePiece>();
                pieces[x, y].Init(x, y, this, PieceType.NORMAL);

                if (pieces[x, y].IsMovable()) {
                    //Debug.Log($"({x},{y})");
                    pieces[x, y].MovableComponent.Move(x, y);
                }

                if (pieces[x, y].IsColored()) {
                    pieces[x, y].ColorComponent.SetColor((ColorType)Random.Range(0, pieces[x, y].ColorComponent.NumColors));
                }
            }
        }
    }

    // Update is called once per frame
    void Update()
    {

    }

    public Vector2 GetWorldPosition(int x, int y)
    {
        return new Vector2(transform.position.x - xDim/2.0f + x, transform.position.y + yDim/2.0f - y);
    }
}
