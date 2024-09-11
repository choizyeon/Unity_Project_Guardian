﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
public class LoadingSceneManager : MonoBehaviour
{
    public static string nextScene;
    [SerializeField] Image progressBar;
    private void Start() {
        StartCoroutine(LoadScene()); 
    }
    public static void LoadScene(string sceneName) 
    { 
        nextScene = sceneName; 
        SceneManager.LoadScene("LoadingScene"); 
    }
    IEnumerator LoadScene() 
    { 
        yield return null;
        switch (nextScene)
        {
            case "MainScene":
                GameManager.Instance.gameState = GameManager.GameState.menu;
                break;

            case "GameScene":
                GameManager.Instance.gameState = GameManager.GameState.start;
                break;
        }

        AsyncOperation op = SceneManager.LoadSceneAsync(nextScene); 
        op.allowSceneActivation = false;
        float timer = 0.0f; 
        while (!op.isDone) //씬 호출 중
        { 
            yield return null;
            timer += Time.deltaTime;
            if (op.progress < 0.9f) //로딩 중
            { 
                progressBar.fillAmount = Mathf.Lerp(progressBar.fillAmount, op.progress, timer);
                if (progressBar.fillAmount >= op.progress) 
                    timer = 0f; 
            } 
            else 
            { 
                progressBar.fillAmount = Mathf.Lerp(progressBar.fillAmount, 1f, timer); 
                if (progressBar.fillAmount == 1.0f) 
                { 
                    op.allowSceneActivation = true;  //씬 활성화
                    yield break; 
                } 
            } 
        } 
    }
}
