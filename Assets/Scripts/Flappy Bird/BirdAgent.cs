using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.MLAgents;
using Unity.MLAgents.Actuators;
using Unity.MLAgents.Sensors;

namespace FlappyBird
{


    public class BirdAgent : Agent
    {
        [SerializeField] Bird bird;
        [SerializeField] private Level level;


        private bool isJumpInputDown;
        void Awake()
        {
            //bird = this.GetComponent<Bird>();
        }

        // Update is called once per frame
        void Update()
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                isJumpInputDown = true;
            }
        }

        public override void OnEpisodeBegin()
        {
            bird.Reset();
        }

        public override void CollectObservations(VectorSensor sensor)
        {
            float worldHeight = 100f;
            float birdNormalizedY = (bird.transform.position.y + (worldHeight / 2f)) / worldHeight;
            sensor.AddObservation(birdNormalizedY);

            //float pipeSpawnXPosition = 100f;
            //Level.PipeComplete pipeComplete = level.GetNextPipeComplete();
            //if (pipeComplete != null && pipeComplete.pipeBottom != null && pipeComplete.pipeBottom.pipeBodyTransform != null)
            //{
            //    sensor.AddObservation(pipeComplete.pipeBottom.GetXPosition() / pipeSpawnXPosition);
            //}
            //else
            //{
                sensor.AddObservation(1f);
           // }

            sensor.AddObservation(bird.GetVelocity().y / 200f);
        }

        public override void OnActionReceived(ActionBuffers actions)
        {
            if (actions.DiscreteActions[0] == 1)
            {
                bird.Jump();
            }
        }

        private void OnTriggerEnter2D(Collider2D other)
        {
            if (other.TryGetComponent<PointTrigger>(out PointTrigger goal))
            {
                Debug.Log("Reward");
                SetReward(1f);
            }
            else
            {
                Debug.Log("No Reward :( ");
                SetReward(-1f);
                EndEpisode();
            }
        }

        public override void Heuristic(in ActionBuffers actionsOut)
        {
            ActionSegment<int> discreteActions = actionsOut.DiscreteActions;
            discreteActions[0] = isJumpInputDown ? 1 : 0;

            isJumpInputDown = false;
        }
    }
}