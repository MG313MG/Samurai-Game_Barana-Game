using System.Collections;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem.iOS;

public class Enemy : MonoBehaviour
{
    Death_and_Hurt_Handler death_and_hurt_handler;

    Defend_Handler defend_handler;

    Change_Face change_face;

    Show_and_Hide show_and_hide;

    [Header("Enemy")]
    public float Health;
    public bool isDamaged_by_Arrow;
    //floats
    [SerializeField] private float Previous_Health;
    [SerializeField] private float Player_Check_Distance; 
    [SerializeField] private float rnd_Hurt_or_Defend;
    private float FaceDir;
    //bools
    [SerializeField] private bool isPlayer_Back;
    [SerializeField] private bool isHitted;
    public bool isDefending;
    private bool isrnd_Selected;
    //layers
    [SerializeField] private LayerMask Player_Layer;
    [Header("Game Objects")]
    [SerializeField] private GameObject Position_of_Check_Player_is_Back;


    private void Start()
    {
        death_and_hurt_handler = GetComponent<Death_and_Hurt_Handler>();
        defend_handler = GetComponent<Defend_Handler>();
        change_face = GetComponent<Change_Face>();
        show_and_hide = GetComponent<Show_and_Hide>();
        Previous_Health = Health;
    }

    private void Update()
    {
        if (transform.localScale.x > 0)
            FaceDir = 1;
        else if (transform.localScale.x < 0)
            FaceDir = -1;

        isPlayer_Back = Physics2D.Raycast(Position_of_Check_Player_is_Back.transform.position, Vector2.right * FaceDir, Player_Check_Distance, Player_Layer);

        if (Health != Previous_Health && Health > 0)
        {
            if (show_and_hide != null)
            {
                show_and_hide.Show();
                StartCoroutine(Hide_Health_Bar());
            }
            isHitted = true;
            StartCoroutine(False_is_Hitted());
            if (change_face != null && isHitted && isPlayer_Back)
            {
                change_face.OnChangeFace();
            }
            if (death_and_hurt_handler != null && defend_handler != null && !isPlayer_Back && !isDamaged_by_Arrow)
            {
                rnd_Hurt_or_Defend = Random.Range(1, 4);
                if (rnd_Hurt_or_Defend == 2)
                {
                    isDefending = true;
                    defend_handler.OnDefend();
                    StartCoroutine(False_is_Hitted());
                }
                else if (!isDefending)
                    death_and_hurt_handler.OnHurt();
            }
            else if (death_and_hurt_handler != null && !isDefending)
                    {
                        death_and_hurt_handler.OnHurt();
                    }
            Previous_Health = Health;
            if (isDamaged_by_Arrow)
                StartCoroutine(False_is_Hitted());
        }

        if (Health <= 0)
        {
            if (death_and_hurt_handler != null)
            {
                death_and_hurt_handler.OnDeath();
                show_and_hide.Hide();
            }
        }
    }

    private IEnumerator False_is_Hitted()
    {
        yield return new WaitForSeconds(1);
        isDamaged_by_Arrow = false;
        isHitted = false;
        isDefending = false;
    }
    private IEnumerator Hide_Health_Bar()
    {
        yield return new WaitForSeconds(5);
        show_and_hide.Hide();
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.black;
        Gizmos.DrawLine(Position_of_Check_Player_is_Back.transform.position, new Vector2(Position_of_Check_Player_is_Back.transform.position.x + (Player_Check_Distance * FaceDir), Position_of_Check_Player_is_Back.transform.position.y));
    }
}
