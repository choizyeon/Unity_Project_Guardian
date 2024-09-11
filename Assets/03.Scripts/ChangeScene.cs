using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChangeScene : MonoBehaviour
{
    public void ChangeMainScene()
    {
        LoadingSceneManager.LoadScene("MainScene");
        GameManager.Instance.gameState = GameManager.GameState.main;
    }
    public void ChangeGameScene()
    {
        LoadingSceneManager.LoadScene("GameScene");
    }
    
    public void MainCancelButton() //메인씬에서 돌아가기 ui 버튼
    {
        gameObject.SetActive(false);
        GameManager.Instance.gameState = GameManager.GameState.main;
    }

    public void GameCancelButton() //게임씬에서 돌아가기 ui 버튼
    {
        gameObject.SetActive(false);
        GameManager.Instance.gameState = GameManager.GameState.playing;
    }

    public void ResetData()
    {
        PlayerPrefs.DeleteAll();
        LoadingSceneManager.LoadScene("MainScene");
    }

    public void ChangeStartScene()
    {
        LoadingSceneManager.LoadScene("StartScene");
        GameManager.Instance.gameState = GameManager.GameState.start;
    }

    public void ApplicationQuit()
    {
        Application.Quit();
    }
}
