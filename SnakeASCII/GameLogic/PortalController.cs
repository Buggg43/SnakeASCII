using SnakeASCII.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SnakeASCII.GameLogic
{
    public class PortalController
    {
        private PortalState _state = new PortalState();

        public void Activate(PortalState s)
        {
            _state = s ?? new PortalState();
            _state.IsActive = s != null && s.IsActive;
        }

        public void Tick(bool removedTail)
        {
            if (!_state.IsActive) return;
            if (removedTail)
            {
                _state.TailFlushRemaining = Math.Max(0, _state.TailFlushRemaining - 1);
                if (_state.TailFlushRemaining == 0)
                    _state.IsActive = false;
            }
        }

        public bool IsActive => _state.IsActive;
        public PortalState State => _state;
        public void Reset() => _state = new PortalState();
    }
}
