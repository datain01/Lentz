using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneInitializer : MonoBehaviour
{
    public GameObject GlassesPrefab;
    public GameObject LensLeftPrefab;
    public GameObject LensRightPrefab;

    

    void Start()
    {
        // 현재 씬의 이름을 가져옵니다.
        string sceneName = SceneManager.GetActiveScene().name;


        if (sceneName == "WorkRoom")
        {
            // WorkRoom 씬에서의 초기화 로직
            Instantiate(LensLeftPrefab, new Vector3(-7, 0, 0), Quaternion.identity);
            Instantiate(LensRightPrefab, new Vector3(-7, 0, 0), Quaternion.identity);
        }
        else if (sceneName == "SeperateRoom")
        {

            GameObject glassesInstance = Instantiate(GlassesPrefab, new Vector3(0, 0, 0), Quaternion.identity);

            glassesInstance.transform.localScale = new Vector3(2, 2, 2); 

        }
    }
}
