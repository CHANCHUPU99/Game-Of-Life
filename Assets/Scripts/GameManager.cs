using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class GameManager : MonoBehaviour
{
    //public GameObject cell;
    int[,] theGrid;
    public int rows = 100;
    public int columns = 100;
    public Tilemap tilemap;
    public Tilemap userTilemap;
    public Tile drawedTile;
    public Tile deadTile;
    private Vector3Int theGridSize = new Vector3Int(200,100,0);

    private void Start() {
        createThegrid();
    }

    private void Update() {
        if(Input.GetMouseButtonDown(0)) {
            Debug.LogWarning("Se presiono click");
            Vector3 coursorPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            Vector3Int tilePos = userTilemap.WorldToCell(coursorPos);
            if(tilePos.x >= 0 && tilePos.x < theGridSize.x &&  tilePos.y >= 0 && tilePos.y < theGridSize.y) {
                UpdateGrid(tilePos);
                //aqui cambio el estado 
            }
        }
    }

    public void runGameOfLife() {
        if(Input.GetKeyDown(KeyCode.Space)) {
            createThegrid();
            Debug.LogWarning("el juego corre aqui");
        }
    }

    private void UpdateGrid(Vector3Int tilePos) {
        TileBase currentTilemap = tilemap.GetTile(tilePos);
        if(currentTilemap == null) {
            tilemap.SetTile(tilePos, drawedTile);
            userTilemap.SetTile(tilePos, drawedTile);
        } else {
            tilemap.SetTile(tilePos, null);
            userTilemap.SetTile(tilePos, null);
        }
    }

    void createThegrid() {
        theGrid = new int[rows, columns];
        for(int i = 0; i < rows; i++) {
            for(int c = 0 ; c < columns; c++) {
                Vector3Int currentGridPos = new Vector3Int(i, c);
                TileBase currentTile = tilemap.GetTile(currentGridPos);
                if(currentTile != null) {
                    theGrid[i,c] = 1;
                } else { 
                    theGrid[i,c] = 0;
                }
            }
        }
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
