using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Collect_Arrow : MonoBehaviour {

    private SamuraiPlayer sp;

    [Header("Collectable Arrow")]
    [SerializeField] private float Collect_Radius;
    [SerializeField] private bool isCollectSamurai;
    [SerializeField] private LayerMask Samurai_Layer;

    void Start()
    {
        sp = FindFirstObjectByType<SamuraiPlayer>();
        StartCoroutine(Destroy_Collectable_Arrow());
    }

    void Update()
    {
        isCollectSamurai = Physics2D.OverlapCircle(transform.position, Collect_Radius, Samurai_Layer);

        if (isCollectSamurai && sp.Arrows < sp.Max_Arrows)
        {
            sp.Arrows += 1;
            Destroy(gameObject);
        }
    }
    private IEnumerator Destroy_Collectable_Arrow()
    {
        yield return new WaitForSeconds(20);
        Destroy (gameObject);
    }
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(transform.position, Collect_Radius);
    }
}
