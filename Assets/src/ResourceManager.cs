using System.Collections;
using UnityEngine;

namespace Common {
	public class ResourceManager
	{
	    public static float PlayerMoveSpeed { get { return 3F; } }
        public static float ZombieMoveSpeed { get { return 1F; } }
        public static int ZombieMaxHp { get { return 100; } }
        public static int SwordAttackDamage { get { return 50; } }
        public static Vector3 CameraOffset { get { return new Vector3(0, 10, 10); } }
        public static int NumInitialZombies { get { return 3; } }
        public static float ZombieSpacing { get { return 5f; } }
        public static float AttackDistance { get { return 1.5f; } }
        public static void setCharacterCollider(ref CapsuleCollider characterCollider)
        {
            characterCollider.center = new Vector3(0, 1, 0);
            characterCollider.radius = 0.5f;
            characterCollider.height = 2f;
        }
    }
}