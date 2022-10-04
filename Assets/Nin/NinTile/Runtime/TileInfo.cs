using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class TileInfo {

    [SerializeField] public Vector3Int position;
    [SerializeField] public Tile tile;

    public TileInfo(Vector3Int _pos, Tile _tile) {
        position = _pos;
        tile = _tile;
    }

}