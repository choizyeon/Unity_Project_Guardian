using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Score : MonoBehaviour
{
    [SerializeField]Text m_Scoretext;
    [SerializeField] Text m_BestScoretext;

    private void OnEnable()
    {
        int score = (int)(GameObject.Find("MainCanvas").GetComponent<Timer>().time * 100);
        m_Scoretext.text = score.ToString();
        if (PlayerPrefs.GetInt("BestScore") < score)
        {
            GameManager.Instance.bestScore = score;
            PlayerPrefs.SetInt("BestScore", score);
        }

        m_BestScoretext.text = PlayerPrefs.GetInt("BestScore").ToString();
    }
}
