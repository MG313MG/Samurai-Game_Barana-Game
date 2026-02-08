using Unity.Collections;
using UnityEngine;

public class Health_Bar_for_Enemies : MonoBehaviour, Show_and_Hide
{
    private Enemy en;
    private float Enemy_Health;

    [Header("Health Bar")]
    [SerializeField] private GameObject Health_Bar;
    [SerializeField] private GameObject BG_Health_Bar;
    [SerializeField] private GameObject Position_of_Health_Bar;

    private float EN_Health;
    private float First_EN_Health;

    private GameObject Instantiate_Health_Bar;
    private GameObject Instantiate_BG_Health_Bar;

    void Start()
    {
        en = GetComponent<Enemy>();

        First_EN_Health = en.Health;

        Instantiate_Health_Bar = Instantiate(Health_Bar, Position_of_Health_Bar.transform.position, Quaternion.identity);
        Instantiate_BG_Health_Bar = Instantiate(BG_Health_Bar, Position_of_Health_Bar.transform.position, Quaternion.identity);

        Instantiate_Health_Bar.transform.localScale = new Vector2(en.transform.localScale.x / 10, Instantiate_Health_Bar.transform.localScale.y);
        Instantiate_BG_Health_Bar.transform.localScale = new Vector2(en.transform.localScale.x / 10, Instantiate_BG_Health_Bar.transform.localScale.y);

        Instantiate_Health_Bar.transform.SetParent(transform);
        Instantiate_BG_Health_Bar.transform.SetParent(transform);

        Instantiate_Health_Bar.GetComponent<SpriteRenderer>().enabled = false;
        Instantiate_BG_Health_Bar.GetComponent<SpriteRenderer>().enabled = false;
    }

    void Update()
    {
        EN_Health = en.Health;

        Instantiate_Health_Bar.transform.localScale = new Vector2(Mathf.Lerp(0,0.5f, EN_Health / (First_EN_Health * 5)), Instantiate_Health_Bar.transform.localScale.y);
    }

    public void Show()
    {
        Instantiate_Health_Bar.GetComponent<SpriteRenderer>().enabled = true;
        Instantiate_BG_Health_Bar.GetComponent<SpriteRenderer>().enabled = true;
    }
    public void Hide()
    {
        Instantiate_Health_Bar.GetComponent<SpriteRenderer>().enabled = false;
        Instantiate_BG_Health_Bar.GetComponent <SpriteRenderer>().enabled = false;
    }
}
