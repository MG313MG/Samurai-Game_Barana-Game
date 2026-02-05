using System.Collections;
using UnityEngine;

public class Skeleton_Arrow : MonoBehaviour
{
    private Rigidbody2D rb;
    private SamuraiPlayer sp;
    private Skeleton_Archer ar;

    [Header("Arrow")]
    [SerializeField] private float Speed;
    [SerializeField] private float xScale;
    [SerializeField] private int Damage_to_Player;
    [SerializeField] private bool Enemy_Damaged;
    [Header("Face to")]
    [SerializeField] private float FaceDir;


    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        sp = FindFirstObjectByType<SamuraiPlayer>();
        ar = FindFirstObjectByType<Skeleton_Archer>();

        Speed = 30;
        //FaceDir = ar.Face;
        xScale = transform.localScale.x;

        if (xScale < 0)
        {
            FaceDir = -1;
        }
        else
        {
            FaceDir = 1;
        }

            Damage_to_Player = 5;

        //transform.localScale = new Vector2(FaceDir * xScale, transform.localScale.y);

        StartCoroutine(Timer_for_Destroy());
    }

    void Update()
    {
        rb.linearVelocity = new Vector2(Speed * FaceDir, rb.linearVelocity.y);
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
        if (collision.GetComponent<SamuraiPlayer>() != null)
        {
            collision.GetComponent<SamuraiPlayer>().Health -= Damage_to_Player;
            Destroy_Arrow();
        }
    }
}
