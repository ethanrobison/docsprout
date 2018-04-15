using System.Collections;
using Code.Doods.AI;
using UnityEngine;

namespace Code.Doods
{
    [RequireComponent(typeof(FlockBehaviour))]
    public class Dood : MonoBehaviour
    {

        public Root Behavior { get; private set; }
        Characters.Walk _walk;
        public Characters.Character Character;
        FlockBehaviour _flock;
        public float stopMovingPeriod = .15f;
        public float minForce = .5f;

        public void Initialize () {
            Behavior = GetComponent<BehaviorTree>().Root;
        }

        void Start () {
            _walk = GetComponent<Characters.Walk>();
            Character = GetComponent<Characters.Character>();
            _flock = GetComponent<FlockBehaviour>();
            Behavior = GetComponent<BehaviorTree>().Root;

        }

        Vector3 lastPos;
        public bool MoveTowards (Vector3 pos, float thresh = 10f, float minDist = 3f) {
            float dist = Vector3.Distance(pos, transform.position);
            float moveDelta = Vector3.Distance(pos, lastPos);
            lastPos = pos;
            if (dist < minDist) {
                _walk.SetDir(Vector2.zero);
                return false;
            }
            if (moveDelta < 0.001f) {
                if (dist < thresh) {
                    if (finishedMove) {
                        _walk.SetDir(Vector2.zero);
                        return false;
                    }
                    if (!finishedMove && !isTiming) {
                        StartCoroutine(StopTimer(dist));
                    }
                }
                else {
                    if (isTiming) {
                        StopCoroutine("stopTimer");
                        isTiming = false;
                    }
                    finishedMove = false;
                }
            }
            else {
                StopCoroutine("stopTimer");
            }
            var direction = (pos - transform.position).normalized;
            Vector3 force = _flock.CalculateForce();
            force = force * Time.deltaTime + direction;
            _walk.SetDir(force);
            return false;
        }

        public void StopMoving () {
            _walk.SetDir(Vector2.zero);
        }

        bool finishedMove;
        bool isTiming;

        IEnumerator StopTimer (float dist) {
            isTiming = true;
            yield return new WaitForSeconds(stopMovingPeriod * dist);
            isTiming = false;
            finishedMove = true;
        }


    }
}