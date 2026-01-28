using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Booom : MonoBehaviour {

	private SamuraiPlayer sp;

    [Header("Booom")]
	[SerializeField] private float Damage_to_Player;
	[SerializeField] private float Damage_Radius;
    [SerializeField] private float Go_Back_Radius;
    [SerializeField] private float xPosition_of_Bomb;
    [SerializeField] private float xPosition_of_Samurai;
	[SerializeField] private bool isSamuraiPlayer_inCollisionsDamage;
    [SerializeField] private bool isSamuraiPlayer_inCollisionsGoBack;
    [SerializeField] private bool DamagetoSP;
    [SerializeField] private bool GoBackPlayer;
	[SerializeField] private LayerMask Samurai_Layer;


    void Start()
	{
        sp = FindFirstObjectByType<SamuraiPlayer>();
        StartCoroutine(Timer_to_Destroy());

        xPosition_of_Bomb = transform.position.x;
        xPosition_of_Samurai = sp.transform.position.x;
	}

    void Update()
	{
        isSamuraiPlayer_inCollisionsDamage = Physics2D.OverlapCircle(transform.position, Damage_Radius, Samurai_Layer);
        isSamuraiPlayer_inCollisionsGoBack = Physics2D.OverlapCircle(transform.position, Go_Back_Radius, Samurai_Layer);

        if (isSamuraiPlayer_inCollisionsDamage && isSamuraiPlayer_inCollisionsGoBack)
            DamagetoSP = true;
        if (isSamuraiPlayer_inCollisionsGoBack && !DamagetoSP && !isSamuraiPlayer_inCollisionsDamage)
            GoBackPlayer = true;
        if (DamagetoSP)
        {
            sp.isHitting = true;
            sp.Samurai_Mode = Samurai_Modes.hurt;
            sp.Damage = Damage_to_Player;
            if (xPosition_of_Samurai < xPosition_of_Bomb)
                sp.isBombRight = true;
            else
                sp.isBombRight = false;
        }
        if (GoBackPlayer)
        {
            sp.isGobackking = true;
            sp.Samurai_Mode = Samurai_Modes.go_back;
            if (xPosition_of_Samurai < xPosition_of_Bomb)
                sp.isBombRight = true;
            else
                sp.isBombRight = false;
        }
    }

	private IEnumerator Timer_to_Destroy()
	{
		yield return new WaitForSeconds (0.7f);
		Destroy(gameObject);
	}

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position,Damage_Radius);
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, Go_Back_Radius);
    }
}
