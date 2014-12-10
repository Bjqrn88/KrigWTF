using System;
using Krig.View;

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
            set { y = value; NotifyPropertyChanged("Y");}
        }
        private bool isSelected;

        public bool IsSelected
        {
            get { return isSelected; } 
            set { isSelected = value; NotifyPropertyChanged("IsSelected");}
        }

        private String cardValue;

        public String CardValue
        {
            get { return cardValue; }
            set { cardValue = value; NotifyPropertyChanged("CardValue");}
        }

        private int warNumber;

        public int WarNumber
        {
            get { return warNumber; }
            set { warNumber = value; NotifyPropertyChanged("WarNumber");}
        }

        private bool isWar; 

        public bool IsWar
        {
            get { return isWar; }
            set { isWar = value; NotifyPropertyChanged("IsWar");}
        }

        public Cards()
        {
            X = 365;
            Y = 395;
        }
    }
}
