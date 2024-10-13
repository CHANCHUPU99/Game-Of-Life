using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RulesOfThegame : MonoBehaviour
{
    private int filas;
    private int columnas;
    private Cell[,] grid;

    public RulesOfThegame(int filas, int columnas) {
        this.filas = filas;
        this.columnas = columnas;
        grid = new Cell[filas, columnas];

        // Inicializar el grid con células muertas
        for (int i = 0; i < filas; i++) {
            for (int j = 0; j < columnas; j++) {
                grid[i, j] = new Cell(false);
            }
        }
    }

    
    public void UpdateCelula(int x, int y, bool estaViva) {
        if (x >= 0 && x < filas && y >= 0 && y < columnas) {
            grid[x, y].bIsAlive = estaViva;
        }
    }
    public void AplicarReglas() {
        Cell[,] nuevoGrid = new Cell[filas, columnas];

        for (int i = 0; i < filas; i++) {
            for (int j = 0; j < columnas; j++) {
                int vecinosVivos = ContarVecinosVivos(i, j);

                if (grid[i, j].bIsAlive) {
                    if (vecinosVivos < 2 || vecinosVivos > 3) {
                        nuevoGrid[i, j] = new Cell(false);
                    } else {
                        nuevoGrid[i, j] = new Cell(true);
                    }
                } else {
                    if (vecinosVivos == 3) {
                        nuevoGrid[i, j] = new Cell(true);
                    } else {
                        nuevoGrid[i, j] = new Cell(false);
                    }
                }
            }
        }

        
        grid = nuevoGrid;
    }
    private int ContarVecinosVivos(int x, int y) {
        int vecinosVivos = 0;

        for (int i = -1; i <= 1; i++) {
            for (int j = -1; j <= 1; j++) {
                // Evitar la célula misma
                if (i == 0 && j == 0)
                    continue;

                int vecinoX = x + i;
                int vecinoY = y + j;

                if (vecinoX >= 0 && vecinoX < filas && vecinoY >= 0 && vecinoY < columnas) {
                    if (grid[vecinoX, vecinoY].bIsAlive) {
                        vecinosVivos++;
                    }
                }
            }
        }

        return vecinosVivos;
    }

}
