
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileManager : MonoBehaviour {

    public Tile tile;
    public Vector3Int positionInGrid;
    public TileGrid grid;

    private void OnDrawGizmos() {
        if (tile) {
            Gizmos.DrawWireCube(transform.position + Vector3.up * tile.placeTaken.y / 2, tile.placeTaken);
        }
    }

    public virtual void CheckNeighbours() {
        Debug.Log(grid.GetNeigbours(positionInGrid).Count);
    }

}