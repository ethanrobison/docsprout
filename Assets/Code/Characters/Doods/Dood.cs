using System.Collections;
using Code.Characters.Doods.AI;
using Code.Doods.AI;
using Code.Session;
using UnityEngine;

namespace Code.Characters.Doods
{
    [RequireComponent(typeof(FlockBehaviour))]
    public class Dood : MonoBehaviour, ISelectable
    {
        public Root Behavior { get; private set; }

        public Character Character;
        public float StopMovingPeriod = .15f;
        
        [HideInInspector] public bool IsSelected;
        private DoodColor _doodColor;

        private FlockBehaviour _flock;
        private Vector3 _lastPos;

        private void Start () {
            Character = GetComponent<Character>();
            _flock = GetComponent<FlockBehaviour>();
            Behavior = GetComponent<BehaviorTree>().Root;
            _doodColor = GetComponent<DoodColor>();
        }


        private bool _finishedMove;
        private bool _isTiming;

        public bool MoveTowards (Vector3 pos, float thresh = 10f, float minDist = 3f) {
            var dist = Vector3.Distance(pos, transform.position);
            var moveDelta = Vector3.Distance(pos, _lastPos);
            _lastPos = pos;
            if (dist < minDist) {
                StopMoving();
                return false;
            }

            if (moveDelta < 0.001f) {
                if (dist < thresh) {
                    if (_finishedMove) {
                        StopMoving();
                        return false;
                    }

                    if (!_finishedMove && !_isTiming) {
                        StartCoroutine(StopTimer(dist));
                    }
                }
                else {
                    if (_isTiming) {
                        StopCoroutine("StopTimer");
                        _isTiming = false;
                    }

                    _finishedMove = false;
                }
            }
            else {
                StopCoroutine("StopTimer");
            }

            var direction = (pos - transform.position).normalized;
            _flock.IsFlocking = true;
            _flock.AlignWeight = .4f;
            _flock.SetDir(direction);
            return false;
        }


        private IEnumerator StopTimer (float dist) {
            _isTiming = true;
            yield return new WaitForSeconds(StopMovingPeriod * dist);
            _isTiming = false;
            _finishedMove = true;
        }


        public void StopMoving () {
            _flock.IsFlocking = false;
            _flock.SetDir(Vector2.zero);
        }
        
        
        void ISelectable.OnSelect() {
            IsSelected = true;
            _doodColor.IsHighlighted = true;
        }
        
        void ISelectable.OnDeselect() {
            IsSelected = false;
            _doodColor.IsHighlighted = false;
        }
    }
}