using System;
using System.ComponentModel;
using System.Windows.Input;
using System.Media;

namespace Proga4
{
    public class GameViewModel : INotifyPropertyChanged
    {
        public int Target, Attempts;
        public string AttemptsText => $"Попытки: {Attempts}";
        public string UserInput { get; set; }
        public string Message { get; set; }
        public ICommand CheckCommand { get; }
        public ICommand NewGameCommand { get; }

        public GameViewModel()
        {
            CheckCommand = new RelayCommand(Check);
            NewGameCommand = new RelayCommand(Start);
            Start();
        }

        private void PlaySound()
        {
            SoundPlayer player = new SoundPlayer("C:/Users/vladz/звук.wav"); 
            player.Play();
        }

        public void Start()
        {
            Target = new Random().Next(1, 101);
            Attempts = 0;
            UserInput = "";
            Message = "Попробуйте угадать число!";
            Notify();
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(UserInput)));
        }

        public void Check()
        {
            int guess;
            if (!int.TryParse(UserInput, out guess)) { Message = "Ошибка!"; Notify(); return; }

            Attempts++;
            if (guess == Target)
            {
                Message = "Угадали!";
                PlaySound();
            }
            else
                Message = guess < Target ? "Слишком маленькое!" : "Слишком большое!";

            Notify();
        }

        public event PropertyChangedEventHandler PropertyChanged;
        void Notify()
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Message)));
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(AttemptsText)));
        }
    }

    public class RelayCommand : ICommand
    {
        Action _run;
        public RelayCommand(Action run) { _run = run; }
        public bool CanExecute(object p) => true;
        public void Execute(object p) => _run();
        public event EventHandler CanExecuteChanged { add { } remove { } }
    }
}
