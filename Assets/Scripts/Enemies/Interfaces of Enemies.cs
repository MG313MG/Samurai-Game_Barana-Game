using UnityEngine;

public interface Death_and_Hurt_Handler
{
    void OnHurt();

    void OnDeath();
}
public interface Defend_Handler
{
    void OnDefend();
}
public interface Change_Face
{
    void OnChangeFace();
}
public interface Show_and_Hide
{
    void Show();

    void Hide();
}
