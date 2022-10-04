using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WalkerPoint : Point {

    private void OnDrawGizmos() {
        Gizmos.DrawIcon(position, "WalkerIcon.png", true);
    }

}
