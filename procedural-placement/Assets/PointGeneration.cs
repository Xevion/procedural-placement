using System.Collections.Generic;
using UnityEngine;

public static class PointGeneration {
    public static List<Vector2> random_sampling(int numPoints, Vector2 regionSize) {
        // Create a new empty list of points, and add some randomly
        var points = new List<Vector2>();
        for (var i = 0; i < numPoints; i++) {
            points.Add(
                new Vector2(
                    Random.Range(-regionSize.x, regionSize.x),
                    Random.Range(-regionSize.y, regionSize.y)));
        }

        return points;
    }

    public static List<Vector2> poisson_sampling(int numPoints, Vector2 regionSize, int radius) {
        float cellSize = radius / Mathf.Sqrt(2);

        int[,] grid = new int[Mathf.CeilToInt(regionSize.x / cellSize), Mathf.CeilToInt(regionSize.y / cellSize)];
        List<Vector2> points = new List<Vector2>();
        List<Vector2> spawnPoints = new List<Vector2>();
        
        spawnPoints.Add(regionSize / 2);
        while (spawnPoints.Count > 0) {
            int spawnIndex = Random.Range(0, spawnPoints.Count);
            Vector2 spawnCenter = spawnPoints[spawnIndex];
        }

        return points;
    }
}