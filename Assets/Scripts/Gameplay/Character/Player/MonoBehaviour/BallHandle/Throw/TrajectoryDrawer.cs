using System;
using System.Collections.Generic;
using Modules.MonoBehaviour;
using UnityEngine;
using Utility.Constants;

namespace Gameplay.Character.Player.MonoBehaviour.BallHandle.Throw
{
    public class TrajectoryDrawer : SwitchableComponent
    {
        [SerializeField] private LineRenderer _lineRenderer;
        [Range(0.02f, 1f)]
        [SerializeField] private float _timeStep = 0.02f;
        [SerializeField] private int _maxLineRendererPoints = 1000;
        [SerializeField] private LayerMask _lineStopperLayerMask;

        public override void Disable()
        {
            _lineRenderer.enabled = false;
            base.Disable();
        }

        public void Draw(Vector3 startPosition, Vector3 velocity)
        {
            _lineRenderer.enabled = true;
            
            List<Vector3> points = new List<Vector3>();
            points.Add(startPosition);

            for (int i = 0; i < _maxLineRendererPoints; i++)
            {
                float timeShift = _timeStep * i;

                Vector3 progressBeforeGravity = velocity * timeShift;
                Vector3 gravityComponent = Vector3.up * (NumericConstants.Half * Physics.gravity.y * timeShift * timeShift);
                Vector3 newPosition = startPosition + progressBeforeGravity + gravityComponent;

                points.Add(newPosition);

                Vector3 directionToPreviousPoint = points[i] - newPosition;
                float distanceTuPreviousPoint = directionToPreviousPoint.magnitude;
                
                if (Physics.Raycast(newPosition, directionToPreviousPoint, distanceTuPreviousPoint, _lineStopperLayerMask))
                    break;
            }

            _lineRenderer.positionCount = points.Count;
            _lineRenderer.SetPositions(points.ToArray());
        }

        public void StopDrawing()
        {
            _lineRenderer.positionCount = 0;
            _lineRenderer.enabled = false;
        }
    }
}