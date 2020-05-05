﻿using System;
using System.Collections.Generic;
using System.Net;
using UnityEditor;
using UnityEngine;

public enum SamplingTypes {
    Random,
    Poisson
};

public class CubeRendering : MonoBehaviour {
    [Tooltip("The sampling method used to generate points")]
    public SamplingTypes samplingMethod = SamplingTypes.Random;
    [Tooltip("The number of points (spheres) placed inside the region")]
    public int numPoints = 100;
    [Tooltip("The size of the rendered cubes")]
    public float cubeSize = 1;

    [Tooltip("The Size of the region points are generated in")]
    public Vector2 regionSize = Vector2.one;

    // Store the previous value, only useful for the inspector
    private SamplingTypes _samplingMethod;
    private int _prevNumPoints;
    private Vector2 _prevRegionSize;

    private List<Vector2> _points;

    void OnValidate() {
        // Check to see if point position related values changed. CubeSize is rendering only, so we ignore it.
        if (numPoints != _prevNumPoints || _prevRegionSize != regionSize) {
            // Update the tracking values
            _prevNumPoints = numPoints;
            _prevRegionSize = regionSize;

            switch (samplingMethod) {
                case SamplingTypes.Random:
                    _points = PointGeneration.random_sampling(numPoints, regionSize, transform.position);
                    break;
                case SamplingTypes.Poisson:
                    _points = new List<Vector2>();
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }
    }

    void OnDrawGizmos() {
        // Draw a wireframe around the entire region
        Gizmos.DrawWireCube(this.transform.position,
            new Vector3(regionSize.x * 2 + cubeSize, regionSize.y * 2 + cubeSize, cubeSize));

        // Render spheres at every point
        if (_points == null) return;
        foreach (var point in _points) {
            Gizmos.DrawSphere(point, cubeSize);
        }
    }
}