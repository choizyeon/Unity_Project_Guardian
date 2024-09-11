using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/* 오브젝트 풀을 관리하는 클래스 */
public class ObjectPool : MonoBehaviour
{
    //풀에 들어갈 오브젝트
    public Poolable m_poolObj; 

    // 미리 할당할 오브젝트의 수
    public int m_allocateCount; 
    public int allocateCount { get { return m_allocateCount; } set { m_allocateCount = value; } }

    //오브젝트를 담고 있는 스택
    Stack<Poolable> m_poolStack = new Stack<Poolable>();

    void Start()
    {
        Allocate();
    }

    //오브젝트 생성
    public void Allocate()
    {
        //count 만큼 생성 후 스택에 담음
        for(int i = 0; i<m_allocateCount; i++)
        {
            Poolable allocateObj = Instantiate(m_poolObj, gameObject.transform);
            allocateObj.Create(this);
            m_poolStack.Push(allocateObj);
        }
    }

    //스택에서 오브젝트를 활성화 시킨 후 반환해줌
    public GameObject Pop()
    {
        Poolable obj = m_poolStack.Pop();
        obj.gameObject.SetActive(this);
        return obj.gameObject;
    }

    //오브젝트를 다시 비활성화 한 후 풀에 반납
    public void Push(Poolable obj)
    {
        obj.gameObject.SetActive(false);
        m_poolStack.Push(obj);
    }
}
