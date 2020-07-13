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
            normal_list = new Vector2[count];
            edgelength_list = new float[count];
        }

        public override void ApplyInternalForces(double elapsed)
        {
            base.ApplyInternalForces(elapsed);

            volume = 0f;

            for (int i = 0; i < count; i++)
            {
                int prev = (i > 0) ? i - 1 : count - 1;
                int next = (i < count - 1) ? i + 1 : 0;

                Vector2 edge1N = new Vector2
                {
                    X = pointmass_list[i].position.X - pointmass_list[prev].position.X,
                    Y = pointmass_list[i].position.Y - pointmass_list[prev].position.Y
                };
                VectorHelper.Perpendicular(ref edge1N);

                Vector2 edge2N = new Vector2
                {
                    X = pointmass_list[next].position.X - pointmass_list[i].position.X,
                    Y = pointmass_list[next].position.Y - pointmass_list[i].position.Y
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

                float xDistance = Mathf.Abs(pointmass_list[i].position.X - pointmass_list[next].position.X);
                float volumeProduct = xDistance * Mathf.Abs(norm.X) * edgeL;
                volume += 0.5f * volumeProduct;
            }

            float invVolume = 1f / volume;

            for (int i = 0; i < count; i++)
            {
                int j = (i < count - 1) ? i + 1 : 0;

                float pressureV = (invVolume * edgelength_list[i] * pressure);
                pointmass_list[i].force.X += normal_list[i].X * pressureV;
                pointmass_list[i].force.Y += normal_list[i].Y * pressureV;
                                  
                pointmass_list[j].force.X += normal_list[j].X * pressureV;
                pointmass_list[j].force.Y += normal_list[j].Y * pressureV;
            }
        }
    }
}