using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    private readonly int[] targetScores = { 100, 250, 600, 1800, 4000 };

public void PlayGame(){
    int totalLevels = targetScores.Length;
    int nextLevel = -1;

    int currentLevel = PlayerPrefs.GetInt("CurrentLevel", 1);

    for (int i = currentLevel - 1; i < totalLevels; i++){
        int currentScore = PlayerPrefs.GetInt("Level" + (i + 1) + "Score", 0);
        if (currentScore < targetScores[i]){
            nextLevel = i + 1;
            break;
        }
    }

    if (nextLevel == -1){
        nextLevel = 1;
    }

    PlayerPrefs.SetInt("CurrentLevel", nextLevel);

    SceneManager.LoadScene(nextLevel);
}

    public void OpenLevels(){
        SceneManager.LoadScene(6);
    }

    public void ExitGame(){
        Application.Quit();
    }
}