using UnityEngine;

public class HardEnemy : Enemy
{
    void Reset()
    {
        maxHealth = 3;
        moveSpeed = 4f;
        scoreValue = 5;
        contactDamage = 2;
    }
}
