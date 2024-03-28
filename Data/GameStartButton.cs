using UnityEngine;
using UnityEngine.SceneManagement;
public class GameStartButton : MonoBehaviour
{
    public void LoadWorkRoomScene()
    {
        SceneManager.LoadScene("WorkRoom");
    }
}
