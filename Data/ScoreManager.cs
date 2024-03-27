using UnityEngine;

public class ScoreManager : MonoBehaviour
{
    public static ScoreManager Instance;

    public int Score { get; private set; }

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else if (Instance != this)
        {
            Destroy(gameObject);
        }
    }

    public void AddScore(int amount)
    {
        Score += amount;
        Debug.Log("Score added: " + amount + ". Current Score: " + Score);
    }

    public void SubtractScore(int amount)
    {
        Score -= amount;
        Debug.Log("Score subtracted: " + amount + ". Current Score: " + Score);
    }
}
