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
    public Transform cameraContainer; //ī�޶� �����̳�
    public float minXLook; //ȸ�� ���� �ּڰ�
    public float maxXLook; //ȸ�� ���� �ִ�
    private float camCurXRot; //���콺�� ���� ��
    public float lookSensitivity; //���콺 ����
    private Vector2 mouseDelta; //���콺 ��Ÿ��
    public bool canLook = true; //�ֺ��� ���ƺ� �� �ִ���

    public Action inventory; //UIInventory���� ������ �޼��带 ���� �κ��丮 ��������Ʈ
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
        //���࿡ �ֺ��� ���ƺ� �� �ִٸ� ī�޶� ��ȯ ����
        if(canLook)
        {
            CameraLook();
        }
    }

    void Move()
    {
        //�� �� ���� ���ϱ�
        //curmovement(Vector2)�� Y��
        Vector3 dir = transform.forward * curMovementInput.y
            //�ű⿡ �¿� ���� ���ϱ�
            + transform.right * curMovementInput.x;
        //���� �̵��ӵ��� ���ϰ� y���� �ѹ� �ʱ�ȭ(������ �ؾ߸� �ٲ�ϱ�)
        dir *= moveSpeed;
        dir.y = _rigidbody.velocity.y;

        //����, �� ���� �������� ����
        _rigidbody.velocity = dir;
    }

    //�̵��� �޾ƿ��� �޼���
    public void OnMove(InputAction.CallbackContext context)
    {
        if (context.phase == InputActionPhase.Performed) //���� �Է��� ���۵ƴٸ�
        {
            //�̵��� ���� �� ������ �����ϱ�
            curMovementInput = context.ReadValue<Vector2>();
        }
        else if (context.phase == InputActionPhase.Canceled) //���� �Է��� ����ٸ�
        {
            //�̵� ���߱�
            curMovementInput = Vector2.zero;
        }
    }

    void CameraLook()
    {
        //x�� ���� (����)
        camCurXRot += mouseDelta.y * lookSensitivity; //���콺 ��Ÿ���� y���� ������ ���� ���� ����
        camCurXRot = Mathf.Clamp(camCurXRot, minXLook, maxXLook); //�� ���� �Ѱ踦 ���� �ʵ��� �ּҰ��� �ִ��� �����ϴ� �Լ� ���

        //y�� ���� (�¿�)
        cameraContainer.localEulerAngles = new Vector3(-camCurXRot, 0, 0); //�¿� ������ �ٲٰ� (��, ���� �ø��� ī�޶� �Ʒ��� ���� �Ǳ� ������ �� ����)
        transform.eulerAngles += new Vector3(0, mouseDelta.x * lookSensitivity, 0); //���Ʒ��� ����

        //���콺�� ��ġ x���� y���� ���� �ٸ� ���⿡ ������ ����
        //�þ߰� �¿�� ���δٴ� �� y��(����)�� �������� ȸ���ϱ� ����
        //����������, ���Ʒ��� ���δٴ� �� x��(�¿�)�� �������� ȸ���ϱ� ����
    }

    //���콺 ���� �޴� �޼���
    public void OnLook(InputAction.CallbackContext context)
    {
        //���콺�� ��� �����ǰ� �ֱ� ������ �׳� ���� �޾ƿ��� ��
        mouseDelta=context.ReadValue<Vector2>();
    }

    public void Onjump(InputAction.CallbackContext context)
    {
        if(context.phase == InputActionPhase.Started&& IsGrounded()) //�Է��� ���Ȱ�, ���ÿ� ���� �� ���� ��쿡
        {
            //������ٵ� ���� ���� jumpPower��ŭ ���� �ֱ�
            _rigidbody.AddForce(Vector2.up * jumpPower, ForceMode.Impulse);
        }
    }

    //���� �����ϴ� �޼���
    bool IsGrounded()
    {
        //ĳ������ ��ġ�� �������� 0.2�辿 ������ ��ġ�� �Ʒ��� ���ϴ� ������ �����
        Ray[] rays = new Ray[4]
        {
            new Ray(transform.position+(transform.forward*0.2f)+(transform.up*0.01f), Vector3.down),
            new Ray(transform.position+(-transform.forward*0.2f)+(transform.up*0.01f), Vector3.down),
            new Ray(transform.position+(transform.right*0.2f)+(transform.up*0.01f), Vector3.down),
            new Ray(transform.position+(-transform.right*0.2f)+(transform.up*0.01f), Vector3.down),
        };

        //�� ������ ���� �۰� ���� �ٴڰ� ����ִ��� Ȯ���ϰ� �� ���ο� ���� �Ҹ��� ��ȯ
        for(int i=0;i<rays.Length;i++)
        {
            if (Physics.Raycast(rays[i], 0.1f, groundLayerMask))
            { return true; }
        }
        return false;
    }
    
    /// <summary>
    /// �κ��丮�� �Ѵ� �޼���
    /// </summary>
    /// <param name="context"></param>
    public void OnInventory(InputAction.CallbackContext context)
    {
        if(context.phase==InputActionPhase.Started) //��������Ʈ�� �Ҵ�� UIInventory�� Toggle �޼��� ����
        {
            inventory?.Invoke();
            ToggleCursor();
        }
    }

    /// <summary>
    /// Ŀ���� ����� �������� Ȯ���ϴ� �޼���
    /// </summary>
    void ToggleCursor()
    {
        bool toggle = Cursor.lockState == CursorLockMode.Locked; //Ŀ���� ��� ��尡 ������� �Ǿ��ִٸ� true
        Cursor.lockState = toggle ? CursorLockMode.None : CursorLockMode.Locked; //���� toggle�� ���� ���� true��� ����, false��� �ᱸ��
        canLook = !toggle; //�׸��� �� �� �ִ��� �������� ���� �Ҵ�
    }
    public IEnumerator SpeedBoostEffect(float modifier, float duration)
    {
        moveSpeed += modifier; // �ӵ� ����
        Debug.Log("�ӵ� ����! ���� �ӵ�: " + moveSpeed);

        yield return new WaitForSeconds(duration); // ���� �ð���ŭ ���

        moveSpeed -= modifier; // ���� �ӵ��� ����
        Debug.Log("ȿ�� ����, �ӵ� ����: " + moveSpeed);
    }
    public IEnumerator JumpBoostEffect(float modifier, float duration)
    {
        jumpPower += modifier; // ������ ����
        Debug.Log("������ ����! ���� ������: " + jumpPower);

        yield return new WaitForSeconds(duration); // ���� �ð���ŭ ���

        jumpPower -= modifier; // ���� ������ ����
        Debug.Log("ȿ�� ����, ������ ����: " + jumpPower);
    }
}
