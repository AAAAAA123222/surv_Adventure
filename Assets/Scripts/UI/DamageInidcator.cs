using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DamageInidcator : MonoBehaviour
{
    public Image image; //Ȱ��ȭ��ų �̹��� ����
    public float flashSpeed; //�󸶳� Ȱ��ȭ��ų���� ���� �ð�

    private Coroutine coroutine;
    
    private void Start()
    {
        //ĳ���͸Ŵ��� �� �÷��̾ ����ִ� ����� ���� onTakeDamage ��������Ʈ�� �� Ŭ���� �� Flash �޼��带 �Ҵ��ϱ�
        CharacterManager.Instance.Player.condition.onTakeDamage += Flash;
    }

    public void Flash()
    {
        if (coroutine !=null)
        {
            StopCoroutine(coroutine);
        }
        image.enabled = true;
        image.color = new Color(1f, 100f / 255f, 100f / 255f);
        coroutine = StartCoroutine(FadeAway());
    }

    private IEnumerator FadeAway()
    {
        //���� ���� �� ���� �� �� ���� ���İ� ���� �ȿ� ����
        float startAlpha = 0.3f;
        float a = startAlpha;

        while (a>0) //���İ��� 0���� Ŭ����
        {
            //���İ��� ����ؼ� ��� �׸�ŭ �̹����� ������ ���߱�
            a -= (startAlpha / flashSpeed) * Time.deltaTime;
            image.color = new Color(1f, 100f / 255f, 100f / 255f, a);
            yield return null;
        }
        //�ݺ��� ������ �ٽ� ����
        image.enabled = false;
    }
}
