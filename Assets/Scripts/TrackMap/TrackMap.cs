using UnityEngine;
using UnityEngine.Splines;

[RequireComponent(typeof(LineRenderer))]
public class MinimapTrackFollower : MonoBehaviour
{
    public SplineContainer splineContainer;
    private LineRenderer lineRenderer;
    public int resolution = 100;

    void Start()
    {
        lineRenderer = GetComponent<LineRenderer>();
        DrawMapLine();
    }

    void DrawMapLine()
    {
        if (splineContainer == null) return;

        lineRenderer.positionCount = resolution;

        for (int i = 0; i < resolution; i++)
        {
            float t = i / (float)(resolution - 1);
            // Position along spline
            Vector3 position = splineContainer.EvaluatePosition(t);
            lineRenderer.SetPosition(i, position);
        }

        // Make the line loop 
        lineRenderer.loop = true;
    }
}