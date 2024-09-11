using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;

public class GetStar : Poolable //오브젝트 풀링 요소
{
    [SerializeField] float speed = 5f;
    
    Canvas canvas;
    RectTransform targetUI;          // 위치 기준 되는 캔버스만 부모로 가지고 있어야함
    RectTransform starCopy;          // 위치 기준 되는 캔버스만 부모로 가지고 있어야함

    float aniDuration = 0.7f;

    private void Start()
    {
        canvas = GameObject.Find("MainCanvas").GetComponent<Canvas>();
        targetUI = GameObject.Find("starImgUI").GetComponent<RectTransform>();
        starCopy = GameObject.Find("starImgUI_copy").GetComponent<RectTransform>();
    }

    void Update()
    {
        //시간 경과 시 이동 속도 변화
        speed = GameObject.Find("MapManager").GetComponent<CreatingMap>().Speed;

        if (GameManager.Instance.gameState == GameManager.GameState.playing)
            this.transform.Translate(Vector3.left * speed * Time.deltaTime, Space.World);

        //맵에서 벗어나면 obj return
        if (this.transform.position.x <= -5.5f)
            Push();
    }
    
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            if (other.GetComponent<Player>().state != Player.PlayerState.die)
            {
                StartCoroutine(MoveToUIAni());
            }
        }
    }

    private IEnumerator MoveToUIAni()
    {
        // 아이템의 월드 위치를 화면 위치로 변환 
        Vector2 itemPos = Camera.main.WorldToScreenPoint(transform.position);
        //목적지 위치
        Vector2 targetPos = targetUI.anchoredPosition;

        RectTransformUtility.ScreenPointToLocalPointInRectangle(
            canvas.GetComponent<RectTransform>(), //캔버스의 rectTransform
            itemPos, //변환할 좌표
            null, //카메라 렌더 모드에 따라 다름 
            out Vector2 localPoint); //변환된 로컬 좌표

        // 원래 있던 아이템은 화면에 안보이게 함
        Vector3 pos = transform.position;
        pos.y = -5.0f;
        transform.position = pos; 

        starCopy.localPosition = localPoint; //이동할 star의 위치를 item위치로 설정해줌 

        starCopy.DOAnchorPos(targetPos, aniDuration).SetEase(Ease.InOutQuad);

        yield return new WaitForSeconds(aniDuration);

        //ui 변화
        GameObject.Find("starText").GetComponent<StarCount>().GetSetStar += 1;
        Push(); //obj return 비활 되면 코루틴 작동x 때문에 마지막에 해줌
    }

}
