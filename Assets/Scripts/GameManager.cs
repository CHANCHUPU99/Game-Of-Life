using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using static UnityEngine.Rendering.DebugUI.Table;

public class GameManager : MonoBehaviour
{
    Cell[,] theGrid;
    public int rows = 100;
    public int columns = 100;
    public Tilemap tilemap;
    public Tilemap userTilemap;
    public Tile drawedTile;
    public Tile deadTile;
    private Vector3Int theGridSize = new Vector3Int(100,100,0);

    private void Start() {
        //createThegrid();
        theGrid = new Cell[rows, columns];
        for (int i = 0; i < rows; i++) {
            for (int c = 0; c < columns; c++) {
                theGrid[i, c] = new Cell(false);
            }
        }
    }

    private void Update() {
        runGameOfLife();
        if(Input.GetMouseButtonDown(0)) {
            Debug.LogWarning("Se presiono click");
            Vector3 coursorPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            Vector3Int tilePos = userTilemap.WorldToCell(coursorPos);
            if(tilePos.x >= 0 && tilePos.x < theGridSize.x &&  tilePos.y >= 0 && tilePos.y < theGridSize.y) {
                updateGrid(tilePos);
                //aqui cambio el estado
            }
        }
    }

    public void runGameOfLife() {
        /*if(Input.GetKeyDown(KeyCode.Space)) {
            createThegrid();
            Cell[,] tempGrid = new Cell[rows, columns];
            for (int i = 0; i < rows; i++) {
                for (int c = 0; c < columns; c++) {
                    tempGrid[i, c] = new Cell(false); 
                }
            }


            for (int i = 0; i < rows; i++) {
                for(int c = 0; c < columns; c++) {
                    int currentNeighs = checkNeighCells(i,c);
                    //tempGrid[i,c] = new Cell(theGrid[i,c].bIsAlive);             
                    //Debug.LogError($"Cell ({i}, {c}) has {currentNeighs} alive neighbors.");
                    if (theGrid[i,c].bIsAlive) {
                        if(currentNeighs < 2 || currentNeighs >= 4) {
                            theGrid[i,c].bIsAlive = false;
                        } else {
                            theGrid[i,c].bIsAlive = true;
                        }
                    } else {
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
            Debug.LogWarning("el juego acabaria aqui........");
        }*/
        if (Input.GetKeyDown(KeyCode.Space)) {
            createThegrid();
            Cell[,] tempGrid = new Cell[rows, columns];
            for (int i = 0; i < rows; i++) {
                for (int c = 0; c < columns; c++) {
                    tempGrid[i, c] = new Cell(false); 
                }
            }

            for (int i = 0; i < rows; i++) {
                for (int c = 0; c < columns; c++) {
                    int currentNeighs = checkNeighCells(i, c);
                    if (theGrid[i, c].bIsAlive) {
                        tempGrid[i, c].bIsAlive = currentNeighs == 2 || currentNeighs == 3;
                    } else {
                        tempGrid[i, c].bIsAlive = currentNeighs == 3;
                    }
                }
            }

            theGrid = tempGrid; 
            printGridState();
            updateVisualGrid();
            Debug.LogWarning("El juego acabó aquí.");
        }
    }

    
    private void updateGrid(Vector3Int tilePos) {
        /*TileBase currentTilemap = tilemap.GetTile(tilePos);
        if(currentTilemap == null) {
            tilemap.SetTile(tilePos, drawedTile);
            userTilemap.SetTile(tilePos, drawedTile);
            theGrid[tilePos.x, tilePos.y].bIsAlive = true;
            Debug.Log($"Pintada celda en {tilePos.x}, {tilePos.y} como viva.");
        } else {
            tilemap.SetTile(tilePos, null);
            userTilemap.SetTile(tilePos, null);
            theGrid[tilePos.x, tilePos.y].bIsAlive = false;
            Debug.Log($"Celda en {tilePos.x}, {tilePos.y} se eliminó.");
        }*/
        if (tilePos.x >= 0 && tilePos.x < rows && tilePos.y >= 0 && tilePos.y < columns) {
            TileBase currentTile = tilemap.GetTile(tilePos);
            if (currentTile == null) {
                tilemap.SetTile(tilePos, drawedTile);
                userTilemap.SetTile(tilePos, drawedTile);
                theGrid[tilePos.x, tilePos.y].bIsAlive = true;
                Debug.Log($"Celda en {tilePos.x}, {tilePos.y} pintada como viva. Estado actual: {theGrid[tilePos.x, tilePos.y].bIsAlive}");
            } else {
                tilemap.SetTile(tilePos, null);
                userTilemap.SetTile(tilePos, null);
                theGrid[tilePos.x, tilePos.y].bIsAlive = false;
                Debug.Log($"Celda en {tilePos.x}, {tilePos.y} eliminada. Estado actual: {theGrid[tilePos.x, tilePos.y].bIsAlive}");
            }
        } else {
            Debug.LogWarning($"Tile position {tilePos} is out of bounds.");
        }

    }

    //Function to Update the grid of the game
    private void updateVisualGrid() {
        for(int i = 0; i < rows; i++) {
            for(int c = 0; c < columns; c++) {
                Vector3Int currentGridPos = new Vector3Int(i, c);
                if (theGrid[i,c].bIsAlive) {
                    Debug.Log($"Drawing live cell at ({i}, {c})");
                    tilemap.SetTile(currentGridPos, drawedTile);
                } else {
                    tilemap.SetTile(currentGridPos, null);
                }
            }
        }
    }

    void createThegrid() {
        /*theGrid = new Cell[rows, columns];
        for(int i = 0; i < rows; i++) {
            for(int c = 0 ; c < columns; c++) {
                theGrid[i, c] = new Cell(false);
                Vector3Int currentGridPos = new Vector3Int(i, c);
                TileBase currentTile = tilemap.GetTile(currentGridPos);
                if(currentTile != null) {
                    theGrid[i,c].bIsAlive = true;
                    Debug.Log($"Cell at {i}, {c} initialized as alive.");
                } else { 
                    theGrid[i, c].bIsAlive = false;
                    Debug.Log($"Cell at {i}, {c} initialized as dead.");
                }
            }
        }*/
        for (int i = 0; i < rows; i++) {
            for (int c = 0; c < columns; c++) {
                Vector3Int currentGridPos = new Vector3Int(i, c, 0);
                TileBase currentTile = tilemap.GetTile(currentGridPos);
                if (currentTile != null) {
                    theGrid[i, c] = new Cell(true);
                    Debug.Log($"Cell at {i}, {c} initialized as alive.");
                } else {
                    theGrid[i, c] = new Cell(false);
                    Debug.Log($"Cell at {i}, {c} initialized as dead.");
                }
            }
        }
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

    void paintCell(int x, int y) {
        
        theGrid[x, y].bIsAlive = true;
        Debug.Log($"Cell ({x}, {y}) marked as alive");
        updateVisualGrid();
    }
    int checkNeighCells(int rows, int columns) {
        /*int neighAlive = 0;
        for( int i = -1; i <= 1; ++i) {
            for(int c = -1; c <= 1; c++) {
                if(i == 0 && c == 0) {
                    continue;
                } 
                int neighPosX = rows + i;
                int neighPosY = columns + c;
                if(neighPosX >= 0 && neighPosX < rows && neighPosY >= 0 &&  neighPosY < columns) {
                    if (theGrid[neighPosX,neighPosY].bIsAlive) {
                        neighAlive++;
                    }
                }
            }
            
        }
        return neighAlive;
        Debug.Log("Neighs alive: " + neighAlive);*/
        ///////////////segundaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaa
        /*int neighAlive = 0;

        // Revisa las celdas alrededor de la celda actual
        for (int i = -1; i <= 1; ++i) {
            for (int j = -1; j <= 1; ++j) {
                if (i == 0 && j == 0) {
                    continue; // No revises la celda actual
                }

                int neighPosX = rows + i;
                int neighPosY = columns + j;

                // Comprueba que los índices estén dentro de los límites
                if (neighPosX >= 0 && neighPosX < rows && neighPosY >= 0 && neighPosY < columns) {
                    if (theGrid[neighPosX, neighPosY].bIsAlive) {
                        neighAlive++;
                    }
                    // Agrega información de depuración sobre las celdas vecinas
                    Debug.Log($"Checking neighbor at ({neighPosX}, {neighPosY}): {theGrid[neighPosX, neighPosY].bIsAlive}");
                }
            }
        }
        Debug.Log($"Cell ({rows}, {columns}) has {neighAlive} alive neighbors.");
        return neighAlive;*/

        ////////////////terceraaaaaaaaaaaaaaa
        int neighAlive = 0;

        // Coordenadas relativas de los vecinos (incluye diagonales)
        int[] dx = { -1, -1, -1, 0, 0, 1, 1, 1 };
        int[] dy = { -1, 0, 1, -1, 1, -1, 0, 1 };

        for (int i = 0; i < 8; i++) {
            int newRow = rows + dx[i];
            int newCol = columns + dy[i];

            // Verificar límites de la matriz
            if (newRow >= 0 && newRow < rows && newCol >= 0 && newCol < columns) {
                if (theGrid[newRow, newCol].bIsAlive) {
                    neighAlive++;
                }
            }
        }

        return neighAlive;
    }

    /* Funcion CreateTheGrid
     * ciclo for para recorrer primeros rows y luego columns
     * despues de cada iteracion guardo esa posicion y tengo que hacer que cheque si esta pintada o no
     * actualmente lo guardo en currentGridPos, despues ocupo currentTile para ahora comparar y si es dif a null entonces hay algo pintado y asi al reves
     */

    /*Funcion para Checar si hay vecinos
     * deberia intentar guardar las posiciones donde ya se que hay cell vivas osea a la hora de detectar si un currenttile es dif a null entonces agrego esa pos a un theGrid 2.0 para volver a utilizarlos 
     * 
     * 
     */
}
