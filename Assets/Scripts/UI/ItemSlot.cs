using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ItemSlot : MonoBehaviour
{
    public ItemData item; //가진 아이템

    public Button button;
    public Image icon;
    public TextMeshProUGUI quantityText;
    private Outline outline;

    public UIInventory inventory; //아이템 슬롯의 위치

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
        //장착되어있을때 외곽선 활성화
        outline.enabled = equipped;
    }

    public void Set()
    {
        //아이콘 객체 켜고 스프라이트는 아이템에 들어있는 스프라이트로 적용
        icon.gameObject.SetActive(true);
        icon.sprite = item.icon;
        //수량 텍스트 적용(수량이 1보다 크면 수량을 텍스트로 표기, 없다면 그냥 없음 표시
        quantityText.text = quantity > 1 ? quantity.ToString() : string.Empty;

        //만약 외곽선이 null이 아니라면
        if(outline!=null)
        {
            //착용 여부에 따라 바꾸기
            outline.enabled = equipped;
        }
    }

    public void Clear()
    {
        //전부 초기화
        item = null;
        icon.gameObject.SetActive(false);
        quantityText.text= string.Empty;
    }

    public void OnClickButton()
    {
        //index를 매개변수 삼아 UIInventory의 SelectItem 함수 실행
        inventory.SelectItem(index);
    }
}
