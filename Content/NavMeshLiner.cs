using EasyButtons;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.InputSystem;
using UnityEngine.Splines;
using UnityEngine.EventSystems;

public class NavMeshLiner : MonoBehaviour, IActivatable
{
    public CharacterCombatManager agent;

    //public Transform target;

    public NavMeshPath path;

    public LineRenderer lineRenderer;

    public Transform navCircle;

    public List<Vector3> points;

    public AnimationCurve curve;

    public SplineContainer splineContainer;

    public int subdivisions;

    public Camera raycastCam;

    public float heightOffset;

    public float maxDistance = 10;

    public bool active = true;

    public bool isActive
    {
        get
        {
            return active;
        }
        set
        {
            active = value;

            if (active)
            {

            }
            else
            {
                PurgeAndClean();
            }
        }
    }

    //[Button]
    //public void TestNavPath()
    //{
    //    DrawNavPath(agent.CombatNavManager.agent, target.position);
    //}

    //public void DrawNavPath(NavMeshAgent agent, Vector3 target)
    //{
    //    path = agent.path;

    //    NavMeshHit targetLocation;

    //    var didHit = NavMesh.SamplePosition(target, out targetLocation, maxDistance, NavMesh.AllAreas);
            
    //    if (didHit)
    //    {
    //        if (agent.CalculatePath(targetLocation.position, path))
    //        {
    //            Spline _spline = splineContainer.Spline;

    //            _spline.Clear();

    //            for (int i = 0; i < path.corners.Length; i++)
    //            {
    //                var _inversePosition = splineContainer.transform.InverseTransformPoint(path.corners[i]);

    //                _inversePosition += (Vector3.up * heightOffset);

    //                var _knot = new BezierKnot(_inversePosition);

    //                _spline.Add(_knot, TangentMode.AutoSmooth);
    //            }

    //            var  _positions = (int)(subdivisions * (_spline.GetLength()/10)) + 4;

    //            lineRenderer.positionCount = _positions;

    //            for (int i = 0; i < _positions; i++)
    //            {
    //                float _t = 0;

    //                if (_positions > 0)
    //                {
    //                   _t = (1f / _positions) * i;
    //                }

    //                Debug.Log (_t);

    //                Vector3 _position = splineContainer.EvaluatePosition(0, _t);

    //                lineRenderer.SetPosition(i, _position + (Vector3.up * heightOffset));
    //            }

    //            navCircle.position = (path.corners[path.corners.Length-1] + (Vector3.up * heightOffset));
    //        }
    //        else
    //        {
    //            Debug.Log("No Path Found");
    //        }
    //    }
    //    else
    //    {
    //        Debug.Log("UI blocked Raycast or no valid position was found");
    //    }
    //}

    //Method for drawing a nav path, provided only the path in question.
    public void DrawNavPath(NavMeshPath path)
    {
        if (path == null || path.corners.Length == 0)
        {
            return;
        }

        navCircle.gameObject.SetActive(true);

        Spline _spline = splineContainer.Spline;

        _spline.Clear();

        for (int i = 0; i < path.corners.Length; i++)
        {
            var _inversePosition = splineContainer.transform.InverseTransformPoint(path.corners[i]);

            _inversePosition += (Vector3.up * heightOffset);

            var _knot = new BezierKnot(_inversePosition);

            _spline.Add(_knot, TangentMode.AutoSmooth);
        }

        var _positions = (int)(subdivisions * (_spline.GetLength() / 10)) + 4;

        lineRenderer.positionCount = _positions;

        for (int i = 0; i < _positions; i++)
        {
            float _t = 0;

            if (_positions > 0)
            {
                _t = (1f / _positions) * i;
            }

            Vector3 _position = splineContainer.EvaluatePosition(0, _t);

            lineRenderer.SetPosition(i, _position + (Vector3.up * heightOffset));
        }

        navCircle.position = (path.corners[path.corners.Length - 1] + (Vector3.up * heightOffset));
    }

    //public void SetAgentDestination()
    //{
    //    if (active && agent != null && path != null)
    //    {
    //        var _agent = agent.CombatNavManager;

    //        _agent.SetPath(path);
    //    }
    //}

    //public void ActivateAgent()
    //{
    //    agent.CombatNavManager.navActive = true;
    //}

    public void PurgeAndClean()
    {
        navCircle.gameObject.SetActive(false);

        Spline _spline = splineContainer.Spline;

        _spline.Clear();

        lineRenderer.positionCount = 0;
    }

    //// Returns 'true' if the pointer is over a UI element
    //private bool IsPointerOverUIElement()
    //{
    //    return EventSystem.current.IsPointerOverGameObject();
    //}
}
