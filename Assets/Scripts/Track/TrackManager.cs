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
    
    
    [FormerlySerializedAs("spline")] [Tooltip("The centreline of the track")] public SplineContainer splineContainer;
    [Space]
    [Tooltip("The layout of the track that is used to create a track")] public SpriteShapeController spriteShape;
    [Header("Track Colliders")]
    public float colliderResolution = 0.5f;
    public EdgeCollider2D innerCollider;
    public PolygonCollider2D outerCollider;
    
    private void Start()
    {
        spriteShape = GetComponentInChildren<SpriteShapeController>();
        splineContainer = GetComponentInChildren<SplineContainer>();
    }
    
    [ContextMenu("Generate Track Visuals")]
    public void BakeTrack()
    {
        Spline spline = splineContainer.Spline;
        
        
        GenerateTrackSpriteShape(spline);
        GenerateTrackCollider();
        AddObjects();
        
    }

    private void GenerateTrackSpriteShape(Spline spline)
    {
        // Get a reference to the transforms of the spline and the sprite shape for easier calculations
        Transform splineTransform = splineContainer.transform;
        Transform shapeTransform = spriteShape.transform;
        
        float trackWidth = 1f;

        // Generate the SpriteShape so that the track is rendered
        spriteShape.spline.Clear();
        for (int i = 0; i < spline.Count; i++)
        {
            
            BezierKnot knot = spline[i];
            
            // Transform the position of the knot to local space of the sprite shape
            Vector3 localKnotPos = shapeTransform.InverseTransformPoint(splineTransform.TransformPoint(knot.Position));
            

            Vector3 tanIn = Vector3.zero;
            Vector3 tanOut = Vector3.zero;
            
            // Get Tangent out using bezier curve formula, the tangent out is the vector from P0 to P1
            BezierCurve curve = spline.GetCurve(i);

            tanOut = curve.P1 - curve.P0;
            
            // Get Tangent in using bezier curve formula, the tangent in is the vector from P2 to P3
            int prevIndex = (i == 0) ? spline.Count - 1 : i - 1;
            curve = spline.GetCurve(prevIndex);
            tanIn = curve.P2 - curve.P3;
            
            // Transform the tangents to world space
            Vector3 worldTanOut = splineTransform.TransformPoint(tanOut);
            Vector3 worldTanIn = splineTransform.TransformPoint(tanIn);
            
            
            spriteShape.spline.InsertPointAt(i, localKnotPos);
            spriteShape.spline.SetTangentMode(i, ShapeTangentMode.Continuous);
            spriteShape.spline.SetLeftTangent(i, tanIn);
            spriteShape.spline.SetRightTangent(i, tanOut);
            
        }

        spriteShape.spline.isOpenEnded = false;
        spriteShape.splineDetail = 256;
        spriteShape.RefreshSpriteShape();
    }

    private void GenerateTrackCollider()
    {
        
        // Generate the colliders so that the track can be interacted with
        List<Vector2> innerPoints = new List<Vector2>(); 
        List<Vector2> outerPoints = new List<Vector2>();
        for (float t = 0; t < 1f; t += colliderResolution)
        {
            Vector3 point = splineContainer.Spline.EvaluatePosition(t);
            Vector3 tangent = splineContainer.Spline.EvaluateTangent(t);
            
            Vector2 perpTangentNorm = new Vector2(tangent.y, -tangent.x).normalized ;
            
            Vector2 innerPoint = (Vector2)point - (perpTangentNorm * 0.5f);
            Vector2 outerPoint = (Vector2)point + (perpTangentNorm * 0.5f);
            
            innerPoints.Add(innerPoint);
            outerPoints.Add(outerPoint);
            
        }
        
        innerCollider.SetPoints(innerPoints);
        outerCollider.SetPath(0, outerPoints);
    }

    private void AddObjects()
    {
        
        
        if (!splineContainer.Spline.TryGetObjectData("Objects", out var objects)) return;

        foreach (var obj in objects)
        {
            GameObject objGameObject = obj.Value as GameObject;
            objGameObject.transform.parent = transform;
            objGameObject.transform.position = splineContainer.Spline.EvaluatePosition(obj.Index);
        }
    }

}
