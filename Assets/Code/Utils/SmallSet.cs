namespace Code.Utils
{
    /// <summary>
    /// Holds membership for up to 16 things that are convertible into integers.
    /// </summary>
    public class SmallSet
    {
        private ushort _set;

        public SmallSet () { _set = 0; }

        public bool Empty {
            get { return _set == 0; }
        }

        public void Add (ushort thing) {
            Logging.Assert(thing < 16, "Index out of range: " + thing);
            _set = (ushort) (_set | (1 << thing));
        }

        public void Remove (ushort thing) {
            Logging.Assert(thing < 16, "Index out of range: " + thing);
            _set = (ushort) (_set & ~(1 << thing));
        }

        public bool Contains (ushort thing) {
            Logging.Assert(thing < 16, "Index out of range: " + thing);
            return (_set & (1 << thing)) != 0;
        }
    }
}