using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.CommandWpf;
using Krig.Command;
using Krig.Model;
using Krig.Game;
using Krig.View;

namespace Krig.ViewModel
{

    public class MainViewModel : ViewModelBase
    {
        private String loadPath = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);

        private UndoRedoController undoRedoController = UndoRedoController.GetInstance();
        private GamePlay gameplay = new GamePlay();
        private static SaveGameData saveGameData = new SaveGameData();
        private SaveLoad SL = new SaveLoad(saveGameData);
        //private Board b = new Board();
        private Card card,cpuCard;
        private List<Card> warCards = new List<Card>(), warCPUCards = new List<Card>();

        //private int backColor = 0;
        //private SolidColorBrush SCB = new SolidColorBrush();

        private bool ItsOn = false;

        private Point moveCardPoint;

        public ObservableCollection<Cards> cards{ get; set; }

        public List<Cards> CardToRemoveList = new List<Cards>();

        public ICommand UndoCommand { get; private set; }
        public ICommand RedoCommand { get; private set; }

        public ICommand DrawCardCommand { get; private set; }
        public ICommand RemoveCardCommand { get; private set; }
        public ICommand MouseDownCardCommand { get; private set; }
        public ICommand MouseMoveCardCommand { get; private set; }
        public ICommand MouseUpCardCommand { get; private set; }
        public ICommand StartGameCommand { get; private set; }
        public ICommand ChooseForWarCommand { get; private set; }
        //public ICommand ChangeBackgroundCommand { get; private set; }
        public ICommand LoadGameCommand { get; private set; }
        public ICommand SaveGameCommand { get; private set; }

        public MainViewModel()
        {
            cards = new ObservableCollection<Cards>()
            {
                //new Cards() {IsSelected = false, X = 365, Y = 35, CardValue = "1"},
                //new Cards() {IsSelected = false, X = 365, Y = 395, CardValue = "2"}
            };

            UndoCommand = new RelayCommand(undoRedoController.Undo, undoRedoController.CanUndo);
            RedoCommand = new RelayCommand(undoRedoController.Redo, undoRedoController.CanRedo);
            DrawCardCommand = new RelayCommand(DrawCard);
            RemoveCardCommand = new RelayCommand(RemoveCard);
            ChooseForWarCommand = new RelayCommand<MouseButtonEventArgs>(ChooseForWar);
            MouseDownCardCommand = new RelayCommand<MouseButtonEventArgs>(MouseDownCard);
            MouseMoveCardCommand = new RelayCommand<MouseEventArgs>(MouseMoveCard);
            MouseUpCardCommand = new RelayCommand<MouseButtonEventArgs>(MouseUpCard);

            StartGameCommand = new RelayCommand(StartGame);

            //ChangeBackgroundCommand = new RelayCommand(ChangeBackground);

            LoadGameCommand = new RelayCommand(LoadGame);
            SaveGameCommand = new RelayCommand(SaveGame);



        }

        private void SaveGame()
        {
            gameplay.prepareSave(saveGameData);
            SL.saveGame();
        }

        public void LoadGame()
        {
            Microsoft.Win32.OpenFileDialog dlg = new Microsoft.Win32.OpenFileDialog();
            dlg.InitialDirectory = loadPath;
            Nullable<bool> result = dlg.ShowDialog();

            if (result == true)
            {
                SL.loadFromXML(dlg.FileName);
            }
        }

      /*  public void ChangeBackground()
        {
            switch (backColor)
            {
                case 0:
                    b.BoardColor.Fill = new SolidColorBrush(Color.FromRgb(123,21,24));
                    //SCB.Color = Color.FromRgb(123,21,24);
                    break;
                case 1:
                    SCB.Color = Color.FromRgb(223,121,44);
                    break;
                case 2:
                    SCB.Color = Color.FromRgb(123,221,44);
                    break;
                case 3:
                    SCB.Color = Color.FromRgb(123,121,224);
                    break;
                case 4:
                    SCB.Color = Color.FromRgb(23,121,224);
                    break;  
                case 5:
                    SCB.Color = Color.FromRgb(123,21,224);
                    break;
                default:
                    SCB.Color = Color.FromRgb(0,100,0);
                    backColor = 0;
                    break;
            }
            //b.BoardColor.Fill =  new SolidColorBrush();
        }*/

        public void StartGame()
        {
            ItsOn = true;
            gameplay.newGame();
        }

        public void ChooseForWar(MouseButtonEventArgs e)
        {
            FrameworkElement cardVisualElement = (FrameworkElement)e.MouseDevice.Target;
            Cards cardsModel = (Cards)cardVisualElement.DataContext;

            if (cardsModel.IsWar)
            {
                if (gameplay.war(warCards, warCPUCards, cardsModel.WarNumber) == 0)
                    DrawWar();
            }
        }

        public void DrawCard()
        {
            if(CardToRemoveList.Count > 0)
                RemoveCard();
            if (ItsOn)
            {
                card = gameplay.drawACard();
                cpuCard = gameplay.getAICard();
                var addCardPlayer = new Cards() {CardValue = card.Value.ToString(), IsWar = false, IsSelected = false};
                var addCardCPU = new Cards()
                    {
                        CardValue = cpuCard.Value.ToString(),
                        X = 365,
                        Y = 35,
                        IsWar = false,
                        IsSelected = false
                    };
                undoRedoController.DrawAndExecute(new DrawCardCommand(cards,addCardPlayer));
                undoRedoController.DrawAndExecute(new DrawCardCommand(cards,addCardCPU));
                int result = gameplay.playRound(card, cpuCard);

                CardToRemoveList.Add(addCardCPU);
                CardToRemoveList.Add(addCardPlayer);

                if (result == 0)
                {
                    DrawWar();
                }
            }
        }

        public void DrawWar()
        {
            
            int x = 330;
            Cards warCPU;
            Cards warPlayer;
            gameplay.checkWarConditions();
            warCards = gameplay.getWarCards();

            for (int i = 0; i < warCards.Count; i++)
            {
                warPlayer = new Cards()
                {
                    CardValue = warCards[i].Value.ToString(),
                    X = x,
                    Y = 205,
                    IsWar = true,
                    WarNumber = i + 1,
                    IsSelected = false
                };
                undoRedoController.DrawAndExecute(new DrawCardCommand(cards, warPlayer));
                x = x - 45;
                CardToRemoveList.Add(warPlayer);
            }

            warCPUCards = gameplay.getAIWarCards();
            x = 400;
            for (int i = 0; i < warCards.Count; i++)
            {
                warCPU = new Cards()
                {
                    CardValue = warCPUCards[i].Value.ToString(),
                    X = x,
                    Y = 265,
                    IsWar = true,
                    IsSelected = false
                };
                undoRedoController.DrawAndExecute(new DrawCardCommand(cards,warCPU));
                x = x + 45;
                CardToRemoveList.Add(warCPU);
            }

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
                Cards cardsModel = (Cards)cardVisualElement.DataContext;
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

        public void RemoveCard()
        {
            undoRedoController.DrawAndExecute(new RemoveCardCommand(cards, CardToRemoveList));
        }

        private static T FindParentOfType<T>(DependencyObject o) where T : class
        {
            DependencyObject parent = VisualTreeHelper.GetParent(o);
            if (parent == null) return null;
            return parent is T ? parent as T : FindParentOfType<T>(parent);
        }
       
    }
}