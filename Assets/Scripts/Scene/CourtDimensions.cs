using UnityEngine;

namespace Scene
{
    public readonly struct CourtDimensions
    {
        public CourtDimensions(Vector3 courtCenter, float xMin, float xMax, float zMin, float zMax)
        {
            CourtCenter = courtCenter;
            ZMax = zMax;
            XMin = xMin;
            XMax = xMax;
            ZMin = zMin;
        }

        public Vector3 CourtCenter { get; }

        public float XMin { get; }

        public float XMax { get; }

        public float ZMin { get; }

        public float ZMax { get; }
    }
}