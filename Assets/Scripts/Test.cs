using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class Test : MonoBehaviour
{
    Cell[,] theGrid;
    public int rows = 100;
    public int columns = 100;
    public Tilemap tilemap;
    public Tilemap userTilemap;
    public Tile drawedTile; 
    public Tile deadTile;   

    private void Start() {
        theGrid = new Cell[rows, columns];
        for (int i = 0; i < rows; i++) {
            for (int c = 0; c < columns; c++) {
                theGrid[i, c] = new Cell(false);
            }
        }
    }

    private void Update() {
        if (Input.GetMouseButtonDown(0)) {
            Vector3 coursorPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            Vector3Int tilePos = userTilemap.WorldToCell(coursorPos);
            if (tilePos.x >= 0 && tilePos.x < rows && tilePos.y >= 0 && tilePos.y < columns) {
                updateGrid(tilePos);
            }
        }
        if (Input.GetKeyDown(KeyCode.Space)) {
            runGameOfLife();
        }
    }

    public void runGameOfLife() {
        Cell[,] tempGrid = new Cell[rows, columns];

        for(int i = 0; i < rows; i++) {
            for(int c = 0; c < columns; c++) {
                int currentNeighs = checkNeighCells(i, c);
                if(theGrid[i, c].bIsAlive) {
                    tempGrid[i, c] = new Cell(currentNeighs == 2 || currentNeighs == 3);
                } else {
                    tempGrid[i, c] = new Cell(currentNeighs == 3);
                }
            }
        }

        theGrid = tempGrid;
        updateVisualGrid();
    }

    private void updateGrid(Vector3Int tilePos) {
        TileBase currentTile = tilemap.GetTile(tilePos);
        if (currentTile == null) {        
            tilemap.SetTile(tilePos, drawedTile);
            userTilemap.SetTile(tilePos, drawedTile);
            theGrid[tilePos.x, tilePos.y].bIsAlive = true;
        } else {       
            tilemap.SetTile(tilePos, deadTile);
            userTilemap.SetTile(tilePos, deadTile);
            theGrid[tilePos.x, tilePos.y].bIsAlive = false;
        }
    }

    private void updateVisualGrid() {
        for (int i = 0; i < rows; i++) {
            for (int c = 0; c < columns; c++) {
                Vector3Int currentGridPos = new Vector3Int(i, c, 0);
                if (theGrid[i, c].bIsAlive) {
                    tilemap.SetTile(currentGridPos, drawedTile);
                } else {
                    tilemap.SetTile(currentGridPos, deadTile);
                }
            }
        }
    }

    int checkNeighCells(int x, int y) {
        int aliveNeighbors = 0;
        for(int i = -1; i <= 1; i++) {
            for(int j = -1; j <= 1; j++) {
                if(i == 0 && j == 0) continue;
                int checkX = x + i;
                int checkY = y + j;

                if(checkX >= 0 && checkX < rows && checkY >= 0 && checkY < columns) {
                    if (theGrid[checkX, checkY].bIsAlive) {
                        aliveNeighbors++;
                    }
                }
            }
        }
        return aliveNeighbors;
    }
}
