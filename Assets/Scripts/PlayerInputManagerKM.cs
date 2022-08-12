using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerInputManagerKM : MonoBehaviour
{
    public BoardManagerKM BoardManager;
    public Camera MainCamera;

    private GameSettings gameSettings;
    private OnlineGameManagerKM onlineGameManager;

    private Ray ray;
    private Vector3 worldPosition;

    private void Start() {
        gameSettings = FindObjectOfType<GameSettings>();
        onlineGameManager = FindObjectOfType<OnlineGameManagerKM>();
    }

    void Update()
    {
        ray = MainCamera.ScreenPointToRay(Input.mousePosition);
        RaycastHit hitData;

        // getting mouse position in world
        if (BoardManager.GetBoardCollider().Raycast(ray, out hitData, 1000)) {
            worldPosition = hitData.point;
            Vector2Int boardPos = BoardManager.GetSpawnPos(worldPosition);

            BoardManager.ShowPositionMark(boardPos);

            // if player cliks LMB spawn stone
            if (Input.GetMouseButtonDown(0))
                SpawnStone(boardPos);

        }
        else {
            BoardManager.HidePositionMark();
        }
    }

    public void Pass() {
        if (gameSettings.GameOnline) {
            onlineGameManager.Pass();
        }
    }

    public void Surrender() {
        if (gameSettings.GameOnline) {
            onlineGameManager.Surrender();
        }
    }


    private void SpawnStone(Vector2Int boardPos) {
        if (gameSettings.GameOnline) {
            onlineGameManager.PutStone(boardPos);
        }
    }

    public void ChangeScene(string name) {
        SceneManager.LoadScene(name);
    }
}
