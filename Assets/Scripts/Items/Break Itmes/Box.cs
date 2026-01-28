using System.Collections;
using UnityEngine;

public class Box : MonoBehaviour
{
    private Animator anim;

    [Header("Box")]
    [SerializeField] private GameObject DropGameObject;
    [SerializeField] private GameObject Arrow_Drop;
    [SerializeField] private GameObject Coin_bag;
    [SerializeField] private int rnd_Drop;
    [SerializeField] private bool is_rnd_Drop_Coin;
    [SerializeField] private bool is_rnd_Drop;
    [SerializeField] private bool isBreaking;

    void Start()
    {
        anim = GetComponent<Animator>();
    }

    void Update()
    {
        anim.SetBool("isBreaking", isBreaking);
    }
    private void Drop_item()
    {
        if (DropGameObject != null)
            Instantiate(DropGameObject, transform.position, Quaternion.identity);
        else
        {
            if (!is_rnd_Drop)
            {
                is_rnd_Drop = true;
                rnd_Drop = Random.Range(1, 5);
                print(rnd_Drop);
                if (rnd_Drop == 2 && !is_rnd_Drop)
                {
                    is_rnd_Drop = true;
                    Instantiate(Coin_bag, transform.position, Quaternion.identity);
                }
                if (!is_rnd_Drop && rnd_Drop == 4)
                {
                    is_rnd_Drop = true;
                    Instantiate(Arrow_Drop, transform.position, Quaternion.identity);
                }
            }
        }
    }
    private IEnumerator Timer_to_Destory()
    {
        yield return new WaitForSeconds(1);
        Destroy(gameObject);
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.GetComponent<Arrow>() != null)
        {
            if (isBreaking)
                return;

            isBreaking = true;
            Drop_item();
            StartCoroutine(Timer_to_Destory());
        }
    }
}
