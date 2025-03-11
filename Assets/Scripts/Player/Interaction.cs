using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;

public class Interaction : MonoBehaviour
{
    public float checkRate = 0.05f; //������ ��� �ֱ�
    private float lastCheckTime; //�� ������ ���������� �� �ð�
    public float maxCheckDistance; //������ �ִ� ��Ÿ�
    public LayerMask layerMask; //� ���̾��� ��ü�� ��������

    public GameObject curInteractGameObject; //��ȣ�ۿ�� ��ü�� ���� ����
    private IInteractable curInteractable; //��ȣ�ۿ�� ��ü ���� ������Ʈ

    public TextMeshProUGUI promptText; //����� �ؽ�Ʈ
    private Camera _camera; //ī�޶�

    private void Start()
    {
        //����ī�޶� ���ͷ��� ī�޶�� ����
        _camera = Camera.main;
    }

    private void Update()
    {
        if (Time.time-lastCheckTime>checkRate) //���� �ð��� ������ ������ �� �ð��� �� ���� ��� �ֱ⺸�� ũ�ٸ�(checkRate����) ���� ����
        {
            lastCheckTime = Time.time;

        //ī�޶� ��������, �� ī�޶��� ���߾ӿ� ������ �����ϰ� �� ������ ���� �׸��� ������ �����ϱ�
        Ray ray = _camera.ScreenPointToRay(new Vector3(Screen.width / 2, Screen.height / 2));
        RaycastHit hit;

        //������ ������ �� �� ������ ���̾��ũ�� ��ü�� hit�� ����
        if(Physics.Raycast(ray,out hit, maxCheckDistance, layerMask)) //�ִ� ��ȣ�ۿ� �Ÿ� �ȿ��� ���� ����� �¾Ҵٸ�
        {
            if (hit.collider.gameObject != curInteractGameObject) //�̹� ��ȣ�ۿ� ���� �ƴ� ���
            {
                curInteractGameObject=hit.collider.gameObject; //���� ��ü�� ���� ��ü�� ����
                curInteractable=hit.collider.GetComponent<IInteractable>(); //���� ��ü�� ������Ʈ�� �����ϱ�
                SetPromptText();
            }
        }
        else //������ �ʾҴٸ�
        {
            //�׳� �� ���ֱ�
            curInteractGameObject= null; 
            curInteractable = null;
            promptText.gameObject.SetActive(false);
            }
        }
    }

    //������Ʈ�� �Ѱ� �� ������ �����ϴ� �޼���
    private void SetPromptText()
    {
        promptText.gameObject.SetActive(true); //���� �ؽ�Ʈ ��ü�� Ȱ��ȭ��Ű��
        promptText.text = curInteractable.GetInteractPrompt(); //�� �ؽ�Ʈ ��ü�� ������ �޾ƿ� ��ȣ�ۿ� ��ü�� ������Ʈ�� ����
    }

    //��ȣ�ۿ������� ��ȣ�ۿ��Ű�� �������̽� �ʱ�ȭ
    public void OnInteractInput(InputAction.CallbackContext context)
    {
        if(context.phase==InputActionPhase.Started&&curInteractable!=null) //Ű�� ������ �� �ȿ� ���� �ִ� ���¶��
        {
            curInteractable.OnInteract(); //��ü�� �÷��̾�� �Ҵ��ϰ� ������ ��ü�� �����ϴ� �޼��� �����Ű��
            //���� �������̽� �ʱ�ȭ
            curInteractGameObject = null; 
            curInteractable = null;
            promptText.gameObject.SetActive(false);
        }
    }


}
