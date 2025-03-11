using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Resource : MonoBehaviour
{
    public ItemData itemToGive; //떨어뜨릴 아이템
    public int quantityPerHit = 1; //칠때마다 나올 수량
    public int capacity; //한계

    public void Gather(Vector3 hitPoint, Vector3 hitNormal)
    {
        for(int i=0;i<quantityPerHit;i++)
        {
            //수량이 0보다 작다면
            if (capacity <= 0)
            {
                //즉시 반복문 종료(이미 부숴져야 되니까)
                break;
            }
            capacity -= 1;
            //떨어뜨릴 아이템의 dropPrefab을 복제한 뒤, 타격점의 살짝 위에서 나오게 한 다음 hitnormal과 vector3의 방향으로 지정
            Instantiate(itemToGive.dropPrefab, hitPoint + Vector3.up, Quaternion.LookRotation(hitNormal, Vector3.up));

        }
    }


}
