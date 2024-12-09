using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class ATK : MonoBehaviour
{
    public GameObject effectPrefab; //발사 이펙트 프리팹
    Animator anim;
    public Text guideTxt;
    private void Start()
    {
        anim = GetComponent<Animator>();
        Time.timeScale = 0;
    }
    void Update()
    {
        // 마우스 왼쪽 버튼을 클릭하고 UI 위에 있지 않은 경우
        if (Input.GetMouseButtonDown(0) && !EventSystem.current.IsPointerOverGameObject())
        {
            anim.SetTrigger("Click");
            Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            mousePosition.z = 0; // z 값을 0으로 설정하여 2D 평면에 맞춤

            //방향 벡터를 기반으로 이펙트 프리팹을 생성 하고, 이동 방향을 설정
            //direction 방향을 나태내는 단위
            Vector2 direction = (mousePosition - transform.position).normalized;

            GameObject effect = Instantiate(effectPrefab, transform.position, Quaternion.identity);
            //리지드 바디를 이용하여 direction으로 방향을 나타내서 날아가게 함
            Rigidbody2D rb = effect.GetComponent<Rigidbody2D>();

            rb.velocity = direction * 10f;
            Destroy(effect, 4f); // 일정 시간 후에 이펙트 제거함
            guideTxt.gameObject.SetActive(false);
            Time.timeScale = 1;
        }
    }

}
