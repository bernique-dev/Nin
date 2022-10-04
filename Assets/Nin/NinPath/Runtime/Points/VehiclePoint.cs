using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VehiclePoint : Point {

    private void OnDrawGizmos() {
        Gizmos.DrawIcon(position, "VehicleIcon.png", true);
    }

}
