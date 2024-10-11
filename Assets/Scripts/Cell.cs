using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cell {

    public bool bIsAlive { get; set;}

    public Cell(bool bIsAlive) {
        this.bIsAlive = bIsAlive;
    }

    public void Morir() {
        bIsAlive = false;
    }
}
