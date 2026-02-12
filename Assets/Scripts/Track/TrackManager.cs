using System;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.Splines;
using UnityEngine.U2D;
using Spline = UnityEngine.Splines.Spline;

[ExecuteAlways]
public class TrackManager : MonoBehaviour {
    
    
    
    [Tooltip("The layout of the track that is used to create a track")] public SpriteShapeController spriteShape;
    [FormerlySerializedAs("spline")] [Tooltip("The centreline of the track")] public SplineContainer splineContainer;

    public float trackWidth = 1f;
    
    [Header("Colliders")] 
    public PolygonCollider2D innerCollider;
    public float colliderRes = 0.01f;
    
    
    private void Start()
    {
        spriteShape = GetComponentInChildren<SpriteShapeController>();
        splineContainer = GetComponentInChildren<SplineContainer>();
    }
    
    [ContextMenu("Generate Track Visuals")]
    private void GenerateTrack()
    {
        Spline spline = splineContainer.Spline;
        
        Transform splineTransform = splineContainer.transform;
        Transform shapeTransform = spriteShape.transform;
        
        float trackWidth = 1f;

        
        spriteShape.spline.Clear();
        for (int i = 0; i < spline.Count; i++)
        {
            
            BezierKnot knot = spline[i];
            
            Vector3 localKnotPos = shapeTransform.InverseTransformPoint(splineTransform.TransformPoint(knot.Position));
            
            Vector3 tanIn = Vector3.zero;
            Vector3 tanOut = Vector3.zero;
            
            // Get Tangent out
            BezierCurve curve = spline.GetCurve(i);
            tanOut = curve.P1 - curve.P0;
            
            int prevIndex = (i == 0) ? spline.Count - 1 : i - 1;
            curve = spline.GetCurve(prevIndex);
            tanIn = curve.P2 - curve.P3;
            
            Vector3 worldTanOut = splineTransform.TransformPoint(tanOut);
            Vector3 worldTanIn = splineTransform.TransformPoint(tanIn);
            
            // if (splineContainer.Spline.TryGetFloatData("TrackWidth", out var trackWidthData)) 
            // {
            //     foreach (var data in trackWidthData) 
            //     {
            //         if (Mathf.Approximately(data.Index, i)) 
            //         {
            //             trackWidth = data.Value;
            //             break;
            //         }
            //     }
            // }
            
            spriteShape.spline.InsertPointAt(i, localKnotPos);
            spriteShape.spline.SetTangentMode(i, ShapeTangentMode.Continuous);
            spriteShape.spline.SetLeftTangent(i, tanIn);
            spriteShape.spline.SetRightTangent(i, tanOut);
            //spriteShape.spline.SetHeight(i, trackWidth);
        }

        spriteShape.spline.isOpenEnded = false;
        spriteShape.splineDetail = 256;
        spriteShape.RefreshSpriteShape();
        
        // Generate Colliders
        List<Vector2> innerPoints = new List<Vector2>();

        for (float t = 0; t < 1f; t += colliderRes)
        {
            Vector3 point = spline.EvaluatePosition(t);
            Vector3 tangent = spline.EvaluateTangent(t);

            Vector3 innerPoint = ((point - Vector3.Cross(tangent, Vector3.up)) / 2) + Vector3.one;

            innerPoints.Add(innerPoint);

        }
        innerCollider.points = innerPoints.ToArray();
        
    }


    private void Update()
    {
      
        
    }


    private void OnDrawGizmos()
    {

    }
}
