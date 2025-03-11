using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ItemSlot : MonoBehaviour
{
    public ItemData item; //���� ������

    public Button button;
    public Image icon;
    public TextMeshProUGUI quantityText;
    private Outline outline;

    public UIInventory inventory; //������ ������ ��ġ

    public int index;
    public bool equipped;
    public int quantity;

    // Start is called before the first frame update
    void Awake()
    {
        outline = GetComponent<Outline>();
    }

    private void OnEnable()
    {
        //�����Ǿ������� �ܰ��� Ȱ��ȭ
        outline.enabled = equipped;
    }

    public void Set()
    {
        //������ ��ü �Ѱ� ��������Ʈ�� �����ۿ� ����ִ� ��������Ʈ�� ����
        icon.gameObject.SetActive(true);
        icon.sprite = item.icon;
        //���� �ؽ�Ʈ ����(������ 1���� ũ�� ������ �ؽ�Ʈ�� ǥ��, ���ٸ� �׳� ���� ǥ��
        quantityText.text = quantity > 1 ? quantity.ToString() : string.Empty;

        //���� �ܰ����� null�� �ƴ϶��
        if(outline!=null)
        {
            //���� ���ο� ���� �ٲٱ�
            outline.enabled = equipped;
        }
    }

    public void Clear()
    {
        //���� �ʱ�ȭ
        item = null;
        icon.gameObject.SetActive(false);
        quantityText.text= string.Empty;
    }

    public void OnClickButton()
    {
        //index�� �Ű����� ��� UIInventory�� SelectItem �Լ� ����
        inventory.SelectItem(index);
    }
}
