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
        public bool IsGameOver { get; set; }

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
            IsGameOver = false;
            Notify();
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(UserInput)));
        }

        public void Check()
        {
            if (IsGameOver) return;

            int guess;
            if (!int.TryParse(UserInput, out guess)) { Message = "Ошибка!"; Notify(); return; }

            Attempts++;
            if (guess == Target)
            {
                string attemptWord = (Attempts % 10 == 1 && Attempts % 100 != 11) ? "попытку" : (Attempts % 10 >= 2 && Attempts % 10 <= 4 && (Attempts % 100 < 10 || Attempts % 100 >= 20)) ? "попытки" : "попыток";
                Message = "Вы угадали! Начните игру заново.";
                PlaySound();
                System.Windows.MessageBox.Show($"Поздравляем! Вы угадали число {Target} за {Attempts} {attemptWord}.", "Победа!", System.Windows.MessageBoxButton.OK, System.Windows.MessageBoxImage.Information);
                IsGameOver = true;
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
