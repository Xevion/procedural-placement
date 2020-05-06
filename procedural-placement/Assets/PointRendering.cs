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
    [Tooltip("The sampling method used to generate points")]
    public SamplingTypes samplingMethod = SamplingTypes.Random;
    [Tooltip("The number of points (spheres) placed inside the region")]
    public int numPoints = 100;
    [Tooltip("The size of the rendered spheres")]
    public float sphereSize = 1;
    [Tooltip("The Size of the region points are generated in")]
    public Vector2 regionSize = Vector2.one;
    [Tooltip("The radius between each point in Poisson disc sampling")]
    public float radius = 3;
    [Tooltip("The number of attempts the sampling algorithm will give to place another point")]
    public int retryAttempts = 30;
    [Tooltip("Draw a wireframe sphere showing the radius between each sphere.")]
    public bool drawSphere = false;

    // Store the previous value, only useful for the inspector
    private SamplingTypes _samplingMethod;
    private int _prevNumPoints;
    private Vector2 _prevRegionSize;
    private float _radius;
    private int _retryAttempts;

    public List<Vector2> points;
    
    public void OnValidate() {
        Debug.Log("Firing onValidate!");
        // Check to see if point position related values changed. SphereSize is rendering only, so we ignore it.
        if (numPoints != _prevNumPoints || _prevRegionSize != regionSize
            || samplingMethod != _samplingMethod || radius != _radius || retryAttempts != _retryAttempts) {
            // Update the property tracking values
            _prevNumPoints = numPoints;
            _prevRegionSize = regionSize;
            _samplingMethod = samplingMethod;
            _radius = radius;
            _retryAttempts = retryAttempts;
        }
    }

    public void regeneratePoints() {
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