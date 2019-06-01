using MathModule;

namespace CollisionModule
{
    public class Utility
    {
        public const float TWO_PI = 6.283185307179586232f;
        public const float RADS_PER_DEG = TWO_PI / 360.0f;
        public const float SQRT12 = 0.7071067811865475244008443621048490f;

        public static void DebugDrawRope(Vector3 position, Quaternion rotation, Vector3 scale, Vector3 begin, Vector3 end, int res, UnityEngine.Color color)
        {
            Gizmos.color = color;
            Matrix4x4 matrix = Matrix4x4.TRS(position, rotation, scale);
            Vector3 p1 = matrix.MultiplyPoint(begin);
            Vector3 p2 = matrix.MultiplyPoint(end);
            int r = res + 2;

            Vector3 deltaX = new Vector3(0.05f, 0.05f, 0);
            Vector3 deltaZ = new Vector3(0, 0.05f, 0.05f);

            for (int i = 0; i < r; i++)
            {
                Gizmos.color = color;
                float t = i * 1.0f / (r - 1);
                float tNext = (i + 1) * 1.0f / (r - 1);

                Vector3 p = Vector3.Lerp(p1, p2, t);
                Vector3 pNext = Vector3.Lerp(p1, p2, tNext);

                if (i != r - 1)
                    Gizmos.DrawLine(p, pNext);  // Line

                Gizmos.color = Color.white;
                Gizmos.DrawLine(p - deltaX, p + deltaX);
                Gizmos.DrawLine(p - deltaZ, p + deltaZ);
            }
        }

        public static void DebugDrawSphere(Vector3 position, Quaternion rotation, Vector3 scale, Vector3 radius, UnityEngine.Color color)
        {
            Gizmos.color = color;
            Vector3 start = position;

            Vector3 xoffs = new Vector3(radius.x * scale.x, 0, 0);
            Vector3 yoffs = new Vector3(0, radius.y * scale.y, 0);
            Vector3 zoffs = new Vector3(0, 0, radius.z * scale.z);

            xoffs = rotation * xoffs;
            yoffs = rotation * yoffs;
            zoffs = rotation * zoffs;

            float step = 5 * RADS_PER_DEG;
            int nSteps = (int)(360.0f / step);

            Vector3 vx = new Vector3(scale.x, 0, 0);
            Vector3 vy = new Vector3(0, scale.y, 0);
            Vector3 vz = new Vector3(0, 0, scale.z);

            Vector3 prev = start - xoffs;

            for (int i = 1; i <= nSteps; i++)
            {
                float angle = 360.0f * i / nSteps;
                Vector3 next = start + rotation * (radius.x * vx * Mathematics.Cos(angle) + radius.y * vy * Mathematics.Sin(angle));
                Gizmos.DrawLine(prev, next);
                prev = next;
            }

            prev = start - xoffs;

            for (int i = 1; i <= nSteps; i++)
            {
                float angle = 360.0f * i / nSteps;
                Vector3 next = start + rotation * (radius.x * vx * Mathematics.Cos(angle) + radius.z * vz * Mathematics.Sin(angle));
                Gizmos.DrawLine(prev, next);
                prev = next;
            }

            prev = start - yoffs;

            for (int i = 1; i <= nSteps; i++)
            {
                float angle = 360.0f * i / nSteps;
                Vector3 next = start + rotation * (radius.y * vy * Mathematics.Cos(angle) + radius.z * vz * Mathematics.Sin(angle));
                Gizmos.DrawLine(prev, next);
                prev = next;
            }

        }

        public static void DebugDrawBox(Vector3 position, Quaternion rotation, Vector3 scale, Vector3 maxVec, UnityEngine.Color color)
        {
            Vector3 minVec = new Vector3(0 - maxVec.x, 0 - maxVec.y, 0 - maxVec.z);

            Matrix4x4 matrix = Matrix4x4.TRS(position, rotation, scale);
            Vector3 iii = matrix.MultiplyPoint(minVec);
            Vector3 aii = matrix.MultiplyPoint(new Vector3(maxVec[0], minVec[1], minVec[2]));
            Vector3 aai = matrix.MultiplyPoint(new Vector3(maxVec[0], maxVec[1], minVec[2]));
            Vector3 iai = matrix.MultiplyPoint(new Vector3(minVec[0], maxVec[1], minVec[2]));
            Vector3 iia = matrix.MultiplyPoint(new Vector3(minVec[0], minVec[1], maxVec[2]));
            Vector3 aia = matrix.MultiplyPoint(new Vector3(maxVec[0], minVec[1], maxVec[2]));
            Vector3 aaa = matrix.MultiplyPoint(maxVec);
            Vector3 iaa = matrix.MultiplyPoint(new Vector3(minVec[0], maxVec[1], maxVec[2]));

            Gizmos.color = color;

            Gizmos.DrawLine(iii, aii);
            Gizmos.DrawLine(aii, aai);
            Gizmos.DrawLine(aai, iai);
            Gizmos.DrawLine(iai, iii);
            Gizmos.DrawLine(iii, iia);
            Gizmos.DrawLine(aii, aia);
            Gizmos.DrawLine(aai, aaa);
            Gizmos.DrawLine(iai, iaa);
            Gizmos.DrawLine(iia, aia);
            Gizmos.DrawLine(aia, aaa);
            Gizmos.DrawLine(aaa, iaa);
            Gizmos.DrawLine(iaa, iia);
        }

        public static void DebugDrawCapsule(Vector3 position, Quaternion rotation, Vector3 scale, float radius, float halfHeight, int upAxis, UnityEngine.Color color)
        {
            Matrix4x4 matrix = Matrix4x4.TRS(position, rotation, scale);

            Gizmos.color = color;

            Vector3 capStart = new Vector3(0.0f, 0.0f, 0.0f);
            capStart[upAxis] = -halfHeight;

            Vector3 capEnd = new Vector3(0.0f, 0.0f, 0.0f);
            capEnd[upAxis] = halfHeight;

            Gizmos.DrawWireSphere(matrix.MultiplyPoint(capStart), radius);
            Gizmos.DrawWireSphere(matrix.MultiplyPoint(capEnd), radius);

            // Draw some additional lines
            Vector3 start = position;

            capStart[(upAxis + 1) % 3] = radius;
            capEnd[(upAxis + 1) % 3] = radius;
            Gizmos.DrawLine(start + rotation * capStart, start + rotation * capEnd);

            capStart[(upAxis + 1) % 3] = -radius;
            capEnd[(upAxis + 1) % 3] = -radius;
            Gizmos.DrawLine(start + rotation * capStart, start + rotation * capEnd);

            capStart[(upAxis + 1) % 3] = 0.0f;
            capEnd[(upAxis + 1) % 3] = 0.0f;

            capStart[(upAxis + 2) % 3] = radius;
            capEnd[(upAxis + 2) % 3] = radius;
            Gizmos.DrawLine(start + rotation * capStart, start + rotation * capEnd);

            capStart[(upAxis + 2) % 3] = -radius;
            capEnd[(upAxis + 2) % 3] = -radius;
            Gizmos.DrawLine(start + rotation * capStart, start + rotation * capEnd);
        }

        public static void DebugDrawCylinder(Vector3 position, Quaternion rotation, Vector3 scale, float radius, float halfHeight, int upAxis, UnityEngine.Color color)
        {
            Gizmos.color = color;
            Vector3 start = position;
            Vector3 offsetHeight = new Vector3(0, 0, 0);
            offsetHeight[upAxis] = halfHeight;
            Vector3 offsetRadius = new Vector3(0, 0, 0);
            offsetRadius[(upAxis + 1) % 3] = radius;

            offsetHeight.x *= scale.x;
            offsetHeight.y *= scale.y;
            offsetHeight.z *= scale.z;
            offsetRadius.x *= scale.x;
            offsetRadius.y *= scale.y;
            offsetRadius.z *= scale.z;

            Gizmos.DrawLine(start + rotation * (offsetHeight + offsetRadius), start + rotation * (-offsetHeight + offsetRadius));
            Gizmos.DrawLine(start + rotation * (offsetHeight - offsetRadius), start + rotation * (-offsetHeight - offsetRadius));

            // Drawing top and bottom caps of the cylinder
            Vector3 yaxis = new Vector3(0, 0, 0);
            yaxis[upAxis] = 1.0f;
            Vector3 xaxis = new Vector3(0, 0, 0);
            xaxis[(upAxis + 1) % 3] = 1.0f;

            float r = offsetRadius.magnitude;
            DebugDrawArc(start - rotation * (offsetHeight), rotation * yaxis, rotation * xaxis, r, r, 0, TWO_PI, color, false, 10.0f);
            DebugDrawArc(start + rotation * (offsetHeight), rotation * yaxis, rotation * xaxis, r, r, 0, TWO_PI, color, false, 10.0f);
        }

        public static void DebugDrawCone(Vector3 position, Quaternion rotation, Vector3 scale, float radius, float height, int upAxis, UnityEngine.Color color)
        {
            Gizmos.color = color;

            Vector3 start = position;

            Vector3 offsetHeight = new Vector3(0, 0, 0);
            offsetHeight[upAxis] = height * 0.5f;
            Vector3 offsetRadius = new Vector3(0, 0, 0);
            offsetRadius[(upAxis + 1) % 3] = radius;
            Vector3 offset2Radius = new Vector3(0, 0, 0);
            offset2Radius[(upAxis + 2) % 3] = radius;

            offsetHeight.x *= scale.x;
            offsetHeight.y *= scale.y;
            offsetHeight.z *= scale.z;
            offsetRadius.x *= scale.x;
            offsetRadius.y *= scale.y;
            offsetRadius.z *= scale.z;
            offset2Radius.x *= scale.x;
            offset2Radius.y *= scale.y;
            offset2Radius.z *= scale.z;

            Gizmos.DrawLine(start + rotation * (offsetHeight), start + rotation * (-offsetHeight + offsetRadius));
            Gizmos.DrawLine(start + rotation * (offsetHeight), start + rotation * (-offsetHeight - offsetRadius));
            Gizmos.DrawLine(start + rotation * (offsetHeight), start + rotation * (-offsetHeight + offset2Radius));
            Gizmos.DrawLine(start + rotation * (offsetHeight), start + rotation * (-offsetHeight - offset2Radius));

            // Drawing the base of the cone
            Vector3 yaxis = new Vector3(0, 0, 0);
            yaxis[upAxis] = 1.0f;
            Vector3 xaxis = new Vector3(0, 0, 0);
            xaxis[(upAxis + 1) % 3] = 1.0f;
            DebugDrawArc(start - rotation * (offsetHeight), rotation * yaxis, rotation * xaxis, offsetRadius.magnitude, offset2Radius.magnitude, 0, TWO_PI, color, false, 10.0f);
        }

        public static void DebugDrawPlane(Vector3 position, Quaternion rotation, Vector3 scale, Vector3 planeNormal, float planeConst, UnityEngine.Color color)
        {
            Matrix4x4 matrix = Matrix4x4.TRS(position, rotation, new Vector3(1, 1, 1));

            Gizmos.color = color;

            Vector3 planeOrigin = planeNormal * planeConst;
            Vector3 vector0 = new Vector3(0, 0, 0);
            Vector3 vector1 = new Vector3(0, 0, 0);
            GetPlaneSpaceVector(planeNormal, ref vector0, ref vector1);
            float vectorLenght = 100.0f;
            Vector3 point0 = planeOrigin + vector0 * vectorLenght;
            Vector3 point1 = planeOrigin - vector0 * vectorLenght;
            Vector3 point2 = planeOrigin + vector1 * vectorLenght;
            Vector3 point3 = planeOrigin - vector1 * vectorLenght;
            Gizmos.DrawLine(matrix.MultiplyPoint(point0), matrix.MultiplyPoint(point1));
            Gizmos.DrawLine(matrix.MultiplyPoint(point2), matrix.MultiplyPoint(point3));
        }

        public static void GetPlaneSpaceVector(Vector3 planeNormal, ref Vector3 vector1, ref Vector3 vector2)
        {
            if (Mathematics.Abs(planeNormal[2]) > SQRT12)
            {
                // Choose p in y-z plane
                float a = planeNormal[1] * planeNormal[1] + planeNormal[2] * planeNormal[2];
                float k = 1.0f / Mathematics.Sqrt(a);
                vector1[0] = 0;
                vector1[1] = -planeNormal[2] * k;
                vector1[2] = planeNormal[1] * k;
                
                // Set q = n x p
                vector2[0] = a * k;
                vector2[1] = -planeNormal[0] * vector1[2];
                vector2[2] = planeNormal[0] * vector1[1];
            }
            else
            {
                // Choose p in x-y plane
                float a = planeNormal[0] * planeNormal[0] + planeNormal[1] * planeNormal[1];
                float k = 1.0f / Mathematics.Sqrt(a);
                vector1[0] = -planeNormal[1] * k;
                vector1[1] = planeNormal[0] * k;
                vector1[2] = 0;
                
                // Set q = n x p
                vector2[0] = -planeNormal[2] * vector1[1];
                vector2[1] = planeNormal[2] * vector1[0];
                vector2[2] = a * k;
            }
        }

        public static void DebugDrawArc(Vector3 center, Vector3 normal, Vector3 axis, float radiusA, float radiusB, float minAngle, float maxAngle,
            UnityEngine.Color color, bool drawSect, float stepDegrees)
        {
            Gizmos.color = color;

            Vector3 vx = axis;
            Vector3 vy = Vector3.Cross(normal, axis);
            float step = stepDegrees * RADS_PER_DEG;
            int nSteps = (int)((maxAngle - minAngle) / step);
            if (nSteps == 0)
                nSteps = 1;

            Vector3 prev = center + radiusA * vx * Mathematics.Cos(minAngle) + radiusB * vy * Mathematics.Sin(minAngle);
            
            if (drawSect)
                Gizmos.DrawLine(center, prev);

            for (int i = 1; i <= nSteps; i++)
            {
                float angle = minAngle + (maxAngle - minAngle) * i * 1.0f / (nSteps * 1.0f);
                Vector3 next = center + radiusA * vx * Mathematics.Cos(angle) + radiusB * vy * Mathematics.Sin(angle);
                Gizmos.DrawLine(prev, next);
                prev = next;
            }

            if (drawSect)
                Gizmos.DrawLine(center, prev);
        }
    }
}