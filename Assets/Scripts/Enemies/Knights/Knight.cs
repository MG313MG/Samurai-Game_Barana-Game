using System.Collections;
using UnityEngine;

public enum Knight_Modes { idle, walk, run, attak, hurt, dead };

public enum Timer_for_Knight { idle_timer, attak_timer, hurt_timer, dead_timer };

public class Knight : MonoBehaviour, Death_and_Hurt_Handler, Defend_Handler, Change_Face
{
    private Animator anim;
    private Rigidbody2D rb;
    private SamuraiPlayer sp;

    public Knight_Modes Knight_Mode;

    [Header("Spearer Skeleton")]
    [SerializeField] private float Speed;
    [SerializeField] private float xScale;
    [SerializeField] private float Distance_from_Player;
    public float Face;
    [Header("Face")]
    [SerializeField] private float FaceDir;
    [SerializeField] private bool FaceRight;
    [Header("Collisions")]
    [SerializeField] private bool isGrounded;
    [SerializeField] private bool isWalled;
    [SerializeField] private bool isPlayer;
    [SerializeField] private bool isWalledForward;
    [SerializeField] private bool isGroundedForward;
    [Header("Animations")]
    //floats
    [SerializeField] private float A_idle_Walk;
    [SerializeField] private float A_Level_of_Attaks;
    [SerializeField] private float A_Hurt_or_Defend;
    //ints
    [SerializeField] private int rnd_idle;
    [SerializeField] private int rnd_attaks;
    [SerializeField] private int rnd_Drop;
    //bools
    [Header("Game Objects")]
    [SerializeField] private GameObject Position_of_isGrounded_Forward;
    [SerializeField] private GameObject Y_Position;
    [SerializeField] private GameObject Position_of_isGrounded;
    //Drops
    [SerializeField] private GameObject Drop_Coin_Bag;
    [Header("Check Distances")]
    [SerializeField] private float Ground_CheckDistance;
    [SerializeField] private float Wall_CheckDistance;
    [SerializeField] private float Player_CheckDistance;
    [Header("Layers")]
    [SerializeField] private LayerMask Ground_Layer;
    [SerializeField] private LayerMask Player_Layer;
    [SerializeField] private LayerMask Wall_Layer;
    [Header("Bool of Archer Mode")]
    [SerializeField] private bool is_idle_Mode;
    [SerializeField] private bool is_Player_was_true;
    [SerializeField] private bool is_Attaking_by_Sword;
    [SerializeField] private bool isRunning;
    [SerializeField] private bool is_Hurt;
    [SerializeField] private bool is_Dead;
    [SerializeField] private bool is_Drop_Selected;

    private float timer_to_add_level_of_attak;

    private bool is_Defending;




    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        anim = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();
        sp = FindFirstObjectByType<SamuraiPlayer>();

        xScale = transform.localScale.x;
    }

    // Update is called once per frame
    void Update()
    {
        Change_Modes();

        Set_Modes();

        Handle_Animations();

        Set_Face();

        Handle_Collisions();
    }

    private void Change_Modes()
    {
        switch (Knight_Mode)
        {
            case Knight_Modes.idle:
                A_idle_Walk = 0;
                StartCoroutine(Timer_for_Knight_Modes(Timer_for_Knight.idle_timer));
                break;
            case Knight_Modes.walk:
                A_idle_Walk = 1;
                Speed = 3;
                rb.linearVelocity = new Vector2(Speed * FaceDir, rb.linearVelocity.y);
                break;
            case Knight_Modes.run:
                //isRunning = true;
                A_idle_Walk = 2;
                Speed = 6;
                rb.linearVelocity = new Vector2(Speed * FaceDir, rb.linearVelocity.y);
                break;
            case Knight_Modes.attak:
                isRunning = false;
                timer_to_add_level_of_attak += Time.deltaTime;
                if (timer_to_add_level_of_attak >= 1.1)
                {
                    timer_to_add_level_of_attak = 0;
                    A_Level_of_Attaks += 1;
                }
                if (A_Level_of_Attaks == 4)
                    A_Level_of_Attaks = 0;
                break;
            case Knight_Modes.hurt:
                if (is_Defending)
                    A_Hurt_or_Defend = 1;
                else
                    A_Hurt_or_Defend = 0;
                StartCoroutine(Timer_for_Knight_Modes(Timer_for_Knight.hurt_timer));
                break;
            case Knight_Modes.dead:
                is_Dead = true;
                StartCoroutine(Timer_for_Knight_Modes(Timer_for_Knight.dead_timer));
                break;
        }
    }

    private void Set_Modes()
    {
        //idle
        if (rnd_idle == 2)
        {
            is_idle_Mode = true;
            Knight_Mode = Knight_Modes.idle;
        }
        //walk
        if (!is_idle_Mode && !isRunning && !is_Attaking_by_Sword && !is_Dead && !is_Hurt)
        {
            Knight_Mode = Knight_Modes.walk;
        }
        //chande face
        if (isWalledForward)
        {
            Player_CheckDistance -= Time.deltaTime * 10;
        }
        //run and attak
        if (isPlayer && !is_Hurt && !is_Dead)
        {
            Distance_from_Player = Mathf.Abs(transform.position.x - sp.transform.position.x);
            if (Distance_from_Player <= 2)
            {
                is_Attaking_by_Sword = true;
                Knight_Mode = Knight_Modes.attak;
            }
            else
            {
                Knight_Mode = Knight_Modes.run;
                isRunning = true;
            }
        }
        else
        {
            is_Attaking_by_Sword = false;
            isRunning = false;
        }
    }

    private void Handle_Animations()
    {
        //floats
        anim.SetFloat("idle_Walk", A_idle_Walk);
        anim.SetFloat("Level_of_Attaks", A_Level_of_Attaks);
        anim.SetFloat("Hurt_or_Defend", A_Hurt_or_Defend);
        //bools
        anim.SetBool("is_Hurt", is_Hurt);
        anim.SetBool("is_Dead", is_Dead);
        anim.SetBool("is_Attaking", is_Attaking_by_Sword);
    }

    private void Set_Face()
    {
        if (isGrounded)
            if (isWalled || !isGroundedForward)
            {
                FaceRight = !FaceRight;
                Player_CheckDistance = 10;
                rnd_idle = Random.Range(1, 6);
            }

        if (FaceRight)
            FaceDir = 1;
        else
            FaceDir = -1;

        transform.localScale = new Vector3(xScale * FaceDir, transform.localScale.y, transform.localScale.z);
    }

    public void OnHurt()
    {
        if (!is_Dead)
        {
            is_Hurt = true;
            Knight_Mode = Knight_Modes.hurt;
        }
    }
    public void OnDefend()
    {
        is_Hurt = true;
        is_Defending = true;
        Knight_Mode = Knight_Modes.hurt;
    }

    public void OnChangeFace()
    {
        FaceRight = !FaceRight;
        Player_CheckDistance = 10;
    }
    public void OnDeath()
    {
        Knight_Mode = Knight_Modes.dead;
    }

    private void Handle_Collisions()
    {
        isPlayer = Physics2D.Raycast(Y_Position.transform.position, Vector2.right * FaceDir, Player_CheckDistance, Player_Layer);
        isWalledForward = Physics2D.Raycast(Y_Position.transform.position, Vector2.right * FaceDir, Player_CheckDistance, Wall_Layer);
        isGrounded = Physics2D.Raycast(Position_of_isGrounded.transform.position, Vector2.down, Ground_CheckDistance, Ground_Layer);
        isGroundedForward = Physics2D.Raycast(Position_of_isGrounded_Forward.transform.position, Vector2.down, Ground_CheckDistance, Ground_Layer);
        isWalled = Physics2D.Raycast(Y_Position.transform.position, Vector2.right * FaceDir, Wall_CheckDistance, Wall_Layer);
    }

    private IEnumerator Timer_for_Knight_Modes(Timer_for_Knight Knight_Timer)
    {
        switch (Knight_Timer)
        {
            case Timer_for_Knight.idle_timer:
                yield return new WaitForSeconds(2);
                is_idle_Mode = false;
                rnd_idle = 1;
                break;
            case Timer_for_Knight.dead_timer:
                yield return new WaitForSeconds(0.5f);
                Destroy(gameObject);
                break;
            case Timer_for_Knight.hurt_timer:
                yield return new WaitForSeconds(0.7f);
                is_Hurt = false;
                break;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.GetComponent<SamuraiPlayer>() != null && collision.GetComponent<SamuraiPlayer>().isHitting == false && is_Attaking_by_Sword)
        {
            collision.GetComponent<SamuraiPlayer>().Health -= 10;
            //collision.GetComponent<SamuraiPlayer>().isHitting = true;
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.blue;
        Gizmos.DrawLine(Position_of_isGrounded.transform.position, new Vector2(Position_of_isGrounded.transform.position.x, Position_of_isGrounded.transform.position.y - Ground_CheckDistance));
        Gizmos.color = Color.red;
        Gizmos.DrawLine(Y_Position.transform.position, new Vector2(transform.position.x + (Player_CheckDistance * FaceDir), Y_Position.transform.position.y));
        Gizmos.color = Color.yellow;
        Gizmos.DrawLine(Position_of_isGrounded_Forward.transform.position, new Vector2(Position_of_isGrounded_Forward.transform.position.x, Position_of_isGrounded_Forward.transform.position.y - Ground_CheckDistance));
        Gizmos.color = Color.green;
        Gizmos.DrawLine(Y_Position.transform.position, new Vector2(Y_Position.transform.position.x + (Wall_CheckDistance * FaceDir), Y_Position.transform.position.y));
    }
}
