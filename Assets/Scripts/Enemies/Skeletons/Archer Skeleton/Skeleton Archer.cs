using JetBrains.Annotations;
using System.Collections;
using System.Threading;
using Unity.Properties;
using UnityEditor.Rendering.Universal;
using UnityEngine;


public enum Archer_Skeleton_Modes {idle, walk, evosion, shot, attak_by_knife, hurt, dead};

public enum Archer_Skeleton_Timer {idle_timer, Evosion_timer, is_player_was_true_timer, dead_timer, hurt_timer};

public class Skeleton_Archer : MonoBehaviour, Death_and_Hurt_Handler
{
    private Animator anim;
    private Rigidbody2D rb;
    private SamuraiPlayer sp;

    public Archer_Skeleton_Modes Archer_Skeleton_Mode;

    [Header("Archer Skeleton")]
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
    [SerializeField] private bool isGroundedForward;
    [Header("Animations")]
    //floats
    [SerializeField] private float A_idle_Walk;
    //ints
    [SerializeField] private int rnd_idle;
    [SerializeField] private int rnd_attaks;
    [SerializeField] private int rnd_shot;
    [SerializeField] private int rnd_Drop;
    //bools
    [Header("Game Objects")]
    [SerializeField] private GameObject Arrow;
    [SerializeField] private GameObject Spawn_Arrow;
    [SerializeField] private GameObject Position_of_isGroundedForward;
    [SerializeField] private GameObject Y_Position;
    [SerializeField] private GameObject Position_of_isGrounded;
    //Drops
    [SerializeField] private GameObject Drop_Arrow;
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
    [SerializeField] private bool is_evosion_Mode;
    [SerializeField] private bool is_shot_Mode;
    [SerializeField] private bool is_Player_was_true;
    [SerializeField] private bool is_Hurt;
    [SerializeField] private bool is_Dead;
    [SerializeField] private bool is_Shot_Mode_Selected;
    [SerializeField] private bool is_Drop_Selected;


    void Start()
    {
        anim = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();
        sp = FindFirstObjectByType<SamuraiPlayer>();

        xScale = transform.localScale.x;
    }

    void Update()
    {
        Face = FaceDir;

        Switch_on_Modes();

        Change_Modes();

        Set_Face();

        Handle_Animations();

        Handle_Collisions();
    }

    private void Switch_on_Modes()
    {
        switch (Archer_Skeleton_Mode)
        {
            case Archer_Skeleton_Modes.idle:
                A_idle_Walk = 0;
                StartCoroutine(Timer_for_Archer_Skeleton_Modes(Archer_Skeleton_Timer.idle_timer));
                break;
            case Archer_Skeleton_Modes.walk:
                Speed = 2;
                rb.linearVelocity = new Vector2(Speed * FaceDir, rb.linearVelocity.y);
                A_idle_Walk = 1;
                break;
            case Archer_Skeleton_Modes.shot:
                Speed = 0;
                is_evosion_Mode = false;
                rb.linearVelocity = new Vector2(Speed * FaceDir, rb.linearVelocity.y);
                break;
            case Archer_Skeleton_Modes.evosion:
                Speed = 2.5f;
                is_shot_Mode = false;
                rb.linearVelocity = new Vector2(Speed * FaceDir, rb.linearVelocity.y);
                break;
            case Archer_Skeleton_Modes.hurt:
                StartCoroutine(Timer_for_Archer_Skeleton_Modes(Archer_Skeleton_Timer.hurt_timer));
                break;
            case Archer_Skeleton_Modes.dead:
                is_Dead = true;
                StartCoroutine(Timer_for_Archer_Skeleton_Modes(Archer_Skeleton_Timer.dead_timer));
                break;
        }
    }

    public void Instantiate_Arrow()
    {
        GameObject Spawner_Arrow = Instantiate(Arrow, Spawn_Arrow.transform.position, Quaternion.identity);
        Spawner_Arrow.transform.localScale = new Vector2(Spawner_Arrow.transform.localScale.x * FaceDir, Spawner_Arrow.transform.localScale.y);
    }

    public void OnHurt()
    {
        if (!is_Dead)
        {
            is_Hurt = true;
            Archer_Skeleton_Mode = Archer_Skeleton_Modes.hurt;
        }
    }

    public void OnDeath()
    {
        Archer_Skeleton_Mode = Archer_Skeleton_Modes.dead;
        if (!is_Drop_Selected)
        {
            is_Drop_Selected = true;
            rnd_Drop = Random.Range(1, 4);
            if (rnd_Drop == 1)
                Instantiate(Drop_Arrow, transform.position, Quaternion.identity);
            else if (rnd_Drop == 2)
                print("Drop coin bag");
            else
                print("No drop");
        }
    }

    private void Change_Modes()
    {
        //walk
        if (!is_idle_Mode && !is_evosion_Mode && !is_shot_Mode && !is_Player_was_true && !is_Hurt && !is_Dead)
            Archer_Skeleton_Mode = Archer_Skeleton_Modes.walk;
        //idle
        if (rnd_idle == 2)
        {
            is_idle_Mode = true;
            Archer_Skeleton_Mode = Archer_Skeleton_Modes.idle;
        }
        //check player to evosion,shot and attak by knife
        if (isPlayer && !is_Hurt && !is_Dead)
        {
            is_Player_was_true = true;
            Distance_from_Player = Mathf.Abs(transform.position.x - sp.transform.position.x);

            if (!is_Shot_Mode_Selected)
            {
                rnd_shot = Random.Range(0, 2);
                is_Shot_Mode_Selected = true;
            }

            if (Distance_from_Player <= 11)
            {
                if (is_shot_Mode) 
                    return;

                is_shot_Mode = true;
                Archer_Skeleton_Mode = Archer_Skeleton_Modes.shot;
            }
            else
            {
                if (is_evosion_Mode)
                    return;

                is_evosion_Mode = true;
                Archer_Skeleton_Mode = Archer_Skeleton_Modes.evosion;
            }
        }

        if (!isPlayer && is_Shot_Mode_Selected)
            is_Shot_Mode_Selected = false;


        if (!isPlayer && is_Player_was_true)
            StartCoroutine(Timer_for_Archer_Skeleton_Modes(Archer_Skeleton_Timer.is_player_was_true_timer));
    }

    private void Set_Face()
    {
        if (isGrounded)
            if (isWalled || !isGroundedForward)
            {
                FaceRight = !FaceRight;
                rnd_idle = Random.Range(1, 6);
            }

        if (FaceRight)
            FaceDir = 1;
        else
            FaceDir = -1;

        transform.localScale = new Vector3(xScale * FaceDir, transform.localScale.y, transform.localScale.z);
    }

    private void Handle_Animations()
    {
        //floats
        anim.SetFloat("idle_Walk", A_idle_Walk);
        anim.SetFloat("Shot", rnd_shot);
        //bools
        anim.SetBool("is_Evosionning", is_evosion_Mode);
        anim.SetBool("is_Shotting", is_shot_Mode);
        anim.SetBool("is_Hurt", is_Hurt);
        anim.SetBool("is_Dead", is_Dead);
    }

    private void Handle_Collisions()
    {
        isPlayer = Physics2D.Raycast(Y_Position.transform.position, Vector2.right * FaceDir, Player_CheckDistance, Player_Layer);
        isGrounded = Physics2D.Raycast(Position_of_isGrounded.transform.position, Vector2.down, Ground_CheckDistance, Ground_Layer);
        isGroundedForward = Physics2D.Raycast(Position_of_isGroundedForward.transform.position, Vector2.down, Ground_CheckDistance, Ground_Layer);
        isWalled = Physics2D.Raycast(Y_Position.transform.position, Vector2.right * FaceDir, Wall_CheckDistance, Wall_Layer);
    }

    private IEnumerator Timer_for_Archer_Skeleton_Modes(Archer_Skeleton_Timer Timer_for_Archer_Skeleton)
    {
        switch (Timer_for_Archer_Skeleton)
        {
            case Archer_Skeleton_Timer.idle_timer:
                yield return new WaitForSeconds(2);
                rnd_idle = 1;
                is_idle_Mode = false;
                break;
            case Archer_Skeleton_Timer.is_player_was_true_timer:
                is_evosion_Mode = false;
                is_shot_Mode = false;
                yield return new WaitForSeconds(0.4f);
                is_Player_was_true = false;
                break;
            case Archer_Skeleton_Timer.hurt_timer:
                yield return new WaitForSeconds(0.7f);
                is_Hurt = false;
                break;
            case Archer_Skeleton_Timer.dead_timer:
                yield return new WaitForSeconds(0.3f);
                Destroy(gameObject);
                break;
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.blue;
        Gizmos.DrawLine(Position_of_isGrounded.transform.position, new Vector2(Position_of_isGrounded.transform.position.x, Position_of_isGrounded.transform.position.y - Ground_CheckDistance));
        Gizmos.color = Color.red;
        Gizmos.DrawLine(Y_Position.transform.position, new Vector2(transform.position.x + (Player_CheckDistance * FaceDir),Y_Position.transform.position.y));
        Gizmos.color = Color.yellow;
        Gizmos.DrawLine(Position_of_isGroundedForward.transform.position,new Vector2(Position_of_isGroundedForward.transform.position.x, Position_of_isGroundedForward.transform.position.y - Ground_CheckDistance));
        Gizmos.color= Color.green;
        Gizmos.DrawLine(Y_Position.transform.position, new Vector2(Y_Position.transform.position.x + (Wall_CheckDistance * FaceDir), Y_Position.transform.position.y));
    }
}
