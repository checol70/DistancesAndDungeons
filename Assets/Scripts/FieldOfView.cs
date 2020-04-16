using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class FieldOfView : MonoBehaviour {
    public float
        ViewRadius;
    [Range(0,364)]
    public float
        ViewAngle;
    public LayerMask 
        targetMask;
    public LayerMask
        WallsMask;
    public List<Transform> visibleTargets = new List<Transform>();
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

    private void Start()
    {
        ViewMesh = new Mesh();
        ViewMesh.name = "ViewMesh";
        viewMeshFilter.mesh = ViewMesh;
        StartCoroutine(FindTargetsWithDelay(.2f));
    }

    IEnumerator FindTargetsWithDelay(float Delay)
    {
        while (true)
        {
            yield return new WaitForSeconds(Delay);
            FindVisibleTargets();
        }
    }
    private void LateUpdate()
    {
        DrawFieldOfView();
    }
    void DrawFieldOfView()
    {
        int StepCount = Mathf.RoundToInt(ViewAngle * MeshResolution);
        float StepAngleSize = ViewAngle / StepCount;
        List<Vector3> viewPoints = new List<Vector3>();
        ViewCastInfo OldViewCastInfo= new ViewCastInfo();
        for (int i = 0; i <= StepCount; i++)
        {
            float angle = transform.eulerAngles.y - ViewAngle / 2 + StepAngleSize * i;
            ViewCastInfo newViewCastInfo = Viewcast(angle);
            
            if (i > 0)
            {
                bool edgeDstThresholdExceeded = (Mathf.Abs(OldViewCastInfo.dst - newViewCastInfo.dst)> edgeDstThresh);
                if(OldViewCastInfo.hit != newViewCastInfo.hit|| (OldViewCastInfo.hit&&newViewCastInfo.hit&& edgeDstThresholdExceeded))
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
        int vertexCount = viewPoints.Count - 1;
        Vector3[] vertices = new Vector3[vertexCount];
        int[] triangles = new int[(vertexCount - 2) * 3];
        vertices[0] = Vector3.zero;
        for (int i = 0; i < vertices.Length - 1; i++)
        {
            vertices[i + 1] = transform.InverseTransformPoint (viewPoints[i]);

            if (i < vertexCount - 2)
            {
                triangles[i * 3] = 0;
                triangles[i * 3 + 1] = i + 1;
                triangles[i * 3 + 2] = i + 2;
            }
        }
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

            bool edgeDstThresholdExceeded = (Mathf.Abs(min.dst - newViewCast.dst) > edgeDstThresh);

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
        Debug.Log("ending maxPoint " + maxPoint);
        Debug.Log("ending minPoint " + minPoint);
        return new EdgeInfo(minPoint, maxPoint);
    }

    void FindVisibleTargets()
    {
        visibleTargets.Clear();
        Collider[] targetsInViewRadius = Physics.OverlapSphere(transform.position, ViewRadius, targetMask);
        
        foreach (Collider target in targetsInViewRadius)
        {
            Vector3 dirToTarget = (target.transform.position - transform.position).normalized;
            if(Vector3.Angle(transform.forward, dirToTarget)< ViewAngle/2)
            {
                float dstToTarget = Vector3.Distance(transform.position, target.transform.position);
                if(!Physics.Raycast(transform.position, dirToTarget, dstToTarget, WallsMask))
                {
                    visibleTargets.Add(target.transform);
                }
            }
        }
    }
    ViewCastInfo Viewcast(float globalAngle)
    {
        Vector3 dir = DirectionFromAngle(globalAngle, true);
        RaycastHit hit;
        if (Physics.Raycast(transform.position,dir,out hit, ViewRadius, WallsMask))
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
