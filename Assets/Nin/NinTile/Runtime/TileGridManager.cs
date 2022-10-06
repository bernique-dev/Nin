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
        List<TileInfo> tileInfos = tileGrid.GetTileInfos();
        foreach (TileInfo tileInfo in tileInfos.Where(ti => ti.willBeInstantiated)) {
            InstantiateTile(tileInfo);
        }
        foreach (TileInfo tileInfo in tileInfos.Where(ti => !ti.willBeInstantiated)) {
            tileGrid.SetTileInstance(tileInfo.positionOnGrid, tileGrid.GetTileInstance(tileInfo.positionOnGrid - tileInfo.offset));
        }
    }

    public void InstantiateTile(TileInfo tileInfo) {
        Vector3Int position = tileInfo.positionOnGrid;
        Tile tile = tileInfo.tile;
        float rotationZ = tileInfo.rotation;
        Quaternion rotation = Quaternion.Euler(0, rotationZ, 0);

        Vector3 tilePositionBeforeOffset = new Vector3(position.x * tileSize.x, position.y * tileSize.y, position.z * tileSize.z);
        Vector3 pivotOffset = new Vector3(tileSize.x, 0, tileSize.z) / 2;
        Vector3 centerOffset = new Vector3(tile.tilesTaken.x * tileSize.x, 0, tile.tilesTaken.y * tileSize.z) / 2;
        if (tile.tilesTaken != Vector3Int.one) centerOffset = VectorUtils.RotatePointAroundPivot(centerOffset, pivotOffset, rotation);
        Vector3 instancePos = tilePositionBeforeOffset + centerOffset;

        GameObject instance = Instantiate(tile.instance, transform);
        instance.transform.position = instancePos;
        instance.transform.rotation = rotation;
        tileGrid.SetTileInstance(position, instance);

        TileManager tileManager = instance.GetComponent<TileManager>();
        tileManager.positionInGrid = position;
        tileManager.grid = tileGrid;
    }

    public void ConnectTiles() {
        foreach (TileInfo tileInfo in tileGrid.GetTileInfos().Where(ti => ti.willBeInstantiated)) {
            ConnectTile(tileInfo.positionOnGrid);
        }
    }

    public void ConnectTile(Vector3Int position) {
        TileManager tileManager = tileGrid.GetTileInstance(position).GetComponent<TileManager>();

        //ADD TILE'S POINTS AND VERTICES TO GRAPH
        walkerGraphManager.graph.points.AddRange(tileManager.walkerPoints);
        vehicleGraphManager.graph.points.AddRange(tileManager.vehiclePoints);

        walkerGraphManager.graph.vertices.AddRange(tileManager.walkerVertices);
        vehicleGraphManager.graph.vertices.AddRange(tileManager.vehicleVertices);


        foreach (KeyValuePair<Vector3Int, Tile> neighbourKvp in tileGrid.GetPositionAndNeigbours(position)) {
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
            Vector3 extentsMin = new Vector3(-32, 0, -32);
            Vector3 extentsMax = new Vector3(32, 0, 32);
            for (float x = extentsMin.x; x <= extentsMax.x; x += tileSize.x) {
                Gizmos.DrawLine(new Vector3(x, 0, extentsMin.z), new Vector3(x, 0, extentsMax.z));
            }
            for (float z = extentsMin.x; z <= extentsMax.x; z += tileSize.z) {
                Gizmos.DrawLine(new Vector3(extentsMin.x, 0, z), new Vector3(extentsMax.x, 0, z));
            }
        }
    }

}
