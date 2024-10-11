using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class Test : MonoBehaviour
{
    public int rows = 100;
    public int columns = 100;
    private Cell[,] theGrid;
    public Tilemap tilemap;
    public Tilemap userTilemap;
    public Tile aliveTile;
    public Tile deadTile;

    private void Start() {
        InitializeGrid();
    }

    private void Update() {
        HandleMouseInput();
        HandleGameOfLifeUpdate();
    }

    // Initialize the grid with dead cells
    private void InitializeGrid() {
        theGrid = new Cell[rows, columns];
        for (int i = 0; i < rows; i++) {
            for (int c = 0; c < columns; c++) {
                theGrid[i, c] = new Cell(false);
            }
        }
    }

    // Handle mouse clicks for grid interaction
    private void HandleMouseInput() {
        if (Input.GetMouseButtonDown(0)) {
            Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            Vector3Int tilePos = tilemap.WorldToCell(mousePos);

            // Debugging: Verificar las coordenadas del mouse
            Debug.Log($"Mouse Pos: {mousePos}, Tile Pos: {tilePos}");

            if (IsValidPosition(tilePos)) {
                ToggleCellAtPosition(tilePos);
            }
        }
    }

    // Check if a given position is within the grid bounds
    private bool IsValidPosition(Vector3Int pos) {
        return pos.x >= 0 && pos.x < rows && pos.y >= 0 && pos.y < columns;
    }

    // Toggle the state of the cell when clicking
    private void ToggleCellAtPosition(Vector3Int tilePos) {
        // Get the current state of the cell
        bool currentState = theGrid[tilePos.x, tilePos.y].bIsAlive;

        // Toggle the state
        theGrid[tilePos.x, tilePos.y].bIsAlive = !currentState;

        // Update the tilemap based on the new state
        if (theGrid[tilePos.x, tilePos.y].bIsAlive) {
            tilemap.SetTile(tilePos, aliveTile);
            userTilemap.SetTile(tilePos, aliveTile);
        } else {
            tilemap.SetTile(tilePos, null);
            userTilemap.SetTile(tilePos, null);
        }

        // Debugging: Informar el nuevo estado
        Debug.Log($"Cell at {tilePos.x}, {tilePos.y} is now {(theGrid[tilePos.x, tilePos.y].bIsAlive ? "alive" : "dead")}");
    }

    // Game of Life update on spacebar press
    private void HandleGameOfLifeUpdate() {
        if (Input.GetKeyDown(KeyCode.Space)) {
            Cell[,] newGrid = new Cell[rows, columns];

            // Initialize the new grid
            for (int i = 0; i < rows; i++) {
                for (int c = 0; c < columns; c++) {
                    newGrid[i, c] = new Cell(false);
                }
            }

            // Apply Game of Life rules
            for (int i = 0; i < rows; i++) {
                for (int c = 0; c < columns; c++) {
                    int aliveNeighbors = CountAliveNeighbors(i, c);
                    newGrid[i, c].bIsAlive = ApplyGameOfLifeRules(theGrid[i, c].bIsAlive, aliveNeighbors);
                }
            }

            // Swap the new grid to the current grid
            theGrid = newGrid;
            UpdateVisualGrid();
        }
    }

    // Count the alive neighbors around a cell
    private int CountAliveNeighbors(int x, int y) {
        int[] dx = { -1, -1, -1, 0, 0, 1, 1, 1 };
        int[] dy = { -1, 0, 1, -1, 1, -1, 0, 1 };
        int aliveCount = 0;

        for (int i = 0; i < 8; i++) {
            int nx = x + dx[i];
            int ny = y + dy[i];

            if (nx >= 0 && nx < rows && ny >= 0 && ny < columns && theGrid[nx, ny].bIsAlive) {
                aliveCount++;
            }
        }

        return aliveCount;
    }

    // Apply the Game of Life rules to a cell
    private bool ApplyGameOfLifeRules(bool isAlive, int aliveNeighbors) {
        if (isAlive) {
            return aliveNeighbors == 2 || aliveNeighbors == 3;
        } else {
            return aliveNeighbors == 3;
        }
    }

    // Update the visual representation of the grid
    private void UpdateVisualGrid() {
        for (int i = 0; i < rows; i++) {
            for (int c = 0; c < columns; c++) {
                Vector3Int cellPos = new Vector3Int(i, c, 0);
                if (theGrid[i, c].bIsAlive) {
                    tilemap.SetTile(cellPos, aliveTile);
                } else {
                    tilemap.SetTile(cellPos, null);
                }
            }
        }
    }
}
