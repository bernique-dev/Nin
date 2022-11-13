using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildingManager : MonoBehaviour, ILoadable, ILoadingWaiter {

    public TileManager tileManager;
    public void Load() { }
    public void Begin() { }

    private void Reset() {
        tileManager = GetComponent<TileManager>();
    }

}