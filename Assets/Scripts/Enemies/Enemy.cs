using Unity.VisualScripting;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    Death_and_Hurt_Handler death_and_hurt_handler;

    [Header("Enemy")]
    public float Health;
    [SerializeField] private float Previous_Health;

    private void Start()
    {
        death_and_hurt_handler = GetComponent<Death_and_Hurt_Handler>();

        Previous_Health = Health;
    }

    private void Update()
    {
        if (Health != Previous_Health && Health > 0)
        {
            if (death_and_hurt_handler != null)
            {
                death_and_hurt_handler.OnHurt();
            }
            Previous_Health = Health;
        }

        if (Health <= 0)
        {
            if (death_and_hurt_handler != null)
            {
                death_and_hurt_handler.OnDeath();
            }
        }
    }
}
