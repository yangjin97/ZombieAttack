using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UMA.CharacterSystem;
using Common;

public class PlayerController : MonoBehaviour
{
    enum Weapon
    {
        Hand, Sword
    };
    public GameObject UMADynamicCharacterAvatar;
    GameObject Player;

    GameObject Sword;
    Weapon currentWeapon;
    public bool hitting = false;

    ZombieController zombieController;
    public GameObject closestZombie;

    public Vector3 getPosition()
    {
        return UMADynamicCharacterAvatar.transform.position;
    }

    void Start()
    {
        Player = GameObject.Find("UMADynamicCharacterAvatar");
        UMADynamicCharacterAvatar = GameObject.Find("UMADynamicCharacterAvatar");
        zombieController = GameObject.Find("Zombies").GetComponent<ZombieController>();
    }

    void Hit()
    {
        zombieController.FindClosestZombieWithin(
            UMADynamicCharacterAvatar.transform.position,
            Common.ResourceManager.AttackDistance, out closestZombie);
        if (closestZombie != null)
        {
            LookAt(closestZombie.transform.position);
        }
        hitting = true;
        UMADynamicCharacterAvatar.GetComponent<Animator>().SetBool("Attack", true);
    }

    GameObject FindDeepChild(string childName)
    {
        Transform[] children = gameObject.GetComponentsInChildren<Transform>();
        foreach (Transform child in children)
        {
            if (child.gameObject.name == childName)
            {
                return child.gameObject;
            }
        }
        return null;
    }

    void CreateSword()
    {
        GameObject rightHand = FindDeepChild("RightHand");
        Sword = Instantiate(Resources.Load("Sword_Unity_Free/Prefabs/Sword_classic") as GameObject);
        Sword.transform.parent = rightHand.transform;
        Sword.transform.localScale = new Vector3(0.08f, 0.08f, 0.08f);
        Sword.transform.localPosition = new Vector3(0f, 0f, 0f);
        Sword.transform.localEulerAngles = new Vector3(-90f, 30f, 0f);
    }

    void Equip(Weapon weapon)
    {
        switch (weapon) {
            case Weapon.Hand:
                break;
            case Weapon.Sword:
                CreateSword();
                break;
            default:
                break;
        }
    }

    void UpdateWeaponStatus()
    {
        Weapon nextWeapon = currentWeapon;
        if (Input.GetKey(KeyCode.E))
        {
            UMADynamicCharacterAvatar.GetComponent<Animator>().SetBool("Sword", true);
            nextWeapon = Weapon.Sword;
        }
        if (Input.GetKey(KeyCode.Q))
        {
            UMADynamicCharacterAvatar.GetComponent<Animator>().SetBool("Sword", false);
            nextWeapon = Weapon.Hand;
        }
        if (Input.GetKeyDown(KeyCode.F))
        {
            Hit();
        } else if (!hitting)
        {
            UMADynamicCharacterAvatar.GetComponent<Animator>().SetBool("Attack", false);
        }
        if (currentWeapon != nextWeapon)
        {
            currentWeapon = nextWeapon;
            Equip(currentWeapon);
        }
    }

    void MovePlayer()
    {
        Vector3 direction = new Vector3(0, 0, 0);
        if (Input.GetKey(KeyCode.UpArrow))
        {
            direction.z -= 1;
        }
        if (Input.GetKey(KeyCode.DownArrow))
        {
            direction.z += 1;
        }
        if (Input.GetKey(KeyCode.RightArrow))
        {
            direction.x -= 1;
        }
        if (Input.GetKey(KeyCode.LeftArrow))
        {
            direction.x += 1;
        }
        if (!Utils.IsEqualTo(direction.x, 0) || !Utils.IsEqualTo(direction.z, 0))
        {
            UMADynamicCharacterAvatar.GetComponent<Animator>().SetFloat("Speed", Common.ResourceManager.PlayerMoveSpeed);
            //print(UMADynamicCharacterAvatar.GetComponent<Animator>().GetFloat("Speed"));
            //print(UMADynamicCharacterAvatar.GetComponent<Animator>().GetCurrentAnimatorStateInfo(0).IsName("Run"));
            //print(UMADynamicCharacterAvatar.GetComponent<Animator>().GetCurrentAnimatorStateInfo(0).IsName("Idle"));
            UMADynamicCharacterAvatar.transform.LookAt(UMADynamicCharacterAvatar.transform.position + direction);
            //print(transform.eulerAngles);
        }
        else
        {
            UMADynamicCharacterAvatar.GetComponent<Animator>().SetFloat("Speed", 0);
        }

        Rigidbody UMADynamicCharacterAvatarRigidbody = UMADynamicCharacterAvatar.GetComponent<Rigidbody>();
        if (UMADynamicCharacterAvatarRigidbody != null)
        {
            //print(UMADynamicCharacterAvatarRigidbody.mass);
            //UMADynamicCharacterAvatarRigidbody.isKinematic = true;
            UMADynamicCharacterAvatarRigidbody.velocity = direction * ResourceManager.PlayerMoveSpeed;
            //print("got it!!!");
        }
    }

    void MoveCamera()
    {
        Vector3 cameraPosition = UMADynamicCharacterAvatar.transform.position + ResourceManager.CameraOffset;
        Camera.main.transform.position = cameraPosition;
    }

    void FixedUpdate()
    {
        UpdateWeaponStatus();
        MovePlayer();
        MoveCamera();
    }

    //// Update is called once per frame
    //void Update()
    //{
    //    UpdateWeaponStatus();
    //}


    public void LookAt(Vector3 target)
    {
        UMADynamicCharacterAvatar.transform.LookAt(target);
    }
}