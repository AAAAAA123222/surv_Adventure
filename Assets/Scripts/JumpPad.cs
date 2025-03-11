using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JumpPad : MonoBehaviour
{
    [Header("JumpPad")]
    public float jumpStrength;
    private float lastBounced = 0f;  // ������ ���� �ð�
    public float bounceInterval = 0.5f; // ���� ���� ����

    private void OnCollisionEnter(Collision collision)
    {
        //�÷��̾� ���̾� Ȯ��
        if (collision.gameObject.layer == LayerMask.NameToLayer("Player"))
        {
            // Rigidbody�� �ִ��� Ȯ��
            Rigidbody rb = collision.rigidbody;
            if (rb != null)
            {
                // ������ ���� ���� ���� �ð��� �����ٸ� ���� ����
                if (Time.time - lastBounced > bounceInterval)
                {
                    lastBounced = Time.time;
                    rb.AddForce(Vector2.up * jumpStrength, ForceMode.Impulse);
                }
            }
        }
    }
}
