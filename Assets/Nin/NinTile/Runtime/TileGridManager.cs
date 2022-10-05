using System.Linq;
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

    public PointGraphManager walkerGraphManager;
    public PointGraphManager vehicleGraphManager;

    private void Start() {
        GenerateGrid();
    }

    public void GenerateGrid() {
        InstantiateTiles();
        ConnectTiles();
    }

    public void InstantiateTiles() {
        foreach (KeyValuePair<Vector3Int, Tile> kvp in tileGrid.GetAllTiles()) {
            InstantiateTile(kvp.Key, kvp.Value);
        }
    }

    public void InstantiateTile(Vector3Int position, Tile tile) {
        Vector3 instancePos = new Vector3(position.x * tileSize.x, position.y * tileSize.y, position.z * tileSize.z) + new Vector3(tile.tilesTaken.x * tileSize.x, 0, tile.tilesTaken.y * tileSize.z) / 2;
        GameObject instance = Instantiate(tile.instance, transform);
        instance.transform.position = instancePos;
        tileGrid.SetTileInstance(position, instance);

        TileManager tileManager = instance.GetComponent<TileManager>();
        tileManager.Initialize();
        tileManager.positionInGrid = position;
        tileManager.grid = tileGrid;
    }

    public void ConnectTiles() {
        foreach (KeyValuePair<Vector3Int, Tile> kvp in tileGrid.GetAllTiles()) {
            ConnectTile(kvp.Key);
        }
    }

    public void ConnectTile(Vector3Int position) {
        TileManager tileManager = tileGrid.GetTileInstance(position).GetComponent<TileManager>();

        //ADD TILE'S POINTS AND VERTICES TO GRAPH
        walkerGraphManager.graph.points.AddRange(tileManager.walkerPoints);
        vehicleGraphManager.graph.points.AddRange(tileManager.vehiclePoints);

        walkerGraphManager.graph.vertices.AddRange(tileManager.walkerVertices);
        vehicleGraphManager.graph.vertices.AddRange(tileManager.vehicleVertices);


        foreach (KeyValuePair<Vector3Int, Tile> neighbourKvp in tileGrid.GetNeigbours(position)) {
            GameObject neighbourInstance = tileGrid.GetTileInstance(neighbourKvp.Key);
            if (neighbourInstance) {
                TileManager neighbourTileManager = neighbourInstance.GetComponent<TileManager>();
                List<PointGraphVertex> vertices = tileManager.GetWalkerLinksWith(neighbourTileManager);
                //Debug.Log(vertices.Count);
                walkerGraphManager.graph.vertices.AddRange(vertices.Where(v => walkerGraphManager.graph.vertices.Find(vb => vb.IsEqualTo(v)) == null));
            }
        }
    }

    private void OnDrawGizmos() {
        if (showGizmos) {

        }
    }

}
