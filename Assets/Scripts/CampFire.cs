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
        //값을 변경할 메서드의 이름을 찾고 그 메서드의 지연시간, 발동주기를 지정 
        InvokeRepeating("DealDamage", 0, damageRate);
    }

    void DealDamage()
    {
        //캠프파이어 데미지 범위 안에 있는 개수만큼 반복 시작
        for (int i = 0; i < things.Count; i++)
        {
            //그 안에 있는 모든 개체에게 IDamagable 인터페이스로부터 상속받은 TakePhysicalDamage 메서드의 내용을 실행
            things[i].TakePhysicalDamage(damage);
        }
    }

    //트리거에서 나올 경우 대상에서 제외시키는 함수
    private void OnTriggerEnter(Collider other)
    {
        if(other.TryGetComponent(out IDamagable damagable)) //IDamagable 클래스를 상속받은 객체일 경우
        {
            //데미지를 가할 대상에 추가
            things.Add(damagable);
        }
    }
    //트리거에서 나올 경우 대상에서 제외시키는 함수
    private void OnTriggerExit(Collider other)
    {
        if(other.TryGetComponent(out IDamagable damagable)) //IDamageable 클래스를 상속받은 객체일 경우
        {
            //데미지를 가할 대상에서 제거
            things.Remove(damagable);
        }
    }


}
