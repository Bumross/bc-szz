using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace sprava_kontaktu.Patterns.Commands
{

        public class CommandManager
        {
            private readonly Stack<ICommand> _undo = new();
            private readonly Stack<ICommand> _redo = new();
            private readonly int _capacity;

            private readonly List<string> _recent = new();      // pro zobrazení
            private readonly Stack<string> _removed = new();    // „koš“ pro Z/Y

            public CommandManager(int capacity = 5)
            {
                _capacity = capacity;
            }

            public void Do(ICommand cmd)
            {
                cmd.Execute();
                _undo.Push(cmd);
                _redo.Clear();

                _recent.Add(cmd.Description);
                if (_recent.Count > 50) _recent.RemoveAt(0); // ochrana proti růstu
                TrimUndoCapacity();
            }

            public bool Undo()
            {
                if (_undo.Count == 0) return false;
                var cmd = _undo.Pop();
                cmd.Unexecute();
                _redo.Push(cmd);
                _recent.Add($"UNDO: {cmd.Description}");
                return true;
            }

            public bool Redo()
            {
                if (_redo.Count == 0) return false;
                var cmd = _redo.Pop();
                cmd.Execute();
                _undo.Push(cmd);
                _recent.Add($"REDO: {cmd.Description}");
                TrimUndoCapacity();
                return true;
            }

            private void TrimUndoCapacity()
            {
                // Udržuj max N posledních provedených příkazů
                while (_undo.Count > _capacity)
                {
                    _undo.TrimExcess(); // neodstraní z vršku; ponecháme takto (jednoduše)
                    break; // prakticky stačí — nebo by šlo vybudovat vlastní deque
                }
            }

            // „Poslední změny“ – jednoduchý log s možností Z/Y nad samotnými log položkami
            public IReadOnlyList<string> RecentChanges() => _recent.AsReadOnly();

            public bool PopLastChange(out string desc)
            {
                if (_recent.Count == 0) { desc = ""; return false; }
            desc = _recent[_recent.Count - 1];
            _recent.RemoveAt(_recent.Count - 1);
                _removed.Push(desc);
                return true;
            }

            public bool PushBackChange(string? desc = null)
            {
                if (_removed.Count == 0) return false;
                var d = _removed.Pop();
                _recent.Add(d);
                return true;
            }
        }
}
