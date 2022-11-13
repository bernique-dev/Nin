using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NinbazBuildingManager : BuildingManager {

    
    public GameObject ninPrefab;
    int ninNumber = 1;

    public void CreateNin() {
        GameObject ninInstance = Instantiate(ninPrefab);
        ninInstance.name = "Nin " + ninNumber++;
        ninInstance.transform.position = tileManager.buildingPoint.position;
        ninInstance.GetComponent<PointPathFollower>().origin = tileManager.buildingPoint;
        ninInstance.GetComponent<PointPathFollower>().graphManager = PointGraphHolder.instance.walkerPointGraphManager;
        ninInstance.GetComponent<PointPathFollower>().CalculateShortestPathFromHereToRandomPoint();
    }

}