using UnityEngine;

namespace Kinematics.Shape
{
    public abstract class Shape : MonoBehaviour
    {
        protected readonly Color color = Color.green;

        private void OnGUI()
        {
            DrawCollider();
            CalculatePosition();
        }

        protected abstract void CalculatePosition();
        protected abstract void DrawCollider();
    }
}