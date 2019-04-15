using UnityEngine;

namespace CollisionModule.Primitives
{
    /// <summary>
    /// Base class for Physics Engine primatives
    /// </summary>
    [RequireComponent(typeof(MeshFilter))]
    [RequireComponent(typeof(MeshRenderer))]
    [System.Serializable]
    public abstract class Primitive : MonoBehaviour
    {
        public string info = "Information about this Primitive"; // Display in inspector

        public void Start()
        {

            if (Application.isPlaying)
            {
                //Destroy(this);  //Probably don't need this class during runtime?
            }
        }

        public static void CreateNewBase(GameObject go, Vector3 position, Quaternion rotation)
        {
            go.transform.position = position;
            go.transform.rotation = rotation;

            MeshRenderer meshRenderer = go.GetComponent<MeshRenderer>();
            Material material = new Material(Shader.Find("Standard"));
            meshRenderer.sharedMaterial = material;
        }

        /// <summary>
        /// Build object mesh and collider
        /// </summary>
        public virtual void BuildMesh()
        {

        }
    }
}