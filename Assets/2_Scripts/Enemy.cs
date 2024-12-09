using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class Enemy : MonoBehaviour
{
    public float moveSpeed;//���� �ӵ�
    public float yOffset;//ü�¹� ��ġ
    public float speedIncreaseInterval; //�ӵ� ���� ���� (�� ����)
    public float speedIncreaseAmount; //�ӵ� ������

    public float maxHp = 10f;//��� ü��
    float currentHp;//���� ���� ü��
    Animator anim;

    public Image hpBar;//ü�¹�
    public Transform target;//ü�¹� ��ȯ ��ġ

    public GameObject hitPrefab;//����Ʈ ������ ���� ������ �� ���� ��ȯ �� ���� �ð��� ����

    GameManager gm;//���� �Ŵ����� ����
    void Start()
    {
        gm = GameManager.gm;//���� �Ŵ����� ���� ���� ���
        StartCoroutine(OverTimeSpeed());
        anim = GetComponent<Animator>();
        currentHp = maxHp;
    }
    
    void Update()
    {
        transform.Translate(Vector2.left * moveSpeed * Time.deltaTime);

        //�� ��ġ�� ü�¹� ��ȯ
        Vector2 screenPosition = Camera.main.WorldToScreenPoint(target.position);
        screenPosition.y += yOffset;//��ȯ�� ��ġ���� �Ӹ��������� ���ϰ�
        hpBar.transform.position = screenPosition;
    }
    IEnumerator OverTimeSpeed()
    {
        while (true)
        {
            yield return new WaitForSeconds(speedIncreaseInterval);//������ �ð��ʸ���
            moveSpeed += speedIncreaseAmount;//���ǵ� ����
            anim.speed += speedIncreaseAmount / 10f;//�ִϸ��̼� ���ǵ� ���� ����
        }
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Shoot"))
        {
            TakeDamage(1);//��� ���� ��
            GameObject hit = Instantiate(hitPrefab, transform.position, Quaternion.identity);

            Destroy(hit, 0.3f);//���� �ð� �� Ÿ�� ������ ����
            Destroy(collision.gameObject);//�Ѿ� ����
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
            //ü���� ������ ���� �ʵ��� �ϴ� �޼���(������, ����, �ְ�)
            currentHp = Mathf.Clamp(currentHp, 0f, maxHp);

            hpBar.fillAmount = currentHp / maxHp;//ü�� 5
        }
        

        if (currentHp <= 0)
        {
            Die();
        }
    }
   
    void Die()
    {
        //���� �������� �Ϲ� ������ �����Ͽ� ���ھ� ����
       if (gameObject.CompareTag("Enemy"))
        {
            gm.IncreaseScore(gm.enemyScore);
        }

        Destroy(gameObject);
    }
}
