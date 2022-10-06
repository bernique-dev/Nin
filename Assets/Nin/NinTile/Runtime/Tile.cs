using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CreateAssetMenu(menuName = "Nin/Environment/Tile")]
public class Tile : ScriptableObject {

    public GameObject instance;
    public Vector3Int tilesTaken = Vector3Int.one;

}