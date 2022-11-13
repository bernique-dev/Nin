
using System;
using System.Linq;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class TileManager : MonoBehaviour {

    public Tile tile;
    public Vector3Int positionInGrid;
    public TileGrid grid;

    public List<Point> walkerPoints;
    public List<Point> walkerPointsToNotLink;
    public List<PointGraphVertex> walkerVertices;
    public List<Point> vehiclePoints;
    public List<Point> vehiclePointsToNotLink;
    public List<PointGraphVertex> vehicleVertices;

    public Point buildingPoint;

    public bool showGizmos;



    private void OnDrawGizmos() {

        if (showGizmos) {
            if (tile) {
                //Calculate PlaceTaken
                //Gizmos.DrawWireCube(transform.position + Vector3.up * tile.placeTaken.y / 2, tile.placeTaken);
            }
            foreach (PointGraphVertex vertex in walkerVertices) {
                Gizmos.color = Color.blue;
                Gizmos.DrawLine(vertex.origin.position, vertex.destination.position);
            }

            foreach (PointGraphVertex vertex in vehicleVertices) {
                Gizmos.color = new Color32(255, 127, 0, 255);
                Gizmos.DrawLine(vertex.origin.position, vertex.destination.position);
            }
        }
    }

    public virtual List<PointGraphVertex> GetWalkerLinksWith(TileManager otherTileManager) {
        List<PointGraphVertex> vertices = new List<PointGraphVertex>();
        CardinalPoint cardinalPoint = otherTileManager.transform.position.IsComparedTo(transform.position);

        // Now without cardinalPoint
        List<Point> pointsToLinkFrom = walkerPoints.Except(walkerPointsToNotLink).ToList();
        List<Point> pointsToLinkTo = otherTileManager.walkerPoints.Except(otherTileManager.walkerPointsToNotLink).ToList(); ;

        foreach (Point pointToLinkFrom in pointsToLinkFrom) {
            foreach (Point pointToLinkTo in pointsToLinkTo) {
                //Debug.Log(Vector3.Distance(pointToLinkFrom.position, pointToLinkTo.position) + "-" + (pointToLinkFrom.position.ToString("F1") + " " + pointToLinkTo.position.ToString("F1")));
                //Debug.Log(pointToLinkFrom.name + "(" + transform.position + ")" + " -> " + pointToLinkFrom.name + "(" + otherTileManager.transform.position + ")" + " - " + Vector3.Distance(pointToLinkFrom.position, pointToLinkTo.position));
                if (Vector3.Distance(pointToLinkFrom.position, pointToLinkTo.position) < 1.5f) {
                    PointGraphVertex vertex = new PointGraphVertex(pointToLinkFrom, pointToLinkTo);
                    vertices.Add(vertex);
                }
            }
            //PointGraphVertex vertex = new PointGraphVertex(pointToLinkFrom, pointToLinkFrom.GetClosestPointAmong(pointsToLinkTo));
        }

        return vertices;
    }
}