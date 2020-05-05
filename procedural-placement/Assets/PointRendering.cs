using System;
using System.Collections.Generic;
using System.Net;
using UnityEditor;
using UnityEngine;
using UnityEngine.Serialization;

public enum SamplingTypes {
    Random,
    Poisson
};

public class PointRendering : MonoBehaviour {
    [Tooltip("The sampling method used to generate points")]
    public SamplingTypes samplingMethod = SamplingTypes.Random;
    [Tooltip("The number of points (spheres) placed inside the region")]
    public int numPoints = 100;
    [Tooltip("The size of the rendered spheres")]
    public float sphereSize = 1;
    [Tooltip("The Size of the region points are generated in")]
    public Vector2 regionSize = Vector2.one;

    // Store the previous value, only useful for the inspector
    private SamplingTypes _samplingMethod;
    private int _prevNumPoints;
    private Vector2 _prevRegionSize;

    private List<Vector2> _points;
    
    private void OnValidate() {
        // Check to see if point position related values changed. CubeSize is rendering only, so we ignore it.
        if (numPoints != _prevNumPoints || _prevRegionSize != regionSize
            || samplingMethod != _samplingMethod) {
            // Update the tracking values
            _prevNumPoints = numPoints;
            _prevRegionSize = regionSize;
            _samplingMethod = samplingMethod;

            switch (samplingMethod) {
                case SamplingTypes.Random:
                    _points = PointGeneration.random_sampling(numPoints, regionSize);
                    break;
                case SamplingTypes.Poisson:
                    _points = new List<Vector2>();
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }
    }

    private void OnDrawGizmos() {
        // Draw a wireframe around the entire region
        Gizmos.DrawWireCube(this.transform.position,
            new Vector3(regionSize.x * 2 + sphereSize, regionSize.y * 2 + sphereSize, sphereSize));

        // Render spheres at every point
        if (_points == null) return;
        var pos = transform.position;
        foreach (var point in _points)
            Gizmos.DrawSphere(new Vector3(point.x + pos.x, point.y + pos.y, pos.z), sphereSize);
    }
}