using System.Collections.Generic;

namespace Code.Utils
{
    public class SmartStack<T> where T : ISmartStackElement
    {
        private readonly Stack<T> _elements = new Stack<T>();

        public int Count () { return _elements.Count; }

        /// <summary>
        /// Pushes an element to the stack. Calls OnPush, then calls Activate.
        /// </summary>
        public void Push (T element) {
            if (_elements.Count != 0) { _elements.Peek().Deactivate(); }

            element.OnPush();
            element.Activate();
            _elements.Push(element);
        }

        /// <summary>
        /// Pops an element from the stack. Deactivates it first, then calls OnPop.
        /// </summary>
        public T Pop (bool suppress = false) {
            var result = _elements.Pop();
            result.Deactivate();
            result.OnPop();

            if (!suppress && _elements.Count != 0) { _elements.Peek().Activate(); }

            return result;
        }

        public T Peek () { return _elements.Peek(); }

        public void Clear () {
            while (_elements.Count != 0) { Pop(true); }
        }
    }

    public interface ISmartStackElement
    {
        // order: OnPush, Activate, Deactivate, OnPop

        void OnPush (); // when we get added
        void Activate (); // when we are newly the top element
        void Deactivate (); // when we are newly no longer the top element
        void OnPop (); // when we get popped off
    }
}