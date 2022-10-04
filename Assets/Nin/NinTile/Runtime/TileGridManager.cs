using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileGridManager : MonoBehaviour {

    public bool showGizmos;

    public Vector3 tileSize = Vector3.one;

    public TileGrid tileGrid {
        get {
            if (m_tileGrid == null) m_tileGrid = new TileGrid();
            return m_tileGrid;
        }
        set {
            m_tileGrid = value;
        }
    }
    [SerializeField] private TileGrid m_tileGrid;

    [HideInInspector] public bool doesTilesFoldout;
    [HideInInspector] public TileInfo tileInfoToAdd;

    private void Start() {
        InstantiateTiles();    
    }

    public void InstantiateTiles() {
        foreach (KeyValuePair<Vector3Int, Tile> kvp in tileGrid.GetAllTiles()) {
            Vector3 instancePos = new Vector3(kvp.Key.x * tileSize.x, kvp.Key.y * tileSize.y, kvp.Key.z * tileSize.z) + new Vector3(kvp.Value.tilesTaken.x * tileSize.x, 0, kvp.Value.tilesTaken.y * tileSize.z) / 2;
            GameObject instance = Instantiate(kvp.Value.instance , transform);
            instance.transform.position = instancePos;
            tileGrid.SetTileInstance(instancePos, instance);

            TileManager tileManager = instance.GetComponent<TileManager>();
            tileManager.positionInGrid = kvp.Key;
            tileManager.grid = tileGrid;
            tileManager.CheckNeighbours();
        }
    }

    private void OnDrawGizmos() {
        if (showGizmos) {

        }
    }

}
