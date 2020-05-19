using UnityEngine;

namespace Kinematics.Shape
{
    [ExecuteInEditMode]
    [RequireComponent(typeof(MeshFilter))]
    public sealed class BoxShape : Shape
    {
        private Vector3 frontTopLeft;
        private Vector3 frontTopRight;
        private Vector3 frontBottomLeft;
        private Vector3 frontBottomRight;
        private Vector3 backTopLeft;
        private Vector3 backTopRight;
        private Vector3 backBottomLeft;
        private Vector3 backBottomRight;

        protected override void CalculatePosition()
        {
            Mesh mesh = GetComponent<MeshFilter>().sharedMesh;
            Bounds bounds = mesh.bounds;
            Vector3 center = bounds.center;
            Vector3 extents = bounds.extents;

            float a = center.x - extents.x;
            float b = center.x + extents.x;
            float c = center.y - extents.y;
            float d = center.y + extents.y;
            float e = center.z - extents.z;
            float f = center.z + extents.z;

            frontTopLeft = new Vector3(a, d, e);
            frontTopRight = new Vector3(b, d, e);
            frontBottomLeft = new Vector3(a, c, e);
            frontBottomRight = new Vector3(b, c, e);
            backTopLeft = new Vector3(a, d, f);
            backTopRight = new Vector3(b, d, f);
            backBottomLeft = new Vector3(a, c, f);
            backBottomRight = new Vector3(b, c, f);

            frontTopLeft = transform.TransformPoint(frontTopLeft);
            frontTopRight = transform.TransformPoint(frontTopRight);
            frontBottomLeft = transform.TransformPoint(frontBottomLeft);
            frontBottomRight = transform.TransformPoint(frontBottomRight);

            backTopLeft = transform.TransformPoint(backTopLeft);
            backTopRight = transform.TransformPoint(backTopRight);
            backBottomLeft = transform.TransformPoint(backBottomLeft);
            backBottomRight = transform.TransformPoint(backBottomRight);
        }

        protected override void DrawCollider()
        {
            CalculatePosition();

            Debug.DrawLine(frontTopLeft, frontTopRight, color);
            Debug.DrawLine(frontTopRight, frontBottomRight, color);
            Debug.DrawLine(frontBottomRight, frontBottomLeft, color);
            Debug.DrawLine(frontBottomLeft, frontTopLeft, color);

            Debug.DrawLine(backTopLeft, backTopRight, color);
            Debug.DrawLine(backTopRight, backBottomRight, color);
            Debug.DrawLine(backBottomRight, backBottomLeft, color);
            Debug.DrawLine(backBottomLeft, backTopLeft, color);

            Debug.DrawLine(frontTopLeft, backTopLeft, color);
            Debug.DrawLine(frontTopRight, backTopRight, color);
            Debug.DrawLine(frontBottomRight, backBottomRight, color);
            Debug.DrawLine(frontBottomLeft, backBottomLeft, color);
        }
    }
}