using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DamageInidcator : MonoBehaviour
{
    public Image image; //활성화시킬 이미지 변수
    public float flashSpeed; //얼마나 활성화시킬지에 대한 시간

    private Coroutine coroutine;
    
    private void Start()
    {
        //캐릭터매니저 안 플레이어에 들어있는 컨디션 안의 onTakeDamage 델리게이트에 이 클래스 안 Flash 메서드를 할당하기
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
        //시작 투명도 값 설정 및 그 값을 알파값 변수 안에 저장
        float startAlpha = 0.3f;
        float a = startAlpha;

        while (a>0) //알파값이 0보다 클동안
        {
            //알파값을 계속해서 깎고 그만큼 이미지의 투명도를 낮추기
            a -= (startAlpha / flashSpeed) * Time.deltaTime;
            image.color = new Color(1f, 100f / 255f, 100f / 255f, a);
            yield return null;
        }
        //반복문 끝나면 다시 끄기
        image.enabled = false;
    }
}
