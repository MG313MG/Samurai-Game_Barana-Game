using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEditor.Rendering.Universal;
using UnityEditorInternal;
using UnityEngine;

public enum Samurai_Modes {idle, walk, run, jump, in_sky, falled,attak_by_sword, attak_by_bow, Spawn_Arrow, attak_by_movement_spear, dead, hurt, go_back};

public enum IEnumerator_Timer_for_Samurai {attak_timer, can_attak_by_sword_timer, can_attak_by_bow_timer, timer_to_minus_1_from_jump, timer_for_false_ishitting, timer_for_go_back, timer_for_dead};

public class SamuraiPlayer : MonoBehaviour {

	private Animator anim;
	private Rigidbody2D rb;

	public Samurai_Modes Samurai_Mode;

	[Header("Samurai")]
	public float Health;
	public float Arrows;
	public float Max_Arrows;
	[SerializeField] private float Previous_Health;
	[SerializeField] private float Speed;
	[SerializeField] private float Stamina;
	[SerializeField] private float Jump;
	[SerializeField] private float xScale;
	[Header("Face")] 
	[SerializeField] private float FaceDir;
	[SerializeField] private bool FaceRight;
	[Header("Animations")]
	//Floats
	[SerializeField] private float A_F_IWR;
	[SerializeField] private float A_F_JIsF;
	[Header("Check Distances")]
	[SerializeField] private float GrondCheckDistance;
	[Header("Layers")]
	[SerializeField] private LayerMask Layer_Ground;
	[Header("Collisions")]
	[SerializeField] private bool isGrounded;
	[Header("Game Objects")]
	[SerializeField] private GameObject Arrow;
	[SerializeField] private GameObject Point_of_Spawn_Arrow;
	[Header("Bool of Modes")]
	[SerializeField] private bool isWalking;
    [SerializeField] private bool isWalking_was_true;
	[SerializeField] private bool isRunning;
    [SerializeField] private bool isRunning_was_true;
	[SerializeField] private bool isJumping;
	[SerializeField] private bool isAttakingbySword;
	[SerializeField] private bool isCanAttakingbySword;
	[SerializeField] private bool isAttaking_byBow;
	[SerializeField] private bool isCanAttakingbyBow;
	[SerializeField] private bool isCanDamagging;
	[SerializeField] private bool isCanGoBackking;
	[SerializeField] private bool isEnemyDamagged;
	[SerializeField] private bool isDead;
    [Header("Float of Modes")]
	[SerializeField] private float Can_Jumping;
	[SerializeField] private float Level_of_Attaks;
	[SerializeField] private float Charge_or_Fire_by_Bow;
	[SerializeField] private float Timer_for_Charge_Bow;

	[Header("Publics")]
	public float Acceleration_of_Arrow;
	public float Face;
	public float Damage;
	public bool isHitting;
	public bool isGobackking;
	public bool isBombRight;
	public bool isHittinged;




    void Start () 
	{
		anim = GetComponent<Animator>();
		rb = GetComponent<Rigidbody2D>();

		Samurai_Mode = Samurai_Modes.idle;

		Previous_Health = Health;

		FaceDir = 1;
		FaceRight = true;

		isCanAttakingbySword = true;
		isCanAttakingbyBow = true;
        isCanDamagging = true;
		isCanGoBackking = true;

        xScale = transform.localScale.x;
	}
	

	void Update ()
    {
		Switch_On_Modes();

        Set_Animations();

        Set_Face();

        Collisions();
        
        Set_Samurai_Mode();

		if (Previous_Health != Health && !isAttakingbySword)
		{
			Previous_Health = Health;
			isHitting = true;
		}
    }

    private void Switch_On_Modes()
    {
        switch (Samurai_Mode)
        {
            case Samurai_Modes.idle:
                A_F_IWR = 0;
                Speed = 0;
                isRunning = false;
                break;
            case Samurai_Modes.walk:
				isWalking = true;
                A_F_IWR = 1;
                Speed = 4;
                rb.linearVelocity = new Vector2(Speed * FaceDir, rb.linearVelocity.y);
                break;
            case Samurai_Modes.run:
                isRunning = true;
                A_F_IWR = 2;
                Speed = 8;
                rb.linearVelocity = new Vector2(Speed * FaceDir, rb.linearVelocity.y);
                break;
            case Samurai_Modes.jump:
                isJumping = true;
				if (isWalking)
				{
					isWalking_was_true = true;
					isWalking = false;
				}
                if (isRunning)
                {
                    isRunning_was_true = true;
                    isRunning = false;
                }
                A_F_JIsF = 0;
                break;
            case Samurai_Modes.in_sky:
                A_F_JIsF = 1;
                break;
            case Samurai_Modes.falled:
                A_F_JIsF = 2;
                isJumping = false;
                break;
            case Samurai_Modes.attak_by_sword:
                Level_of_Attaks += 1;
                isEnemyDamagged = false;
                isAttakingbySword = true;
                if (Level_of_Attaks == 3)
                    Level_of_Attaks = 0;
                StartCoroutine(Enum_Timer_for_Samurai(IEnumerator_Timer_for_Samurai.attak_timer));
                isCanAttakingbySword = false;
                break;
            case Samurai_Modes.attak_by_bow:
                isAttaking_byBow = true;
                //isCanAttakingbyBow = false;
                if (Timer_for_Charge_Bow < 5)
                    Timer_for_Charge_Bow += Time.deltaTime;
                if (Timer_for_Charge_Bow > 5)
                    Timer_for_Charge_Bow = 5;
                Charge_or_Fire_by_Bow = 0;
                break;
            case Samurai_Modes.Spawn_Arrow:
                if (Timer_for_Charge_Bow > 1.5f && !isHitting && Arrows > 0)
                {
                    Arrows -= 1;
                    Charge_or_Fire_by_Bow = 1;
                    isAttaking_byBow = false;
                    Acceleration_of_Arrow = Timer_for_Charge_Bow;
                    Instantiate(Arrow, Point_of_Spawn_Arrow.transform.position, Quaternion.identity);
                    Acceleration_of_Arrow = Timer_for_Charge_Bow;
                    isCanAttakingbyBow = false;
                    StartCoroutine(Enum_Timer_for_Samurai(IEnumerator_Timer_for_Samurai.can_attak_by_bow_timer));
                }
                if (Timer_for_Charge_Bow < 1.5)
                    isAttaking_byBow = false;
                Timer_for_Charge_Bow = 0;
                break;
            case Samurai_Modes.hurt:
				if (isHittinged) 
					return;
                isHittinged = true;
                StartCoroutine(Enum_Timer_for_Samurai(IEnumerator_Timer_for_Samurai.timer_for_false_ishitting));
                //isHitting = true;
                if (isCanDamagging)
                {
                    isCanDamagging = false;
                    Health -= Damage;
                    print("Damage");
                }
                if (isBombRight && FaceRight)
                    FaceRight = true;
                if (isBombRight && !FaceRight)
                    FaceRight = true;
                if (!isBombRight && !FaceRight)
                    FaceRight = false;
                if (!isBombRight && FaceRight)
                    FaceRight = false;
                Speed = 10;
                rb.linearVelocity = new Vector2(Damage * -FaceDir, rb.linearVelocity.y);
                break;
            case Samurai_Modes.go_back:
                if (isCanGoBackking)
                {
                    isCanGoBackking = false;
                    StartCoroutine(Enum_Timer_for_Samurai(IEnumerator_Timer_for_Samurai.timer_for_go_back));
                    if (isBombRight && FaceRight)
                        FaceRight = true;
                    if (isBombRight && !FaceRight)
                        FaceRight = true;
                    if (!isBombRight && !FaceRight)
                        FaceRight = false;
                    if (!isBombRight && FaceRight)
                        FaceRight = false;
                    Speed = 5;
                    print("Go Back");
                }
                rb.linearVelocity = new Vector2(Speed * -FaceDir, rb.linearVelocity.y);
                break;
			case Samurai_Modes.dead:
				isDead = true;
				StartCoroutine(Enum_Timer_for_Samurai(IEnumerator_Timer_for_Samurai.timer_for_dead));
                break;
        }
    }

    private void Set_Samurai_Mode()
    {
		//idle
		if (isGrounded && !Input.GetKey(KeyCode.D) && !Input.GetKey(KeyCode.A) && !isHitting && !isDead)
		{
			Samurai_Mode = Samurai_Modes.idle;
		}
		//Walk
        if (isGrounded && (Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.A)) && !isRunning && !isAttaking_byBow && !isJumping && !isHitting && !isDead)
        {
            Samurai_Mode = Samurai_Modes.walk;
        }
		//Run
		if (isGrounded && (Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.A)) && Input.GetKey(KeyCode.LeftControl) && Stamina != 0 && !isAttakingbySword && !isAttaking_byBow && !isJumping && !isHitting && !isDead)
		{
			Samurai_Mode = Samurai_Modes.run;
		}
		//Jump_In Sky_Fall
		if (Input.GetKeyDown(KeyCode.Space) && Can_Jumping > 0 && !isAttakingbySword && !isAttaking_byBow && !isHitting && !isDead) 
		{
			Samurai_Mode = Samurai_Modes.jump;
			rb.linearVelocity = new Vector2(rb.linearVelocity.x, Jump);
			StartCoroutine(Enum_Timer_for_Samurai(IEnumerator_Timer_for_Samurai.timer_to_minus_1_from_jump));
			//Can_Jumping -= 1;
		}
		if (isGrounded)
		{
			isJumping = false;
			Can_Jumping = 2;
			if (isWalking_was_true)
			{
				isWalking = true;
				isWalking_was_true = false;
				Samurai_Mode = Samurai_Modes.walk;
			}
			if (isRunning_was_true)
			{
				isRunning = true;
				isRunning_was_true = false;
				Samurai_Mode = Samurai_Modes.run;
			}
		}
		if (rb.linearVelocity.y > 0)
			Samurai_Mode = Samurai_Modes.jump;
		if (rb.linearVelocity.y <= 0.3 && !isGrounded)
			Samurai_Mode = Samurai_Modes.in_sky;
		if (isGrounded && isJumping)
			Samurai_Mode = Samurai_Modes.falled;
		if (!isGrounded && (Input.GetKey(KeyCode.D) || (Input.GetKey(KeyCode.A))))
		{
			if (isRunning_was_true)
				Speed = 3;
			else
				Speed = 3;
			rb.linearVelocity = new Vector2(Speed * 2 * FaceDir, rb.linearVelocity.y);
		}
		//Attak by Sword(Levels)
		if (Input.GetMouseButtonDown(0) && !isJumping && !isRunning && isCanAttakingbySword && !isAttaking_byBow && !isHitting && !isDead)
			Samurai_Mode = Samurai_Modes.attak_by_sword;
		//Attak by Bow
		if (Input.GetMouseButton(1) && !isJumping && !isRunning && !isAttakingbySword && isCanAttakingbyBow && !isHitting && !isDead)
		{
			Samurai_Mode = Samurai_Modes.attak_by_bow;
			if (Timer_for_Charge_Bow < 6)
				Timer_for_Charge_Bow += Time.deltaTime;
		}
        if (Input.GetMouseButtonUp(1))
		{
			Samurai_Mode = Samurai_Modes.Spawn_Arrow;
		}
		//Hit
		if (isHitting)
			Samurai_Mode = Samurai_Modes.hurt;
		//Go Back
		if (isGobackking)
			Samurai_Mode = Samurai_Modes.go_back;
		if (Health <= 0)
		{
			Samurai_Mode = Samurai_Modes.dead;
		}
    }

    private void Set_Animations()
	{
		//floats
		anim.SetFloat("idle/walk/run",A_F_IWR);
        anim.SetFloat("Jump/In Sky", A_F_JIsF);
		anim.SetFloat("Attak_level", Level_of_Attaks);
		anim.SetFloat("Charge_or_Fire_by_Bow", Charge_or_Fire_by_Bow);
		//bools
		anim.SetBool("isJumping", isJumping);
		anim.SetBool("isAttaking_bySword",isAttakingbySword);
		anim.SetBool("isAttaking_byBow", isAttaking_byBow);
		anim.SetBool("isHitting", isHitting);
        anim.SetBool("isGoBacking", isGobackking);
		anim.SetBool("isDead", isDead);
    }

	private void Collisions()
	{
		isGrounded = Physics2D.Raycast(transform.position, Vector2.down, GrondCheckDistance, Layer_Ground);
	}

	private void Set_Face()
	{
		if (FaceRight)
			FaceDir = 1;
		else 
			FaceDir = -1;
		transform.localScale = new Vector2(xScale * FaceDir, transform.localScale.y);

		if (isHitting)
			return;

		if (Input.GetKey(KeyCode.D))
		{
			FaceRight = true;
			FaceDir = 1;
		}
        if (Input.GetKey(KeyCode.A))
        {
			FaceRight = false;
            FaceDir = -1;
        }
		Face = FaceDir;
    }

	public IEnumerator Enum_Timer_for_Samurai(IEnumerator_Timer_for_Samurai Timer_Mode)
	{
		switch (Timer_Mode)
		{
			case IEnumerator_Timer_for_Samurai.attak_timer:
				yield return new WaitForSeconds(0.4f);
				isAttakingbySword = false;
				StartCoroutine(Enum_Timer_for_Samurai(IEnumerator_Timer_for_Samurai.can_attak_by_sword_timer));
				break;
			case IEnumerator_Timer_for_Samurai.can_attak_by_sword_timer:
				yield return new WaitForSeconds(0.1f);
                isCanAttakingbySword = true;
				break;
			case IEnumerator_Timer_for_Samurai.can_attak_by_bow_timer:
				yield return new WaitForSeconds(0.2f);
				isCanAttakingbyBow = true;
				isAttaking_byBow = false;
				break;
            case IEnumerator_Timer_for_Samurai.timer_to_minus_1_from_jump:
                yield return new WaitForSeconds(0.1f);
				Can_Jumping -= 1;
                break;
			case IEnumerator_Timer_for_Samurai.timer_for_false_ishitting:
				yield return new WaitForSeconds(0.5f);
				isHitting = false;
                isCanDamagging = true;
				yield return new WaitForSeconds(1);
				isHittinged = false;
				break;
			case IEnumerator_Timer_for_Samurai.timer_for_go_back:
				yield return new WaitForSeconds(0.2f);
				isGobackking = false;
				isCanGoBackking = true;
				break;
			case IEnumerator_Timer_for_Samurai.timer_for_dead:
				yield return new WaitForSeconds(1);
				gameObject.SetActive(false);
				break;
        }
	}

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision == null) return;

		else if (collision.GetComponent<Enemy>() != null)
		{
			if (!isEnemyDamagged && isAttakingbySword)
			{
				collision.GetComponent<Enemy>().Health -= 10;
				isEnemyDamagged = true;
			}
		}
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.blue;
        Gizmos.DrawLine(transform.position, new Vector3(transform.position.x, transform.position.y - GrondCheckDistance, transform.position.z));

    }
}
