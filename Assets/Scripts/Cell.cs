using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cell {
    /*public bool bIsAlive { get; set;}

    public Cell(bool bIsAlive) {
        this.bIsAlive = bIsAlive;
    }

    public void Morir() {
        bIsAlive = false;
    }*/

    public bool bIsAlive;

   
    public Cell() {
        bIsAlive = false; 
    }
    public Cell(bool isAlive) {
        bIsAlive = isAlive;
    }
}
