using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;

public class Interaction : MonoBehaviour
{
    public float checkRate = 0.05f; //광선을 쏘는 주기
    private float lastCheckTime; //그 광선을 마지막으로 쏜 시간
    public float maxCheckDistance; //광선의 최대 사거리
    public LayerMask layerMask; //어떤 레이어의 객체를 추출할지

    public GameObject curInteractGameObject; //상호작용된 객체를 담을 변수
    private IInteractable curInteractable; //상호작용된 객체 안의 컴포넌트

    public TextMeshProUGUI promptText; //출력할 텍스트
    private Camera _camera; //카메라

    private void Start()
    {
        //메인카메라를 인터랙션 카메라로 지정
        _camera = Camera.main;
    }

    private void Update()
    {
        if (Time.time-lastCheckTime>checkRate) //현재 시간에 이전에 광선을 쏜 시간을 뺀 값이 쏘는 주기보다 크다면(checkRate마다) 내용 실행
        {
            lastCheckTime = Time.time;

        //카메라를 기준으로, 그 카메라의 정중앙에 광선을 저장하고 그 정보를 담을 그릇도 변수로 저장하기
        Ray ray = _camera.ScreenPointToRay(new Vector3(Screen.width / 2, Screen.height / 2));
        RaycastHit hit;

        //저장한 광선을 쏜 뒤 적합한 레이어마스크의 객체를 hit로 전달
        if(Physics.Raycast(ray,out hit, maxCheckDistance, layerMask)) //최대 상호작용 거리 안에서 무언가 제대로 맞았다면
        {
            if (hit.collider.gameObject != curInteractGameObject) //이미 상호작용 중이 아닌 경우
            {
                curInteractGameObject=hit.collider.gameObject; //맞은 객체를 현재 객체로 저장
                curInteractable=hit.collider.GetComponent<IInteractable>(); //맞은 객체의 컴포넌트도 저장하기
                SetPromptText();
            }
        }
        else //맞지도 않았다면
        {
            //그냥 다 없애기
            curInteractGameObject= null; 
            curInteractable = null;
            promptText.gameObject.SetActive(false);
            }
        }
    }

    //프롬프트를 켜고 그 내용을 변경하는 메서드
    private void SetPromptText()
    {
        promptText.gameObject.SetActive(true); //설명 텍스트 객체를 활성화시키고
        promptText.text = curInteractable.GetInteractPrompt(); //그 텍스트 객체의 내용을 받아온 상호작용 객체의 프롬프트로 변경
    }

    //상호작용했을때 상호작용시키고 인터페이스 초기화
    public void OnInteractInput(InputAction.CallbackContext context)
    {
        if(context.phase==InputActionPhase.Started&&curInteractable!=null) //키를 눌렀고 또 안에 뭐가 있는 상태라면
        {
            curInteractable.OnInteract(); //객체를 플레이어에게 할당하고 떨어진 객체를 제거하는 메서드 실행시키기
            //이후 인터페이스 초기화
            curInteractGameObject = null; 
            curInteractable = null;
            promptText.gameObject.SetActive(false);
        }
    }


}
