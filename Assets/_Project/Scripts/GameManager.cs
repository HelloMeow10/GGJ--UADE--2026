using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static Assassin SelectedAssassin = Assassin.Jorge;

    public static WaitForSeconds ContentSizeFitterFix = new(0.1f);
}