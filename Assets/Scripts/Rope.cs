using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rope : MonoBehaviour
{
    private LineRenderer lr;
    private List<RopeSegment> ropeSegments = new List<RopeSegment>();
    private float segmentLength = 1f;

    public int segmentCount = 6;

    // Start is called before the first frame update
    void Start()
    {
        lr = GetComponent<LineRenderer>();

        Vector3 segmentOrigin = transform.position;

        for (int i = 0; i < segmentCount; i++)
        {
            ropeSegments.Add(new RopeSegment(segmentOrigin));
            segmentOrigin.y -= segmentLength;
        }

        DrawRope();
    }

    // Update is called once per frame
    void Update()
    {
        DrawRope();
    }

    private void FixedUpdate()
    {
        this.Simulate();
    }

    private void Simulate() {

        Vector2 gravity = new Vector2(0, -50f);

        for (int i = 0; i < segmentCount; i++)
        {
            RopeSegment firstSegment = ropeSegments[i];
            Vector2 velocity = firstSegment.currentPos = firstSegment.oldPos;
            firstSegment.oldPos = firstSegment.currentPos;
            firstSegment.currentPos += velocity;
            firstSegment.currentPos += gravity * Time.deltaTime;
            ropeSegments[i] = firstSegment;
        }//CONSTRAINTS
        for (int i = 0; i < 50; i++)
        {
            this.ApplyConstraint();
        }
    }

    private void ApplyConstraint()
    {
        for (int i = 0; i < this.segmentCount - 1; i++)
        {
            RopeSegment firstSeg = this.ropeSegments[i];
            RopeSegment secondSeg = this.ropeSegments[i + 1];

            float dist = (firstSeg.currentPos - secondSeg.currentPos).magnitude;
            float error = Mathf.Abs(dist - this.segmentLength);
            Vector2 changeDir = Vector2.zero;

            if (dist > segmentLength)
            {
                changeDir = (firstSeg.currentPos - secondSeg.currentPos).normalized;
            } else if (dist < segmentLength)
            {
                changeDir = (secondSeg.currentPos - firstSeg.currentPos).normalized;
            }

            Vector2 changeAmount = changeDir * error;
            if (i != 0)
            {
                firstSeg.currentPos -= changeAmount * 0.5f;
                this.ropeSegments[i] = firstSeg;
                secondSeg.currentPos += changeAmount * 0.5f;
                this.ropeSegments[i + 1] = secondSeg;
            }
            else
            {
                secondSeg.currentPos += changeAmount;
                this.ropeSegments[i + 1] = secondSeg;
            }
        }
    }

    private void DrawRope() {
        lr.startWidth = 0.1f;
        lr.endWidth = 0.1f;

        Vector3[] segmentPositions = new Vector3[segmentCount];
        for (int i = 0; i < segmentCount; i++)
        {
            segmentPositions[i] = ropeSegments[i].currentPos;
        }

        lr.positionCount = segmentPositions.Length;
        lr.SetPositions(segmentPositions);
    }

    public struct RopeSegment 
    {
        public Vector2 currentPos;
        public Vector2 oldPos;

        public RopeSegment(Vector2 pos) {
            currentPos = pos;
            oldPos = pos;
        }
    }
}
