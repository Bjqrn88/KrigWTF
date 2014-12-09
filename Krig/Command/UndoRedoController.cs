using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Krig.Command
{
    public class UndoRedoController

    {
        private static UndoRedoController controller = new UndoRedoController();

        private readonly Stack<IUndoRedoCommand> undoStack = new Stack<IUndoRedoCommand>();
        private readonly Stack<IUndoRedoCommand> redoStack = new Stack<IUndoRedoCommand>();

        private UndoRedoController() { }

        public static UndoRedoController GetInstance() { return controller; }

        public void DrawAndExecute(IUndoRedoCommand command)
        {
            undoStack.Push(command);
            redoStack.Clear();
            command.Execute();
        }

        public bool CanUndo()
        {
            return undoStack.Any();
        }

        public void Undo()
        {
            if (!undoStack.Any()) throw new InvalidOperationException();
            IUndoRedoCommand command = undoStack.Pop();
            redoStack.Push(command);
            command.UnExecute();
        }

        public bool CanRedo()
        {
            return redoStack.Any();
        }

        public void Redo()
        {
            if (!redoStack.Any()) throw new InvalidOperationException();
            IUndoRedoCommand command = redoStack.Pop();
            undoStack.Push(command);
            command.Execute();
        }
    }
}
