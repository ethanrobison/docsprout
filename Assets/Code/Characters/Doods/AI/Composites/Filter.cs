namespace Code.Characters.Doods.AI
{
    /// <summary>
    /// Adds preconditions to an ordinary sequence.
    /// Note that these preconditions are checked every tick.
    /// </summary>
    //public class Filter : Sequence {
    //	readonly List<Func<Dood, bool>> _preconditions = new List<Func<Dood, bool>> ();

    //	public Filter (Dood dood) : base (dood) { }

    //	public void AddPrecondition (Func<Dood, bool> precondition)
    //	{
    //		_preconditions.Add (precondition);
    //	}

    //	protected override Status Update ()
    //	{
    //		// todo should this just get calculated once?
    //		for (int i = 0, c = _preconditions.Count; i < c; i++) {
    //			if (!_preconditions [i] (_dood)) { return Status.Failure; }
    //		}

    //		return base.Update ();
    //	}
    //}
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