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
    
    private float currentTime = 0f;
    
    private Rigidbody rbody;
    public GameObject personObj;
    public GameObject zombieObj;
    private void Start()
    {
        rbody = GetComponent<Rigidbody>(); // 리지드바디를 참조
      //  personObj = GameObject.Find("Girl");
       // zombieObj = GameObject.Find("Zombie");
    }

    //public Transform Target;
    /// <summary>
    /// 에피소드가 시작할때마다 호출되는 코드
    /// 사람과 좀비의 위치를 리셋
    /// </summary>
    public override void OnEpisodeBegin()
    {
        currentTime = 0f;
        rbody.angularVelocity = Vector3.zero;
        rbody.velocity = Vector3.zero;
        
        //Target.transform.localPosition = new Vector3(-1.2f,0.5f,0);
        //transform.localPosition = new Vector3(Random.Range(-13f, 0f), 0.5f, Random.Range(-3f, 7));

        zombieObj.transform.localPosition = new Vector3(-1.2f,0.5f,0);
        personObj.transform.localPosition = new Vector3(Random.Range(-13f, 0f), 0.5f, Random.Range(-3f, 7));
    }
    /// <summary>
    /// 환경 관찰 및 정보 수집 정책 결정을 위해 브레인에 정보전달
    /// </summary>
    /// <param name="sensor"></param>
    public override void CollectObservations(VectorSensor sensor)
    {
       sensor.AddObservation(zombieObj.transform.localPosition); // target위치
       sensor.AddObservation(personObj.transform.localPosition); // 자신의 위치
        
       sensor.AddObservation(zombieObj.GetComponent<Rigidbody>().velocity.x); // 좀비의 속도
       sensor.AddObservation(zombieObj.GetComponent<Rigidbody>().velocity.z); 
        
        sensor.AddObservation(personObj.GetComponent<Rigidbody>().velocity.x); // 사람의 속도
        sensor.AddObservation(personObj.GetComponent<Rigidbody>().velocity.z); 
       
       //sensor.AddObservation(Target.localPosition); // target위치
       //sensor.AddObservation(transform.localPosition); // 자신의 위치
        
       //sensor.AddObservation(rbody.velocity.x); // 자신의 속도
       //sensor.AddObservation(rbody.velocity.z); // 자신의 속도
        
       //sensor.AddObservation(Target.GetComponent<Rigidbody>().velocity.x); // 좀비의 속도
       //sensor.AddObservation(Target.GetComponent<Rigidbody>().velocity.z); // 좀비의 속도
       
    }

    // public GameObject viewModal = null;

    /// <summary>
    /// 브레인으로 전달받은 행동을 실행하는 메소드
    /// </summary>
    /// <param name="actionBuffers"></param>
    public override void OnActionReceived(ActionBuffers actionBuffers)
    {
        currentTime += Time.deltaTime;

        float MoveX = actionBuffers.ContinuousActions[0];
        float MoveZ = actionBuffers.ContinuousActions[1];

        zombieObj.transform.rotation =
            Quaternion.LookRotation(personObj.transform.position - zombieObj.transform.position).normalized;
        transform.position += new Vector3(MoveX, 0, MoveZ) * Time.deltaTime * Random.Range(1f, 5f);
        // transform.LookAt(Target);

        float distanceToTarget = Vector3.Distance(zombieObj.transform.localPosition, personObj.transform.localPosition);

        if (gameObject.CompareTag("zombie"))
        {
            AddReward(-0.001f);
        }
        if (gameObject.CompareTag("Player"))
        {
            AddReward(0.001f);
        }
        
       // if (distanceToTarget > 20f)
       // {
        //    if (gameObject.CompareTag("Player"))
          //  {
        //        AddReward(0.001f);
         //   }
      //  }
        
        if (distanceToTarget <= 1.42f)
        {
            if (gameObject.CompareTag("Player"))
            {
                SetReward(-1f);
            }

            if (gameObject.CompareTag("zombie"))
            {
                AddReward(+1f);
            }
            EndEpisode();
        }
        

        if (currentTime > 50f)
        {
            if (gameObject.CompareTag("Player"))
            {
                AddReward(+1f);
            }
            if (gameObject.CompareTag("zombie"))
            {
                SetReward(-1f);
            }

            EndEpisode();
        }
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
