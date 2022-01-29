using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Reflection;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace Fitime
{
    public class ExerciseViewModel : INotifyPropertyChanged
    {
        public ExerciseViewModel(string sets, string setDuration, string exercisesPerSet, string breakDuration)
        {
            Sets = int.Parse(sets);
            SetDuration = int.Parse(setDuration);
            ExercisesPerSet = int.Parse(exercisesPerSet);
            BreakDuration = int.Parse(breakDuration);
        }
        #region Binding Properties
        private string _setNumber;
        public string SetNumber
        {
            get
            {
                return _setNumber;
            }
            set
            {
                if(_setNumber != value)
                {
                    _setNumber = value;
                    OnPropertyChanged(nameof(SetNumber));
                }
            }
        }

        private string _exerciseState;
        public string ExerciseState
        {
            get
            {
                return _exerciseState;
            }
            set
            {
                if (_exerciseState != value)
                {
                    _exerciseState = value;
                    OnPropertyChanged(nameof(ExerciseState));
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
                if (_timer != value)
                {
                    _timer = value;
                    OnPropertyChanged(nameof(Timer));
                }
            }
        }
        private Color _color;
        public Color Color
        {
            get
            {
                return _color;
            }
            set
            {
                if(_color != value)
                {
                    _color = value;
                    OnPropertyChanged(nameof(Color));
                }
            }
        }
        #endregion

        #region Properties
        public int Sets { get; set; }
        public int SetDuration { get; set; }
        public int ExercisesPerSet { get; set; }
        public int BreakDuration { get; set; }
        private INavigation navigation { get; set; }
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

        #region Private Methods
        public async Task TimerExecuteAsync(INavigation navigation)
        {
            var assenbly = typeof(App).GetTypeInfo().Assembly;
            System.IO.Stream audioStream = assenbly.GetManifestResourceStream("Fitime.censor-beep-01.mp3");
            var audio = Plugin.SimpleAudioPlayer.CrossSimpleAudioPlayer.Current;
            audio.Load(audioStream);
            SetDuration += 2;
            BreakDuration += 2;
            await Task.Run(async () =>
            {
                for (int set = 0; set < Sets; set++)
                {
                    Device.BeginInvokeOnMainThread(() => { SetNumber = "Set : " + Convert.ToString(set + 1); });

                    for (int exercise = 0; exercise < ExercisesPerSet; exercise++)
                    {
                        string exer = "Exercise " + Convert.ToString(exercise + 1);
                        Device.BeginInvokeOnMainThread(() => { ExerciseState = exer; Color = Color.LightGreen; });
                        for (int time = 1; time < SetDuration; time++)
                        { 

                            Thread.Sleep(1000);
                            Device.BeginInvokeOnMainThread(() =>
                            {
                                Timer = Convert.ToString(SetDuration - time);
                                if (SetDuration - time < 4)
                                {
                                    audio.Play();
                                }
                            });
                        }

                        Device.BeginInvokeOnMainThread(() => { ExerciseState = exer; Color = Color.Red; });
                        for (int time = 1; time < BreakDuration; time++)
                        {
                            Thread.Sleep(1000);
                            Device.BeginInvokeOnMainThread(() =>
                            {
                                Timer = Convert.ToString(BreakDuration - time);
                                if (BreakDuration - time < 4)
                                {
                                    audio.Play();
                                }
                            });
                        }
                    }
                    Device.BeginInvokeOnMainThread(async () => { await navigation.PopAsync(); });
                }
                Device.BeginInvokeOnMainThread(() => { Timer = ""; ExerciseState = ""; SetNumber = ""; Color = Color.Silver; });
            });

        }
        #endregion

    }
}
