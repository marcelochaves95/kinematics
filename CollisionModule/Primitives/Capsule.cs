using UnityEngine;

namespace CollisionModule.Primitives
{
    /// <summary>
    /// Capsule
    /// </summary>
    [RequireComponent(typeof(RigidBody))]
    [RequireComponent(typeof(CapsuleShape))]
    public class Capsule : Primitive
    {
        public CapsuleMeshSettings meshSettings = new CapsuleMeshSettings();

        public static GameObject CreateNew(Vector3 position, Quaternion rotation)
        {
            GameObject go = new GameObject();
            Capsule capsule = go.AddComponent<Capsule>();
            CreateNewBase(go, position, rotation);
            capsule.BuildMesh();
            go.name = "Capsule";
            return go;
        }

        public override void BuildMesh()
        {
            GetComponent<MeshFilter>().sharedMesh = meshSettings.Build();
            CapsuleShape capsuleShape = GetComponent<CapsuleShape>();
            capsuleShape.Height = meshSettings.height;
            capsuleShape.Radius = meshSettings.radius;
            capsuleShape.UpAxis = meshSettings.upAxis;
        }
    }
}