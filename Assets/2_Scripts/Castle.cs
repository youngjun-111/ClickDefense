using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Castle : MonoBehaviour
{
    public float maxHp = 10f;//���� ü��
    public float damage = 1f;//�Ϲ� ���� ���ݷ�
    public float bossDamage = 3f;//���� ���� ���ݷ�

    public static Image hp;
    void Start()
    {
        hp = GameObject.Find("Hp").GetComponent<Image>();
        hp.fillAmount = 1;
    }

    void Update()
    {
        if(hp.fillAmount == 0)
        {
            SceneManager.LoadScene("GameOver");
        }
    }

    

    private void OnTriggerEnter2D(Collider2D other)
    {
        if(other.CompareTag("Enemy"))
        {
            hp.fillAmount -= damage / maxHp;
            Destroy(other.gameObject);
        }
        else
        {
            hp.fillAmount -= bossDamage / maxHp;
            Destroy(other.gameObject);
        }

    }
}
