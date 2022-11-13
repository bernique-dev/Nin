using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

/// <summary>
/// Representation of a Tile for the TileGrid
/// </summary>
[CreateAssetMenu(menuName = "Nin/Environment/Tile")]
public class Tile : ScriptableObject {

    /// <summary>
    /// GameObject instantiated when placed (during Runtime)
    /// </summary>
    public GameObject instance;
    /// <summary>
    /// Diemnsions of tiles taken when placed on the TileGrid
    /// </summary>
    public Vector3Int tilesTaken = Vector3Int.one;

}