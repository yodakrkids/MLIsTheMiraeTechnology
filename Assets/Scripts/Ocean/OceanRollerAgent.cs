using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.MLAgents;
using Unity.MLAgents.Sensors;
using Unity.MLAgents.Actuators;
using Random = UnityEngine.Random;

public class OceanRollerAgent : Agent
{
    private Transform tr;
    private void Start()
    {
        tr = GetComponent<Transform>();
    }

    public Transform Target; // Agent가 잡을 Target

    /// <summary>
    /// Agent가 플랫폼 외부에 떨어지면 리셋
    /// </summary>
    public override void OnEpisodeBegin()
    {
    }
    /// <summary>
    /// 환경 관찰 및 정보 수집 정책 결정을 위해 브레인에 정보전달
    /// </summary>
    /// <param name="sensor"></param>
    public override void CollectObservations(VectorSensor sensor)
    {
    }

    public float forceMultiplier = 10;
    // public GameObject viewModal = null;
    
    /// <summary>
    /// 브레인으로 전달받은 행동을 실행하는 메소드
    /// </summary>
    /// <param name="actionBuffers"></param>
    public override void OnActionReceived(ActionBuffers actionBuffers)
    {
    }
    
    // 테스트용으로 개발자가 직접 움직이는 함수

    public override void Heuristic(in ActionBuffers actionsOut)
    {
    }
}
