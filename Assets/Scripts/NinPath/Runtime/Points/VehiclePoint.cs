using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Point associated to a Vehicle's possible position
/// </summary>
public class VehiclePoint : Point {

    private void OnDrawGizmos() {
        Gizmos.DrawIcon(position, "VehicleIcon.png", true);
    }

}
