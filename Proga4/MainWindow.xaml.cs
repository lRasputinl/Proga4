using System;
using System.ComponentModel;
using System.Windows.Input;

namespace Proga4
{
    public class GameViewModel : INotifyPropertyChanged
    {
        private string _userInput;
        private string _message;
        private int _targetNumber;
        private int _attempts;

        public string UserInput
        {
            get => _userInput;
            set
            {
                _userInput = value;
                OnPropertyChanged(nameof(UserInput));
            }
        }

        public string Message
        {
            get => _message;
            set
            {
                _message = value;
                OnPropertyChanged(nameof(Message));
            }
        }

        public string AttemptsText => $"Количество попыток: {_attempts}";

        public ICommand CheckCommand { get; }

        public GameViewModel()
        {
            NewGame();
            CheckCommand = new RelayCommand(CheckGuess);
        }

        private void NewGame()
        {
            Random rand = new Random();
            _targetNumber = rand.Next(1, 101);
            _attempts = 0;
            UserInput = "";
            Message = "";
            OnPropertyChanged(nameof(AttemptsText));
        }

        private void CheckGuess()
        {
            if (!int.TryParse(UserInput, out int guess))
            {
                Message = "Введите корректное число!";
                return;
            }

            if (guess < 1 || guess > 100)
            {
                Message = "Число должно быть от 1 до 100.";
                return;
            }

            _attempts++;
            OnPropertyChanged(nameof(AttemptsText));

            if (guess < _targetNumber)
                Message = "Слишком маленькое.";
            else if (guess > _targetNumber)
                Message = "Слишком большое.";
            else
                Message = $"Поздравляем, вы угадали за {_attempts} попыток!";
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged(string propertyName) =>
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
    public class RelayCommand : ICommand
    {
        private readonly Action _execute;
        private readonly Func<bool> _canExecute;

        public RelayCommand(Action execute, Func<bool> canExecute = null)
        {
            _execute = execute ?? throw new ArgumentNullException(nameof(execute));
            _canExecute = canExecute;
        }

        public bool CanExecute(object parameter) => _canExecute?.Invoke() ?? true;
        public void Execute(object parameter) => _execute();

        public event EventHandler CanExecuteChanged
        {
            add { CommandManager.RequerySuggested += value; }
            remove { CommandManager.RequerySuggested -= value; }
        }
    }
}
