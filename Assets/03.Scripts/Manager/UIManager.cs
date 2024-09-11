using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class UIManager : MonoBehaviour
{
    public Text dialogText;
    public Text NPCName;
    public GameObject dialogPanel;
    public int dialogIdx = 0;
    public bool isDialogAction = true;
    public GameObject scanObject;
    public DialogueData dialogue;

    bool isTyping = false; // 현재 대화 텍스트 출력 중 여부 확인

    public static UIManager instance = null;
    private static UIManager _instance;

    public static UIManager Instance
    {
        get
        {
            if (!_instance)
            {
                _instance = FindObjectOfType(typeof(UIManager)) as UIManager;

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
    }

    public void DialogueAction(GameObject scanObj) //대화 창 띄우기
    {
        scanObject = scanObj;
        NPCData npcData = scanObject.GetComponent<NPCData>();
        NPCName.text = npcData.ObjName;

        Dialogue(npcData.id);

        dialogPanel.SetActive(isDialogAction);
    }

    void Dialogue(int id)
    {
        int questDialogIdx = QuestManager.Instance.GetQuestDialogIndex(id);
        string dialogData = dialogue.GetDialogue(id + questDialogIdx, dialogIdx);

        if (isTyping) //대화가 출력중일 때 버튼을 또 누르면
        {
            //대화가 시작할 때 dialogIdx++; 되므로 dialogIdx - 1을 넣어줘야함
            dialogData = dialogue.GetDialogue(id + questDialogIdx, dialogIdx - 1);
            DOTween.Kill(dialogText); // 현재 텍스트 애니메이션을 중지
            dialogText.text = dialogData; // 현재 대화 텍스트 즉시 표시
            isTyping = false;
            return;
        }

        if (dialogData == null) //대화 끝났을 때
        {
            isDialogAction = false;
            dialogIdx = 0;
            QuestManager.Instance.CheckQuest(id);
            dialogText.text = "";

            return;
        }

        dialogText.text = "";
        isTyping = true;

        // 텍스트 애니메이션, 완료 시 플래그를 false로 변경
        dialogText.DOText(dialogData, dialogData.Length * 0.08f).OnComplete(() => isTyping = false);

        isDialogAction = true;
        dialogIdx++;
    }

    public void SetObject()
    {
        GameObject canvas = GameObject.Find("Canvas");
        dialogText = canvas.transform.Find("DialogueBoxImage").Find("DialogueText").GetComponent<Text>();
        NPCName = canvas.transform.Find("DialogueBoxImage").Find("NPCNameText").GetComponent<Text>();
        dialogPanel = canvas.transform.Find("DialogueBoxImage").gameObject;
        dialogue = canvas.GetComponent<DialogueData>();
    }
}
