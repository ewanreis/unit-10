using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(BezierCollider2D))] 
public class BezierCollider2DEditor : Editor 
{
    BezierCollider2D bezierCollider;
    EdgeCollider2D edgeCollider;

    int lastPointsQuantity = 0;
    Vector2 lastFirstPoint = Vector2.zero;
    Vector2 lastHandlerFirstPoint = Vector2.zero;
    Vector2 lastSecondPoint = Vector2.zero;
    Vector2 lastHandlerSecondPoint = Vector2.zero;

    public override void OnInspectorGUI() 
    {
        bezierCollider = (BezierCollider2D) target;

        edgeCollider = bezierCollider.GetComponent<EdgeCollider2D>();

        if (edgeCollider.hideFlags != HideFlags.HideInInspector)
        {
            edgeCollider.hideFlags = HideFlags.HideInInspector;
        }

        if (edgeCollider != null)
        {
            bezierCollider.pointsQuantity = EditorGUILayout.IntField ("curve points",bezierCollider.pointsQuantity, GUILayout.MinWidth(100));
            bezierCollider.firstPoint = EditorGUILayout.Vector2Field ("first point",bezierCollider.firstPoint, GUILayout.MinWidth(100));
            bezierCollider.handlerFirstPoint = EditorGUILayout.Vector2Field ("handler first Point",bezierCollider.handlerFirstPoint, GUILayout.MinWidth(100));
            bezierCollider.secondPoint = EditorGUILayout.Vector2Field ("second point",bezierCollider.secondPoint, GUILayout.MinWidth(100));
            bezierCollider.handlerSecondPoint = EditorGUILayout.Vector2Field ("handler secondPoint",bezierCollider.handlerSecondPoint, GUILayout.MinWidth(100));

            EditorUtility.SetDirty(bezierCollider);

            if (bezierCollider.pointsQuantity > 0  && !bezierCollider.firstPoint.Equals(bezierCollider.secondPoint) &&
                (
                    lastPointsQuantity != bezierCollider.pointsQuantity ||
                    lastFirstPoint != bezierCollider.firstPoint ||
                    lastHandlerFirstPoint != bezierCollider.handlerFirstPoint ||
                    lastSecondPoint != bezierCollider.secondPoint ||
                    lastHandlerSecondPoint != bezierCollider.handlerSecondPoint
                ))
            {
                lastPointsQuantity = bezierCollider.pointsQuantity;
                lastFirstPoint = bezierCollider.firstPoint;
                lastHandlerFirstPoint = bezierCollider.handlerFirstPoint;
                lastSecondPoint = bezierCollider.secondPoint;
                lastHandlerSecondPoint = bezierCollider.handlerSecondPoint;
                edgeCollider.points = bezierCollider.calculate2DPoints();
            }

        }
    }

    void OnSceneGUI () 
    {
        if (bezierCollider != null)
        {
            Handles.color = Color.grey;

            Handles.DrawLine(bezierCollider.transform.position+(Vector3)bezierCollider.handlerFirstPoint,bezierCollider.transform.position+(Vector3)bezierCollider.firstPoint);
            Handles.DrawLine(bezierCollider.transform.position+(Vector3)bezierCollider.handlerSecondPoint,bezierCollider.transform.position+(Vector3)bezierCollider.secondPoint);

            var fmh_66_135_638019549433284734 = Quaternion.identity; bezierCollider.firstPoint = Handles.FreeMoveHandle(bezierCollider.transform.position+((Vector3)bezierCollider.firstPoint),0.04f*HandleUtility.GetHandleSize(bezierCollider.transform.position+((Vector3)bezierCollider.firstPoint)),Vector3.zero,Handles.DotHandleCap) - bezierCollider.transform.position;
            var fmh_67_137_638019549433310414 = Quaternion.identity; bezierCollider.secondPoint = Handles.FreeMoveHandle(bezierCollider.transform.position+((Vector3)bezierCollider.secondPoint),0.04f*HandleUtility.GetHandleSize(bezierCollider.transform.position+((Vector3)bezierCollider.secondPoint)),Vector3.zero,Handles.DotHandleCap) - bezierCollider.transform.position;
            var fmh_68_149_638019549433315505 = Quaternion.identity; bezierCollider.handlerFirstPoint = Handles.FreeMoveHandle(bezierCollider.transform.position+((Vector3)bezierCollider.handlerFirstPoint),0.04f*HandleUtility.GetHandleSize(bezierCollider.transform.position+((Vector3)bezierCollider.handlerFirstPoint)),Vector3.zero,Handles.DotHandleCap) - bezierCollider.transform.position;
            var fmh_69_151_638019549433320299 = Quaternion.identity; bezierCollider.handlerSecondPoint = Handles.FreeMoveHandle(bezierCollider.transform.position+((Vector3)bezierCollider.handlerSecondPoint),0.04f*HandleUtility.GetHandleSize(bezierCollider.transform.position+((Vector3)bezierCollider.handlerSecondPoint)),Vector3.zero,Handles.DotHandleCap) - bezierCollider.transform.position;

            if (GUI.changed)
            {
                EditorUtility.SetDirty (target);
            }
        }
    }

}