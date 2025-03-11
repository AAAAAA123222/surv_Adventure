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
        if (!attacking) //공격중이 아닐 경우에만
        {
            //스테미나 사용 메서드 실행했는데 그게 참이라면
            if (CharacterManager.Instance.Player.condition.UseStamina(useStamina))
            {
                //공격중이라는 변수 활성화하고 애니메이션 재생
                attacking = true;
                animator.SetTrigger("Attack");
                Invoke("OnCanAttack", attackRate); //attackRate만큼의 시간이 지난 뒤 OnCanAttack 활성화
            }
        }
    }

    void OnCanAttack()
    {
        attacking = false;
    }

    public void OnHit()
    {
        //광선 만들기
        Ray ray = _camera.ScreenPointToRay(new Vector3(Screen.width / 2, Screen.height / 2, 0));
        RaycastHit hit;

        //만약 광선이 뭔가 맞았다면
        if(Physics.Raycast(ray,out hit, attackDistance))
        {
            //자원을 채취할 수 있는 도구인데다가 맞은 피사체가 리소스 컴포넌트가 있었다면
            if(doesGatherResources&&hit.collider.TryGetComponent(out Resource resource))
            {
                //그 피사체의 컴포넌트에서 Gather 실행
                resource.Gather(hit.point, hit.normal);
            }
            //맞은 피사체가 리소스 컴포넌트가 있었다면
            if(hit.collider.TryGetComponent(out NPC npc))
            {
                //그 피사체의 컴포넌트에서 데미지 실행
                npc.TakePhysicalDamage(damage);
            }
        }

    }

}
