using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowCollider : MonoBehaviour
{
  [SerializeField]  EnemyCactus cactus;
    private void Awake()
    {
        //cactus = GetComponentInParent<EnemyCactus>();
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.GetComponent<PlayerCharacter>())
        {
            cactus.FollowTarget = true;
            cactus.PlayerInRange = true;
            cactus.UpdateTarget();
        }
        else if (other.GetComponent<DoppelCharacter>())
        {
            cactus.FollowTarget = true;
            cactus.DoppelInRange = true;
            cactus.UpdateTarget();
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.GetComponent<PlayerCharacter>())
        {
            cactus.PlayerInRange = false;
            cactus.UpdateTarget();
        }
        else if (other.GetComponent<DoppelCharacter>())
        {
            cactus.DoppelInRange = false;
            cactus.UpdateTarget();
        }
    }
}
