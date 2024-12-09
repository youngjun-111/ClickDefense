using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class ATK : MonoBehaviour
{
    public GameObject effectPrefab; //�߻� ����Ʈ ������
    Animator anim;
    public Text guideTxt;
    private void Start()
    {
        anim = GetComponent<Animator>();
        Time.timeScale = 0;
    }
    void Update()
    {
        // ���콺 ���� ��ư�� Ŭ���ϰ� UI ���� ���� ���� ���
        if (Input.GetMouseButtonDown(0) && !EventSystem.current.IsPointerOverGameObject())
        {
            anim.SetTrigger("Click");
            Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            mousePosition.z = 0; // z ���� 0���� �����Ͽ� 2D ��鿡 ����

            //���� ���͸� ������� ����Ʈ �������� ���� �ϰ�, �̵� ������ ����
            //direction ������ ���³��� ����
            Vector2 direction = (mousePosition - transform.position).normalized;

            GameObject effect = Instantiate(effectPrefab, transform.position, Quaternion.identity);
            //������ �ٵ� �̿��Ͽ� direction���� ������ ��Ÿ���� ���ư��� ��
            Rigidbody2D rb = effect.GetComponent<Rigidbody2D>();

            rb.velocity = direction * 10f;
            Destroy(effect, 4f); // ���� �ð� �Ŀ� ����Ʈ ������
            guideTxt.gameObject.SetActive(false);
            Time.timeScale = 1;
        }
    }

}
