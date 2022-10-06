using System;
using System.Linq;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class TileGrid {

    private List<TileInfo> tileInfos {
        get {
            if (m_tiles == null) m_tiles = new List<TileInfo>();
            return m_tiles;
        }
    }
    [SerializeField] private List<TileInfo> m_tiles;

    private Dictionary<Vector3Int, GameObject> tileInstances {
        get {
            if (m_tileInstances == null) m_tileInstances = new Dictionary<Vector3Int, GameObject>();
            return m_tileInstances;
        }
    }
    private Dictionary<Vector3Int, GameObject> m_tileInstances;

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

    public static Vector3Int GetRotatedVector3Int(float rotation, Vector3Int tilesTaken) {
        Vector3Int result = tilesTaken;
        for (float currentRotation = rotation; currentRotation > 0; currentRotation -= 90) {
            result = new Vector3Int(result.z, 0, -result.x);
        }
        return result;
    }
    public static Vector3 GetRotatedVector3(float rotation, Vector3 tilesTaken) {
        Vector3 result = tilesTaken;
        for (float currentRotation = rotation; currentRotation > 0; currentRotation -= 90) {
            result = new Vector3(result.z, 0, -result.x);
        }
        return result;
    }

    public static Vector3Int GetRotatedTileExtent(float rotation, Vector3Int tilesTaken) {
        Vector3Int tilesExtent = GetRotatedVector3Int(rotation, tilesTaken);
        Vector3Int tilesExtentDirection = new Vector3Int(Mathf.Clamp(tilesExtent.x, -1, 1), 0, Mathf.Clamp(tilesExtent.z, -1, 1));
        return tilesExtentDirection;
    }


    public void RemoveTile(Vector3Int pos) {
        TileInfo potentialTile = tileInfos.Find(ti => ti.positionOnGrid == pos);
        if (potentialTile != null) {
            tileInfos.Remove(potentialTile);
        }
    }

    public void SetTileInstance(Vector3Int pos, GameObject instance) {
        if (tileInstances.ContainsKey(pos)) {
            tileInstances[pos] = instance;
        } else {
            tileInstances.Add(pos, instance);
        }
    }

    public Tile GetTile(Vector3Int pos) {
        TileInfo searchedTile = GetTileInfo(pos);
        return searchedTile != null ? searchedTile.tile : null;
    }

    private TileInfo GetTileInfo(Vector3Int pos) {
        TileInfo searchedTile = tileInfos.Find(ti => ti.positionOnGrid == pos);
        return searchedTile;
    }

    private TileInfo GetInstantiatedTileInfo(Vector3Int pos) {
        TileInfo searchedTile = tileInfos.Find(ti => ti.positionOnGrid == pos);
        return searchedTile.willBeInstantiated ? searchedTile : GetInstantiatedTileInfo(pos - searchedTile.offset);
    }

    public Dictionary<Vector3Int, Tile> GetPositionsAndTiles() {
        return tileInfos.ToDictionary(ti => ti.positionOnGrid, ti => ti.tile);
    }

    public List<TileInfo> GetTileInfos() {
        return tileInfos;
    }

    public GameObject GetTileInstance(Vector3Int pos) {
        return tileInstances[pos];
    }

    public Dictionary<Vector3Int, Tile> GetPositionAndNeigbours(Vector3Int pos) {
        TileInfo tileInfo = GetTileInfo(pos);
        List<TileInfo> result = new List<TileInfo>();

        for (int x = 0; x < tileInfo.tile.tilesTaken.x; x++) {
            for (int z = 0; z < tileInfo.tile.tilesTaken.z; z++) {
                Vector3Int offsetPos = pos + new Vector3Int(x, 0, z);
                result.AddRange(tileInfos.Where(t => Vector3Int.Distance(t.positionOnGrid, offsetPos) == 1).ToList());;
            }
        }

        string test = pos + "([" + string.Join(',', result.Select(ti => ti.positionOnGrid)) + "]";

        for (int x = 0; x < tileInfo.tile.tilesTaken.x; x++) {
            for (int z = 0; z < tileInfo.tile.tilesTaken.z; z++) {
                Vector3Int offsetPos = pos + new Vector3Int(x, 0, z);
                result = result.Where(ti => ti.positionOnGrid != offsetPos).ToList();
            }
        }

        test += " -> [" + string.Join(',', result.Select(ti => ti.positionOnGrid)) + "])";
        //Debug.Log(test);
        
        return result.ToDictionary(ti => ti.positionOnGrid, ti => ti.tile);
    }

}