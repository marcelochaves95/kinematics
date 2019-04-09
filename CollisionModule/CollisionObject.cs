using System;
using System.Collections;

using MathModule;

namespace CollisionModule
{
    public struct CollisionObject : Object
    {
        public interface ICollisionCallbackEventHandler
        {
            void OnVisitPersistentManifold(PersistentManifold pm);
            void OnFinishedVisitingManifolds();
        }
    }
}