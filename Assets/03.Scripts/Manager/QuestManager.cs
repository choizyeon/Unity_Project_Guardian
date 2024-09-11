using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuestManager : MonoBehaviour
{
    public int questId;
    public int questActionIndex;
    [SerializeField] GameObject[] m_questObject;
    Dictionary<int, QuestData> m_questList;

    public static QuestManager instance = null;
    private static QuestManager _instance;

    public static QuestManager Instance
    {
        get
        {
            if (!_instance)
            {
                _instance = FindObjectOfType(typeof(QuestManager)) as QuestManager;

                if (_instance == null)
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
            DontDestroyOnLoad(Instance);
        }
        else if (_instance != this)
        {
            Destroy(gameObject);
        }
        DontDestroyOnLoad(gameObject);

        GenerateQuest();

    }

    void GenerateQuest()
    {
        m_questList = new Dictionary<int, QuestData>();

        m_questList.Add(10, new QuestData("공주님의 부탁", new int[] { 1000, 1000 }));
        m_questList.Add(20, new QuestData("린다에게 조언 듣기", new int[] { 3000, 2000, 1000 }));
        m_questList.Add(30, new QuestData("공주님에게 별 갖다주기", new int[] { 1000 }));
        m_questList.Add(40, new QuestData("첫번 째 퀘스트 끝", new int[] { 0 }));

        if (!PlayerPrefs.HasKey("QuestID"))
            PlayerPrefs.SetInt("QuestID", questId);
        else
            questId = PlayerPrefs.GetInt("QuestID");

       // if (!PlayerPrefs.HasKey("QuestIdx"))
       //     PlayerPrefs.SetInt("QuestIdx", questActionIndex);
       // else
       //     questActionIndex = PlayerPrefs.GetInt("QuestIdx");
    }

    public int GetQuestDialogIndex(int id)
    {
        return questId + questActionIndex;
    }

    public void CheckQuest(int id)
    {
        questId = PlayerPrefs.GetInt("QuestID");

        //다음 대화로
        if (id == m_questList[questId].npcId[questActionIndex] && QuestCondition())
        {
            questActionIndex++;
            //PlayerPrefs.SetInt("QuestIdx", questActionIndex);
        }
        ControlObject();

        //퀘스트 마지막 대화 후 다음 퀘스트로
        if (questActionIndex == m_questList[questId].npcId.Length && QuestCondition())
            NextQuest();

        //return m_questList[questId].questName;
    }

    void NextQuest()
    {
        Debug.Log("Next Quest");
        questId += 10;
        PlayerPrefs.SetInt("QuestID", questId);
        questActionIndex = 0;
        //PlayerPrefs.SetInt("QuestIdx", questActionIndex);
    }

    void ControlObject()
    {
        switch (questId)
        {
            case 10:
                break;

            case 20:
                if (questActionIndex == 2) //린다랑 대화 끝났을 때 숲으로 가는 길을 열어준다.
                {
                    m_questObject[0].SetActive(false); //0: roadblock
                    PlayerPrefs.SetInt("RoadBlock", 1);
                }
                break;

            case 30:
                if (questActionIndex == 1) //공주랑 대화가 끝나면 
                {
                    int star = PlayerPrefs.GetInt("Star") - 5;
                    PlayerPrefs.SetInt("Star", star);
                    int hp = PlayerPrefs.GetInt("FullHP") + 1;
                    PlayerPrefs.SetInt("FullHP", hp);

                    GameObject.Find("Canvas").GetComponent<StarTextMain>().SetStar();
                }
                break;
            case 40:
                break;
        }
    }

    //퀘스트 시작 조건
    bool QuestCondition()
    {
        bool result = true;

        switch (questId) //퀘스트 성공 조건이 있는 부분 작성
        {
            case 10:
                break;

            case 20:
                if (questActionIndex == 3 && PlayerPrefs.GetInt("Star") < 5)
                    result = false;
                break;

            case 30:
                break;
        }
        if (result == false) questActionIndex--;
        return result;
    }

    //씬 전환하면 GameObject 다시 셋팅
    public void SetObject()
    {
        m_questObject[0] = GameObject.Find("RoadBlock");
        if (PlayerPrefs.GetInt("RoadBlock") == 1)
        {
                m_questObject[0].SetActive(false);
        }
    }
}