using System;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Informations about a Tile:
///     - The Tile itself
///     - The Tile's position
///     - The instantiation state (if it's the tile where the GO is instatiated or not)
///     - The offset compared to the tile's instance on the TileGrid
///     - The rotation of the Tile on the TileGrid
/// </summary>
[Serializable]
public class TileInfo {

    /// <summary>
    /// Tile
    /// </summary>
    [SerializeField] public Tile tile;
    /// <summary>
    /// Position on the TileGrid
    /// </summary>
    [SerializeField] public Vector3Int positionOnGrid;
    /// <summary>
    /// Instantiation state (true if the GO is instantiated on this position, else false)
    /// </summary>
    public bool willBeInstantiated;
    /// <summary>
    /// Offset compared to the tile's instance on the TileGrid
    /// </summary>
    public Vector3Int offset;
    /// <summary>
    /// Rotation of the tile's instance on the TileGrid
    /// </summary>
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