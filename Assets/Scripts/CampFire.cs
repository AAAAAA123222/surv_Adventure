using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Campfire : MonoBehaviour
{
    public int damage;
    public float damageRate;

    List<IDamagable> things = new List<IDamagable>();

    private void Start()
    {
        //���� ������ �޼����� �̸��� ã�� �� �޼����� �����ð�, �ߵ��ֱ⸦ ���� 
        InvokeRepeating("DealDamage", 0, damageRate);
    }

    void DealDamage()
    {
        //ķ�����̾� ������ ���� �ȿ� �ִ� ������ŭ �ݺ� ����
        for (int i = 0; i < things.Count; i++)
        {
            //�� �ȿ� �ִ� ��� ��ü���� IDamagable �������̽��κ��� ��ӹ��� TakePhysicalDamage �޼����� ������ ����
            things[i].TakePhysicalDamage(damage);
        }
    }

    //Ʈ���ſ��� ���� ��� ��󿡼� ���ܽ�Ű�� �Լ�
    private void OnTriggerEnter(Collider other)
    {
        if(other.TryGetComponent(out IDamagable damagable)) //IDamagable Ŭ������ ��ӹ��� ��ü�� ���
        {
            //�������� ���� ��� �߰�
            things.Add(damagable);
        }
    }
    //Ʈ���ſ��� ���� ��� ��󿡼� ���ܽ�Ű�� �Լ�
    private void OnTriggerExit(Collider other)
    {
        if(other.TryGetComponent(out IDamagable damagable)) //IDamageable Ŭ������ ��ӹ��� ��ü�� ���
        {
            //�������� ���� ��󿡼� ����
            things.Remove(damagable);
        }
    }


}
