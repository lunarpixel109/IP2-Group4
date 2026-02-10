using System;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.Splines;
using UnityEngine.U2D;
using Spline = UnityEngine.Splines.Spline;

[ExecuteAlways]
public class TrackRenderer : MonoBehaviour {
    
    [Tooltip("The layout of the track that is used to create a track")] public SpriteShapeController spriteShape;
    [FormerlySerializedAs("spline")] [Tooltip("The centreline of the track")] public SplineContainer splineContainer;


    private void Start()
    {
        spriteShape = GetComponentInChildren<SpriteShapeController>();
        splineContainer = GetComponentInChildren<SplineContainer>();
    }
    
    [ContextMenu("Generate Track Visuals")]
    private void ConvertTrack()
    {
        Spline spline = splineContainer.Spline;
        
        Transform splineTransform = splineContainer.transform;
        Transform shapeTransform = spriteShape.transform;
        
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
            
            spriteShape.spline.InsertPointAt(i, localKnotPos);
            spriteShape.spline.SetTangentMode(i, ShapeTangentMode.Continuous);
            spriteShape.spline.SetLeftTangent(i, tanIn);
            spriteShape.spline.SetRightTangent(i, tanOut);
            
            
        }

        spriteShape.spline.isOpenEnded = false;
        spriteShape.splineDetail = 256;
        spriteShape.RefreshSpriteShape();
    }


    private void Update()
    {
        for (int i = 0; i < spriteShape.spline.GetPointCount(); i++)
        {
            Debug.Log($"SS{i} LEFT: {spriteShape.spline.GetLeftTangent(i)} RIGHT: {spriteShape.spline.GetRightTangent(i)}"); ;
        }

        for (int i = 0; i < splineContainer.Spline.Count; i++)
        {
            Debug.Log($"KT{i} IN: {splineContainer.Spline[i].TangentIn} OUT: {splineContainer.Spline[i].TangentOut} ROT: {math.Euler(splineContainer.Spline[i].Rotation)}");
        }
        
    }


    private void OnDrawGizmos()
    {
        foreach (BezierKnot knot in splineContainer.Spline)
        {
            
        }
    }
}
