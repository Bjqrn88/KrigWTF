using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Krig.Model;

namespace Krig.Command
{
    internal class StartGameCommand : IUndoRedoCommand
    {
        public void Execute()
        {
        }

        public void UnExecute()
        {
        }
    }

}