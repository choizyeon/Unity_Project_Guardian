using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMove : MonoBehaviour
{
    //플레이어 방향
    enum PlayerLook
    {
        front,
        back,
        left,
        right,
        climb,
    };
    PlayerLook m_playerLook = PlayerLook.right;         // 시작 시 오른쪽 보고있음
    Vector3 m_dirVec;                                   // 캐릭터 방향에 맞춰 벡터 설정
    GameObject m_playerImage;                           // 캐릭터 이미지
    
    Animator m_playerAni;                               // 플레이어 애니메이션

    [SerializeField] GameObject m_idleFaceImage;        // 얼굴 이미지 따로 설정해줌
    [SerializeField] GameObject m_runFaceImage;         // 얼굴 이미지 따로 설정해줌

    Rigidbody m_rigidbody;
    float m_speed = 5.0f;                               // 플레이어 속도
    float h, v;                                         // 수평, 수직 값
    float m_prevY;                                      // 사다리 오를 때 이전 위치 저장

    [SerializeField] GameObject m_dialogueStartBtn;     // 대화 시작 버튼
    [SerializeField] GameObject m_PanelJoyStick;        // 조이스틱
    GameObject m_scanObject = null;                     // 플레이어가 스캔하고 있는 오브젝트

    void Start()
    {
        m_playerImage = transform.Find("Knight").gameObject;
        m_playerAni = this.GetComponent<Animator>();
        m_rigidbody = gameObject.GetComponent<Rigidbody>();

        //조이스틱 입력 처리 , 이벤트 핸들러 추가
        m_PanelJoyStick.GetComponent<PanelJoyStick>().EventStickMove += OnEventStickMove;
    }

    void OnEventStickMove(Vector3 dir)
    {
        //조이스틱의 이동 방향 3D 좌표계로 변환
        Vector3 worldDir = new Vector3();
        worldDir.x = dir.x;
        worldDir.y = 0f;
        worldDir.z = dir.y;

        //대화창이 닫혀있을 때만 h, v 값 업데이트
        h = UIManager.Instance.isDialogAction ? 0 : dir.x;
        v = UIManager.Instance.isDialogAction ? 0 : dir.y;
    }

    void Update()
    {
    }

    private void FixedUpdate()
    {
        //레이캐스트로 npc 스캔
        ScanObjecct();

        //조이스틱 사용하지 않고 있을 경우 (키보드 이동)
        if (!m_PanelJoyStick.GetComponent<PanelJoyStick>().isStickDown)
        {
            //대화창 열려있는 경우 입력 방향 무시
            h = UIManager.Instance.isDialogAction ? 0 : Input.GetAxis("Horizontal");
            v = UIManager.Instance.isDialogAction ? 0 : Input.GetAxis("Vertical");
        }

        // 이동, 이미지 변경, 애니메이션 변경, npc와의 상호작용
        if (m_playerLook != PlayerLook.climb)
        {
            Move();
            ChangeImage();
            ChangeAni();
            NPCAction();
        }
        else
        {
            Climbimg();
        }
    }

    // 플레이어 이동
    void Move()
    {
        m_rigidbody.MovePosition(this.transform.position + 
            new Vector3(h * m_speed, 0, v * m_speed) * Time.deltaTime);

        if (h < 0 && Mathf.Abs(v) < 0.7f) m_playerLook = PlayerLook.left;
        else if (h > 0 && Mathf.Abs(v) < 0.7f) m_playerLook = PlayerLook.right;
        else if (v < 0) m_playerLook = PlayerLook.front;
        else if (v > 0) m_playerLook = PlayerLook.back;
    }

    // 플레이어 이미지 변경
    void ChangeImage()
    {
        switch (m_playerLook)
        {
            case PlayerLook.front:
                m_dirVec = Vector3.back;
                SetImage("Knight_front", 40, 0, 0);
                break;
            case PlayerLook.back:
                m_dirVec = Vector3.forward;
                SetImage("Knight_back", 40, 0, 0);
                break;
            case PlayerLook.left:
                m_dirVec = Vector3.left;
                SetImage("Knight", -40, 180, 0);
                break;
            case PlayerLook.right:
                m_dirVec = Vector3.right;
                SetImage("Knight", 40, 0, 0);
                //m_playerAni.SetInteger("look", 1);
                break;
        }
    }

    // 플레이어 상/하/좌/우 이미지 셋팅
    void SetImage(string s, float a, float b, float c)
    {
        m_playerImage.SetActive(false);
        m_playerImage = transform.Find(s).gameObject;
        m_playerImage.SetActive(true);
        m_playerImage.transform.rotation = Quaternion.Euler(a, b, c);
    }

    // 플레이어 애니메이션 변경
    void ChangeAni()
    {
        //idle 애니메이션
        if (h == 0 && v == 0)
        {
            m_playerAni.SetInteger("state", 0);
            m_runFaceImage.SetActive(false);
            m_idleFaceImage.SetActive(true);
        }
        //run 애니메이션
        else
        {
            m_playerAni.SetInteger("state", 1);
            m_idleFaceImage.SetActive(false);
            m_runFaceImage.SetActive(true);
        }
    }

    //사다리 오르기
    private void OnCollisionStay(Collision collision)
    {
        if (collision.transform.tag == "Ladder")
        {
            m_playerLook = PlayerLook.climb;
            m_prevY = this.transform.position.y;
        }

    }

    void Climbimg()
    {
        m_rigidbody.useGravity = false;
        m_rigidbody.MovePosition(this.transform.position + new Vector3(0, m_speed, 0) * Time.deltaTime);
        if (transform.position.y >= m_prevY + 1.5f)
        {
            m_rigidbody.useGravity = true;
            m_playerLook = PlayerLook.back;
        }

    }

    //레이캐스트로 NPC 오브젝트 스캔
    void ScanObjecct()
    {
        Debug.DrawRay(m_rigidbody.position, m_dirVec * 1.5f, new Color(1, 0, 1));
        RaycastHit rayHit;

        if (Physics.Raycast(transform.position, m_dirVec, out rayHit, 1.5f))
        {
            if (rayHit.collider.tag == "NPC")
            {
                m_scanObject = rayHit.collider.gameObject;
                m_dialogueStartBtn.SetActive(true);
            }
        }
        else
        {
            m_scanObject = null;
            m_dialogueStartBtn.SetActive(false);
        }
    }

    void NPCAction()
    {
        if(Input.GetKeyDown(KeyCode.Space) && m_scanObject != null)
        {
            UIManager.Instance.DialogueAction(m_scanObject);
        }
    }

    public void DialogueButton()
    {
        if (m_scanObject != null)
        {
            UIManager.Instance.DialogueAction(m_scanObject);
        }
    }
}
