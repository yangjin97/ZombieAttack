using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Common;

public class ZombieController : MonoBehaviour
{
    PlayerController playerController;

    List<GameObject> zombies;
    List<int> hps;
    int closestZombieIndex;

    void ReapDeadZombies()
    {
        for (int i=0; i<zombies.Count; i++)
        {
            if (zombies[i].GetComponent<Animator>().
                GetCurrentAnimatorStateInfo(0).IsName("Dead"))
            {
                hps.RemoveAt(i);
                Destroy(zombies[i]);
                zombies.RemoveAt(i);
                i--;
                //print(hps.Count);
                //print(zombies.Count);
            }
        }
    }

    void GeneratePosition(GameObject zombie, int rank)
    {
        float distance = Random.Range(rank * Common.ResourceManager.ZombieSpacing,
            (rank+1) * Common.ResourceManager.ZombieSpacing);
        float angle = Random.Range(0f, 360f);

        zombie.transform.Rotate(new Vector3(0, angle, 0));
        zombie.transform.Translate(new Vector3(distance, 0, distance));
    }

    public void HitClosestZombie()
    {
        if (closestZombieIndex != -1)
        {
            hps[closestZombieIndex] -= ResourceManager.SwordAttackDamage;
            print(hps[closestZombieIndex]);
            if (hps[closestZombieIndex] <= 0)
            {
                zombies[closestZombieIndex].GetComponent<Animator>().SetBool("Dead", true);
            }
        }
    }

    public void FindClosestZombieWithin(Vector3 origin, float distance,
        out GameObject closestZombie)
    {
        closestZombieIndex = -1;
        float minDistance = distance + 1;
        for (int i=0; i<zombies.Count; i++)
        {
            GameObject zombie = zombies[i];
            float curDistance = Utils.FlatDistance(origin, zombie.transform.position);
            //print("curDistance: " + curDistance);
            if (curDistance < minDistance)
            {
                minDistance = curDistance;
                closestZombieIndex = i;
            }
        }
        //print("minDistance: " + minDistance);
        if (closestZombieIndex == -1)
        {
            closestZombie = null;
            return;
        }
        closestZombie = zombies[closestZombieIndex];
    }

    GameObject GenerateZombie(int ring)
    {
        // Generate a zombie at the designated ring
        GameObject zombie = Instantiate(Resources.Load("Zombie/Prefabs/Zombie") as GameObject);
        GeneratePosition(zombie, ring);
        Utils.AttachCapsuleCollider(zombie, false);
        Utils.AttachRigidBody(zombie, true);
        return zombie;
    }

    void Start()
    {
        zombies = new List<GameObject>();
        hps = new List<int>();
        for (int i=0; i< Common.ResourceManager.NumInitialZombies; i++)
        {
            zombies.Add(GenerateZombie(i + 1));
            hps.Add(ResourceManager.ZombieMaxHp);
        }

        playerController = GameObject.Find("Player").GetComponent<PlayerController>();
    }

    void moveZombies()
    {
        if (playerController == null)
        {
            return;
        }

        Vector3 playerPosition = playerController.getPosition();
        for (int i=0; i<zombies.Count; i++)
        {
            if (hps[i] <= 0)
            {
                return;
            }
            GameObject zombie = zombies[i];
            zombie.transform.LookAt(playerPosition);
            if (Utils.FlatDistance(zombie.transform.position, playerPosition)
                < ResourceManager.AttackDistance)
            {
                zombie.GetComponent<Animator>().SetBool("Attack", true);
            }
            else
            {
                zombie.GetComponent<Animator>().SetBool("Attack", false);
                Rigidbody zombieRigidbody = zombie.GetComponent<Rigidbody>();
                if (zombieRigidbody != null)
                {
                    zombie.GetComponent<Animator>().SetFloat("Speed", 1);
                    zombieRigidbody.velocity = zombie.transform.forward * ResourceManager.ZombieMoveSpeed;
                    //print(zombieRigidbody.velocity);
                }
            }
        }
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        moveZombies();
        ReapDeadZombies();
    }
}
