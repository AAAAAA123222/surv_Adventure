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
        if (instance == null) //씬에 인스턴스가 없다면
        {
            //이걸 인스턴스로 지정하고 삭제 불가로 지정
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else //인스턴스가 있고
        {
            if(instance==this) //그 인스턴스가 이거라면
            {
                //이걸 지우기
                Destroy(gameObject);
            }
        }
    }



}
