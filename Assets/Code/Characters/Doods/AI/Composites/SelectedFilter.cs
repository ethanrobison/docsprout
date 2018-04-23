namespace Code.Characters.Doods.AI
{
    public class SelectedFilter : FilterSelector
    {
        public SelectedFilter (Dood dood) : base(dood) { }
        protected override bool Precondition () { return Dood.IsSelected; }
    }
}