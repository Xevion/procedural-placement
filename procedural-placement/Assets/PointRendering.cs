using System;
using System.Collections.Generic;
using UnityEngine;

public enum SamplingTypes {
    Random,
    Poisson,
    ImprovedPoisson
};

public class PointRendering : MonoBehaviour {
    public String seed = "";
    public SamplingTypes samplingMethod = SamplingTypes.Random;
    public int numPoints = 100;
    public float sphereSize = 1;
    public Vector2 regionSize = Vector2.one;
    public float radius = 3;
    public int retryAttempts = 30;
    public bool drawSphere;

    public List<Vector2> points;

    public void RegeneratePoints() {
        switch (samplingMethod) {
            case SamplingTypes.Random:
                points = PointGeneration.random_sampling(numPoints, regionSize);
                break;
            case SamplingTypes.Poisson:
                points = PointGeneration.poisson_sampling(numPoints, regionSize, radius, retryAttempts);
                numPoints = points.Count;
                break;
            case SamplingTypes.ImprovedPoisson:
                points = PointGeneration.improved_poisson_sampling(numPoints, regionSize, radius, retryAttempts);
                numPoints = points.Count;
                break;
            default:
                throw new ArgumentOutOfRangeException();
        }
    }

    private void OnDrawGizmos() {
        // Draw a wireframe around the entire region
        Gizmos.DrawWireCube(this.transform.position,
            new Vector3(regionSize.x + sphereSize, regionSize.y + sphereSize, sphereSize));

        // Render spheres at every point
        if (points == null) return;
        var pos = transform.position;
        foreach (var point in points) {
            var center = new Vector3(point.x + pos.x - (regionSize.x / 2), point.y + pos.y - (regionSize.y / 2));
            Gizmos.DrawSphere(center,
                sphereSize);
            if(drawSphere)
                Gizmos.DrawWireSphere(center, radius / 2);
        }
    }
}