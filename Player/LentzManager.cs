using UnityEngine;

public class LentzManager : MonoBehaviour
{
    public static LentzManager Instance { get; private set; }
    public int lentzAmount; // Inspector에서 조절 가능
    public LentzDisplay lentzDisplay; // Inspector에서 LentzDisplay 컴포넌트 할당

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

        UpdateLentzDisplay(); // 초기 UI 업데이트
    }

    public void AddLentz(int amount)
    {
        lentzAmount += amount;
        UpdateLentzDisplay();
    }

    public void UseLentz(int amount)
    {
        if (lentzAmount >= amount)
        {
            lentzAmount -= amount;
            UpdateLentzDisplay();
        }
        else
        {
            Debug.Log("Not enough Lentz");
        }
    }

    private void UpdateLentzDisplay()
    {
        if (lentzDisplay != null) // lentzDisplay가 할당되었는지 확인
        {
            lentzDisplay.UpdateDisplay(lentzAmount);
        }
    }
}
