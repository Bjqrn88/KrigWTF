using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.CommandWpf;
using Krig.Command;
using Krig.Model;

namespace Krig.ViewModel
{

    public class MainViewModel : ViewModelBase
    {
        private UndoRedoController undoRedoController = UndoRedoController.GetInstance();

        private Point moveCardPoint;

        public ObservableCollection<Cards> cards{ get; set; }

        public ICommand UndoCommand { get; private set; }
        public ICommand RedoCommand { get; private set; }

        public ICommand DrawCardCommand { get; private set; }
        public ICommand RemoveCardCommand { get; private set; }
        public ICommand MouseDownCardCommand { get; private set; }
        public ICommand MouseMoveCardCommand { get; private set; }
        public ICommand MouseUpCardCommand { get; private set; }

        public MainViewModel()
        {
            cards = new ObservableCollection<Cards>()
            {
                new Cards() {IsSelected = false, X = 365, Y = 35},
                new Cards() {IsSelected = false, X = 365, Y = 395}
            };

            UndoCommand = new RelayCommand(undoRedoController.Undo, undoRedoController.CanUndo);
            RedoCommand = new RelayCommand(undoRedoController.Redo, undoRedoController.CanRedo);
            DrawCardCommand = new RelayCommand(DrawCard);
            RemoveCardCommand = new RelayCommand<IList>(RemoveCard);

            MouseDownCardCommand = new RelayCommand<MouseButtonEventArgs>(MouseDownCard);
            MouseMoveCardCommand = new RelayCommand<MouseEventArgs>(MouseMoveCard);
            MouseUpCardCommand = new RelayCommand<MouseButtonEventArgs>(MouseUpCard);

        }

        public void DrawCard()
        {
            undoRedoController.DrawAndExecute(new DrawCardCommand(cards, new Cards()));
        }

        public void MouseDownCard(MouseButtonEventArgs e)
        {
            e.MouseDevice.Target.CaptureMouse();
        }

        public void MouseMoveCard(MouseEventArgs e)
        {
            if (Mouse.Captured != null)
            {
                FrameworkElement cardVisualElement = (FrameworkElement)e.MouseDevice.Target;
                Cards cardsModel = (Cards) cardVisualElement.DataContext;
                Canvas canvas = FindParentOfType<Canvas>(cardVisualElement);
                Point mousePosition = Mouse.GetPosition(canvas);
                if (moveCardPoint == default(Point)) moveCardPoint = mousePosition;
                cardsModel.X = (int) mousePosition.X;
                cardsModel.Y = (int)mousePosition.Y;
            }
        }

        public void MouseUpCard(MouseButtonEventArgs e)
        {
            FrameworkElement cardVisualElement = (FrameworkElement)e.MouseDevice.Target;
            Cards cardsModel = (Cards)cardVisualElement.DataContext;
            Canvas canvas = FindParentOfType<Canvas>(cardVisualElement);
            Point mousePosition = Mouse.GetPosition(canvas);
            undoRedoController.DrawAndExecute(new MoveCardCommand(cardsModel, (int)moveCardPoint.X, (int)moveCardPoint.Y, (int)mousePosition.X, (int)mousePosition.Y));
            moveCardPoint = new Point();
            e.MouseDevice.Target.ReleaseMouseCapture();
        }

        public void RemoveCard(IList _cards)
        {
            undoRedoController.DrawAndExecute(new RemoveCardCommand(cards, _cards.Cast<Cards>().ToList()));
        }

        private static T FindParentOfType<T>(DependencyObject o)
        {
            dynamic parent = VisualTreeHelper.GetParent(o);
            return parent.GetType().IsAssignableFrom(typeof(T)) ? parent : FindParentOfType<T>(parent);
        }
       
    }
}