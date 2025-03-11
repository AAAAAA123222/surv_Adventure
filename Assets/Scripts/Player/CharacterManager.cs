using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterManager : MonoBehaviour
{
    private static CharacterManager instance;
    public static CharacterManager Instance
    {
        get
        {
            if (instance == null)
            {
                instance = new GameObject("CharacterManager").AddComponent<CharacterManager>();
            }
            return instance;
        }

    }

    public Player player;
    public Player Player
    {
        get { return player; }
        set { player = value; }
    }

    private void Awake()
    {
        if (instance == null) //���� �ν��Ͻ��� ���ٸ�
        {
            //�̰� �ν��Ͻ��� �����ϰ� ���� �Ұ��� ����
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else //�ν��Ͻ��� �ְ�
        {
            if(instance==this) //�� �ν��Ͻ��� �̰Ŷ��
            {
                //�̰� �����
                Destroy(gameObject);
            }
        }
    }



}
