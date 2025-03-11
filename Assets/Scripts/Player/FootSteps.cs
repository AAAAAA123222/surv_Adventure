using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Footsteps : MonoBehaviour
{
    public AudioClip[] footstepClips;
    private AudioSource audioSource;
    private Rigidbody _rigidbody;
    public float footstepThreshold;
    public float footstepRate;
    private float footStepTime;

    // Start is called before the first frame update
    void Start()
    {
        _rigidbody=GetComponent<Rigidbody>();
        audioSource=GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        //ĳ���Ͱ� ���� �پ���������
        if (Mathf.Abs(_rigidbody.velocity.y) < 0.1f)
        {
            //�̵��Ÿ��� ������ġ�� �Ѿ��
            if(_rigidbody.velocity.magnitude>footstepThreshold)
            {
                if(Time.time-footStepTime>footstepRate)
                {
                    footStepTime = Time.time;
                    audioSource.PlayOneShot(footstepClips[Random.Range(0, footstepClips.Length)]);
                }
            }
        }
        
    }
}
