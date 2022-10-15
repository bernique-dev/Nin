using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Point associated to a Walker's possible position
/// </summary>
public class WalkerPoint : Point {

    private void OnDrawGizmos() {
        Gizmos.DrawIcon(position, "WalkerIcon.png", true);
    }

}
