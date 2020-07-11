using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class InteractableEnemy : InteractableMovible
{
    [SerializeField] float range;
    [SerializeField] float huntForce;
    [SerializeField] float damage;
    bool isActive;
    bool isHounting;
    SphereCollider myTrigger;
    Rigidbody rb;

    protected override void Awake2()
    {
        //rb = GetComponent<Rigidbody>();
        ////myTrigger = gameObject.AddComponent<SphereCollider>();
        ////myTrigger.radius = range;
        ////myTrigger.isTrigger = true;
    }

    public override void DoEffect(PlayerMain playerMain)
    {
        print("DOEFFECT");
        playerMain.TakeDamage(damage);
    }

    public void Hunt(Transform _player)
    {
        Vector3 dir = (_player.position - transform.position).normalized;
        transform.forward = dir;
        rb.AddForce(dir * huntForce, ForceMode.Impulse);
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, range);
    }
}
