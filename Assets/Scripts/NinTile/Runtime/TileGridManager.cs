using System.Linq;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Manages the TileGrid and its Tiles' instantiation
/// </summary>
public class TileGridManager : MonoBehaviour, ILoadable {

    public static TileGridManager instance;

    /// <summary>
    /// Show Gizmos on scene
    /// </summary>
    public bool showGizmos;

    /// <summary>
    /// Size of Tile in the world
    /// </summary>
    public Vector3 tileSize = Vector3.one;

    /// <summary>
    /// Managed TileGrid
    /// </summary>
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

    /// <summary>
    /// Inspector's tiles' foldout state
    /// </summary>
    [HideInInspector] public bool doesTilesFoldout;
    /// <summary>
    /// Temporary TileInfo used before adding it to TileGrid in the Inspector
    /// </summary>
    [HideInInspector] public TileInfo tileInfoToAdd;

    /// <summary>
    /// GraphManager of the Walkers
    /// </summary>
    public PointGraphManager walkerGraphManager;
    /// <summary>
    /// GraphManager of the Vehicles
    /// </summary>
    public PointGraphManager vehicleGraphManager;

    private void Awake() {
        if (instance == null) instance = this;
    }

    public void Load() {
        GenerateGrid();
        walkerGraphManager.Load();
        vehicleGraphManager.Load();
    }

    /// <summary>
    /// Instatiate and connect Tiles
    /// </summary>
    public void GenerateGrid() {
        InstantiateTiles();
        ConnectTiles();
    }

    /// <summary>
    /// Instantiate all Tiles in TileGrid
    /// </summary>
    public void InstantiateTiles() {
        List<TileInfo> tileInfos = tileGrid.GetTileInfos();
        // Instantiate All Tiles
        foreach (TileInfo tileInfo in tileInfos.Where(ti => ti.willBeInstantiated)) {
            InstantiateTile(tileInfo);
        }
        // Associate non-instantiated Tiles to their instantiated Tile
        foreach (TileInfo tileInfo in tileInfos.Where(ti => !ti.willBeInstantiated)) {
            tileGrid.SetTileInstance(tileInfo.positionOnGrid, tileGrid.GetTileInstance(tileInfo.positionOnGrid - tileInfo.offset));
        }
    }

    /// <summary>
    /// Instantiate specified Tile
    /// </summary>
    /// <param name="tileInfo">TileInfo of the Tile to add</param>
    public void InstantiateTile(TileInfo tileInfo) {
        // Sets easier accesses to info
        Vector3Int position = tileInfo.positionOnGrid;
        Tile tile = tileInfo.tile;
        float rotationZ = tileInfo.rotation;
        Quaternion rotation = Quaternion.Euler(0, rotationZ, 0);

        // Calculates positions
        Vector3 tilePositionBeforeOffset = new Vector3(position.x * tileSize.x, position.y * tileSize.y, position.z * tileSize.z);
        Vector3 pivotOffset = new Vector3(tileSize.x, 0, tileSize.z) / 2;
        Vector3 centerOffset = new Vector3(tile.tilesTaken.x * tileSize.x, 0, tile.tilesTaken.z * tileSize.z) / 2;
        if (tile.tilesTaken != Vector3Int.one) centerOffset = VectorUtils.RotatePointAroundPivot(centerOffset, pivotOffset, rotation);
        Vector3 instancePos = tilePositionBeforeOffset + centerOffset;


        // Instantiates and positions Tile. Associates it to position in the TileGrid
        GameObject instance = Instantiate(tile.instance, transform);
        instance.transform.position = instancePos;
        instance.transform.rotation = rotation;
        tileGrid.SetTileInstance(position, instance);

        TileManager tileManager = instance.GetComponent<TileManager>();
        tileManager.positionInGrid = position;
        tileManager.grid = tileGrid;
    }

    /// <summary>
    /// Connect all instantiated Tiles
    /// </summary>
    public void ConnectTiles() {
        foreach (TileInfo tileInfo in tileGrid.GetTileInfos().Where(ti => ti.willBeInstantiated)) {
            ConnectTile(tileInfo.positionOnGrid);
        }
    }

    /// <summary>
    /// Connect instantiated Tile at specified position
    /// </summary>
    /// <param name="position"></param>
    public void ConnectTile(Vector3Int position) {
        TileManager tileManager = tileGrid.GetTileInstance(position).GetComponent<TileManager>();

        //Add Tile's points and vertices to graphs
        walkerGraphManager.graph.points.AddRange(tileManager.walkerPoints);
        vehicleGraphManager.graph.points.AddRange(tileManager.vehiclePoints);

        walkerGraphManager.graph.vertices.AddRange(tileManager.walkerVertices);
        vehicleGraphManager.graph.vertices.AddRange(tileManager.vehicleVertices);


        foreach (KeyValuePair<Vector3Int, Tile> neighbourKvp in tileGrid.GetPositionAndNeigbours(position)) {
            GameObject neighbourInstance = tileGrid.GetTileInstance(neighbourKvp.Key);
            if (neighbourInstance) {
                TileManager neighbourTileManager = neighbourInstance.GetComponent<TileManager>();
                List<PointGraphVertex> vertices = tileManager.GetWalkerLinksWith(neighbourTileManager);
                //todo Add to vehicleGraph (needs road to test)
                walkerGraphManager.graph.vertices.AddRange(vertices.Where(v => walkerGraphManager.graph.vertices.Find(vb => vb.Equals(v)) == null));
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
