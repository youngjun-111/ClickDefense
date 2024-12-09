using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class Boss : MonoBehaviour
{
    public float moveSpeed;//���� �ӵ�
    public float yOffset;//ü�¹� ��ġ
    public float speedIncreaseInterval; //�ӵ� ���� ���� (�� ����)
    public float speedIncreaseAmount; //�ӵ� ������

    public float bossMaxHp = 20f;//���� ü��
    float currentBossHp;//���� ���� ü��
    Animator anim;

    public Image bossBar;//���� ü�¹�
    public Transform target;//ü�¹� ��ȯ ��ġ

    public GameObject hitPrefab;//����Ʈ ������ ���� ������ �� ���� ��ȯ �� ���� �ð��� ����

    GameManager gm;//���� �Ŵ����� ����
    void Start()
    {
        gm = GameManager.gm;//���� �Ŵ����� ���� �����Ͽ� ����
        StartCoroutine(OverTimeSpeed());
        anim = GetComponent<Animator>();
        currentBossHp = bossMaxHp;
    }

    void Update()
    {
        transform.Translate(Vector2.left * moveSpeed * Time.deltaTime);

        //�� ��ġ�� ü�¹� ��ȯ
        Vector2 screenPosition = Camera.main.WorldToScreenPoint(target.position);
        screenPosition.y += yOffset;//��ȯ�� ��ġ���� �Ӹ��������� ���ϰ�
        bossBar.transform.position = screenPosition;
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
            TakeBossDamage(1);//���� ���� �� ���ݷ�
            GameObject hit = Instantiate(hitPrefab, transform.position, Quaternion.identity);

            Destroy(hit, 0.3f);//���� �ð� �� Ÿ�� ������ ����
            Destroy(collision.gameObject);//�Ѿ� ����
        }

        if (collision.CompareTag("Skill"))
        {
            Destroy(this.gameObject);
            Die();
        }
    }

    void TakeBossDamage(float damage)
    {
        if (gameObject.CompareTag("Boss"))
        {
            currentBossHp -= damage;//1����
            //������ �������� �ʰ� �ϴ� �޼���
            currentBossHp = Mathf.Clamp(currentBossHp, 0f, bossMaxHp);

            bossBar.fillAmount = currentBossHp / bossMaxHp;//ü�� 20
        }

        if (currentBossHp <= 0)
        {
            Die();
        }

    }
    void Die()
    {
        //���� �������� �Ϲ� ������ �����Ͽ� ���ھ� ����
        if (gameObject.CompareTag("Boss"))
        {
            gm.IncreaseScore(gm.bossScore);
        }
        Destroy(gameObject);
    }
}
