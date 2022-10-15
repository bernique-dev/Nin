using System.Linq;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Debug component to easily generate Point fields and generate a Graph from it
/// </summary>
public class PointFieldManager : MonoBehaviour {

    public GameObject pointPrefab;

    public Vector3 fieldStart;
    public Vector3 fieldEnd;
    public Vector3 fieldGap;

    public PointGraphManager graphManager;

    private List<Point> points;

    /// <summary>
    /// Instantiate Points
    /// </summary>
    public void GenerateField() {
        int children = transform.childCount;
        points = new List<Point>();
        for (int i = children - 1; i >= 0; i--) {
            GameObject.DestroyImmediate(transform.GetChild(i).gameObject);
        }
        for (float x = fieldStart.x; x <= fieldEnd.x; x += fieldGap.x) {
            for (float y = fieldStart.y; y <= fieldEnd.y; y += fieldGap.y) {
                for (float z = fieldStart.z; z <= fieldEnd.z; z += fieldGap.z) {
                    GameObject instance = Instantiate(pointPrefab, new Vector3(x, y, z), Quaternion.identity);
                    instance.transform.parent = transform;
                    instance.name = "Point (" + new Vector3(x,y,z) +")";
                    points.Add(instance.GetComponent<Point>());
                }
            }
        }
    }

    /// <summary>
    /// Generates graph from previously instantiated Points
    /// </summary>
    public void GenerateGraph() {
        graphManager.graph.points = points;
        graphManager.graph.vertices = new List<PointGraphVertex>();
        List<Point> treatedPoints = new List<Point>();
        foreach (Point point in points) {
            treatedPoints.Add(point);
            foreach (Point otherPoint in points.Where(p => !treatedPoints.Contains(p) && AreNeighbours(point, p)).ToList()) {
                PointGraphVertex vertex = new PointGraphVertex(point, otherPoint);
                graphManager.graph.vertices.Add(vertex);
            }
        }
    }

    /// <summary>
    /// Delete a specified number of random vertices
    /// </summary>
    /// <param name="nb">Number of ramdom removed vertices</param>
    public void DeleteRandomVertices(int nb) {
        List<PointGraphVertex> vertices = graphManager.graph.vertices;
        for (int delete = 0; delete < nb; delete++) {
            vertices.RemoveAt(Random.Range(0, vertices.Count));
        }
    }

    /// <summary>
    /// Returns if p1 and p2 are Neighbours in the Point field
    /// </summary>
    private bool AreNeighbours(Point p1, Point p2) {
        Vector3 pos1 = p1.position;
        Vector3 pos2 = p2.position;
        return Mathf.Abs(pos1.x - pos2.x) <= Mathf.Sqrt(2 * fieldGap.x * fieldGap.x)
            && Mathf.Abs(pos1.y - pos2.y) <= Mathf.Sqrt(2 * fieldGap.y * fieldGap.y)
            && Mathf.Abs(pos1.z - pos2.z) <= Mathf.Sqrt(2 * fieldGap.z * fieldGap.z);
    }

}
