using UnityEngine;

public class Tigh : MonoBehaviour
{
    private SamuraiPlayer sp;

    [SerializeField] private float Damage_to_Player;
    [SerializeField] private float timer;

    [SerializeField] private bool Stay_Damaging;

    void Start()
    {
        sp = FindFirstObjectByType<SamuraiPlayer>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        timer += Time.deltaTime;
        if (collision.GetComponent<SamuraiPlayer>() != null && timer >= 0.2f)
        {
            timer = 0;
            collision.GetComponent<SamuraiPlayer>().Health -= Damage_to_Player;
        }
    }
    private void OnTriggerStay2D(Collider2D collision)
    {
        timer += Time.deltaTime;
        if (collision.GetComponent<SamuraiPlayer>() != null && timer >= 5)
        {
            if (!Stay_Damaging)
                return;
            timer = 0;
            collision.GetComponent<SamuraiPlayer>().Health -= Damage_to_Player / 2;
        }
    }
}
