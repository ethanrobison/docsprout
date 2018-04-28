namespace Code.Characters.Doods.AI
{
    public enum Status
    {
        Invalid,
        Running,
        Success,
        Failure
    }


    public abstract class BehaviorTreeNode
    {
        private Status _status;

        protected readonly Dood Dood;

        protected Root Root {
            get { return Dood.Comps.Behavior; }
        }

        protected BehaviorTreeNode (Dood dood) { Dood = dood; }

        public virtual void OnInitialize () { }

        public virtual void OnTerminate (Status result) { }

        // force the node to wrap up (mostly makes sense for leaves)
        public virtual void Abort () {
            if (_status != Status.Invalid) OnTerminate(Status.Failure);
        }

        protected abstract Status Update ();

        public Status Tick () {
            if (_status != Status.Running) OnInitialize();
            _status = Update();
            if (_status != Status.Running) OnTerminate(_status);
            return _status;
        }
    }


    public class Root : BehaviorTreeNode
    {
        private BehaviorTreeNode _child;

        public Root (Dood dood) : base(dood) { }

        public void SetChild (BehaviorTreeNode child) { _child = child; }

        protected override Status Update () { return _child.Tick(); }
    }
}