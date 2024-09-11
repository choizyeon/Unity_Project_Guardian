using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public enum GameState
    {
        start,
        main,
        playing,
        pause,
        gameOver,
        menu,
    };

    public GameState gameState;
    public int fullHP = 2;
    public int bestScore = 0;
    public int star = 0;

    // 싱글톤 패턴 = 하나의 클래스 인스턴스만 존재하도록 보장하는 디자인 패턴
    // 인스턴스 == 객체
    // 클래스의 유일한 인스턴스를 저장하는 정적 변수
    private static GameManager _instance;
    // 인스턴스에 접근하기 위한 프로퍼티
    public static GameManager Instance
    {
        get
        {
            // 인스턴스 할당
            if (!_instance)
            {
                _instance = FindObjectOfType(typeof(GameManager)) as GameManager;

                if (_instance == null) //찾아도 존재하지 않음
                    Debug.Log("no Singleton obj");
            }
            return _instance;
        }
    }

    private void Awake()
    {
        if (_instance == null)
        {
            _instance = this;
            DontDestroyOnLoad(Instance); //씬 전환 시에도 파괴되지 않음
        }
        // 인스턴스가 존재하면 새로 생기는 인스턴스를 삭제
        else if (_instance != this)
        {
            Destroy(gameObject);
        }
        // 씬 전환돼도 현재 게임오브젝트 삭제안함
        DontDestroyOnLoad(gameObject);

        SavePref();       
    }

    public void SavePref()
    {
        if (!PlayerPrefs.HasKey("Star")) PlayerPrefs.SetInt("Star", star);
        else star = PlayerPrefs.GetInt("Star");

        if (!PlayerPrefs.HasKey("FullHP"))  PlayerPrefs.SetInt("FullHP", fullHP);
        else fullHP = PlayerPrefs.GetInt("FullHP");

        if (!PlayerPrefs.HasKey("BestScore")) PlayerPrefs.SetInt("BestScore", bestScore);
        else bestScore = PlayerPrefs.GetInt("BestScore");
    }

}
