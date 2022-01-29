using Android;
using Android.Media;
using Android.Webkit;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Reflection;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;

namespace Fitime
{
    public class MainPageViewModel:INotifyPropertyChanged
    {
        public MainPageViewModel()
        {
            TimerCommand = new Command(async () =>
            {
                var ExercisePageVM = new ExerciseViewModel(Sets, SetDuration, ExercisesPerSet, BreakDuration);
                var Exercise = new Exercise();
                Exercise.BindingContext = ExercisePageVM;
                await Application.Current.MainPage.Navigation.PushAsync(Exercise);
            });
            ExercisesPerSet = "0";
            State = string.Empty;
            Sets = "0";
            SetDuration = "0";
            BreakDuration = "0";
        }

        #region Binding Properties
        private string _sets;
        public string Sets
        {
            get
            {
                return _sets;
            }
            set
            {
                if(_sets != value)
                {
                    _sets = value;
                    OnPropertyChanged(nameof(Sets));
                }
            }
        }
        private string _setDuration;
        public string SetDuration
        {
            get
            {
                return _setDuration;
            }
            set
            {
                if(_setDuration != value)
                {
                    _setDuration = value;
                    OnPropertyChanged(nameof(SetDuration));
                }
            }
        }

        private string _breakDuration;
        public string BreakDuration
        {
            get
            {
                return _breakDuration;
            }
            set
            {
                if (_breakDuration != value)
                {
                    _breakDuration = value;
                    OnPropertyChanged(nameof(BreakDuration));
                }
            }
        }

        private string _exercisesPerSet;
        public string ExercisesPerSet
        {
            get
            {
                return _exercisesPerSet;
            }
            set
            {
                if (_exercisesPerSet != value)
                {
                    _exercisesPerSet = value;
                    OnPropertyChanged(nameof(ExercisesPerSet));
                }
            }
        }

        private string _timer;
        public string Timer
        {
            get
            {
                return _timer;
            }
            set
            {
                if(_timer != value)
                {
                    _timer = value;
                    OnPropertyChanged(nameof(Timer));
                }
            }
        }

        private string _state;
        public string State
        {
            get
            {
                return _state;
            }
            set
            {
                if (_state != value)
                {
                    _state = value;
                    OnPropertyChanged(nameof(State));
                }
            }
        }
        #endregion
        #region ICommand
        public ICommand TimerCommand { get; set; }
        #endregion
        #region INotifyPropertyChanged
        public event PropertyChangedEventHandler PropertyChanged;

        protected void OnPropertyChanged(string propertyName)
        {
            var handler = PropertyChanged;
            if (handler != null)
                handler(this, new PropertyChangedEventArgs(propertyName));
        }
        #endregion 

        private void TimerExecute()
        {
            TimerExecuteAsync();
        }
        private async Task TimerExecuteAsync()
        {
            int setsCount = int.Parse(Sets);
            int setDuration = int.Parse(SetDuration);                                      
            int breakDuration = int.Parse(BreakDuration);
            int exercisesPerSet = int.Parse(ExercisesPerSet);
            var assenbly = typeof(App).GetTypeInfo().Assembly;
            System.IO.Stream audioStream = assenbly.GetManifestResourceStream("Fitime.censor-beep-01.mp3");
            var audio = Plugin.SimpleAudioPlayer.CrossSimpleAudioPlayer.Current;
            audio.Load(audioStream);
            for (int set = 0; set<setsCount; set++)
            {
                await Task.Run(async () =>
                {
                    for (int exercise = 0; exercise < exercisesPerSet; exercise++)
                    {
                        string exer = "Exercise " + Convert.ToString(exercise + 1);
                        //await Task.Run(  () =>
                        //{
                            Device.BeginInvokeOnMainThread(() => { Timer = "0"; State = exer + " In Progress"; });
                            for (int time = 0; time < setDuration; time++)
                            {
                                Thread.Sleep(1000);
                                Device.BeginInvokeOnMainThread(() =>
                                {
                                    Timer = Convert.ToString(time + 1);
                                    if (setDuration - time < 4)
                                    {
                                        audio.Play();
                                    }
                                });
                            }
                        //});
                        //await Task.Run(() =>
                        //{
                            Device.BeginInvokeOnMainThread(() => { Timer = "0"; State = exer + "Rest"; });
                            for (int time = 0; time < breakDuration; time++)
                            {
                                Thread.Sleep(1000);
                                Device.BeginInvokeOnMainThread(() =>
                                {
                                    Timer = Convert.ToString(time + 1);
                                    if (breakDuration - time < 4)
                                    {
                                        audio.Play();
                                    }
                                });
                            }
                        //});
                    }
                });
            }
            Timer = "0";
            State = "";
        }

    }
}
