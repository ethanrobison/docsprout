using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Code.Characters.Doods.AI;
using Code.Characters.Doods.Needs;
using UnityEngine;

namespace Code.Characters.Doods
{
    [RequireComponent(typeof(FlockBehaviour))]
    public class Dood : MonoBehaviour, ISelectable
    {
        public float StopMovingPeriod = .15f;
        [HideInInspector] public bool IsSelected;
        public DoodComponents Comps { get; private set; }

        private Vector3 _lastPos;

        private void Start () { Comps = new DoodComponents(this); }


        private bool _finishedMove, _isTiming;

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
            Comps.Flock.IsFlocking = true;
            Comps.Flock.AlignWeight = .4f;
            Comps.Flock.SetDir(direction);
            return false;
        }


        private IEnumerator StopTimer (float dist) {
            _isTiming = true;
            yield return new WaitForSeconds(StopMovingPeriod * dist);
            _isTiming = false;
            _finishedMove = true;
        }


        public void StopMoving () {
            Comps.Flock.IsFlocking = false;
            Comps.Flock.SetDir(Vector2.zero);
        }


        //
        // Selectable interface

        void ISelectable.OnSelect () {
            IsSelected = true;
            Comps.Color.IsHighlighted = true;
        }

        void ISelectable.OnDeselect () {
            IsSelected = false;
            Comps.Color.IsHighlighted = false;
        }
    }

    public class DoodComponents
    {
        public Root Behavior { get; private set; }
        public Character Character { get; private set; }
        public DoodColor Color { get; private set; }
        public List<Need> Needs { get; private set; }
        public FlockBehaviour Flock { get; private set; }


        public DoodComponents (Dood dood) {
            var go = dood.gameObject;
            Character = go.GetComponent<Character>();
            Behavior = go.GetComponentInChildren<BehaviorTree>().Root;
            Color = go.GetComponentInChildren<DoodColor>();
            Needs = go.GetComponents<Need>().ToList();
            Flock = go.GetComponent<FlockBehaviour>();
        }
    }
}