using Codice.Client.Common.WebApi.Responses;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Splines;
using UnityEngine.U2D;

[CustomEditor(typeof(TrackManager))]
public class TrackEditor : Editor
{
    #region  FOLDOUTS
    bool defaultInspectorFoldout = false;
    bool decalsFoldout = false;
    bool objectsFoldout = false;
    bool checkpointsFoldout = false;
    #endregion

    #region FEILDS
    float decalRatio = 1.0f;
    float decalRotation = 0f;
    Sprite decalSprite;

    float objectRatio = 1.0f;
    float objectRotation = 0f;
    GameObject objectPrefab;

    #endregion

    public override void OnInspectorGUI()
    {
        TrackManager trackManager = (TrackManager)target;

        if (GUILayout.Button("Update Track"))
        {
            trackManager.BakeTrack();
        }
        
        
        
        
        decalsFoldout = EditorGUILayout.Foldout(decalsFoldout, "Decals");
        if (decalsFoldout)
        {
            trackManager.splineContainer.Spline.TryGetObjectData("Objects", out var objects);
            decalRatio = EditorGUILayout.Slider("Time", decalRatio, 0f, 1f);
            decalRotation = EditorGUILayout.FloatField("Rotation", decalRotation);

            decalSprite = (Sprite)EditorGUILayout.ObjectField(decalSprite, typeof(Sprite), false);
            
            if (GUILayout.Button("Add Decal"))
            {
                GameObject decal = new GameObject($"Decal_{decalSprite.name}");
                SpriteRenderer sr = decal.AddComponent<SpriteRenderer>();
                sr.sprite = decalSprite;
                decal.transform.rotation = Quaternion.Euler(0f, 0f, decalRotation);
                objects.Add(new DataPoint<Object>(decalRatio, decal));
                trackManager.BakeTrack();
            }
        }

        objectsFoldout = EditorGUILayout.Foldout(objectsFoldout, "Powerups");
        if (objectsFoldout)
        {
            objectRatio = EditorGUILayout.Slider("Time", objectRatio, 0f, 1f);
            objectRotation = EditorGUILayout.FloatField("Rotation", objectRotation);

            objectPrefab = (GameObject)EditorGUILayout.ObjectField(objectPrefab, typeof(GameObject), false);
            
            if (GUILayout.Button("Add Powerup"))
            {
                trackManager.splineContainer.Spline.TryGetObjectData("Objects", out var objects);
                objects.Add(new DataPoint<Object>(objectRatio, objectPrefab));
                trackManager.BakeTrack();
            }
        }
        

        defaultInspectorFoldout = EditorGUILayout.Foldout(defaultInspectorFoldout, "Track Manager");
        if (defaultInspectorFoldout)
        {
            DrawDefaultInspector();
        }
        

    }
    
    
}
