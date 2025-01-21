using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Pin : MonoBehaviour
{
    private bool _done;
    public static int totalScore = 0;

    private int[] pinScores = { 10, 25, 60, 180, 400 };
    
    private int[] targetScores = { 100, 250, 600, 1800, 4000 };
    public static int currentLevelScore = 0;


    private void OnCollisionEnter(Collision collision)
    {
        if ((collision.collider.CompareTag("Ball") || collision.collider.CompareTag("Pin1")) && !_done && gameObject.CompareTag("Pin1"))
        {
            HandlePinCollision(0);
        }

        if ((collision.collider.CompareTag("Ball") || collision.collider.CompareTag("Pin2")) && !_done && gameObject.CompareTag("Pin2"))
        {
            HandlePinCollision(1);
        }

        if ((collision.collider.CompareTag("Ball") || collision.collider.CompareTag("Pin3")) && !_done && gameObject.CompareTag("Pin3"))
        {
            HandlePinCollision(2);
        }

        if ((collision.collider.CompareTag("Ball") || collision.collider.CompareTag("Pin4")) && !_done && gameObject.CompareTag("Pin4"))
        {
            HandlePinCollision(3);
        }

        if ((collision.collider.CompareTag("Ball") || collision.collider.CompareTag("Pin5")) && !_done && gameObject.CompareTag("Pin5"))
        {
            HandlePinCollision(4);
        }
    }

private void HandlePinCollision(int stageIndex)
{
    if (_done) return;

    float velocity = GetComponent<Rigidbody>().velocity.magnitude;

    if (velocity < 10)
    {
        int pinScore = pinScores[stageIndex];

        int currentSceneIndex = SceneManager.GetActiveScene().buildIndex;
        int maxScoreForLevel = targetScores[currentSceneIndex - 1];
        int currentScore = PlayerPrefs.GetInt("Level" + currentSceneIndex + "Score", 0);
        int newScore = Mathf.Min(currentScore + pinScore, maxScoreForLevel);

        PlayerPrefs.SetInt("Level" + currentSceneIndex + "Score", newScore);
        currentLevelScore = newScore;

        GameObject.FindGameObjectWithTag("Poing").GetComponent<TextMeshProUGUI>().text = $"Level Score: {currentLevelScore}";
        _done = true;

        CheckAndLoadNextLevel();
    }
}



    private void CheckAndLoadNextLevel()
    {
        int currentSceneIndex = SceneManager.GetActiveScene().buildIndex;

        if (currentSceneIndex >= 1 && currentSceneIndex <= targetScores.Length)
        {
            Debug.Log("Current Level Score: " + currentLevelScore);
            Debug.Log("Target Score for this level: " + targetScores[currentSceneIndex - 1]);

            if (currentLevelScore >= targetScores[currentSceneIndex - 1])
            {
                LoadNextLevel();
            }
            else
            {
                Debug.Log("Keep trying!");
            }
        }
        else
        {
            Debug.LogError("Invalid scene index");
        }
    }

    private void LoadNextLevel()
{
    int currentSceneIndex = SceneManager.GetActiveScene().buildIndex;

    Debug.Log("Next Scene Index: " + (currentSceneIndex + 1));

    int nextSceneIndex = currentSceneIndex + 1;

    if (nextSceneIndex <= 5)
    {
        StartCoroutine(GenerateFeedBackAndLoadLevel(nextSceneIndex));
    }
    else
    {
        SceneManager.LoadScene(0);
    }
}

private IEnumerator GenerateFeedBackAndLoadLevel(int nextSceneIndex)
{
    Ball ball = FindObjectOfType<Ball>();

    if (ball != null)
    {
        ball.GenerateFeedBack();
    }

    yield return new WaitForSecondsRealtime(2);

    SceneManager.LoadScene(nextSceneIndex);
}

}
