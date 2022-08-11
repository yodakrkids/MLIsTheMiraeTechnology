using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.MLAgents;
using Unity.MLAgents.Sensors;
using Unity.MLAgents.Actuators;
using Random = UnityEngine.Random;


public class BoyAgent : Agent
{
    private float currentTime = 0f; // 에피소드가 시작한 시간 계산
    private Rigidbody rbody;
    public float npcSpeed = 1.0f;
    
    
    // Start is called before the first frame update
    void Start()
    {
        rbody = GetComponent<Rigidbody>();
    }

    public GameObject Target; // 꽃게의 position
    
    /// <summary>
    /// 에피소드가 처음 시작할때 꽃게의 장소를 랜덤으로 주기 위해 작성
    /// </summary>
    public override void OnEpisodeBegin()
    {
        currentTime = 0f;
        
        rbody.angularVelocity = Vector3.zero;
        rbody.velocity = Vector3.zero;

        Target.transform.localPosition = new Vector3(Random.Range(-4.5f, 4.4f),0.5f,Random.Range(-4f, 4f));
    }
    
    /// <summary>
    /// 움직이기 위해 필요한 정보를 brain에 전달
    /// </summary>
    /// <param name="sensor"></param>
    public override void CollectObservations(VectorSensor sensor)
    {
        sensor.AddObservation(Target.transform.localPosition); // target위치
        sensor.AddObservation(transform.localPosition); // 자신의 위치
        
        sensor.AddObservation(rbody.velocity.x); // 자신의 속도
        sensor.AddObservation(rbody.velocity.z); // 자신의 속도
    }

    /// <summary>
    /// 전달받은 정보를 통해 실행하는 메소드
    /// </summary>
    /// <param name="actionBuffers"></param>
    public override void OnActionReceived(ActionBuffers actionBuffers)
    {
        currentTime += Time.deltaTime;
        
        float moveX = actionBuffers.ContinuousActions[0];
        float moveZ = actionBuffers.ContinuousActions[1];
        
        transform.position += new Vector3(moveX, 0, moveZ) * Time.deltaTime * npcSpeed;
    }
}
