using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Poolable : MonoBehaviour
{
    //자신이 속한 풀 참조, 상속을 통한 다양한 오브젝트 타입 구현 가능 
    protected ObjectPool m_pool;

    //오브젝트 풀을 할당하고 오브젝트를 비활성화함
    public virtual void Create(ObjectPool pool)
    {
        m_pool = pool;
        gameObject.SetActive(false);
    }

    //오브젝트를 다시 풀로 반환 (자동 반환 등 향후 확장 가능성을 위해 작성)
    public virtual void Push()
    {
        Debug.Log("Poolable Class Push()");
        m_pool.Push(this);
    }
}
