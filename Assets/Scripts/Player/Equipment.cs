using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Equipment : MonoBehaviour
{
    public Equip curEquip; //현재 장착하고 있는 장비의 변수
    public Transform equipParent; //장비의 카메라 위치의 변수

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
        //장착하고 있는 경우 해제
        UnEquip();

        //장착할 장비를 설정
        //equipPrefab을 복제한 뒤 equipParent에 상속시킨 뒤, 복제된 객체의 Equip 컴포넌트를 curEquip에 할당
        curEquip = Instantiate(data.equipPrefab, equipParent).GetComponent<Equip>();
    }

    public void UnEquip()
    {
        //curEquip이 비어있지 않다면
        if(curEquip != null)
        {
            //장착중인 객체 지우고 착용중인 장비 없애주기
            Destroy(curEquip.gameObject);
            curEquip = null;
        }
    }


    public void OnAttackInput(InputAction.CallbackContext context)
    {
        //마우스가 클릭된 상황이며
        //착용중인 장비가 비어있지 않고
        //마우스 시점이 고정된 상황에만 발동
        if(context.phase==InputActionPhase.Performed&&curEquip!=null&&controller.canLook)
        {
            //curEquip 안의 공격 애니메이션 재생
            curEquip.OnAttackInput();
        }
    }
}
