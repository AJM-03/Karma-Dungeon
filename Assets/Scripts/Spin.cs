using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spin : MonoBehaviour
{
    public float speed;

    public float defaultSpeed;
    public float spinAttackSpeed;
    public float multiSpinAttackSpeed;
    public float randomFireAttackSpeed;
    public float homingAttackSpeed;
    public float trapAttackSpeed;
    public float enemySpawnSpeed;
    public float shieldSpawnSpeed;

    public float angryDefaultSpeed;
    public float angrySpinAttackSpeed;
    public float angryMultiSpinAttackSpeed;
    public float angryRandomFireAttackSpeed;
    public float angryHomingAttackSpeed;
    public float angryTrapAttackSpeed;


    void Update()
    {
        transform.Rotate(Vector3.back * speed * Time.deltaTime);
    }
}
