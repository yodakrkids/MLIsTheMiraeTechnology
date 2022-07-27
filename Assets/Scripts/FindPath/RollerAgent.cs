using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.MLAgents;
using Unity.MLAgents.Sensors;
using Unity.MLAgents.Actuators;
using Random = UnityEngine.Random;

public class RollerAgent : Agent
{
    private Rigidbody rigidbody;
    private void Start()
    {
        rigidbody = GetComponent<Rigidbody>(); // Agent의 리지드바디를 참조
    }

    public Transform Target; // Agent가 잡을 Target

    /// <summary>
    /// Agent가 플랫폼 외부에 떨어지면 리셋
    /// </summary>
    public override void OnEpisodeBegin()
    {
        if (this.transform.localPosition.y < 0)
        {
            this.rigidbody.angularVelocity = Vector3.zero;
            this.rigidbody.velocity = Vector3.zero;
            this.transform.localPosition = new Vector3(0, 0.5f, 0); 
        }
        Target.localPosition = new Vector3(Random.value * 8 - 4, 0.5f, Random.value * 8 - 4);
    }
    /// <summary>
    /// 환경 관찰 및 정보 수집 정책 결정을 위해 브레인에 정보전달
    /// </summary>
    /// <param name="sensor"></param>
    public override void CollectObservations(VectorSensor sensor)
    {
        sensor.AddObservation(Target.localPosition); // target위치
        sensor.AddObservation(this.transform.localPosition); // 자신의 위치
        
        sensor.AddObservation(rigidbody.velocity.x); // 자신의 속도
        sensor.AddObservation(rigidbody.velocity.z); // 자신의 속도
    }

    public float forceMultiplier = 10;
    
    /// <summary>
    /// 브레인으로 전달받은 행동을 실행하는 메소드
    /// </summary>
    /// <param name="actionBuffers"></param>
    public override void OnActionReceived(ActionBuffers actionBuffers)
    {
        Vector3 controlSignal = Vector3.zero;
        controlSignal.x = actionBuffers.ContinuousActions[0];
        controlSignal.z = actionBuffers.ContinuousActions[1];
        rigidbody.AddForce(controlSignal * forceMultiplier);
        
        float distanceToTarget = Vector3.Distance(this.transform.localPosition, Target.localPosition);
        
        if (distanceToTarget < 1.42)
        {
            SetReward(1.0f);
            EndEpisode();
        }
        
        else if (this.transform.localPosition.y < 0)
        {
            EndEpisode();
        }
    }
    
    // 테스트용으로 개발자가 직접 움직이는 함수

    public override void Heuristic(in ActionBuffers actionsOut)
    {
        //base.Heuristic(actionsOut);
        var continuousActionsOut = actionsOut.ContinuousActions;
        continuousActionsOut[0] = Input.GetAxis("Horizontal");
        continuousActionsOut[1] = Input.GetAxis("Vertical");
    }
}
