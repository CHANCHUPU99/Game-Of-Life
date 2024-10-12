using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class Test : MonoBehaviour
{
    public Cell[,] theGrid;
    public Tilemap tilemap;
    public Tilemap userTilemap;
    public Tile drawedTile; 
    public Tile deadTile;   
    public int rows = 350;
    public int columns = 350;

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
        for (int i = 0; i < rows; i++) {
            for (int c = 0; c < columns; c++) {
                tempGrid[i, c] = new Cell();
            }
        }
        for (int i = 0; i < rows; i++) {
            for (int c = 0; c < columns; c++) {
                int currentNeighs = checkNeighCells(i, c);
                if (theGrid[i, c].bIsAlive) {
                    if (currentNeighs < 2 || currentNeighs > 3) {
                        tempGrid[i, c].bIsAlive = false;
                    } else {
                        tempGrid[i, c].bIsAlive = true;
                    }
                }
                else {
                    if (currentNeighs == 3) {
                        tempGrid[i, c].bIsAlive = true;
                    } else {
                        tempGrid[i, c].bIsAlive = false;
                    }
                }
            }
        }
        theGrid = tempGrid;
        printGridState();
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
                Vector3Int currentGridPos = new Vector3Int(c, i);
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
                if(i == 0 && j == 0) {
                    continue;
                }
                int checkX = x + i;
                int checkY = y + j;

                if(checkX >= 0 && checkX < rows && checkY >= 0 && checkY < columns) {
                    if (theGrid[checkX, checkY].bIsAlive) {
                        aliveNeighbors++;
                    }
                    //Debug.Log($"Checking neighbor at ({checkX}, {checkY}): {theGrid[checkX, checkY].bIsAlive}");
                }
            }
        }
        return aliveNeighbors;
    }

    void printGridState() {
        string gridState = "";
        for (int i = 0; i < rows; i++) {
            for (int c = 0; c < columns; c++) {
                gridState += theGrid[i, c].bIsAlive ? "1 " : "0 ";
            }
            gridState += "\n";
        }
        Debug.Log($"Estado actual de theGrid:\n{gridState}");
    }
}
