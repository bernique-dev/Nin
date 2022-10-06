using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class TileInfo {

    [SerializeField] public Vector3Int positionOnGrid;
    [SerializeField] public Tile tile;
    public bool willBeInstantiated;
    public Vector3Int offset;
    public float rotation;

    public TileInfo(Vector3Int _pos, Tile _tile) : this(_pos, _tile, true) { }


    public TileInfo(Vector3Int _pos, Tile _tile, bool _willBeInstantiated) : this(_pos, _tile, _willBeInstantiated, Vector3Int.zero) { }
    public TileInfo(Vector3Int _pos, Tile _tile, bool _willBeInstantiated, Vector3Int _offset) {
        positionOnGrid = _pos;
        tile = _tile;
        willBeInstantiated = _willBeInstantiated;
        offset = _offset;
    }
    public TileInfo(Vector3Int _pos, Tile _tile, bool _willBeInstantiated, Vector3Int _offset, float _rotation) {
        positionOnGrid = _pos;
        tile = _tile;
        willBeInstantiated = _willBeInstantiated;
        offset = _offset;
        rotation = _rotation;
    }


}