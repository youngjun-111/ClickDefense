using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class Enemy : MonoBehaviour
{
    public float moveSpeed;//적의 속도
    public float yOffset;//체력바 위치
    public float speedIncreaseInterval; //속도 증가 간격 (초 단위)
    public float speedIncreaseAmount; //속도 증가량

    public float maxHp = 10f;//잡몹 체력
    float currentHp;//현재 몬스터 체력
    Animator anim;

    public Image hpBar;//체력바
    public Transform target;//체력바 소환 위치

    public GameObject hitPrefab;//이펙트 프리팹 따로 설정된 값 없이 소환 후 일정 시간후 삭제

    GameManager gm;//게임 매니저를 참조
    void Start()
    {
        gm = GameManager.gm;//게임 매니저에 대한 참조 얻기
        StartCoroutine(OverTimeSpeed());
        anim = GetComponent<Animator>();
        currentHp = maxHp;
    }
    
    void Update()
    {
        transform.Translate(Vector2.left * moveSpeed * Time.deltaTime);

        //몹 위치에 체력바 소환
        Vector2 screenPosition = Camera.main.WorldToScreenPoint(target.position);
        screenPosition.y += yOffset;//소환된 위치에서 머리위쪽으로 향하게
        hpBar.transform.position = screenPosition;
    }
    IEnumerator OverTimeSpeed()
    {
        while (true)
        {
            yield return new WaitForSeconds(speedIncreaseInterval);//지정한 시간초마다
            moveSpeed += speedIncreaseAmount;//스피드 증가
            anim.speed += speedIncreaseAmount / 10f;//애니메이션 스피드 같이 증가
        }
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Shoot"))
        {
            TakeDamage(1);//잡몹 잡을 때
            GameObject hit = Instantiate(hitPrefab, transform.position, Quaternion.identity);

            Destroy(hit, 0.3f);//일정 시간 후 타격 프리팹 삭제
            Destroy(collision.gameObject);//총알 삭제
        }

        if(collision.CompareTag("Skill"))
        {
            Destroy(this.gameObject);
            Die();
        }
    }
    
    void TakeDamage(float damage)
    {
        if(gameObject.CompareTag("Enemy"))
        {
            currentHp -= damage;//1
            //체력이 음수가 되지 않도록 하는 메서드(벨류값, 최저, 최고)
            currentHp = Mathf.Clamp(currentHp, 0f, maxHp);

            hpBar.fillAmount = currentHp / maxHp;//체력 5
        }
        

        if (currentHp <= 0)
        {
            Die();
        }
    }
   
    void Die()
    {
        //적이 보스인지 일반 적인지 구분하여 스코어 증가
       if (gameObject.CompareTag("Enemy"))
        {
            gm.IncreaseScore(gm.enemyScore);
        }

        Destroy(gameObject);
    }
}
