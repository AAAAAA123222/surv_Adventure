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
    //ui ��������
    public UICondition uiCondition;

    //ui����� �ȿ� �ִ� ������ UI ��������
    Condition health { get { return uiCondition.health; } }
    Condition hunger { get { return uiCondition.hunger; } }
    Condition stamina { get { return uiCondition.stamina; } }

    //���� ����� ������ �پ�� ü���� ��
    public float noHungerHealthDecay;

    //������ �Ծ����� �ߵ��� ��������Ʈ
    public event Action onTakeDamage;

    //��ġ�� �ð��� �������� �����ϴ� ��� �����ϱ�
    private void Update()
    {
        //�������� ����� �ð���ŭ ���� ��ų� �ø���
        hunger.Subtract(hunger.passiveValue * Time.deltaTime);
        stamina.Add(stamina.passiveValue * Time.deltaTime);

        //���� ������� 0�̶�� ü���� ���
        if(hunger.curValue ==0f)
        {
            health.Subtract(noHungerHealthDecay * Time.deltaTime);
        }
        
        //���� ü���� 0�̶�� ��� ȣ��
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
        Debug.Log("�̰� �ʹ� ������.");
    }

    //�������� ���� �� 
    public void TakePhysicalDamage(int damage)
    {
        health.Subtract(damage);
        onTakeDamage?.Invoke();
    }

    public bool UseStamina(float amount)
    {
        //���� ���� �ִ� ���׹̳��� ���� �پ�� ���׹̳����� ���ٸ�
        if(stamina.curValue-amount<0f)
        {
            //�׳� ���ư���(�ƿ� �����ϱ�)
            return false;
        }

        //���׹̳� ��� �� ��ȯ
        stamina.Subtract(amount);
        return true;
    }

}
