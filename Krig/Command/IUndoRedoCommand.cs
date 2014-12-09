namespace Krig.Command
{
    public interface IUndoRedoCommand
    {
        void Execute();
      
        void UnExecute();
    }
}
