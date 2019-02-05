using System;
using System.Collections;
using UnityEngine;

namespace Common {
    public class Utils
    {
        public static void AttachRigidBody(GameObject obj, bool isKinematic)
        {
            Rigidbody rigidbody = obj.AddComponent(typeof(Rigidbody)) as Rigidbody;
            rigidbody.isKinematic = isKinematic;
        }

        public static void AttachCapsuleCollider(GameObject obj, bool isTrigger)
        {
            // Obj must have a Mesh component attached
            CapsuleCollider collider = obj.AddComponent<CapsuleCollider>() as CapsuleCollider;
            ResourceManager.setCharacterCollider(ref collider);
            collider.isTrigger = isTrigger;
        }

        public static bool IsEqualTo(double a, double b)
        {
            return Math.Abs(a - b) < double.Epsilon;
        }

        public static float FlatDistance(Vector3 p1, Vector3 p2)
        {
            return Vector2.Distance(new Vector2(p1.x, p1.z), new Vector2(p2.x, p2.z));
        }
    }
}