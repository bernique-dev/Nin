using System;
using System.Linq;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Representation of the Tile Grid
/// </summary>
[Serializable]
public class TileGrid {

    /// <summary>
    /// List of TileInfos in the grid
    /// </summary>
    private List<TileInfo> tileInfos {
        get {
            if (m_tiles == null) m_tiles = new List<TileInfo>();
            return m_tiles;
        }
    }
    [SerializeField] private List<TileInfo> m_tiles;

    /// <summary>
    /// Dictionary associating TileGrid's positions with Tile's instances (ONLY USE IN RUNTIME)
    /// </summary>
    private Dictionary<Vector3Int, GameObject> tileInstances {
        get {
            if (m_tileInstances == null) m_tileInstances = new Dictionary<Vector3Int, GameObject>();
            return m_tileInstances;
        }
    }
    private Dictionary<Vector3Int, GameObject> m_tileInstances;

    /// <summary>
    /// Returns if a Tile is settable
    /// </summary>
    /// <param name="pos">Position to test</param>
    /// <param name="tile">Tile to position</param>
    /// <param name="rotation">Rotation to use for the positioning</param>
    public bool CanSetTile(Vector3Int pos, Tile tile, float rotation) {
        bool result = true;
        Vector3Int tilesExtent = GetRotatedVector3Int(rotation, tile.tilesTaken);
        Vector3Int tilesExtentDirection = GetRotatedTileExtent(rotation, tile.tilesTaken);
        for (int x = 0; x <= Mathf.Abs(tilesExtent.x) - 1; x++) {
            for (int z = 0; z <= Mathf.Abs(tilesExtent.z) - 1; z++) {
                Vector3Int tileOffset = new Vector3Int(x * tilesExtentDirection.x, 0, z * tilesExtentDirection.z);
                Vector3Int offsetPos = pos + tileOffset;
                TileInfo potentialTile = tileInfos.Find(ti => offsetPos == ti.positionOnGrid);
                if (potentialTile != null) {
                    result = false;
                }
            }
        }
        return result;
    }

    /// <summary>
    /// Sets a Tile at a specified position with a specified rotation
    /// </summary>
    /// <param name="pos">Position to place at</param>
    /// <param name="tile">Tile to position</param>
    /// <param name="rotation">Rotation to use for the positioning</param>
    public void SetTile(Vector3Int pos, Tile tile, float rotation) {
        if (!CanSetTile(pos, tile, rotation)) {
            throw new Exception("Can't place tile (" + tile.instance.name +") on " + (pos));
        }
        Vector3Int tilesExtent = GetRotatedVector3Int(rotation, tile.tilesTaken);
        Vector3Int tilesExtentDirection = GetRotatedTileExtent(rotation, tile.tilesTaken);
        for (int x = 0; x <= Mathf.Abs(tilesExtent.x) - 1; x++) {
            for (int z = 0; z <= Mathf.Abs(tilesExtent.z) - 1; z++) {
                Vector3Int tileOffset = new Vector3Int(x * tilesExtentDirection.x, 0, z * tilesExtentDirection.z);
                Vector3Int offsetPos = pos + tileOffset;
                TileInfo potentialTile = tileInfos.Find(ti => offsetPos == ti.positionOnGrid);
                if (potentialTile != null) {
                    Debug.Log(potentialTile.tile);
                    throw new Exception("Can't set tile ! Tile already on " + (pos + tileOffset));
                } else {
                    tileInfos.Add(new TileInfo(pos + tileOffset, tile, tileOffset == Vector3Int.zero, tileOffset, rotation));
                }
            }
        }
    }

    /// <summary>
    /// Returns specified vector after specified rotation
    /// </summary>
    /// <param name="rotation">Rotation to apply on vector</param>
    /// <param name="vector">Vector to rotate</param>
    public static Vector3Int GetRotatedVector3Int(float rotation, Vector3Int vector) {
        Vector3Int result = vector;
        for (float currentRotation = rotation; currentRotation > 0; currentRotation -= 90) {
            result = new Vector3Int(result.z, 0, -result.x);
        }
        return result;
    }

    /// <summary>
    /// Returns specified vector after specified rotation
    /// </summary>
    /// <param name="rotation">Rotation to apply on vector</param>
    /// <param name="vector">Vector to rotate</param>
    public static Vector3 GetRotatedVector3(float rotation, Vector3 vector) {
        Vector3 result = vector;
        for (float currentRotation = rotation; currentRotation > 0; currentRotation -= 90) {
            result = new Vector3(result.z, 0, -result.x);
        }
        return result;
    }

    /// <summary>
    /// Returns tiles extent obtained after rotating specified Tile taken by specified rotation
    /// </summary>
    /// <param name="rotation">Rotation to apply on tiles taken</param>
    /// <param name="tilesTaken">Tiles taken</param>
    /// <returns></returns>
    public static Vector3Int GetRotatedTileExtent(float rotation, Vector3Int tilesTaken) {
        Vector3Int tilesExtent = GetRotatedVector3Int(rotation, tilesTaken);
        Vector3Int tilesExtentDirection = new Vector3Int(Mathf.Clamp(tilesExtent.x, -1, 1), 0, Mathf.Clamp(tilesExtent.z, -1, 1));
        return tilesExtentDirection;
    }

    /// <summary>
    /// Remove Tile at specified position
    /// </summary>
    /// <param name="pos">Position of the Tile to remove</param>
    public void RemoveTile(Vector3Int pos) {
        TileInfo potentialTile = tileInfos.Find(ti => ti.positionOnGrid == pos);
        if (potentialTile != null) {
            tileInfos.Remove(potentialTile);
        }
    }

    /// <summary>
    /// Sets specified Tile instance at specified position
    /// </summary>
    /// <param name="pos">Position to set instance at</param>
    /// <param name="instance">Instance to set</param>
    public void SetTileInstance(Vector3Int pos, GameObject instance) {
        if (tileInstances.ContainsKey(pos)) {
            tileInstances[pos] = instance;
        } else {
            tileInstances.Add(pos, instance);
        }
    }

    /// <summary>
    /// Returns Tile at specified position
    /// </summary>
    /// <param name="pos">Requested position</param>
    public Tile GetTile(Vector3Int pos) {
        TileInfo searchedTile = GetTileInfo(pos);
        return searchedTile != null ? searchedTile.tile : null;
    }

    /// <summary>
    /// Returns TileInfo at specified position
    /// </summary>
    /// <param name="pos">Requested position</param>
    private TileInfo GetTileInfo(Vector3Int pos) {
        TileInfo searchedTile = tileInfos.Find(ti => ti.positionOnGrid == pos);
        return searchedTile;
    }

    /// <summary>
    /// Returns instance's TileInfo at specified position (if multiple Tiles are taken, returns the instatiated TileInfo)
    /// </summary>
    /// <param name="pos">Requested position</param>
    private TileInfo GetInstantiatedTileInfo(Vector3Int pos) {
        TileInfo searchedTile = tileInfos.Find(ti => ti.positionOnGrid == pos);
        return searchedTile.willBeInstantiated ? searchedTile : GetInstantiatedTileInfo(pos - searchedTile.offset);
    }

    /// <summary>
    /// Returns Dictionary associating positions and Tiles placed at it
    /// </summary>
    public Dictionary<Vector3Int, Tile> GetPositionsAndTiles() {
        return tileInfos.ToDictionary(ti => ti.positionOnGrid, ti => ti.tile);
    }

    /// <summary>
    /// Returns all TileInfos
    /// </summary>
    public List<TileInfo> GetTileInfos() {
        return tileInfos;
    }

    /// <summary>
    /// Returns Tile's instance at specified position
    /// </summary>
    /// <param name="pos">Position requested</param>
    public GameObject GetTileInstance(Vector3Int pos) {
        return tileInstances[pos];
    }

    /// <summary>
    /// Returns Dictionary associating a position to its neighbouring Tiles (check all Tiles if more than one TIle is taken)
    /// </summary>
    /// <param name="pos">Position to check neighbours from</param>
    public Dictionary<Vector3Int, Tile> GetPositionAndNeigbours(Vector3Int pos) {
        TileInfo tileInfo = GetTileInfo(pos);
        List<TileInfo> result = new List<TileInfo>();

        // Gets all Tiles taken neighbours (same building included)
        for (int x = 0; x < tileInfo.tile.tilesTaken.x; x++) {
            for (int z = 0; z < tileInfo.tile.tilesTaken.z; z++) {
                Vector3Int offsetPos = pos + new Vector3Int(x, 0, z);
                result.AddRange(tileInfos.Where(t => Vector3Int.Distance(t.positionOnGrid, offsetPos) == 1).ToList());;
            }
        }

        // Removes all positions from Tiles taken
        for (int x = 0; x < tileInfo.tile.tilesTaken.x; x++) {
            for (int z = 0; z < tileInfo.tile.tilesTaken.z; z++) {
                Vector3Int offsetPos = pos + new Vector3Int(x, 0, z);
                result = result.Where(ti => ti.positionOnGrid != offsetPos).ToList();
            }
        }
        
        return result.ToDictionary(ti => ti.positionOnGrid, ti => ti.tile);
    }

}