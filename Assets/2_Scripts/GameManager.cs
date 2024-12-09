using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    [Header("�� ��ȯ ���� ����")]
    public Vector2 limitMin;//��ȯ�� ��������
    public Vector2 limitMax;//��ȯ�� �ִ����
    public GameObject[] enemeis;
    public GameObject bat;
    public GameObject flyEye;
    public GameObject ghost;
    public GameObject boss;
    int spawnCount;//��� ��ȯ�ϱ� ���� ����

    [Header("���� ���� ����")]
    public int score = 0;//���� ���ھ�
    public int enemyScore = 1;//�׳� ����
    public int bossScore = 5;//���� ����
    public int bossSpawnTriggerScore = 10;//������ ��ȯ�Ǵ� ���ھ�
    public bool bossSpawned = false;//������ ��ȯ �Ǿ��ִ��� Ȯ��
    public Text scoreTxt;//ȹ�� ���ھ� �ؽ�Ʈ
    public Button healtBtn;//ȸ�� ��ư
    int currentScore;

    [Header("���� ���� ����")]
    public int coin;//��Ż ����
    public Text coinTxt;//������ ǥ���� �ؽ�Ʈ
    int coinUsed;

    [Header("��ų ���� ����")]
    public GameObject skillPrefab;//��ų ����Ʈ
    public Button skillBtn;//��ų ��� ��ư
    public Image skillCoolTime;//��ų ��Ÿ��
    public Transform skillPoint;//��ų ��ȯ ��ġ

    public static GameManager gm;

    bool pause;//�Ͻ����� ����
    private void Awake()
    {
        gm = this;
    }
    void Start()
    {
        enemeis = new GameObject[3];
        enemeis[0] = bat;
        enemeis[1] = flyEye;
        enemeis[2] = ghost;

        StartCoroutine(SpawnEnmey());

        pause = false;

        score = currentScore;


    }
    void Update()
    {
        ShowInfo();
        BtnCheck();
    }

    public void ShowInfo()
    {
        scoreTxt.text = "Score : " + score;
        if (coin == 0)
        {
            coinTxt.text = ": 0" + " Coin";
        }
        else
        {
            coinTxt.text = ": " + coin + "Coin";
        }
    }
    IEnumerator SpawnEnmey()
    {
        while(true)
        {
            spawnCount++;//1++
            float randomY = Random.Range(limitMin.y, limitMax.y);
            Vector2 spawnPos = new Vector2(limitMin.x, randomY);
            bossSpawned = false;
            //������ ���� ���ھ �ǰ�, ��ȯ �Ǿ� ���� �ʴٸ� ��ȯ
            //���� ���ھ� ���� ��ȯ
            if (score >= bossSpawnTriggerScore && !bossSpawned)
            {
                bossSpawnTriggerScore += 20;//20���� �����༭ ���� �������� �����̸� �ش�.
                spawnPos.y = -3f;
                Instantiate(boss, spawnPos, Quaternion.identity);
                bossSpawned = true;
            }else//�ƴϸ� �׳� �� ��ȯ
            {
                int randomIndex = Random.Range(0, enemeis.Length);
                Instantiate(enemeis[randomIndex], spawnPos, Quaternion.identity);
            }
            yield return new WaitForSeconds(1f);//��ȯ�Ǵ� �ֱ� 1��
        }
    }
    public void IncreaseScore(int amount)
    {
        score += amount;//�Ϲ� ���ʹ� 1�� ������ 5��
        coin += amount * 5;//������ ������ * 5�� ���� ��ų ���� �Ҹ��ϴ� ��ȭ
    }

    public void healtHp()
    {
        coinUsed = 30;
        coin -= coinUsed;//ü�� ȸ�� ���
        Castle.hp.fillAmount += 0.5f;//ü�� ȸ����
    }
    public void BtnCheck()
    {
        coinUsed = 300;
        //������ �� ���� �Ͻ� ��Ȱ��ȭ�� ���༭ ������ �ȳ�����
        //�����κ��� �۰� �Ǵ� �ʾ��Ʈ�� 1���� �۰� �Ǵ� Ÿ�� �������� 0�̸� ��ư�� ��Ȱ��ȭ����
        if (Castle.hp.fillAmount == 1 || coin < 50 || Time.timeScale == 0)
        {
            healtBtn.interactable = false;
        }
        else if (Castle.hp.fillAmount < 1 || coin > 50 || Time.timeScale == 1)
        {
            healtBtn.interactable = true;
        }
        //�����κ��� �۰� �Ǵ� �ʾ��Ʈ�� 1���� �۰� �Ǵ� Ÿ�� �������� 0�̸� ��ư�� ��Ȱ��ȭ����
        if (coin < coinUsed || skillCoolTime.fillAmount < 1 || Time.timeScale == 0)
        {
            skillBtn.interactable = false;
        }
        else if(coin < coinUsed || skillCoolTime.fillAmount < 1 || Time.timeScale == 1)
        {
            skillBtn.interactable = true;
        }
    }

    public void SkillButton()
    {
        coinUsed = 300;
        GameObject skill = Instantiate(skillPrefab, skillPoint.position, Quaternion.identity);//������ ��ġ
        coin -= coinUsed;
        if (skill != null)
        {
            Destroy(skill, 1.05f);//�ִϸ��̼��� ����Ǹ� ���� �ǰ��ϱ�
        }
    }
    public void SkillCoolTime()//��ư Ŭ���������� �ڷ�ƾ�� ���� �ǰԲ� �Լ� ���� ���� ��ư�� �־��ֱ�
    {
        StartCoroutine(CoolTime());
    }

    IEnumerator CoolTime()//��Ÿ�� ���
    {
        float cooldown = 3f; //��Ÿ�� �ð�
        float overTime = 0f; //��� �ð��� 0���� �����ϱ�

        while (overTime < cooldown)//��� �ð��� ��Ÿ�Ӻ��� ���� ���
        {
            overTime += Time.deltaTime;//����ð��� �����Ӵ� �ð��� ������

            skillCoolTime.fillAmount = overTime / cooldown;//��� �ð��� ��Ÿ���� �����༭ �ʾ��Ʈ�� ä����

            yield return null;
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawLine(limitMin, limitMax);//��ȯ ���� �׷����� ��ġ �ľ�
    }
    public void StopGame()
    {

        //�Ͻ����� �ϴµ�Ŭ���ϸ� �Ѿ��� ��� ��ȯ��...
        //���� ��ũ��Ʈ�� ã�Ƽ� Ÿ�ӽ������� 0 �̸� ��ũ��Ʈ�� ����
        
        if (pause == false)
        {
            Time.timeScale = 0;
            pause = true;
            if (Time.timeScale == 0)
            {
                ATK atk = GameObject.Find("player").GetComponent<ATK>();
                atk.enabled = false;
            }
        }
    }

    public void PlayGame()
    {
        if(pause == true)
        {
            Time.timeScale = 1;

            pause = false;
        }
        if (Time.timeScale == 1)
        {
            ATK atk = GameObject.Find("player").GetComponent<ATK>();

            atk.enabled = true;
        }
    }
}
