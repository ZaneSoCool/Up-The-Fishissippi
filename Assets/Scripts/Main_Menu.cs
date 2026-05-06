using UnityEngine;

public class Main_Menu : MonoBehaviour
{
    public RoomTransitionManager roomTransitioner;
    public player player;
    public GameObject UI;

    public void startButtonPressed()
    {
        roomTransitioner.GoToRoom("level_1");
        player.gameObject.SetActive(true);
        UI.SetActive(true);
    }
}
