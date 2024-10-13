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
    private Vector3Int theGridSize = new Vector3Int(50,50,0);

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
        if(Input.GetKeyDown(KeyCode.Space)) {
            createThegrid();
            Cell[,] tempGrid = new Cell[rows, columns];
            for(int i = 0; i < rows; i++) {
                for(int c = 0; c < columns; c++) {
                    tempGrid[i, c] = new Cell(false); 
                }
            }
            for(int i = 0; i < rows; i++) {
                for(int c = 0; c < columns; c++) {
                    int currentNeighs = checkNeighCells(i,c);
                    //tempGrid[i,c] = new Cell(theGrid[i,c].bIsAlive);             
                    if(theGrid[i,c].bIsAlive) {
                        if(currentNeighs < 2 || currentNeighs >= 4) {
                            theGrid[i,c].bIsAlive = false;
                        } else {
                            theGrid[i,c].bIsAlive = true;
                        }
                    } else {
                        if(currentNeighs == 3) {
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
        }
    }

    
    private void updateGrid(Vector3Int tilePos) {
        TileBase currentTilemap = tilemap.GetTile(tilePos);
        if(currentTilemap == null) {
            tilemap.SetTile(tilePos, drawedTile);
            userTilemap.SetTile(tilePos, drawedTile);
            theGrid[tilePos.x, tilePos.y].bIsAlive = true;
            Debug.Log("viveeeeeee");
        } else {
            tilemap.SetTile(tilePos, null);
            userTilemap.SetTile(tilePos, null);
            theGrid[tilePos.x, tilePos.y].bIsAlive = false;
            Debug.Log("MUEREEEEEE");
        } 
    }

    //Function to Update the grid of the game
    private void updateVisualGrid() {
        for(int i = 0; i < rows; i++) {
            for(int c = 0; c < columns; c++) {
                Vector3Int currentGridPos = new Vector3Int(i, c);
                if (theGrid[i,c].bIsAlive) {
                    Debug.Log("deberia ser 1");
                    tilemap.SetTile(currentGridPos, drawedTile);
                } else {
                    tilemap.SetTile(currentGridPos, null);
                }
            }
        }
    }

    void createThegrid() {
        theGrid = new Cell[rows, columns];
        for(int i = 0; i < rows; i++) {
            for(int c = 0 ; c < columns; c++) {
                theGrid[i, c] = new Cell(false);
                Vector3Int currentGridPos = new Vector3Int(i, c);
                TileBase currentTile = tilemap.GetTile(currentGridPos);
                if(currentTile != null) {
                    theGrid[i,c].bIsAlive = true;
                    Debug.Log("se creo alive");
                } else { 
                    theGrid[i, c].bIsAlive = false;
                    Debug.Log("se creo dead");
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
    }
    int checkNeighCells(int rows, int columns) {
        /*int neighAlive = 0;
        for( int i = -1; i <= 1; ++i) {
            for(int c = -1; c <= 1; c++) {
                if(i == 0 && c == 0) {
                    continue;
                } 
                int neighPosX = rows + i;
                nt neighPosY = columns + c;
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
        int neighAlive = 0;
        for (int i = -1; i <= 1; ++i) {
            for (int j = -1; j <= 1; ++j) {
                if (i == 0 && j == 0) {
                    continue; 
                }
                int neighPosX = rows + i;
                int neighPosY = columns + j;           
                if (neighPosX >= 0 && neighPosX < rows && neighPosY >= 0 && neighPosY < columns) {
                    if (theGrid[neighPosX, neighPosY].bIsAlive) {
                        neighAlive++;
                    }
                   

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
