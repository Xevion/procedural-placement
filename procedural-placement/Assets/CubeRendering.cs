using System.Collections.Generic;
using System.Net;
using UnityEditor;
using UnityEngine;

public enum SamplingTypes {
    Random,
    Poisson
};

public class CubeRendering : MonoBehaviour {
    public SamplingTypes samplingMethod = SamplingTypes.Random;
    public int numPoints = 100;
    public float cubeSize = 1;

    public Vector2 regionSize = Vector2.one;

    // Store the previous value, only useful for the inspector
    [Tooltip("Testing.")] private int _prevNumPoints;
    private Vector2 _prevRegionSize;

    private List<Vector2> _points;

    void OnValidate() {
        // Check to see if point position related values changed. CubeSize is rendering only, so we ignore it.
        if (numPoints != _prevNumPoints || _prevRegionSize != regionSize) {
            // Update the tracking values
            _prevNumPoints = numPoints;
            _prevRegionSize = regionSize;

            if (samplingMethod == SamplingTypes.Random)
                _points = PointGeneration.random_sampling(numPoints, regionSize, transform.position);
            else if (samplingMethod == SamplingTypes.Poisson) {
                _points = new List<Vector2>();
            }
        }
    }

    void OnDrawGizmos() {
        // Draw a wireframe around the entire region
        Gizmos.DrawWireCube(this.transform.position,
            new Vector3(regionSize.x * 2 + cubeSize, regionSize.y * 2 + cubeSize, cubeSize));

        // Render spheres at every point
        if (_points != null)
            foreach (Vector2 point in _points) {
                Gizmos.DrawSphere(point, cubeSize);
            }
    }
}