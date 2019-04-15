using UnityEngine;

namespace CollisionModule.Primitive
{
    /// <summary>
    /// Box
    /// </summary>
    [RequireComponent(typeof(RigidBody))]
    [RequireComponent(typeof(BoxShape))]
    public class Box : Primitive
    {
        public BBoxMeshSettings meshSettings = new BBoxMeshSettings();

        public static GameObject CreateNew(Vector3 position, Quaternion rotation)
        {
            GameObject go = new GameObject();
            Box bBox = go.AddComponent<Box>();
            CreateNewBase(go, position, rotation);
            bBox.BuildMesh();
            go.name = "Box";
            return go;
        }

        public override void BuildMesh()
        {
            GetComponent<MeshFilter>().sharedMesh = meshSettings.Build();
            GetComponent<BBoxShape>().Extents = meshSettings.extents / 2f;
        }
    }
}