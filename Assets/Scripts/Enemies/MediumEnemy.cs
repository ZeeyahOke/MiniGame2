using UnityEngine;

public class MediumEnemy : Enemy
{
    void Reset()
    {
        maxHealth = 2;
        moveSpeed = 3f;
        scoreValue = 3;
        contactDamage = 1;
    }
}
