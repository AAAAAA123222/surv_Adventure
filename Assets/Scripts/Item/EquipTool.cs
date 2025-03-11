using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EquipTool : Equip
{
    public float attackRate;
    private bool attacking;
    public float attackDistance;
    public float useStamina;

    [Header("Resource Gathering")]
    public bool doesGatherResources;

    [Header("Combat")]
    public bool doesDealDamage;
    public int damage;

    private Animator animator;
    private Camera _camera;

    // Start is called before the first frame update
    void Start()
    {
        animator= GetComponent<Animator>();
        _camera = Camera.main;
    
    }

    public override void OnAttackInput()
    {
        if (!attacking) //�������� �ƴ� ��쿡��
        {
            //���׹̳� ��� �޼��� �����ߴµ� �װ� ���̶��
            if (CharacterManager.Instance.Player.condition.UseStamina(useStamina))
            {
                //�������̶�� ���� Ȱ��ȭ�ϰ� �ִϸ��̼� ���
                attacking = true;
                animator.SetTrigger("Attack");
                Invoke("OnCanAttack", attackRate); //attackRate��ŭ�� �ð��� ���� �� OnCanAttack Ȱ��ȭ
            }
        }
    }

    void OnCanAttack()
    {
        attacking = false;
    }

    public void OnHit()
    {
        //���� �����
        Ray ray = _camera.ScreenPointToRay(new Vector3(Screen.width / 2, Screen.height / 2, 0));
        RaycastHit hit;

        //���� ������ ���� �¾Ҵٸ�
        if(Physics.Raycast(ray,out hit, attackDistance))
        {
            //�ڿ��� ä���� �� �ִ� �����ε��ٰ� ���� �ǻ�ü�� ���ҽ� ������Ʈ�� �־��ٸ�
            if(doesGatherResources&&hit.collider.TryGetComponent(out Resource resource))
            {
                //�� �ǻ�ü�� ������Ʈ���� Gather ����
                resource.Gather(hit.point, hit.normal);
            }
            //���� �ǻ�ü�� ���ҽ� ������Ʈ�� �־��ٸ�
            if(hit.collider.TryGetComponent(out NPC npc))
            {
                //�� �ǻ�ü�� ������Ʈ���� ������ ����
                npc.TakePhysicalDamage(damage);
            }
        }

    }

}
