using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PointGraphHolder : MonoBehaviour {

    public static PointGraphHolder instance;

    public PointGraphManager walkerPointGraphManager;
    public PointGraphManager vehiclePointGraphManager;


    private void Awake() {
        if (instance == null) instance = this;
    }

}