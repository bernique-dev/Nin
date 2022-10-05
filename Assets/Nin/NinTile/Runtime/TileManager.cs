
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

    private Dictionary<CardinalPoint, List<Point>> cardinalPointsAndLinkableWalkerPoints;
    private Dictionary<CardinalPoint, List<Point>> cardinalPointsAndLinkableVehiclePoints;

    public bool showGizmos;

    public void Initialize() {
        cardinalPointsAndLinkableWalkerPoints = new Dictionary<CardinalPoint, List<Point>>();
        InitializeCardinalPointsAndLinkablePoints(cardinalPointsAndLinkableWalkerPoints, walkerPoints, walkerPointsToNotLink);
        cardinalPointsAndLinkableVehiclePoints = new Dictionary<CardinalPoint, List<Point>>();
        InitializeCardinalPointsAndLinkablePoints(cardinalPointsAndLinkableVehiclePoints, vehiclePoints, vehiclePointsToNotLink);

    }

    public void InitializeCardinalPointsAndLinkablePoints(Dictionary<CardinalPoint, List<Point>> dict, List<Point> points, List<Point> pointsToExclude) {
        foreach (CardinalPoint cp in Enum.GetValues(typeof(CardinalPoint))) {
            dict.Add(cp, new List<Point>());
        }
        foreach (Point point in points.Except(pointsToExclude)) {
            foreach (CardinalPoint cp in point.position.CouldBeComparedTo(transform.position)) {
                dict[cp].Add(point);
            }
        }
    }

    private void OnDrawGizmos() {

        if (showGizmos) {
            if (tile) {
                Gizmos.DrawWireCube(transform.position + Vector3.up * tile.placeTaken.y / 2, tile.placeTaken);
            }
            foreach (PointGraphVertex vertex in walkerVertices) {
                Gizmos.color = Color.blue;
                Gizmos.DrawLine(vertex.origin.position, vertex.destination.position);
            }

            foreach (PointGraphVertex vertex in vehicleVertices) {
                Gizmos.color = new Color32(255, 127, 0, 255);
                Gizmos.DrawLine(vertex.origin.position, vertex.destination.position);
            }
            if (cardinalPointsAndLinkableWalkerPoints == null || cardinalPointsAndLinkableVehiclePoints == null) {
                Initialize();
            }
            Dictionary<Point, string> pointAndCPString = new Dictionary<Point, string>();
            foreach (KeyValuePair<CardinalPoint, List<Point>> kvp in cardinalPointsAndLinkableWalkerPoints) {
                CardinalPoint cardinalPoint = kvp.Key;
                foreach (Point point in kvp.Value) {
                    string letter = Enum.GetName(typeof(CardinalPoint), cardinalPoint).Substring(0, 1);
                    if (pointAndCPString.ContainsKey(point)) {
                        pointAndCPString[point] += letter;
                    }
                    else {
                        pointAndCPString.Add(point, letter);
                    }
                }
            }
#if UNITY_EDITOR
            foreach (KeyValuePair<Point, string> kvp in pointAndCPString) {
                Handles.Label(kvp.Key.position + Vector3.up, kvp.Value);
            }
#endif
        }
    }

    public virtual List<PointGraphVertex> GetWalkerLinksWith(TileManager otherTileManager) {
        List<PointGraphVertex> vertices = new List<PointGraphVertex>();
        CardinalPoint cardinalPoint = otherTileManager.transform.position.IsComparedTo(transform.position);

        List<Point> pointsToLinkFrom = cardinalPointsAndLinkableWalkerPoints[cardinalPoint];
        List<Point> pointsToLinkTo = otherTileManager.cardinalPointsAndLinkableWalkerPoints[cardinalPoint.GetOpposite()];

        foreach (Point pointToLinkFrom in pointsToLinkFrom) {
            foreach (Point pointToLinkTo in pointsToLinkTo) {
                //Debug.Log(Vector3.Distance(pointToLinkFrom.position, pointToLinkTo.position) + "-" + (pointToLinkFrom.position.ToString("F1") + " " + pointToLinkTo.position.ToString("F1")));
                if (Vector3.Distance(pointToLinkFrom.position, pointToLinkTo.position) <= Mathf.Sqrt(2)) {
                    PointGraphVertex vertex = new PointGraphVertex(pointToLinkFrom, pointToLinkTo);
                    vertices.Add(vertex);
                }
            }
            //PointGraphVertex vertex = new PointGraphVertex(pointToLinkFrom, pointToLinkFrom.GetClosestPointAmong(pointsToLinkTo));
        }

        return vertices;
    }


}