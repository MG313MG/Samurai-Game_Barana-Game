using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Arrow : MonoBehaviour {

	private Rigidbody2D rb;
    private SamuraiPlayer sp;

	[Header("Arrow")]
	[SerializeField] private float Speed;
	[SerializeField] private float xScale;
	[SerializeField] private int Damage_to_Enemy;
	[SerializeField] private bool Enemy_Damaged;
	[Header("Face to")]
	[SerializeField] private float FaceDir;


	void Start () 
	{
		rb = GetComponent<Rigidbody2D>();
		sp = FindFirstObjectByType<SamuraiPlayer>();

		Speed = sp.Acceleration_of_Arrow;
		FaceDir = sp.Face;
		xScale = transform.localScale.x;

		Damage_to_Enemy = Mathf.RoundToInt(Speed * 1.5f);

		transform.localScale = new Vector2(FaceDir * xScale, transform.localScale.y);

		StartCoroutine(Timer_for_Destroy());
	}
	
	void Update () 
	{
		rb.linearVelocity = new Vector2(Speed * 6 * FaceDir, rb.linearVelocity.y);
		if (Speed > 0)
			Speed -= Time.deltaTime;
		if (Speed < 0)
			Speed = 0;

	}
	public void Destroy_Arrow()
	{
		Destroy(gameObject);
	}
	
	private IEnumerator Timer_for_Destroy()
	{
		yield return new WaitForSeconds(5);
		Destroy_Arrow();
	}

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.GetComponent<Enemy>() != null)
		{
			if (Enemy_Damaged)
				return;

			Enemy_Damaged = true;
			collision.GetComponent<Enemy>().isDamaged_by_Arrow = true;
            collision.GetComponent<Enemy>().Health -= Damage_to_Enemy;
			Destroy_Arrow();
		}
    }
}
