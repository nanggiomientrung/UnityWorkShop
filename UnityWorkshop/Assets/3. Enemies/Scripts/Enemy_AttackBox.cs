using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy_AttackBox : MonoBehaviour
{
    private float damage;
    [SerializeField] protected string hitTag;

    public void SetAttackDamage(float AttackDamage)
    {
        damage = AttackDamage;
    }
    private void OnTriggerEnter2D(Collider2D coll)
    {
        if (coll.CompareTag(hitTag))
        {
            IActor heath = coll.gameObject.GetComponent<IActor>();
            if (heath != null)
            {
                heath.TakeDamage(damage);
            }
        }
    }
}
