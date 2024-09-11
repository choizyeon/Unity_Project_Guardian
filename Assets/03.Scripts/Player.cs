using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/* Game Scene 에서 사용되는 Player */
public class Player : MonoBehaviour
{

    public enum PlayerState
    {
        idle,
        move,
        hit,
        die,
    };

    PlayerState m_playerState;
    Animator m_playerAnimator;
    Rigidbody m_rigidbody;

    int m_playerPos;
    Vector3[] m_playerPosIdx = { new Vector3(1f, 1.025f, -6f), new Vector3(1f, 1.025f, -10f), new Vector3(1f, 1.025f, -14f) };

    float m_speed = 0.15f;
    int m_hp;

    Coroutine m_coroutine = null;
    [SerializeField] GameObject m_runFace;

    public PlayerState state { get { return m_playerState; } set { m_playerState = value; } }
    public int hp { get { return m_hp; } set { m_hp = value; } }
 
    void Start()
    {
        //m_hp = GameManager.Instance.fullHP;
        m_hp = PlayerPrefs.GetInt("FullHP");
        m_playerState = PlayerState.idle;
        m_playerAnimator = this.GetComponent<Animator>();
        m_rigidbody = gameObject.GetComponent<Rigidbody>();

        m_playerPos = 1;
    }

    void Update()
    {
        if (GameManager.Instance.gameState == GameManager.GameState.pause) return;

            PlayerMove();

        if (GameManager.Instance.gameState == GameManager.GameState.playing && m_playerState == PlayerState.idle)
            ChangeState(PlayerState.move);
        if (m_playerState != PlayerState.die && m_hp <= 0)
            ChangeState(PlayerState.die);
    }

    void PlayerMove()
    {
     
       if (Input.GetKeyDown("up"))
       {
           PlayerUp();
       }

       if (Input.GetKeyDown("down"))
       {
           PlayerDown();
       }

        this.transform.position 
            = Vector3.Lerp(this.transform.position, m_playerPosIdx[m_playerPos], m_speed);
    }

    public void PlayerUp()
    {
        if (m_playerState == PlayerState.move || m_playerState == PlayerState.hit)
        {
            switch (m_playerPos)
            {
                case 0:
                    return;
                case 1:
                    m_playerPos = 0;
                    break;
                case 2:
                    m_playerPos = 1;
                    break;
            }
        }
        
    }
    public void PlayerDown()
    {
        if (m_playerState == PlayerState.move || m_playerState == PlayerState.hit)
        {
            switch (m_playerPos)
            {
                case 0:
                    m_playerPos = 1;
                    return;
                case 1:
                    m_playerPos = 2;
                    break;
                case 2:
                    return;
            }
        }
    }

    public void ChangeState(PlayerState state)
    {
        if (m_playerState == state) return;
        m_playerState = state;

        switch (state)
        {
            case PlayerState.idle:
                m_playerAnimator.SetInteger("state", 0);
                break;

            case PlayerState.move:
                m_playerState = PlayerState.move;
                m_playerAnimator.SetInteger("state", 1);
                break;

            case PlayerState.hit:
                m_playerState = PlayerState.hit;
                m_playerAnimator.SetInteger("state", 2);
                if (m_coroutine == null)
                    StartCoroutine(PlayerHit());
                break;

            case PlayerState.die:
                m_playerState = PlayerState.die;
                m_playerAnimator.SetInteger("state", 3);
                GameManager.Instance.gameState = GameManager.GameState.gameOver;
                break;
        }
    }

    private IEnumerator PlayerHit()
    {
        if(m_hp <= 0)
        {
            m_coroutine = null;
            yield return null;
        }

        var wfs = new WaitForSeconds(0.07f);
        for (int i = 0; i<10; i++)
        {
            ChangeChildAlpha(0.3f);
            yield return wfs;
            ChangeChildAlpha(1f);
            yield return wfs;
        }

        ChangeState(PlayerState.move);

        m_coroutine = null;
    }

    //무적상태일 때 캐릭터의 투명도 변화
    void ChangeChildAlpha(float n)
    {
        Transform[] allChildren = GetComponentsInChildren<Transform>();
        foreach (Transform child in allChildren)
        {
            if (child.GetComponent<SpriteRenderer>() != null)
            {
                SpriteRenderer spr = child.GetComponent<SpriteRenderer>();
                Color color = spr.color;
                color.a = n;
                spr.color = color;
            }
        }
    }
}
