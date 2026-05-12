using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameOverUI : MonoBehaviour
{
    [SerializeField] private GameObject panel;

    void OnEnable()  => GameManager.OnGameOverEvent += ShowPanel;
    void OnDisable() => GameManager.OnGameOverEvent -= ShowPanel;

    void Start() => panel.SetActive(false);

    private void ShowPanel() => panel.SetActive(true);

    public void OnRestart()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void OnMainMenu()
    {
        SceneManager.LoadScene("MainMenu");
    }

    public void OnQuit()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }
}
