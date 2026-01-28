using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bomb_and_TNTs : MonoBehaviour {

    [Header("Bomb or TNT")]
    [SerializeField] private float Time_to_Booom;
    [Header("Booom Game Object")]
    [SerializeField] private GameObject Booom;


    void Start()
    {
        StartCoroutine(Timer_for_Booom());
    }

    private IEnumerator Timer_for_Booom()
    {
        yield return new WaitForSeconds(Time_to_Booom);
        Instantiate(Booom, transform.position, Quaternion.identity);
        Destroy(gameObject);
    }
}
