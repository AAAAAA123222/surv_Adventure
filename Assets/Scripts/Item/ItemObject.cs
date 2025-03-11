using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//�Ʒ� ��ü�� ���� �����ϰ� ������ �� �ְ� ������ִ� �������̽�
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

    //��ȣ�ۿ� �޼���
    public void OnInteract()
    {
        //�÷��̾� �ȿ� �ش� ��ü�� �ְ� �ִ� �̺�Ʈ�� �����Ų ��
        CharacterManager.Instance.Player.itemData = data;
        CharacterManager.Instance.Player.additem?.Invoke();

        //�ٴڿ� �ִ� ��ü ����
        Destroy(gameObject);
    }
}
