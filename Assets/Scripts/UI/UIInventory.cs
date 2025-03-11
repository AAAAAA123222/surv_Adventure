using System.Collections;
using System.Collections.Generic;
using System.Xml;
using TMPro;
using UnityEngine;

public class UIInventory : MonoBehaviour
{
    //�����۽��� Ŭ������ ������ ���� ����
    public ItemSlot[] slots;
    
    //�κ��丮 â, ���� �ǳ� �� ���� ��ġ ����
    public GameObject inventoryWindow;
    public Transform slotPanel;
    public Transform dropPosition; 

    [Header("Select Item")]
    //Ŀ���� ���� �� �������� ������ ������ ������ ����
    public TextMeshProUGUI selectedItemName;
    public TextMeshProUGUI selectedItemDescription;
    public TextMeshProUGUI selectedStatName;
    public TextMeshProUGUI selectedStatValue;
    //��ư�� �������
    public GameObject useButton;
    public GameObject EquipButton;
    public GameObject unequipButton;
    public GameObject dropButton;

    //������ �޾ƿ� �÷��̾� ��Ʈ�ѷ��� ����� ��������
    private PlayerController controller;
    private PlayerCondition condition;

    //������ �����۰� �������� �ε����� ������ ���� ����
    ItemData selectedItem;
    int selectedItemIndex = 0;

    //������ �������� �ε���
    int curEquipIndex;

    void Start()
    {
        //�÷��̾� ���� �ʱ�ȭ
        controller = CharacterManager.Instance.Player.controller;
        condition= CharacterManager.Instance.Player.condition;
        dropPosition = CharacterManager.Instance.Player.dropPosition;

        //��Ʈ�ѷ��� �κ��丮 �׼ǿ� Toggle �Ҵ�
        controller.inventory += Toggle;

        //�÷��̾� ��ü�� ����ִ� �ֵ������ �׼ǿ� AddItem �Ҵ�
        CharacterManager.Instance.Player.additem += AddItem;

        //�κ��丮�� ���� ���� �迭�� �����ǳ��� ���� �ִ� ������ ������ ����ŭ �����ϱ�
        inventoryWindow.SetActive(false);
        slots = new ItemSlot[slotPanel.childCount];

        for (int i = 0; i < slots.Length; i++)
        {
            //�����ǳ��� n��° �ڽĿ� �Ҵ�� ItemSlot ������Ʈ�� ������ �� �迭�� �Ҵ�
            slots[i]=slotPanel.GetChild(i).GetComponent<ItemSlot>();
            slots[i].index = i;
            slots[i].inventory = this;
        }

        //���� ���� ���� �ʱ�ȭ
        ClearSelectedItemWindow();
        UpdateUI();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    //Ŭ������ �� ǥ�õǴ� ���� �ʱ�ȭ�ϴ� �޼���
    void ClearSelectedItemWindow()
    {
        //���õ� �������� ǥ���ϴ� �� �ʱ�ȭ
        selectedItemName.text = string.Empty;
        selectedItemDescription.text = string.Empty;
        selectedStatName.text = string.Empty;
        selectedStatValue.text = string.Empty;

        //��ư ���� ��Ȱ��ȭ
        useButton.SetActive(false);
        EquipButton.SetActive(false);
        unequipButton.SetActive(false);
        useButton.SetActive(false);
        dropButton.SetActive(false);
    }

    //â�� �Ѵ� �޼���
    public void Toggle()
    {
        if(IsOpen())//���������� ����
        {
            inventoryWindow.SetActive(false);
        }
        else//���������� ����
        {
            inventoryWindow.SetActive(true);
        }

    }

    //�����ִ��� Ȯ���ϴ� �޼���
    public bool IsOpen()
    {
        //���̾��Ű â���� Ȱ��ȭ�� �Ǿ��ִ��� Ȯ���ϰ� �� ���� ��ȯ
        return inventoryWindow.activeInHierarchy;
    }

    void AddItem()
    {
        ItemData data = CharacterManager.Instance.Player.itemData;

        //�ߺ� ���� ���� Ȯ���ϱ� (ScriptableObject�� canStack)
        if(data.canStack)
        {
            ItemSlot slot = GetItemStack(data);
            if(slot!=null)
            {
                slot.quantity++;//������ �ø���
                UpdateUI();
                CharacterManager.Instance.Player.itemData = null;
                return;
            }
        }

        //�ߺ��� �ȵȴٸ� �� ���� ��������
        ItemSlot emptySlot = GetEmptySlot();
        //�ִٸ�
        if(emptySlot!=null)
        {
            emptySlot.item = data;
            emptySlot.quantity = 1;
            UpdateUI();
            CharacterManager.Instance.Player.itemData= null;
            return;
        }
        //���ٸ� ������
        ThrowItem(data);
        //���� ĳ���Ͱ� ��� �ִ� ������ ������ �ʱ�ȭ
        CharacterManager.Instance.Player.itemData = null;
    }

    void UpdateUI()
    {
        //���� ����ŭ
        for(int i=0;i<slots.Length;i++)
        {
            if (slots[i].item != null) //���� ���� �������� ������� �ʴٸ� �������� �� ����
            { 
                slots[i].Set();
            }
            else //����ִٸ� �ʱ�ȭ
            { 
                slots[i].Clear();
            }
        }

    }


    ItemSlot GetItemStack(ItemData data)
    {
        //���� ����ŭ
        for (int i = 0; i < slots.Length; i++)
        {
            //���Կ� �� �������� �ְ� �� �� ������ �Ű������� �ִ� ������ �ѱ��� �ʴ´ٸ�
            if (slots[i].item == data&& slots[i].quantity<data.maxStackAmount)
            {
                //�� �� ��ȯ
                return slots[i];
            }
        }
        return null;
    }

    ItemSlot GetEmptySlot()
    {
        //���� ����ŭ
        for (int i = 0; i < slots.Length; i++)
        {
            //���� �� �������� ����ִٸ�
            if (slots[i].item==null)
            {
                //�� ���� ��ȯ
                return slots[i];
            }
        }
        //������ ���ٰ� ��ȯ
        return null;
    }

    void ThrowItem(ItemData data)
    {
        //������ ���� �������� ������ �� ����������� ��ġ�� ������ ������ �����ϱ�
        Instantiate(data.dropPrefab, dropPosition.position, Quaternion.Euler(Vector3.one * Random.value * 360));
    }
    
    public void SelectItem(int index)
    {
        //�������� ������ ���ư���
        if (slots[index].item == null)
        {
            return;
        }

        selectedItem = slots[index].item;
        selectedItemIndex = index;

        selectedItemName.text = selectedItem.displayName;
        selectedItemDescription.text =selectedItem.description;

        //�ϴ� ���� �ؽ�Ʈ �ʱ�ȭ
        selectedStatName.text = string.Empty;
        selectedStatValue.text = string.Empty;

        //�����ۿ� ü���̳� ������� �����ϴ� ���� ����ִٸ� �����ϱ�
        for(int i = 0; i < selectedItem.consumables.Length; i++)
        {
            //�ؽ�Ʈ�� �����ϴ� Ÿ�԰� ����� ��ȯ���� �� �ٹٲ�
            selectedStatName.text += selectedItem.consumables[i].type.ToString() + "\n";
            selectedStatValue.text += selectedItem.consumables[i].value.ToString() + "\n";

        }

        //�׸��� ������Ÿ�Կ� ���� ����ư�� ���/������ư�� ǥ���ϱ�
        useButton.SetActive(selectedItem.type == ItemType.Consumable);
        EquipButton.SetActive(selectedItem.type == ItemType.Equipable && !slots[index].equipped); //�� ��� �����Ǿ����� ��������
        unequipButton.SetActive(selectedItem.type == ItemType.Equipable && slots[index].equipped); //�� ��� �����Ǿ���������
        //�����ư�� �׳� ǥ��
        dropButton.SetActive(true);
    }

    public void OnUseButton()
    {
        if(selectedItem.type==ItemType.Consumable)
        {
            //�ȿ� ����ִ� �����Ӻ� ����ŭ �ݺ�
            for(int i=0;i<selectedItem.consumables.Length;i++)
            {
                //�� �� �����Ӻ��� ������ �� Ȯ��
                switch(selectedItem.consumables[i].type)
                {
                    //ü���̶�� ü��ȸ��
                    case ConsumableType.Health:
                        condition.Heal(selectedItem.consumables[i].value);
                        break;
                    //������̶�� �����ȸ��
                    case ConsumableType.Hunger:
                        condition.Eat(selectedItem.consumables[i].value);
                        break;
                }
            }

            //���� �׳� �� �޼��� ������ StartCoroutine�� �ߴµ� ���۰� ���ÿ� ������ �Ǿ������ controller���� �����ϴ°ɷ� �ذ��߽��ϴ�.
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
        //���õ� ������ ������
        ThrowItem(selectedItem);
        //���� �� ������
        RemoveSelectedItem();
    }
    
    void RemoveSelectedItem()
    {
        slots[selectedItemIndex].quantity--;

        if (slots[selectedItemIndex].quantity <= 0 )//���� ���� �� ���ٸ�
        {
            //��� �� �ʱ�ȭ
            selectedItem = null;
            slots[selectedItemIndex].item = null;
            selectedItemIndex = -1;
            ClearSelectedItemWindow();
        }
        UpdateUI();
    }

    public void OnEquipButton()
    {
        //���� �������� �������� �ִٸ� �� ������ UnEquip �޼��� ����
        if (slots[curEquipIndex].equipped)
        {
            UnEquip(curEquipIndex);
        }
        slots[selectedItemIndex].equipped = true; //����, ������ �������� ���� ���� Ȱ��ȭ
        curEquipIndex = selectedItemIndex; //���� ������ �������� �ε����� ����
        CharacterManager.Instance.Player.equip.EquipNew(selectedItem); //�� ��ü�� �÷��̾ ���� �����Ű��
        UpdateUI(); //UI ������Ʈ
        SelectItem(selectedItemIndex); //������ �ٽ� �����س���
    }

    void UnEquip(int index)
    {
        slots[index].equipped = false; //�Էµ� �ε����� equipped ������ ��Ȱ��ȭ��Ű��
        CharacterManager.Instance.Player.equip.UnEquip(); //�÷��̾�� ���� ���� ����
        UpdateUI(); //UI ������Ʈ

        if(selectedItemIndex==index) //���� ������ ������ �ε����� ���õ� �ε����� ���ٸ�
        {
            SelectItem(selectedItemIndex); //�ٽ� ������ ����
        }
    }
    public void OnUnEquipButton()
    {
        //�׳� ���� ����
        UnEquip(selectedItemIndex);
    }
}
