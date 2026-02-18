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

    public GameObject decalPrefab;
    
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
        AddDecals();
        AddCheckpoints();
        
        
        
        
        
    }

    private void GenerateTrackSpriteShape(Spline spline)
    {
        Transform splineTransform = splineContainer.transform;
        Transform shapeTransform = spriteShape.transform;
        
        float trackWidth = 1f;

        // Generate the SpriteShape so that the track is rendered
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
            
            if (splineContainer.Spline.TryGetFloatData("TrackWidth", out var trackWidthData)) 
            {
                foreach (var data in trackWidthData) 
                {
                    if (Mathf.Approximately(data.Index, i)) 
                    {
                        trackWidth = data.Value;
                        break;
                    }
                }
            }
            
            spriteShape.spline.InsertPointAt(i, localKnotPos);
            spriteShape.spline.SetTangentMode(i, ShapeTangentMode.Continuous);
            spriteShape.spline.SetLeftTangent(i, tanIn);
            spriteShape.spline.SetRightTangent(i, tanOut);
            // spriteShape.spline.SetHeight(i, trackWidth);
            
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

    private void AddDecals()
    {
        
        
        splineContainer.Spline.TryGetObjectData("Decals", out var decals);

        foreach (var decal in decals)
        {
            GameObject decalObject = Instantiate(decalPrefab, transform);
            decalObject.GetComponent<SpriteRenderer>().sprite = (Sprite)decal.Value;
            decalObject.transform.position = splineContainer.transform.TransformPoint(splineContainer.Spline.EvaluatePosition(decal.Index));
        }
    }

    private void AddCheckpoints()
    {
        
    }

    private void Update()
    {
    }
}
