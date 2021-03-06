﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class MovementIndicatorScript : MonoBehaviour {


    public float
        ViewRadius;
    public LayerMask 
        targetMask;
    public LayerMask
        obstacleMask;
    public float
        MeshResolution,
        edgeDstThresh;
    public MeshFilter
        viewMeshFilter;
    private Mesh
        ViewMesh;
    public int
        edgeResolveIterations;
    public float
        maskCutAway = 0.2f;
    public List<Vector3>
        ViewPointRevealed = new List<Vector3>();
    private CharacterScript character;

    private void Start()
    {
        character = gameObject.transform.root.gameObject.GetComponent<CharacterScript>();
        gameObject.transform.parent.GetComponent<CharacterScript>().MovementIndicator = gameObject.transform;
        ViewMesh = new Mesh();
        ViewMesh.name = "ViewMesh";
        viewMeshFilter = gameObject.GetComponent<MeshFilter>();
        viewMeshFilter.mesh = ViewMesh;
        obstacleMask = 1 << 9;
        edgeResolveIterations = 4;
    }

    
    private void LateUpdate()
    {
        if (gameObject.transform.root.gameObject == CameraScript.GameController.ActivePlayer && CameraScript.GameController.PlayerTurn)
        {
            ViewRadius = character.RemainingMovement;
            DrawFieldOfView();
            gameObject.transform.localScale = Vector3.one;
        }
        else gameObject.transform.localScale = Vector3.zero;
    }
    void DrawFieldOfView()
    {
        int StepCount = Mathf.RoundToInt(360 / 4f);
        List<Vector3> viewPoints = new List<Vector3>();
        ViewCastInfo OldViewCastInfo = new ViewCastInfo();
        for (int i = 0; i <= StepCount; i++)
        {
            float angle = transform.eulerAngles.y + 4f * i;
            ViewCastInfo newViewCastInfo = Viewcast(angle);
            
            if (i > 0)
            {
                bool edgeDstThresholdExceeded = (Mathf.Abs(OldViewCastInfo.dst - newViewCastInfo.dst) > 1);
                if (OldViewCastInfo.hit != newViewCastInfo.hit || (OldViewCastInfo.hit && newViewCastInfo.hit && edgeDstThresholdExceeded))
                {
                    EdgeInfo edge = new EdgeInfo();


                    edge = FindEdge(OldViewCastInfo, newViewCastInfo);

                    viewPoints.Add(edge.PointA);
                    viewPoints.Add(edge.PointB);
                }
            }
            viewPoints.Add(newViewCastInfo.point);
            OldViewCastInfo = newViewCastInfo;
        }
        ViewPointRevealed = viewPoints;
        int vertexCount = viewPoints.Count + 1;
        Vector3[] vertices = new Vector3[vertexCount];
        int[] triangles = new int[((vertexCount - 2) * 3) + 3];
        vertices[0] = Vector3.zero;
        for (int i = 0; i < vertices.Length - 1; i++)
        {
            vertices[i + 1] = transform.InverseTransformPoint(viewPoints[i]);

            if (i < vertexCount - 2)
            {
                triangles[i * 3] = 0;
                triangles[i * 3 + 1] = i + 1;
                triangles[i * 3 + 2] = i + 2;
            }
        }

        triangles[triangles.Length - 3] = 0;
        triangles[triangles.Length - 2] = 1;
        triangles[triangles.Length - 1] = vertices.Length - 1;

        ViewMesh.Clear();
        ViewMesh.vertices = vertices;
        ViewMesh.triangles = triangles;
        ViewMesh.RecalculateNormals();
    }


    EdgeInfo FindEdge(ViewCastInfo min, ViewCastInfo max)
    {
        float minAngle = min.angle;
        float maxAngle = max.angle;

        Vector3 minPoint = min.point;
        Vector3 maxPoint = max.point;
        
        for(int i = 0; i< edgeResolveIterations; i++)
        {
            float angle = (minAngle + maxAngle) / 2;
            ViewCastInfo newViewCast = Viewcast(angle);

            bool edgeDstThresholdExceeded = (Mathf.Abs(min.dst - newViewCast.dst) > 1);

            if (newViewCast.hit == min.hit&& !edgeDstThresholdExceeded)
            {
                minAngle = angle;
                minPoint = newViewCast.point;
            }
            else
            {
                maxAngle = angle;
                maxPoint = newViewCast.point; 
            }
        }
        return new EdgeInfo(minPoint, maxPoint);
    }

    
    ViewCastInfo Viewcast(float globalAngle)
    {
        Vector3 dir = DirectionFromAngle(globalAngle, true);
        RaycastHit hit;
        if (Physics.Raycast(transform.position,dir,out hit, ViewRadius, obstacleMask))
        {
            return new ViewCastInfo(true, hit.point, hit.distance, globalAngle);
        }
        else
        {
            return new ViewCastInfo(false, transform.position + dir * ViewRadius, ViewRadius, globalAngle);
        }
    }
    public Vector3 DirectionFromAngle(float angleInDegrees, bool isGlobalAngle)
    {
        if (!isGlobalAngle)
        {
            angleInDegrees += transform.eulerAngles.y;
        }
        return new Vector3(Mathf.Sin(angleInDegrees * Mathf.Deg2Rad), 0, Mathf.Cos(angleInDegrees * Mathf.Deg2Rad));
    }
    struct ViewCastInfo
    {
        public bool hit;
        public Vector3 point;
        public float dst;
        public float angle;

        public ViewCastInfo(bool _hit, Vector3 _point, float _dst, float _angle)
        {
            hit = _hit;
            point = _point;
            dst = _dst;
            angle = _angle;
        }

    }

    public struct EdgeInfo
    {
        public Vector3
            PointA,
            PointB;
        public EdgeInfo(Vector3 _pointA, Vector3 _pointB)
        {
            PointA = _pointA;
            PointB = _pointB;
        }
    }
}
