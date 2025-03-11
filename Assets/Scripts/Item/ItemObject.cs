using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//아래 객체를 쉽게 접근하고 시전할 수 있게 만들어주는 인터페이스
public interface IInteractable
{
    public string GetInteractPrompt();
    public void OnInteract();
}


public class ItemObject : MonoBehaviour, IInteractable
{
    public ItemData data;

    public string GetInteractPrompt()
    {
        string str = $"{data.displayName}\n{data.description}";
        return str;
    }

    //상호작용 메서드
    public void OnInteract()
    {
        //플레이어 안에 해당 객체를 넣고 넣는 이벤트를 적용시킨 뒤
        CharacterManager.Instance.Player.itemData = data;
        CharacterManager.Instance.Player.additem?.Invoke();

        //바닥에 있는 객체 삭제
        Destroy(gameObject);
    }
}
