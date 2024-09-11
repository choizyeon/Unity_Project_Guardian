using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;

public class GameStartText : MonoBehaviour
{
    public GameObject m_startText;
    public GameObject m_readyText;
    float aniTime = 0.8f;

    void OnEnable()
    {
        // m_ready = GameObject.Find("UI").transform.Find("readyText").gameObject;
        // m_start = GameObject.Find("UI").transform.Find("startText").gameObject;
        GameManager.Instance.gameState = GameManager.GameState.main;
        StartCoroutine(StartGame());
    }

    IEnumerator StartGame()
    {
        yield return StartCoroutine(TextScaleAni(m_readyText.gameObject, aniTime));

        yield return new WaitForSeconds(aniTime); // 애니메이션 대기

        yield return StartCoroutine(TextScaleAni(m_startText.gameObject, aniTime));

        if (GameManager.Instance.gameState != GameManager.GameState.pause)
            GameManager.Instance.gameState = GameManager.GameState.playing; //게임 state playing 으로 바꿈
    }

    IEnumerator TextScaleAni(GameObject obj, float time)
    {
        obj.transform.localScale = Vector3.zero; // 초기 크기 0으로 설정
        obj.gameObject.SetActive(true); // 텍스트 활성화
        obj.transform.DOScale(Vector3.one, time).SetEase(Ease.OutQuad); // time 동안 크기를 1로

        yield return new WaitForSeconds(time); // 애니메이션 대기

        time = (time - 0.5f >= 0.1f) ? time - 0.5f : 0.1f;

        obj.transform.DOScale(Vector3.zero, time).SetEase(Ease.InQuad).OnComplete(()
            => obj.gameObject.SetActive(false)); // 0.3초 동안 크기를 0으로 줄이고 비활성화
    }
}
