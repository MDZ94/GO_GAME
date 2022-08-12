using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoardManagerKM : MonoBehaviour
{
    public GameObject Board;
    private Collider boardCollider;
    public GameObject StoneBlackPrefab;
    public GameObject StoneWhitePrefab;
    public Transform StonesParent;
    public GameObject PositionMarkPrefab;
    public GameObject BoardLinePrefab;
    public Transform BoardLinesParent;
    public Camera MainCamera;

    private GameObject positionMark;


    private Vector3 boardWorldPos;
    private Vector3 boardWorldSize;
    private Vector3 stoneSize;
    private Vector3 boardLineSize;
    private float boardLineLenght;
    
    private int boardSize = 9;
    private Vector3 fieldSize;
    private Vector3 boardCorner;

    private Dictionary<Vector2Int, GameObject> boardStones = new Dictionary<Vector2Int, GameObject>();

    void Start() {
        positionMark = Instantiate(PositionMarkPrefab);
        boardCollider = Board.GetComponent<Collider>();
        boardWorldPos = Board.transform.position;

        boardWorldSize = GetSize(Board);
        stoneSize = GetSize(StoneBlackPrefab);
        boardLineSize = GetSize(BoardLinePrefab);

        fieldSize = boardWorldSize / boardSize;
        boardCorner = boardWorldPos - ((boardWorldSize - fieldSize) / 2);

        boardLineLenght = GetWorldSpawnPos(new Vector2Int(boardSize-1, 0), 0).x - GetWorldSpawnPos(new Vector2Int(0, 0), 0).x;
        SpawnBoardLines();

    }

    public Vector2Int GetSpawnPos(Vector3 worldPos) {
        Vector3 realPosOnBoard = worldPos - boardCorner;
        int x = (int)Mathf.Round(realPosOnBoard.x / fieldSize.x);
        int y = (int)Mathf.Round(realPosOnBoard.z / fieldSize.z);
        return new Vector2Int(x, y);
    }

    public void ShowPositionMark(Vector2Int boardPos) {
        var worldSpawnPos = GetWorldSpawnPos(boardPos, stoneSize.y / 2);
        positionMark.SetActive(true);
        positionMark.transform.position = worldSpawnPos;
    }

    public void HidePositionMark() {
        positionMark.SetActive(false);
    }


    public bool SpawnStone(Vector2Int boardPos, bool isBlack) {
        if (!boardStones.ContainsKey(boardPos)) {
            Vector3 worldSpawnPos = GetWorldSpawnPos(boardPos, stoneSize.y / 2);
            GameObject newStone = Instantiate(GetStonePrefab(isBlack), StonesParent);
            newStone.transform.position = worldSpawnPos;
            boardStones.Add(boardPos, newStone);
            Debug.Log("BoardManagerKM - Stone " + boardPos.x + "," + boardPos.y + " spawned");
            return true;
        }
        else return false;
    }

    public bool DestroyStone(Vector2Int boardPos) {
        if (boardStones.ContainsKey(boardPos)) {
            Destroy(boardStones[boardPos]);
            boardStones.Remove(boardPos);
            Debug.Log("BoardManagerKM - Stone " + boardPos.x + "," + boardPos.y + " destroyed");
            return true;
        }
        else return false;
    }

    public Collider GetBoardCollider() {
        return boardCollider;
    }

    private Vector3 GetWorldSpawnPos(Vector2Int boardPos, float yOffset) {
        float worldX = boardCorner.x + boardPos.x * fieldSize.x;
        float worldZ = boardCorner.z + boardPos.y * fieldSize.z;
        return new Vector3(worldX, boardWorldPos.y + yOffset, worldZ);
    }


    private GameObject GetStonePrefab(bool isBlack) {
        GameObject stonePrefab = isBlack ? StoneBlackPrefab : StoneWhitePrefab;
        return stonePrefab;
    }

    private Vector3 GetSize(GameObject gameObject) {
        var mesh = gameObject.GetComponentsInChildren<MeshFilter>()[0].sharedMesh;
        var size = mesh.bounds.size;
        size.Scale(gameObject.transform.localScale);
        return size;
    }

    private void SpawnBoardLines() {
        float offset = (boardWorldSize.y / 2);
        // spawning horizontal lines
        for (int i = 0; i < boardSize; i++) {
            Vector3 spawnPoint = GetWorldSpawnPos(new Vector2Int(i, boardSize / 2), offset);
            spawnPoint = SwitchXWithZ(spawnPoint);
            GameObject boardLine = Instantiate(BoardLinePrefab, BoardLinesParent);
            boardLine.transform.position = spawnPoint;
            boardLine.transform.localScale = SetY(boardLine.transform.localScale, boardLineLenght / 2 + boardLineSize.x / 4);
        }
        // spawning vertical lines
        for (int i = 0; i < boardSize; i++) {
            Vector3 spawnPoint = GetWorldSpawnPos(new Vector2Int(boardSize / 2, i), offset);
            spawnPoint = SwitchXWithZ(spawnPoint);
            GameObject boardLine = Instantiate(BoardLinePrefab, BoardLinesParent);
            boardLine.transform.position = spawnPoint;
            boardLine.transform.rotation = Quaternion.Euler(0,90,90);
            boardLine.transform.localScale = SetY(boardLine.transform.localScale, boardLineLenght / 2 + boardLineSize.x / 4);
        }
    }

    private Vector3 SwitchXWithZ(Vector3 vector3) {
        float x = vector3.x;
        vector3.x = vector3.z;
        vector3.z = x;
        return vector3;
    }

    private Vector3 SetY(Vector3 vector3, float y) {
        vector3.y = y;
        return vector3;
    }
}

