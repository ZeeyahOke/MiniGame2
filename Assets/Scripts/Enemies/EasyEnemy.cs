using UnityEngine;

public class EasyEnemy : Enemy
{
    void Reset()
    {
        maxHealth = 1;
        moveSpeed = 2f;
        scoreValue = 1;
        contactDamage = 1;
    }
}
