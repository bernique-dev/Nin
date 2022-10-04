using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Point : MonoBehaviour {

    public Vector3 position {
        get {
            return transform.position;
        }
    }

    public override string ToString() {
        return name;
    }


}