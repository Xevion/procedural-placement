using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(PointRendering))]
public class SamplingEditor : Editor 
{
    public override void OnInspectorGUI()
    {
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
        
        PointRendering pr = (PointRendering) target;
        DrawDefaultInspector();

        EditorGUILayout.HelpBox("This is a help box", MessageType.Info);
    }
}
