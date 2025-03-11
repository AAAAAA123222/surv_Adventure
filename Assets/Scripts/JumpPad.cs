using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JumpPad : MonoBehaviour
{
    [Header("JumpPad")]
    public float jumpStrength;
    private float lastBounced = 0f;  // 마지막 점프 시간
    public float bounceInterval = 0.5f; // 점프 간격 설정

    private void OnCollisionEnter(Collision collision)
    {
        //플레이어 레이어 확인
        if (collision.gameObject.layer == LayerMask.NameToLayer("Player"))
        {
            // Rigidbody가 있는지 확인
            Rigidbody rb = collision.rigidbody;
            if (rb != null)
            {
                // 마지막 점프 이후 일정 시간이 지났다면 점프 가능
                if (Time.time - lastBounced > bounceInterval)
                {
                    lastBounced = Time.time;
                    rb.AddForce(Vector2.up * jumpStrength, ForceMode.Impulse);
                }
            }
        }
    }
}
