using UnityEngine;

namespace CollisionModule.Primitives
{
    /// <summary>
    /// Cone
    /// </summary>
    [RequireComponent(typeof(RigidBody))]
    [RequireComponent(typeof(ConeShape))]
    public class Cone : Primitive
    {
        public ConeMeshSettings meshSettings = new ConeMeshSettings();

        public static GameObject CreateNew(Vector3 position, Quaternion rotation)
        {
            GameObject go = new GameObject();
            Cone cone = go.AddComponent<Cone>();
            CreateNewBase(go, position, rotation);
            cone.BuildMesh();
            go.name = "Cone";

            return go;
        }

        public override void BuildMesh()
        {
            GetComponent<MeshFilter>().sharedMesh = meshSettings.Build();
            ConeShape cone = GetComponent<ConeShape>();
            cone.Radius = meshSettings.radius;
            cone.Height = meshSettings.height;
        }
    }
}