using Codice.Client.Common.WebApi.Responses;
using UnityEditor;
using UnityEngine;
using UnityEngine.Splines;

[CustomEditor(typeof(TrackManager))]
public class TrackEditor : Editor
{
    
    bool basicFoldout = true;
    bool decalsFoldout = true;
    
    float t = 1.0f;
    Sprite decalSprite;
    
    public override void OnInspectorGUI()
    {
        TrackManager trackManager = (TrackManager)target;

        if (GUILayout.Button("Update Track"))
        {
            trackManager.BakeTrack();
        }
        
        
        basicFoldout = EditorGUILayout.Foldout(basicFoldout, "Track Manager");
        if (basicFoldout)
        {
            DrawDefaultInspector();
        }
        
        trackManager.splineContainer.Spline.TryGetObjectData("Decals", out var decals);
        
        decalsFoldout = EditorGUILayout.Foldout(decalsFoldout, "Decals");
        if (decalsFoldout)
        {
            EditorGUILayout.BeginHorizontal();
            t = EditorGUILayout.Slider("Time", t, 0f, 1f);
            EditorGUILayout.EndHorizontal();
            decalSprite = (Sprite)EditorGUILayout.ObjectField(decalSprite, typeof(Sprite), false);
            
            if (GUILayout.Button("Add Decal"))
            {
                decals.Add(new DataPoint<Object>(t, decalSprite));
                trackManager.BakeTrack();
            }
        }
        
    }
    
    
}
