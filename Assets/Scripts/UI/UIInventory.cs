using System.Collections;
using System.Collections.Generic;
using System.Xml;
using TMPro;
using UnityEngine;

public class UIInventory : MonoBehaviour
{
    //아이템슬롯 클래스를 가져올 변수 생성
    public ItemSlot[] slots;
    
    //인벤토리 창, 슬롯 판넬 및 버릴 위치 생성
    public GameObject inventoryWindow;
    public Transform slotPanel;
    public Transform dropPosition; 

    [Header("Select Item")]
    //커서를 갖다 댄 아이템의 정보를 저장할 변수를 생성
    public TextMeshProUGUI selectedItemName;
    public TextMeshProUGUI selectedItemDescription;
    public TextMeshProUGUI selectedStatName;
    public TextMeshProUGUI selectedStatValue;
    //버튼들 갖고오기
    public GameObject useButton;
    public GameObject EquipButton;
    public GameObject unequipButton;
    public GameObject dropButton;

    //정보를 받아올 플레이어 컨트롤러와 컨디션 가져오기
    private PlayerController controller;
    private PlayerCondition condition;

    //가져온 아이템과 아이템의 인덱스를 저장할 변수 생성
    ItemData selectedItem;
    int selectedItemIndex = 0;

    //착용한 아이템의 인덱스
    int curEquipIndex;

    void Start()
    {
        //플레이어 변수 초기화
        controller = CharacterManager.Instance.Player.controller;
        condition= CharacterManager.Instance.Player.condition;
        dropPosition = CharacterManager.Instance.Player.dropPosition;

        //컨트롤러의 인벤토리 액션에 Toggle 할당
        controller.inventory += Toggle;

        //플레이어 자체에 들어있는 애드아이템 액션에 AddItem 할당
        CharacterManager.Instance.Player.additem += AddItem;

        //인벤토리를 끄고 슬롯 배열을 슬롯판넬이 갖고 있는 아이템 슬롯의 수만큼 생성하기
        inventoryWindow.SetActive(false);
        slots = new ItemSlot[slotPanel.childCount];

        for (int i = 0; i < slots.Length; i++)
        {
            //슬롯판넬의 n번째 자식에 할당된 ItemSlot 컴포넌트를 가져온 뒤 배열에 할당
            slots[i]=slotPanel.GetChild(i).GetComponent<ItemSlot>();
            slots[i].index = i;
            slots[i].inventory = this;
        }

        //이후 정보 전부 초기화
        ClearSelectedItemWindow();
        UpdateUI();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    //클릭했을 때 표시되는 정보 초기화하는 메서드
    void ClearSelectedItemWindow()
    {
        //선택된 아이템을 표시하는 값 초기화
        selectedItemName.text = string.Empty;
        selectedItemDescription.text = string.Empty;
        selectedStatName.text = string.Empty;
        selectedStatValue.text = string.Empty;

        //버튼 전부 비활성화
        useButton.SetActive(false);
        EquipButton.SetActive(false);
        unequipButton.SetActive(false);
        useButton.SetActive(false);
        dropButton.SetActive(false);
    }

    //창을 켜는 메서드
    public void Toggle()
    {
        if(IsOpen())//열려있으면 끄고
        {
            inventoryWindow.SetActive(false);
        }
        else//닫혀있으면 끄기
        {
            inventoryWindow.SetActive(true);
        }

    }

    //열려있는지 확인하는 메서드
    public bool IsOpen()
    {
        //하이어라키 창에서 활성화가 되어있는지 확인하고 그 값을 반환
        return inventoryWindow.activeInHierarchy;
    }

    void AddItem()
    {
        ItemData data = CharacterManager.Instance.Player.itemData;

        //중복 가능 여부 확인하기 (ScriptableObject의 canStack)
        if(data.canStack)
        {
            ItemSlot slot = GetItemStack(data);
            if(slot!=null)
            {
                slot.quantity++;//수량만 늘리고
                UpdateUI();
                CharacterManager.Instance.Player.itemData = null;
                return;
            }
        }

        //중복이 안된다면 빈 슬롯 가져오기
        ItemSlot emptySlot = GetEmptySlot();
        //있다면
        if(emptySlot!=null)
        {
            emptySlot.item = data;
            emptySlot.quantity = 1;
            UpdateUI();
            CharacterManager.Instance.Player.itemData= null;
            return;
        }
        //없다면 버리고
        ThrowItem(data);
        //이후 캐릭터가 담고 있는 아이템 정보를 초기화
        CharacterManager.Instance.Player.itemData = null;
    }

    void UpdateUI()
    {
        //슬롯 수만큼
        for(int i=0;i<slots.Length;i++)
        {
            if (slots[i].item != null) //슬롯 안의 아이템이 비어있지 않다면 아이템의 값 조절
            { 
                slots[i].Set();
            }
            else //비어있다면 초기화
            { 
                slots[i].Clear();
            }
        }

    }


    ItemSlot GetItemStack(ItemData data)
    {
        //슬롯 수만큼
        for (int i = 0; i < slots.Length; i++)
        {
            //슬롯에 그 아이템이 있고 또 그 수량이 매개변수의 최대 수량을 넘기지 않는다면
            if (slots[i].item == data&& slots[i].quantity<data.maxStackAmount)
            {
                //그 값 반환
                return slots[i];
            }
        }
        return null;
    }

    ItemSlot GetEmptySlot()
    {
        //슬롯 수만큼
        for (int i = 0; i < slots.Length; i++)
        {
            //슬롯 안 아이템이 비어있다면
            if (slots[i].item==null)
            {
                //그 슬롯 반환
                return slots[i];
            }
        }
        //없으면 없다고 반환
        return null;
    }

    void ThrowItem(ItemData data)
    {
        //데이터 안의 프리팹을 복제한 뒤 드랍포지션의 위치에 무작위 각도로 생성하기
        Instantiate(data.dropPrefab, dropPosition.position, Quaternion.Euler(Vector3.one * Random.value * 360));
    }
    
    public void SelectItem(int index)
    {
        //아이템이 없으면 돌아가기
        if (slots[index].item == null)
        {
            return;
        }

        selectedItem = slots[index].item;
        selectedItemIndex = index;

        selectedItemName.text = selectedItem.displayName;
        selectedItemDescription.text =selectedItem.description;

        //일단 스탯 텍스트 초기화
        selectedStatName.text = string.Empty;
        selectedStatValue.text = string.Empty;

        //아이템에 체력이나 배고픔을 조정하는 값이 들어있다면 실행하기
        for(int i = 0; i < selectedItem.consumables.Length; i++)
        {
            //텍스트에 상응하는 타입과 밸류를 변환해준 뒤 줄바꿈
            selectedStatName.text += selectedItem.consumables[i].type.ToString() + "\n";
            selectedStatValue.text += selectedItem.consumables[i].value.ToString() + "\n";

        }

        //그리고 아이템타입에 따라 사용버튼과 장비/해제버튼을 표시하기
        useButton.SetActive(selectedItem.type == ItemType.Consumable);
        EquipButton.SetActive(selectedItem.type == ItemType.Equipable && !slots[index].equipped); //그 장비가 장착되어있지 않을때만
        unequipButton.SetActive(selectedItem.type == ItemType.Equipable && slots[index].equipped); //그 장비가 장착되어있을때만
        //드랍버튼은 그냥 표기
        dropButton.SetActive(true);
    }

    public void OnUseButton()
    {
        if(selectedItem.type==ItemType.Consumable)
        {
            //안에 들어있는 컨슈머블 수만큼 반복
            for(int i=0;i<selectedItem.consumables.Length;i++)
            {
                //그 안 컨슈머블의 변수의 값 확인
                switch(selectedItem.consumables[i].type)
                {
                    //체력이라면 체력회복
                    case ConsumableType.Health:
                        condition.Heal(selectedItem.consumables[i].value);
                        break;
                    //배고픔이라면 배고픔회복
                    case ConsumableType.Hunger:
                        condition.Eat(selectedItem.consumables[i].value);
                        break;
                }
            }

            //원래 그냥 이 메서드 내에서 StartCoroutine을 했는데 시작과 동시에 삭제가 되어버려서 controller에서 실행하는걸로 해결했습니다.
            switch(selectedItem.boostType)
            {
                case boostType.None:
                    break;
                case boostType.Speed:
                    controller.StartCoroutine(controller.SpeedBoostEffect(selectedItem.modifier, selectedItem.duration));
                    break;
                case boostType.JumpPower:
                    controller.StartCoroutine(controller.JumpBoostEffect(selectedItem.modifier, selectedItem.duration));
                    break;
            }

            RemoveSelectedItem(); 
        }
    }

    public void OnDropButton()
    {
        //선택된 아이템 버리기
        ThrowItem(selectedItem);
        //이후 값 내리기
        RemoveSelectedItem();
    }
    
    void RemoveSelectedItem()
    {
        slots[selectedItemIndex].quantity--;

        if (slots[selectedItemIndex].quantity <= 0 )//이후 가진 게 없다면
        {
            //모든 값 초기화
            selectedItem = null;
            slots[selectedItemIndex].item = null;
            selectedItemIndex = -1;
            ClearSelectedItemWindow();
        }
        UpdateUI();
    }

    public void OnEquipButton()
    {
        //만약 착용중인 아이템이 있다면 그 아이템 UnEquip 메서드 실행
        if (slots[curEquipIndex].equipped)
        {
            UnEquip(curEquipIndex);
        }
        slots[selectedItemIndex].equipped = true; //이후, 선택한 아이템의 착용 여부 활성화
        curEquipIndex = selectedItemIndex; //이후 착용한 아이템의 인덱스를 저장
        CharacterManager.Instance.Player.equip.EquipNew(selectedItem); //그 객체를 플레이어에 직접 착용시키기
        UpdateUI(); //UI 업데이트
        SelectItem(selectedItemIndex); //아이템 다시 선택해놓기
    }

    void UnEquip(int index)
    {
        slots[index].equipped = false; //입력된 인덱스의 equipped 변수를 비활성화시키고
        CharacterManager.Instance.Player.equip.UnEquip(); //플레이어에서 직접 착용 해제
        UpdateUI(); //UI 업데이트

        if(selectedItemIndex==index) //만약 선택한 아이템 인덱스가 제시된 인덱스와 같다면
        {
            SelectItem(selectedItemIndex); //다시 아이템 선택
        }
    }
    public void OnUnEquipButton()
    {
        //그냥 착용 해제
        UnEquip(selectedItemIndex);
    }
}
