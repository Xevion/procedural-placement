using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

// The Point Rendering Editor must allow simple and easy adjustments of the points
// allowing complete control over when and how points are generated.
// - Point Generation can be turned off and be done manually (or automatically, when changes are detected)
// - Sampling Methods can change, and different fields will be visible depending on the active sampling method.
//     - 'Random' requires no 'Radius' or 'Retry Attempts' field
//     - 'Poisson' and 'Improved Poisson' require no 'Num Points' field (but they should have a text area)
// - A 'seed' field should be given, a checkbox for seed auto re-picking (edits of other fields can cause said seed to be randomized,
//   allowing one to observe the effect of different sampling methods or arguments with the same seed being used.)
// - A simple text field showing how many points were generated and how long it took to do so.
// - Limits for negative values
// - Safeguards for dangerous/laggy values, possibly?

// Attributes
// Random Seed
// Sampling Method
// Number of Points (Only for 'Random' Sampling Method)
// Sphere Display Size
// Point Minimum Radius (Only for 'Poisson' and 'Improved Poisson' Sampling Methods)
// Point Retry Attempts (Only for 'Poisson' and 'Improved Poisson' Sampling Methods)
// Autogenerate?
// Draw Radius?
// Draw Parent Lines? (Only for 'Poisson' and 'Improved Poisson' Sampling Methods)
// Randomize Seed?
// Choose Point Sphere Radius?
// Generate Button
// Generation Time / Point Count

[CustomEditor(typeof(PointRendering))]
public class SamplingEditor : Editor {
    private string _message;

    public override void OnInspectorGUI() {
        PointRendering points = (PointRendering) target;

        EditorGUI.BeginChangeCheck();
        points.samplingMethod = (SamplingTypes) EditorGUILayout.EnumPopup("Sampling Method", points.samplingMethod);
        points.seed = EditorGUILayout.TextField("Seed Value", points.seed);
        // Simple checks for point regeneration and point
        
        if (points.samplingMethod == SamplingTypes.Random)
            points.numPoints = EditorGUILayout.IntField("Number of Points", points.numPoints);
        else {
            points.radius = EditorGUILayout.Slider("Point Minimum Radius", points.radius, 0.1f, 50f);
            points.retryAttempts = EditorGUILayout.IntSlider("Point Retry Attempts", points.retryAttempts, 1, 500);
        }
        bool regenerate = EditorGUI.EndChangeCheck();

        // Check if redraw is needed
        EditorGUI.BeginChangeCheck();
        points.sphereSize = EditorGUILayout.FloatField("Sphere Display Size", points.sphereSize);
        bool redraw = EditorGUI.EndChangeCheck();

        // Handling for different types of point sampling methods
        if (regenerate) {
            float t1 = Time.realtimeSinceStartup;
            points.regeneratePoints();
            float t2 = Time.realtimeSinceStartup;
            this._message = $"{points.points.Count:n0} points in {t2 - t1:0.0000}s";
        }

        // Display timings in Editor GUI
        EditorGUILayout.HelpBox(_message, MessageType.Info);

        if (regenerate || redraw)
            EditorUtility.SetDirty(target); // Redraw Gizmos
    }
}