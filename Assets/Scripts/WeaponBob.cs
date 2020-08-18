using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class WeaponBob : MonoBehaviour
{
    public GameObject player;
    PlayerMovement playerMovement;

    Vector3 initialPosition;
    public Vector3 velocityDeltas;
    public Vector3 verticalDeltas;

    MovingAverage horizontalAvg = new MovingAverage(20);
    MovingAverage verticalAvg = new MovingAverage(6);


    void Start()
    {
        playerMovement = player.GetComponent<PlayerMovement>();
        initialPosition = transform.localPosition;
        Debug.Log(player.transform.localPosition);
        Debug.Log(player.transform.position);
    }

    // Update is called once per frame
    void Update()
    {
        float horizontalVelocityRatio = horizontalAvg.update(playerMovement.horizontalVel.sqrMagnitude / (playerMovement.MAX_VEL * playerMovement.MAX_VEL));
        float rawVertVelRatio = playerMovement.isGround ? 0 : Mathf.Clamp(playerMovement.normalVel.y / 10f, -1f, 1f);
        float verticalVelocityRatio = verticalAvg.update(Mathf.Sqrt(Mathf.Abs(rawVertVelRatio)) * Mathf.Sign(rawVertVelRatio));
        transform.localPosition = initialPosition + horizontalVelocityRatio * velocityDeltas + verticalVelocityRatio * verticalDeltas;
    }

    class MovingAverage {
        Queue<float> q = new Queue<float>();
        float sum = 0f;
        int windowSize;

        public MovingAverage(int windowSize) {
            this.windowSize = windowSize;
        }

        public float update(float value) {
            if(q.Count == windowSize) {
                sum -= q.Peek();
                q.Dequeue();
            }

            sum += value;
            q.Enqueue(value);
            return sum / windowSize;
        }
    }
}
