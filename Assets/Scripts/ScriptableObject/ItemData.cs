using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum ItemType
{
    Equipable,
    Consumable,
    Resource
}

public enum ConsumableType
{
    Health,
    Hunger
}

public enum boostType
{
    None,
    Speed,
    JumpPower
}

[Serializable]
public class ItemDataConsumable
{
    public ConsumableType type;
    public float value;
}

[CreateAssetMenu(fileName ="Item",menuName ="New Item")]
public class ItemData : ScriptableObject
{
    [Header("Info")]
    public string displayName; //이름
    public string description; //설명
    public ItemType type; //아이템 종류
    public Sprite icon; //UI 아이콘
    public GameObject dropPrefab; //떨어뜨렸을때 나올 프리팹

    [Header("Stacking")]
    public bool canStack; //겹치기 여부
    public int maxStackAmount; //겹쳐야 할 수

    [Header("Consumable")]
    public ItemDataConsumable[] consumables; //소모품이라면, 체력과 배고픔 중 어떤 것에 영향을 주는지

    [Header("changeOverTime")]
    public boostType boostType;
    public float duration; //지속시간
    public float modifier; //값

    [Header("Equip")]
    public GameObject equipPrefab; //장비라면, 착용했을 때 어떤 프리팹을 활성화시킬지



}
