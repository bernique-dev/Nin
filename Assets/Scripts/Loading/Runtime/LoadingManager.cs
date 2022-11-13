using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LoadingManager : MonoBehaviour {

    public static LoadingManager instance;

    public bool isLoadingFinished;
    public float loadingTime = -1;

    private void Awake() {
        if (instance == null) instance = this;
    }

    private void Start() {
        Load();
    }

    public void Load() {
        isLoadingFinished = false;
        loadingTime = -1;
        float startupTime = Time.realtimeSinceStartup;


        if (TileGridManager.instance) {
            TileGridManager.instance.Load();
        } else {
            foreach (var item in FindObjectsOfType<PointGraphManager>()) {
                item.Load();
            }
        }
        if (PointPathFollower.instances != null) PointPathFollower.instances.ForEach(ppf => ppf.Load());
        //todo Call all NinBehaviour
        //todo Call all BuildingBehaviour

        loadingTime = Time.realtimeSinceStartup - startupTime;
        isLoadingFinished = true;
        Debug.Log("Game loaded");


        if (PointPathFollower.instances != null) PointPathFollower.instances.ForEach(ppf => ppf.Begin());

    }
}
