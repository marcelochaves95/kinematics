using System;
using System.Collections;

using UnityEngine;

using MathModule;

namespace CollisionModule
{
    public class CollisionObject : MonoBehaviour
    {
        public interface ICollisionCallbackEventHandler
        {
            void OnVisitPersistentManifold(PersistentManifold pm);
            void OnFinishedVisitingManifolds();
        }
    }
}