namespace Krig.Model
{
    public class Cards : NotifyBase
    {
        private int x;

        public int X
        {
            get { return x; } 
            set { x = value; NotifyPropertyChanged("X");}
        }

        private int y;

        public int Y
        {
            get { return y; } 
            set { y = value; NotifyPropertyChanged("Y"); }
        }
        private bool isSelected;

        public bool IsSelected
        {
            get { return isSelected; } 
            set { isSelected = value; NotifyPropertyChanged("IsSelected");}
        }

        public Cards()
        {
            X = Y = 100;
        }
    }
}
