using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.MLAgents;
using Unity.MLAgents.Actuators;
using Unity.MLAgents.Sensors;

public class MoveToGoalAgent : Agent
{
    [SerializeField] private Transform _target;
    [SerializeField] private float _moveSpeed = 1f;
    [SerializeField] private Material _winMat;
    [SerializeField] private Material _loseMat;
    [SerializeField] private MeshRenderer _floorMeshRenderer;

    public override void OnEpisodeBegin()
    {
        transform.localPosition = new Vector3(Random.Range(-3f, 1f), 0, Random.Range(-2f, 2f));
        _target.localPosition = new Vector3(Random.Range(2.5f, 4f), 0, Random.Range(-3f, 3f));
    }

    public override void CollectObservations(VectorSensor sensor)
    {
        sensor.AddObservation(transform.localPosition);
        sensor.AddObservation(_target.localPosition);
    }

    public override void OnActionReceived(ActionBuffers actions)
    {
        float moveX = actions.ContinuousActions[0];
        float moveZ = actions.ContinuousActions[1];

        transform.localPosition += new Vector3(moveX, 0, moveZ) * Time.deltaTime * _moveSpeed;
    }

    public override void Heuristic(in ActionBuffers actionsOut)
    {
        ActionSegment<float> continiusActions = actionsOut.ContinuousActions;
        continiusActions[0] = Input.GetAxisRaw("Horizontal");
        continiusActions[1] = Input.GetAxisRaw("Vertical");
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent<Goal>(out Goal goal))
        {
            SetReward(1f);
            _floorMeshRenderer.material = _winMat;
            EndEpisode();
        }
        if (other.TryGetComponent<Wall>(out Wall wall))
        {
            SetReward(-1f);
            _floorMeshRenderer.material = _loseMat;
            EndEpisode();
        }
    }
}
