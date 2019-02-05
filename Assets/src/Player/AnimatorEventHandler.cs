using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Common;

public class AnimatorEventHandler : MonoBehaviour
{
    PlayerController playerController;
    ZombieController zombieController;

    void Hit()
    {
        playerController.hitting = false;
        zombieController.HitClosestZombie();
    }

    void FootR() { }
    void FootL() { }

    // Start is called before the first frame update
    void Start()
    {
        playerController = GameObject.Find("Player").GetComponent<PlayerController>();
        zombieController = GameObject.Find("Zombies").GetComponent<ZombieController>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
