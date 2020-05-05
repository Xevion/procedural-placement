using System.Collections.Generic;
using UnityEngine;

public class PointGeneration {
    public static List<Vector2> random_sampling(int numPoints, Vector2 range, Vector2 offset) {
        // Create a new empty list of points, and add some randomly
        List<Vector2> points = new List<Vector2>();
        for (int i = 0; i < numPoints; i++) {
            points.Add(
                new Vector2(
                    Random.Range(-range.x, range.x),
                    Random.Range(-range.y, range.y)));
        }

        return points;
    }
}