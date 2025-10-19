using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class WinSceneController : MonoBehaviour
{
    [SerializeField] private Text timeText;

    void Start() {
        int finalTime = PlayerPrefs.GetInt("FinalTime", 0);
        timeText.text = "Your Time: " + finalTime.ToString() + " s";
        PlayerPrefs.SetInt("FinalTime", 0);
    }

    public void playAgain() {
        SceneManager.LoadScene("MainScene");
    }

    public void goToMainMenu() {
        SceneManager.LoadScene("HomeScene");
    }


}
