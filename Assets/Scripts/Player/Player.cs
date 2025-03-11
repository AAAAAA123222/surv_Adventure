using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{

    public PlayerController controller;
    public PlayerCondition condition;
    public Equipment equip;

    public ItemData itemData;
    public Action additem;

    public Transform dropPosition; //아이템 던질 위치
    private void Awake()
    {
        //캐릭터매니저 안에 들어있는 플레이어 값을 이걸로 설정하고
        CharacterManager.Instance.Player = this;
        //스스로의 객체 안에 있는 플레이어컨트롤러와 플레이어컨디션을 가져오기
        controller = GetComponent<PlayerController>();
        condition=GetComponent<PlayerCondition>();
        equip=GetComponent<Equipment>();
    }

}
