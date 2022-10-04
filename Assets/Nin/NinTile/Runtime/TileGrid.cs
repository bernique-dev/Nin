using System;
using System.Linq;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class TileGrid {

    private List<TileInfo> tiles {
        get {
            if (m_tiles == null) m_tiles = new List<TileInfo>();
            return m_tiles;
        }
    }
    [SerializeField] private List<TileInfo> m_tiles;

    private Dictionary<Vector3, GameObject> tileInstances {
        get {
            if (m_tileInstances == null) m_tileInstances = new Dictionary<Vector3, GameObject>();
            return m_tileInstances;
        }
    }
    private Dictionary<Vector3, GameObject> m_tileInstances;

    public void SetTile(Vector3Int pos, Tile tile) {
        TileInfo potentialTile = tiles.Find(ti => ti.position == pos);
        if (potentialTile != null) {
            tiles.Remove(potentialTile);
        }
        tiles.Add(new TileInfo(pos, tile));
    }

    public void RemoveTile(Vector3Int pos) {
        TileInfo potentialTile = tiles.Find(ti => ti.position == pos);
        if (potentialTile != null) {
            tiles.Remove(potentialTile);
        }
    }

    public void SetTileInstance(Vector3 pos, GameObject instance) {
        if (tileInstances.ContainsKey(pos)) {
            tileInstances[pos] = instance;
        }
        else {
            tileInstances.Add(pos, instance);
        }
    }

    public Tile GetTile(Vector3Int pos) {
        TileInfo searchedTile = tiles.Find(ti => ti.position == pos);
        return searchedTile != null ? searchedTile.tile : null;
    }

    public Dictionary<Vector3Int, Tile> GetAllTiles() {
        return tiles.ToDictionary(ti => ti.position, ti => ti.tile);
    }

    public GameObject GetTileInstance(Vector3 pos) {
        return tileInstances[pos];
    }

    public Dictionary<Vector3Int, Tile> GetNeigbours(Vector3Int pos) {
        return tiles.Where(t => Vector3Int.Distance(t.position, pos) == 1).ToDictionary(ti => ti.position, ti => ti.tile);
    }

}