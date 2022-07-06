using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeroController : MonoBehaviour
{
    #region Constants and Fields
    [SerializeField]
    Vector3 m_dir;
    [SerializeField]
    float m_speed = 4f;
    [SerializeField]
    Transform m_firePos;
    [SerializeField]
    GameObject m_firePrefab;
    [SerializeField]
    GameObject[] m_fxObjects;
    Animator m_animator;
    GameObjectPool<FireController> m_firePool;

    Vector3 m_startPos;
    float m_moveVal;
    bool m_isDrag;
    int m_buffState;
    #endregion
    #region Properties
    public int curBuffSet { get { return m_buffState; } set { m_buffState = value; } }

    public Transform HeroTransform { get { return this.transform; } }
    #endregion
    #region Public Methods
    public void RemoveFire(FireController fire)
    {
        fire.gameObject.SetActive(false);
        m_firePool.Set(fire);
    }
    public void SetFxInvincible()
    {
        m_fxObjects[1].SetActive(true);
        m_animator.Play("Invincible", 0, 0f);
        CancelInvoke("CreateFire");
    }
    public void SetFxMagnet(bool isOn)
    {
        m_fxObjects[0].SetActive(isOn);
    }

    public void ResetHero()
    {
        m_fxObjects[1].SetActive(false);
        m_animator.Play("Idle");
        InvokeRepeating("CreateFire", 1f, 0.2f);
    }


    #endregion
    #region Methods

    public void Setdie()
    {
        CancelInvoke("CreateFire");
        gameObject.SetActive(false);
    }
    void InitFxObject()
    {
        for(int i =0; i< m_fxObjects.Length; i++)
        {
            m_fxObjects[i].SetActive(false);
        }
    }

    void CreateFire()
    {
        var fire = m_firePool.Get();
        fire.gameObject.SetActive(true);
        fire.transform.position = m_firePos.position;

        //새로 생성
        /*var obj = Instantiate(m_firePrefab);
        obj.transform.position = m_firePos.position;*/
        /*재귀함수로 만들기
        Invoke("CreateFire", 0.2f);*/
    }

    void ResultState()
    {
        GameStateManager.Instance.SetState(GameStateManager.GameState.Result);
    }

    #endregion
    #region Unity Methods
    void Start()
    {
        m_animator = GetComponent<Animator>();
        InitFxObject();
        m_firePool = new GameObjectPool<FireController>(10, () =>
        {
            var obj = Instantiate(m_firePrefab);
            obj.SetActive(false);
            var fire = obj.GetComponent<FireController>();
            fire.Initfire(this);

            return fire;
        });
        m_buffState = 0;
        //인보크로 (함수, 대기시간, 반복시간)
        InvokeRepeating("CreateFire", 0.1f, 0.2f);
    }

    void Update()
    {
        m_dir = new Vector3(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"));
        m_moveVal = m_speed * Time.deltaTime;
        if (Input.GetMouseButtonDown(0))
        {
            m_startPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            m_isDrag = true;
        }
        if(Input.GetMouseButtonUp(0))
        {
            m_isDrag = false;
            m_dir = Vector3.zero;
        }
        if(m_isDrag)
        {
            //마우스 포지션은 스크린좌표, 인게임상 좌표는 월드좌표!
            var endPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            var dir = endPos - m_startPos;
            dir.y = 0f;
            m_dir = dir.normalized;
            m_moveVal = Mathf.Abs(dir.x);
            m_startPos = endPos;

            var hit = Physics2D.Raycast(transform.position, m_dir, m_moveVal, 1 << LayerMask.NameToLayer("Wall"));
            if (hit.collider != null)
            {
                if(m_dir !=Vector3.right && hit.collider.CompareTag("Collider_Left") || m_dir != Vector3.left && hit.collider.CompareTag("Collider_Right"))
                    m_moveVal = hit.distance;
            }
        }
        transform.position += m_dir * m_moveVal;
        //키보드 안쓰고 마우스만 쓸 경우, dir을 정규화시키지않아도 사용가능
        //transform.position += m_dir;
        


    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.CompareTag("Monster") && m_buffState != 2)
        {
            EffectManager.Instance.CreateEffect(transform.position);
            MonsterManager.Instance.RemoveMonsterAll();
            this.gameObject.SetActive(false);
            CancelInvoke("CreateFire");
            Invoke("ResultState", 2f);
        }
    }

    #endregion




}
