using UnityEngine;

public class Tigh : MonoBehaviour
{
    private SamuraiPlayer sp;

    private float timer;

    void Start()
    {
        sp = FindFirstObjectByType<SamuraiPlayer>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.GetComponent<SamuraiPlayer>() != null)
        {
            collision.GetComponent<SamuraiPlayer>().Health -= 3;
        }
    }
    private void OnTriggerStay2D(Collider2D collision)
    {
        timer += Time.deltaTime;
        if (collision.GetComponent<SamuraiPlayer>() != null && timer > 4)
        {
            timer = 0;
            collision.GetComponent<SamuraiPlayer>().Health -= 1;
        }
    }
}
