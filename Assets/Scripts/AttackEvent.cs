using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackEvent : MonoBehaviour
{

    private Player player;
    private AiZombie zombie;

    private void Start()
    {
        player = GetComponent<Player>();
        zombie = GetComponent<AiZombie>();
    }
    public void DamageEvent()
    {
        player.Fire();
    }

    public void ZombieAttack()
    {
        zombie.ZombieDamage();
    }
    public void BossAttack()
    {
        zombie.BossDamage();
    }
}
