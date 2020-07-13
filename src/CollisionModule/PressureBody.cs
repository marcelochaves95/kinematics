using Kinematics.MathModule;

namespace Kinematics.CollisionModule
{
    public class PressureBody : SpringBody
    {
        internal float volume;
        internal float pressure;
        internal Vector2[] normal_list;
        internal float[] edgelength_list;


        public PressureBody(Shape s, float mass, float gasPressure, float edgeSpringK, float edgeSpringDamp, float shapeSpringK, float shapeSpringDamp)
            : base(s, mass, edgeSpringK, edgeSpringDamp, shapeSpringK, shapeSpringDamp)
        {
            pressure = gasPressure;
            normal_list = new Vector2[Count];
            edgelength_list = new float[Count];
        }

        public override void ApplyInternalForces(double elapsed)
        {
            base.ApplyInternalForces(elapsed);

            volume = 0f;

            for (int i = 0; i < Count; i++)
            {
                int prev = (i > 0) ? i - 1 : Count - 1;
                int next = (i < Count - 1) ? i + 1 : 0;

                Vector2 edge1N = new Vector2
                {
                    X = PointMassList[i].position.X - PointMassList[prev].position.X,
                    Y = PointMassList[i].position.Y - PointMassList[prev].position.Y
                };
                VectorHelper.Perpendicular(ref edge1N);

                Vector2 edge2N = new Vector2
                {
                    X = PointMassList[next].position.X - PointMassList[i].position.X,
                    Y = PointMassList[next].position.Y - PointMassList[i].position.Y
                };
                VectorHelper.Perpendicular(ref edge2N);

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

                normal_list[i] = norm;
                edgelength_list[i] = edgeL;

                float xDistance = Mathf.Abs(PointMassList[i].position.X - PointMassList[next].position.X);
                float volumeProduct = xDistance * Mathf.Abs(norm.X) * edgeL;
                volume += 0.5f * volumeProduct;
            }

            float invVolume = 1f / volume;

            for (int i = 0; i < Count; i++)
            {
                int j = (i < Count - 1) ? i + 1 : 0;

                float pressureV = (invVolume * edgelength_list[i] * pressure);
                PointMassList[i].force.X += normal_list[i].X * pressureV;
                PointMassList[i].force.Y += normal_list[i].Y * pressureV;
                                  
                PointMassList[j].force.X += normal_list[j].X * pressureV;
                PointMassList[j].force.Y += normal_list[j].Y * pressureV;
            }
        }
    }
}