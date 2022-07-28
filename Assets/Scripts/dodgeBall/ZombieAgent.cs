using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.MLAgents;
using Unity.MLAgents.Sensors;
using Unity.MLAgents.Actuators;
using Random = UnityEngine.Random;

public class ZombieAgent : Agent
{
    private Rigidbody rbody;
    private void Start()
    {
        rbody = GetComponent<Rigidbody>(); // Agent의 리지드바디를 참조
    }

    public Transform Target; // Agent가 잡을 Target

    /// <summary>
    /// 에피소드가 시작할때마다 호출되는 코드
    /// </summary>
    public override void OnEpisodeBegin()
    {
        rbody.angularVelocity = Vector3.zero;
        rbody.velocity = Vector3.zero;
        transform.localPosition = new Vector3(Random.Range(-3f, 3f), 0.5f, Random.Range(-1.5f, 1.5f));
    }
    /// <summary>
    /// 환경 관찰 및 정보 수집 정책 결정을 위해 브레인에 정보전달
    /// </summary>
    /// <param name="sensor"></param>
    public override void CollectObservations(VectorSensor sensor)
    {
        sensor.AddObservation(Target.localPosition); // target위치
        sensor.AddObservation(transform.localPosition); // 자신의 위치
        
        sensor.AddObservation(rbody.velocity.x); // 자신의 속도
        sensor.AddObservation(rbody.velocity.z); // 자신의 속도
        
        sensor.AddObservation(Target.GetComponent<Rigidbody>().velocity.x); // 사람의 속도
        sensor.AddObservation(Target.GetComponent<Rigidbody>().velocity.z); // 사람의 속도
    }

    // public GameObject viewModal = null;
    
    /// <summary>
    /// 브레인으로 전달받은 행동을 실행하는 메소드
    /// </summary>
    /// <param name="actionBuffers"></param>
    public override void OnActionReceived(ActionBuffers actionBuffers)
    {
        float MoveX = actionBuffers.ContinuousActions[0];
        float MoveZ = actionBuffers.ContinuousActions[1];

        transform.rotation = Quaternion.LookRotation(Target.transform.position - transform.position).normalized;
        transform.position += new Vector3(MoveX, 0, MoveZ) * Time.deltaTime * 10.5f;
       // transform.LookAt(Target);
        
        float distanceToTarget = Vector3.Distance(transform.localPosition, Target.localPosition);
            
        if (distanceToTarget < 1.42)
        {
            SetReward(1.0f);
            EndEpisode();
        }
        
        SetReward(-0.001f);
    }
    
    // 테스트용으로 개발자가 직접 움직이는 함수

    public override void Heuristic(in ActionBuffers actionsOut)
    {
        //base.Heuristic(actionsOut);
        ActionSegment<float> continuousActions = actionsOut.ContinuousActions;
        continuousActions[0] = Input.GetAxisRaw("Horizontal");
        continuousActions[1] = Input.GetAxisRaw("Vertical");
    }
}
