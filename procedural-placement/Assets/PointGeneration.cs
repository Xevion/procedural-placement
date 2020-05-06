using System.Collections.Generic;
using NUnit.Framework.Constraints;
using UnityEngine;

public static class PointGeneration {
    public static List<Vector2> random_sampling(int numPoints, Vector2 regionSize) {
        // Create a new empty list of points, and add some randomly
        List<Vector2> points = new List<Vector2>();
        for (var i = 0; i < numPoints; i++) {
            points.Add(
                new Vector2(
                    Random.Range(0, regionSize.x * 2),
                    Random.Range(0, regionSize.y * 2)
            ));
        }

        return points;
    }

    public static List<Vector2> poisson_sampling(int numPoints, Vector2 regionSize, int radius, int attempts = 30) {
        float cellSize = radius / Mathf.Sqrt(2);

        // A grid for
        int[,] grid = new int[Mathf.CeilToInt(regionSize.x / cellSize), Mathf.CeilToInt(regionSize.y / cellSize)];
        List<Vector2> points = new List<Vector2>();
        List<Vector2> spawnPoints = new List<Vector2>();

        spawnPoints.Add(regionSize / 2);
        while (spawnPoints.Count > 0) {
            int spawnIndex = Random.Range(0, spawnPoints.Count);
            Vector2 spawnCenter = spawnPoints[spawnIndex];
            bool candidateAccepted = false;

            for (int i = 0; i < attempts; i++) {
                float angle = Random.value * Mathf.PI * 2; // Random radian angle
                Vector2 dir = new Vector2(Mathf.Sin(angle), Mathf.Cos(angle));
                Vector2 candidate = spawnCenter + dir * Random.Range(radius, radius * 2);

                if (poisson_valid(candidate, regionSize, cellSize, points, grid, radius)) {
                    points.Add(candidate);
                    spawnPoints.Add(candidate);
                    grid[(int) (candidate.x / cellSize), (int) (candidate.y / cellSize)] = points.Count;
                    candidateAccepted = true;
                    break;
                }
            }

            if (!candidateAccepted) {
                spawnPoints.RemoveAt(spawnIndex);
            }
        }

        return points;
    }

    private static bool IsValidPoint(Vector2 point, Vector2 region) {
        return point.x >= 0 && point.y >= 0 && point.x < region.x && point.y < region.y;
    }
    private static bool poisson_valid(Vector2 candidate, Vector2 regionSize, float cellSize, List<Vector2> points,
        int[,] grid, float radius) {
        // Check that the point is valid
        if (!IsValidPoint(candidate, regionSize))
            return false;

        int cellX = (int) (candidate.x / cellSize);
        int cellY = (int) (candidate.y / cellSize);
        int searchStartX = Mathf.Max(0, cellX - 2);
        int searchEndX = Mathf.Min(cellX + 2, grid.GetLength(0) - 1);
        int searchStartY = Mathf.Max(0, cellY - 2);
        int searchEndY = Mathf.Min(cellY + 2, grid.GetLength(1) - 1);

        for (int x = searchStartX; x <= searchEndX; x++) {
            for (int y = searchStartY; y <= searchEndY; y++) {
                int pointIndex = grid[x, y] - 1;
                 if (pointIndex == -1) continue;
                
                float sqrDst = (candidate - points[pointIndex]).sqrMagnitude;
                if (sqrDst < radius * radius) {
                    return false;
                }
            }
        }
        
        return true;
    }
}