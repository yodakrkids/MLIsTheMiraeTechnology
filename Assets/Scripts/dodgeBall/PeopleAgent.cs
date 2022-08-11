using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;
using Unity.MLAgents;
using Unity.MLAgents.Sensors;
using Unity.MLAgents.Actuators;
using Random = UnityEngine.Random;

public class PeopleAgent : Agent
{
    private Rigidbody rbody;
    private Vector3 movement;

    private void Start()
    {
        rbody = GetComponent<Rigidbody>(); // Agent의 리지드바디를 참조
    }
    public GameObject Tree; // 피해야할 나무
    public Transform Target; // Agent가 잡을 Target

    /// <summary>
    /// 에피소드가 시작할때마다 호출되는 코드
    /// </summary>
    public override void OnEpisodeBegin()
    {
        if (transform.localPosition.y < 0)
        {
            rbody.angularVelocity = Vector3.zero;
            rbody.velocity = Vector3.zero;
            transform.localPosition = new Vector3(0, 0.5f, 0);
        }

        Target.localPosition = new Vector3(Random.Range(-6f, 8f), 0.5f, Random.Range(-9.0f, 4f));
        for (int i = 0; i < Tree.transform.childCount; i++)
        {
            Tree.transform.GetChild(i).localPosition = new Vector3(Random.Range(-8f, 10f), -3.0f, Random.Range(-11.0f, 6f));
        }

    }
    
    /// <summary>
    /// 환경 관찰 및 정보 수집 정책 결정을 위해 브레인에 정보전달
    /// </summary>
    /// <param name="sensor"></param>
    public override void CollectObservations(VectorSensor sensor)
    {
        sensor.AddObservation(Target.localPosition); // target위치
        sensor.AddObservation(GameObject.FindWithTag("Tree").transform.localPosition); // 나무의 위치? 나무들의 위치?
        sensor.AddObservation(transform.localPosition); // 자신의 위치
        
        sensor.AddObservation(rbody.velocity.x); // 자신의 속도
        sensor.AddObservation(rbody.velocity.z); // 자신의 속도
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

        movement = new Vector3(MoveX, 0, MoveZ);

        if (!(MoveX == 0 && MoveZ == 0))
        {
            transform.rotation =
                Quaternion.Lerp(transform.rotation, Quaternion.LookRotation(movement), Time.deltaTime * 10.0f);
        }
        transform.position += new Vector3(MoveX, 0, MoveZ) * Time.deltaTime * 10.0f;

        float distanceToTarget = Vector3.Distance(transform.localPosition, Target.localPosition);

        if (distanceToTarget < 1.42)
        {
            SetReward(1.0f);
            EndEpisode();
        }

        // 플랫폼 밖으로 나가면 Episode 종료
        if (transform.localPosition.y < -0.5f)
        {
            EndEpisode();
        }

        AddReward(-1f / MaxStep);
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
