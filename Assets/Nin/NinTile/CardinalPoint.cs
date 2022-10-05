using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum CardinalPoint { North, South, West, East }

public static class CardinalPointExtensions {

    public static Vector3Int GetVector3Int(this CardinalPoint cardinalPoint) {
        Vector3Int result = Vector3Int.zero;
        switch (cardinalPoint) {
            case CardinalPoint.North:
                result = Vector3Int.forward;
                break;
            case CardinalPoint.South:
                result = Vector3Int.back;
                break;
            case CardinalPoint.West:
                result = Vector3Int.left;
                break;
            case CardinalPoint.East:
                result = Vector3Int.right;
                break;
        }
        return result;
    }

    public static Vector3 GetVector3(this CardinalPoint cardinalPoint) {
        return cardinalPoint.GetVector3Int();
    }

    public static CardinalPoint IsComparedTo(this Vector3 origin, Vector3 destination) {
        Vector3 difference = origin - destination;

        CardinalPoint result = CardinalPoint.North;

        if (Mathf.Abs(difference.x) > Mathf.Abs(difference.z)) {
            result = difference.x < 0 ? CardinalPoint.West : CardinalPoint.East;
        } else {
            result = difference.z > 0 ? CardinalPoint.North : CardinalPoint.South;
        }

        return result;
    }
    public static List<CardinalPoint> CouldBeComparedTo(this Vector3 origin, Vector3 destination) {
        Vector3 difference = origin - destination;

        List<CardinalPoint> result = new List<CardinalPoint>();

        //Debug.Log(difference.x + ">=" + difference.z + " = " + (Mathf.Abs(difference.x) >= Mathf.Abs(difference.z)) + " | " + difference.x + "<=" + difference.z + " = " + (Mathf.Abs(difference.x) <= Mathf.Abs(difference.z)));
        
        if (Mathf.Abs(difference.x) >= Mathf.Abs(difference.z)) {
            CardinalPoint potentialCardinalPoint = difference.x < 0 ? CardinalPoint.West : CardinalPoint.East;
            result.Add(potentialCardinalPoint);
        }
        if (Mathf.Abs(difference.x) <= Mathf.Abs(difference.z) || difference.x == -difference.z) {
            CardinalPoint potentialCardinalPoint = difference.z > 0 ? CardinalPoint.North : CardinalPoint.South;
            result.Add(potentialCardinalPoint);
        }

        return result;
    }

    public static CardinalPoint GetOpposite(this CardinalPoint cardinalPoint) {
        CardinalPoint result = CardinalPoint.North;
        switch (cardinalPoint) {
            case CardinalPoint.North:
                result = CardinalPoint.South;
                break;
            case CardinalPoint.South:
                result = CardinalPoint.North;
                break;
            case CardinalPoint.West:
                result = CardinalPoint.East;
                break;
            case CardinalPoint.East:
                result = CardinalPoint.West;
                break;
        }
        return result;
    }


}