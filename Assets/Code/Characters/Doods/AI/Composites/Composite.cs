using System.Collections.Generic;

namespace Code.Characters.Doods.AI
{
    public abstract class Composite : BehaviorTreeNode
    {
        protected readonly List<BehaviorTreeNode> Children = new List<BehaviorTreeNode>();

        protected Composite (Dood dood) : base(dood) { }

        public void AddToEnd (BehaviorTreeNode node) { Children.Add(node); }

        public void AddToFront (BehaviorTreeNode node) { Children.Insert(0, node); }
    }


    public class Selector : Composite
    {
        private int _current;

        public Selector (Dood dood) : base(dood) { }

        public override void OnInitialize () { _current = 0; }

        public override void OnTerminate (Status result) {
            if (_current < Children.Count) { Children[_current].OnTerminate(result); }

            base.OnTerminate(result);
        }

        protected override Status Update () {
            // todo we need to make sure to shut things down?
            while (true) {
                var status = Children[_current].Tick();
                if (status != Status.Failure) { return status; }

                if (++_current == Children.Count) { return Status.Failure; }
            }
        }
    }


    public class Sequence : Composite
    {
        private int _current;

        public Sequence (Dood dood) : base(dood) { }

        public override void OnInitialize () { _current = 0; }

        public override void OnTerminate (Status result) {
            if (_current < Children.Count) { Children[_current].OnTerminate(result); }

            base.OnTerminate(result);
        }

        protected override Status Update () {
            while (true) {
                var status = Children[_current].Tick();
                if (status != Status.Success) { return status; }

                if (++_current == Children.Count) { return Status.Success; }
            }
        }
    }
}