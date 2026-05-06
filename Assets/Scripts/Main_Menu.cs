using UnityEngine;
using UnityEngine.SceneManagement;

public class Main_Menu : MonoBehaviour
{
    public RoomTransitionManager roomTransitioner;
    public player player;
    public GameObject UI;
    public GameObject creditsCanvas;

    public void Start()
    {
        creditsCanvas.SetActive(false);
    }

    public void startButtonPressed()
    {
        // SceneManager.LoadScene("opeining");
        roomTransitioner.GoToRoom("opeining");
        UI.SetActive(true);
        player.inputEnabled = true;
    }

    public void creditsButtonPressed()
    {
        if (creditsCanvas.activeInHierarchy)
        {
            creditsCanvas.SetActive(false);
        } else
        {
            creditsCanvas.SetActive(true);
        }
        
    }
}
