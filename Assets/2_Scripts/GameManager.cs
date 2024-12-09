using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    [Header("적 소환 관련 변수")]
    public Vector2 limitMin;//소환될 최저라인
    public Vector2 limitMax;//소환될 최대라인
    public GameObject[] enemeis;
    public GameObject bat;
    public GameObject flyEye;
    public GameObject ghost;
    public GameObject boss;
    int spawnCount;//계속 소환하기 위한 변수

    [Header("점수 관련 변수")]
    public int score = 0;//현재 스코어
    public int enemyScore = 1;//그냥 점수
    public int bossScore = 5;//보스 점수
    public int bossSpawnTriggerScore = 10;//보스가 소환되는 스코어
    public bool bossSpawned = false;//보스가 소환 되어있는지 확인
    public Text scoreTxt;//획득 스코어 텍스트
    public Button healtBtn;//회복 버튼
    int currentScore;

    [Header("코인 관련 변수")]
    public int coin;//토탈 코인
    public Text coinTxt;//코인을 표시할 텍스트
    int coinUsed;

    [Header("스킬 관련 변수")]
    public GameObject skillPrefab;//스킬 이펙트
    public Button skillBtn;//스킬 사용 버튼
    public Image skillCoolTime;//스킬 쿨타임
    public Transform skillPoint;//스킬 소환 위치

    public static GameManager gm;

    bool pause;//일시정지 변수
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
            //보스가 일정 스코어가 되고, 소환 되어 있지 않다면 소환
            //일정 스코어 마다 소환
            if (score >= bossSpawnTriggerScore && !bossSpawned)
            {
                bossSpawnTriggerScore += 20;//20점씩 더해줘서 다음 보스까지 딜레이를 준다.
                spawnPos.y = -3f;
                Instantiate(boss, spawnPos, Quaternion.identity);
                bossSpawned = true;
            }else//아니면 그냥 적 소환
            {
                int randomIndex = Random.Range(0, enemeis.Length);
                Instantiate(enemeis[randomIndex], spawnPos, Quaternion.identity);
            }
            yield return new WaitForSeconds(1f);//소환되는 주기 1초
        }
    }
    public void IncreaseScore(int amount)
    {
        score += amount;//일반 몬스터는 1점 보스는 5점
        coin += amount * 5;//코인은 점수의 * 5로 해줌 스킬 사용시 소모하는 재화
    }

    public void healtHp()
    {
        coinUsed = 30;
        coin -= coinUsed;//체력 회복 비용
        Castle.hp.fillAmount += 0.5f;//체력 회복량
    }
    public void BtnCheck()
    {
        coinUsed = 300;
        //지정한 값 이하 일시 비활성화를 해줘서 음수가 안나오게
        //쓸코인보다 작고 또는 필어마운트가 1보다 작고 또는 타임 스케일이 0이면 버튼을 비활성화해줘
        if (Castle.hp.fillAmount == 1 || coin < 50 || Time.timeScale == 0)
        {
            healtBtn.interactable = false;
        }
        else if (Castle.hp.fillAmount < 1 || coin > 50 || Time.timeScale == 1)
        {
            healtBtn.interactable = true;
        }
        //쓸코인보다 작고 또는 필어마운트가 1보다 작고 또는 타임 스케일이 0이면 버튼을 비활성화해줘
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
        GameObject skill = Instantiate(skillPrefab, skillPoint.position, Quaternion.identity);//지정된 위치
        coin -= coinUsed;
        if (skill != null)
        {
            Destroy(skill, 1.05f);//애니메이션이 종료되면 삭제 되게하기
        }
    }
    public void SkillCoolTime()//버튼 클릭했을때만 코루틴이 실행 되게끔 함수 따로 만들어서 버튼에 넣어주기
    {
        StartCoroutine(CoolTime());
    }

    IEnumerator CoolTime()//쿨타임 기능
    {
        float cooldown = 3f; //쿨타임 시간
        float overTime = 0f; //경과 시간을 0부터 시작하기

        while (overTime < cooldown)//경과 시간이 쿨타임보다 작을 경우
        {
            overTime += Time.deltaTime;//경과시간에 프레임당 시간을 더해줌

            skillCoolTime.fillAmount = overTime / cooldown;//경과 시간에 쿨타운을 나눠줘서 필어마운트를 채워줌

            yield return null;
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawLine(limitMin, limitMax);//소환 라인 그려놔서 위치 파악
    }
    public void StopGame()
    {

        //일시정지 하는데클릭하면 총알이 계속 소환됨...
        //어택 스크립트를 찾아서 타임스케일이 0 이면 스크립트를 꺼줌
        
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
