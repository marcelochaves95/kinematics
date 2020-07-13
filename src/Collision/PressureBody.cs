using Kinematics.Common;

namespace Kinematics.Collision
{
    public class PressureBody : SpringBody
    {
        private float _volume;
        private readonly float _pressure;
        private readonly Vector2[] _normalList;
        private float[] _edgeLengthList;


        public PressureBody(Shape s, float mass, float gasPressure, float edgeSpringK, float edgeSpringDamp, float shapeSpringK, float shapeSpringDamp)
            : base(s, mass, edgeSpringK, edgeSpringDamp, shapeSpringK, shapeSpringDamp)
        {
            _pressure = gasPressure;
            _normalList = new Vector2[Count];
            _edgeLengthList = new float[Count];
        }

        public override void ApplyInternalForces(double elapsed)
        {
            base.ApplyInternalForces(elapsed);

            _volume = 0f;

            for (int i = 0; i < Count; i++)
            {
                int prev = (i > 0) ? i - 1 : Count - 1;
                int next = (i < Count - 1) ? i + 1 : 0;

                Vector2 edge1N = new Vector2
                {
                    X = PointMassList[i].Position.X - PointMassList[prev].Position.X,
                    Y = PointMassList[i].Position.Y - PointMassList[prev].Position.Y
                };
                Vector2.Perpendicular(ref edge1N);

                Vector2 edge2N = new Vector2
                {
                    X = PointMassList[next].Position.X - PointMassList[i].Position.X,
                    Y = PointMassList[next].Position.Y - PointMassList[i].Position.Y
                };
                Vector2.Perpendicular(ref edge2N);

                Vector2 norm = new Vector2
                {
                    X = edge1N.X + edge2N.X,
                    Y = edge1N.Y + edge2N.Y
                };

                float nL = Mathf.Sqrt((norm.X * norm.X) + (norm.Y * norm.Y));
                if (nL > 0.001f)
                {
                    norm.X /= nL;
                    norm.Y /= nL;
                }

                float edgeL = Mathf.Sqrt((edge2N.X * edge2N.X) + (edge2N.Y * edge2N.Y));

                _normalList[i] = norm;
                _edgeLengthList[i] = edgeL;

                float xDistance = Mathf.Abs(PointMassList[i].Position.X - PointMassList[next].Position.X);
                float volumeProduct = xDistance * Mathf.Abs(norm.X) * edgeL;
                _volume += 0.5f * volumeProduct;
            }

            float invVolume = 1f / _volume;

            for (int i = 0; i < Count; i++)
            {
                int j = (i < Count - 1) ? i + 1 : 0;

                float pressureV = (invVolume * _edgeLengthList[i] * _pressure);
                PointMassList[i].Force.X += _normalList[i].X * pressureV;
                PointMassList[i].Force.Y += _normalList[i].Y * pressureV;
                                  
                PointMassList[j].Force.X += _normalList[j].X * pressureV;
                PointMassList[j].Force.Y += _normalList[j].Y * pressureV;
            }
        }
    }
}