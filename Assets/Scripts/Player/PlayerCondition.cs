using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IDamagable
{
    void TakePhysicalDamage(int damage);
}


public class PlayerCondition : MonoBehaviour, IDamagable
{
    //ui 가져오기
    public UICondition uiCondition;

    //ui컨디션 안에 있는 각각의 UI 가져오기
    Condition health { get { return uiCondition.health; } }
    Condition hunger { get { return uiCondition.hunger; } }
    Condition stamina { get { return uiCondition.stamina; } }

    //만약 배고픔 없을때 줄어들 체력의 값
    public float noHungerHealthDecay;

    //데미지 입었을때 발동할 델리게이트
    public event Action onTakeDamage;

    //수치가 시간이 지날수록 변동하는 기능 수행하기
    private void Update()
    {
        //프레임이 재생된 시간만큼 값을 깎거나 올리고
        hunger.Subtract(hunger.passiveValue * Time.deltaTime);
        stamina.Add(stamina.passiveValue * Time.deltaTime);

        //만약 배고픔이 0이라면 체력을 깎기
        if(hunger.curValue ==0f)
        {
            health.Subtract(noHungerHealthDecay * Time.deltaTime);
        }
        
        //만약 체력이 0이라면 사망 호출
        if(health.curValue==0f)
        {
            Die();
        }
    }

    public void Heal(float amount)
    {
        health.Add(amount);
    }

    public void Eat(float amount)
    {
        hunger.Add(amount);
    }
    public void Die()
    {
        Debug.Log("이건 너무 아프다.");
    }

    //데미지를 입을 때 
    public void TakePhysicalDamage(int damage)
    {
        health.Subtract(damage);
        onTakeDamage?.Invoke();
    }

    public bool UseStamina(float amount)
    {
        //현재 갖고 있는 스테미나의 값이 줄어들 스테미나보다 적다면
        if(stamina.curValue-amount<0f)
        {
            //그냥 돌아가기(아예 못쓰니까)
            return false;
        }

        //스테미나 깎고 참 반환
        stamina.Subtract(amount);
        return true;
    }

}
