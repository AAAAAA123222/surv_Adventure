using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Resource : MonoBehaviour
{
    public ItemData itemToGive; //����߸� ������
    public int quantityPerHit = 1; //ĥ������ ���� ����
    public int capacity; //�Ѱ�

    public void Gather(Vector3 hitPoint, Vector3 hitNormal)
    {
        for(int i=0;i<quantityPerHit;i++)
        {
            //������ 0���� �۴ٸ�
            if (capacity <= 0)
            {
                //��� �ݺ��� ����(�̹� �ν����� �Ǵϱ�)
                break;
            }
            capacity -= 1;
            //����߸� �������� dropPrefab�� ������ ��, Ÿ������ ��¦ ������ ������ �� ���� hitnormal�� vector3�� �������� ����
            Instantiate(itemToGive.dropPrefab, hitPoint + Vector3.up, Quaternion.LookRotation(hitNormal, Vector3.up));

        }
    }


}
