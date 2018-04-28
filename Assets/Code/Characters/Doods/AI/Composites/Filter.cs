namespace Code.Characters.Doods.AI
{
    public abstract class FilterSelector : Selector
    {
        protected FilterSelector (Dood dood) : base(dood) { }
        protected abstract bool Precondition ();

        protected override Status Update () { return !Precondition() ? Status.Failure : base.Update(); }
    }

    
    public abstract class FilterSequence : Sequence
    {
        protected FilterSequence (Dood dood) : base(dood) { }
        protected abstract bool Precondition ();

        protected override Status Update () { return !Precondition() ? Status.Failure : base.Update(); }
    }
}