using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    [Header("Movement")]
    public float moveSpeed;
    public float jumpPower;
    private Vector2 curMovementInput;
    public LayerMask groundLayerMask;

    [Header("Look")]
    public Transform cameraContainer; //카메라 컨테이너
    public float minXLook; //회전 범위 최솟값
    public float maxXLook; //회전 범위 최댓값
    private float camCurXRot; //마우스의 방향 값
    public float lookSensitivity; //마우스 감도
    private Vector2 mouseDelta; //마우스 델타값
    public bool canLook = true; //주변을 돌아볼 수 있는지

    public Action inventory; //UIInventory에서 가져올 메서드를 담을 인벤토리 델리게이트
    private Rigidbody _rigidbody;


    private void Awake()
    {
        _rigidbody=GetComponent<Rigidbody>();
    }

    private void Start()
    {
        Cursor.lockState= CursorLockMode.Locked;
    }
    private void FixedUpdate()
    {
        Move();        
    }

    private void LateUpdate()
    {
        //만약에 주변을 돌아볼 수 있다면 카메라 전환 시작
        if(canLook)
        {
            CameraLook();
        }
    }

    void Move()
    {
        //앞 뒤 방향 정하기
        //curmovement(Vector2)의 Y값
        Vector3 dir = transform.forward * curMovementInput.y
            //거기에 좌우 값을 정하기
            + transform.right * curMovementInput.x;
        //이후 이동속도를 곱하고 y값을 한번 초기화(점프를 해야만 바뀌니까)
        dir *= moveSpeed;
        dir.y = _rigidbody.velocity.y;

        //이후, 이 값을 방향으로 지정
        _rigidbody.velocity = dir;
    }

    //이동을 받아오는 메서드
    public void OnMove(InputAction.CallbackContext context)
    {
        if (context.phase == InputActionPhase.Performed) //만약 입력이 시작됐다면
        {
            //이동할 값을 그 값으로 지정하기
            curMovementInput = context.ReadValue<Vector2>();
        }
        else if (context.phase == InputActionPhase.Canceled) //만약 입력이 멈췄다면
        {
            //이동 멈추기
            curMovementInput = Vector2.zero;
        }
    }

    void CameraLook()
    {
        //x값 지정 (상하)
        camCurXRot += mouseDelta.y * lookSensitivity; //마우스 델타에서 y값에 감도를 곱한 값을 지정
        camCurXRot = Mathf.Clamp(camCurXRot, minXLook, maxXLook); //그 값이 한계를 넘지 않도록 최소값과 최댓값을 제한하는 함수 사용

        //y값 지정 (좌우)
        cameraContainer.localEulerAngles = new Vector3(-camCurXRot, 0, 0); //좌우 방향을 바꾸고 (단, 위로 올리면 카메라가 아래로 가야 되기 때문에 값 반전)
        transform.eulerAngles += new Vector3(0, mouseDelta.x * lookSensitivity, 0); //위아래를 조정

        //마우스의 위치 x값과 y값이 각각 다른 방향에 지정된 이유
        //시야가 좌우로 꺾인다는 건 y축(상하)을 기준으로 회전하기 때문
        //마찬가지로, 위아래로 꺾인다는 건 x축(좌우)을 기준으로 회전하기 때문
    }

    //마우스 방향 받는 메서드
    public void OnLook(InputAction.CallbackContext context)
    {
        //마우스는 계속 유지되고 있기 때문에 그냥 값만 받아오면 됨
        mouseDelta=context.ReadValue<Vector2>();
    }

    public void Onjump(InputAction.CallbackContext context)
    {
        if(context.phase == InputActionPhase.Started&& IsGrounded()) //입력이 눌렸고, 동시에 땅에 서 있을 경우에
        {
            //리지드바디에 위를 향해 jumpPower만큼 힘을 주기
            _rigidbody.AddForce(Vector2.up * jumpPower, ForceMode.Impulse);
        }
    }

    //땅을 검출하는 메서드
    bool IsGrounded()
    {
        //캐릭터의 위치를 기준으로 0.2배씩 곱해진 위치에 아래를 향하는 광선을 만들고
        Ray[] rays = new Ray[4]
        {
            new Ray(transform.position+(transform.forward*0.2f)+(transform.up*0.01f), Vector3.down),
            new Ray(transform.position+(-transform.forward*0.2f)+(transform.up*0.01f), Vector3.down),
            new Ray(transform.position+(transform.right*0.2f)+(transform.up*0.01f), Vector3.down),
            new Ray(transform.position+(-transform.right*0.2f)+(transform.up*0.01f), Vector3.down),
        };

        //그 광선을 아주 작게 쏴서 바닥과 닿아있는지 확인하고 그 여부에 따라 불리언 반환
        for(int i=0;i<rays.Length;i++)
        {
            if (Physics.Raycast(rays[i], 0.1f, groundLayerMask))
            { return true; }
        }
        return false;
    }
    
    /// <summary>
    /// 인벤토리를 켜는 메서드
    /// </summary>
    /// <param name="context"></param>
    public void OnInventory(InputAction.CallbackContext context)
    {
        if(context.phase==InputActionPhase.Started) //델리게이트에 할당된 UIInventory의 Toggle 메서드 실행
        {
            inventory?.Invoke();
            ToggleCursor();
        }
    }

    /// <summary>
    /// 커서를 잠금을 해제할지 확인하는 메서드
    /// </summary>
    void ToggleCursor()
    {
        bool toggle = Cursor.lockState == CursorLockMode.Locked; //커서의 잠금 모드가 잠금으로 되어있다면 true
        Cursor.lockState = toggle ? CursorLockMode.None : CursorLockMode.Locked; //이후 toggle의 값에 따라 true라면 열고, false라면 잠구기
        canLook = !toggle; //그리고 볼 수 있는지 없는지를 값에 할당
    }
    public IEnumerator SpeedBoostEffect(float modifier, float duration)
    {
        moveSpeed += modifier; // 속도 증가
        Debug.Log("속도 증가! 현재 속도: " + moveSpeed);

        yield return new WaitForSeconds(duration); // 지속 시간만큼 대기

        moveSpeed -= modifier; // 원래 속도로 복귀
        Debug.Log("효과 종료, 속도 복귀: " + moveSpeed);
    }
    public IEnumerator JumpBoostEffect(float modifier, float duration)
    {
        jumpPower += modifier; // 점프력 증가
        Debug.Log("점프력 증가! 현재 점프력: " + jumpPower);

        yield return new WaitForSeconds(duration); // 지속 시간만큼 대기

        jumpPower -= modifier; // 원래 점프력 복귀
        Debug.Log("효과 종료, 점프력 복귀: " + jumpPower);
    }
}
