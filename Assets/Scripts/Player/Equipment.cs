using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Equipment : MonoBehaviour
{
    public Equip curEquip; //���� �����ϰ� �ִ� ����� ����
    public Transform equipParent; //����� ī�޶� ��ġ�� ����

    private PlayerController controller;
    private PlayerCondition condition;

    // Start is called before the first frame update
    void Start()
    {
        controller = GetComponent<PlayerController>();
        condition = GetComponent<PlayerCondition>();
    }

    public void EquipNew(ItemData data)
    {
        //�����ϰ� �ִ� ��� ����
        UnEquip();

        //������ ��� ����
        //equipPrefab�� ������ �� equipParent�� ��ӽ�Ų ��, ������ ��ü�� Equip ������Ʈ�� curEquip�� �Ҵ�
        curEquip = Instantiate(data.equipPrefab, equipParent).GetComponent<Equip>();
    }

    public void UnEquip()
    {
        //curEquip�� ������� �ʴٸ�
        if(curEquip != null)
        {
            //�������� ��ü ����� �������� ��� �����ֱ�
            Destroy(curEquip.gameObject);
            curEquip = null;
        }
    }


    public void OnAttackInput(InputAction.CallbackContext context)
    {
        //���콺�� Ŭ���� ��Ȳ�̸�
        //�������� ��� ������� �ʰ�
        //���콺 ������ ������ ��Ȳ���� �ߵ�
        if(context.phase==InputActionPhase.Performed&&curEquip!=null&&controller.canLook)
        {
            //curEquip ���� ���� �ִϸ��̼� ���
            curEquip.OnAttackInput();
        }
    }
}
