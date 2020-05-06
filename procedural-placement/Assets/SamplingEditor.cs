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
// Draw Sphere?
// Draw Parent Lines? (Only for 'Poisson' and 'Improved Poisson' Sampling Methods)
// Randomize Seed?
// Generate Button
// Generation Time / Point Count

[CustomEditor(typeof(PointRendering))]
public class SamplingEditor : Editor 
{
    public override void OnInspectorGUI()
    {

        PointRendering points = (PointRendering) target;
        DrawDefaultInspector();

        if(points.samplingMethod == SamplingTypes.Random)
        EditorGUILayout.HelpBox($"You are using the \"{points.samplingMethod}\" Sampling Method!", MessageType.Error);
    }
}
