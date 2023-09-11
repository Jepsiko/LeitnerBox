using CommunityToolkit.Mvvm.ComponentModel;
using System.Collections.ObjectModel;

namespace LeitnerBox.Model
{
    public partial class Drawer : ObservableObject
    {
        [ObservableProperty]
        ObservableCollection<Question> questions;

        [ObservableProperty]
        int size;

        [ObservableProperty]
        int id;

        [ObservableProperty]
        Color lightColor;

        [ObservableProperty]
        Color darkColor;

        TimeSpan dayOffset;

        public Drawer(int id, int dayOffset) {
            questions = new ObservableCollection<Question>();
            Size = 0;
            Id = id;
            this.dayOffset = new TimeSpan(dayOffset, 0, 0, 0);
            SetUntrainableColor();
        }

        public bool Trainable(DateTime subjectCreationDate)
        {
            return (DateTime.Today - subjectCreationDate - dayOffset).Days % Id == 0 && Size > 0;
        }

        public DateTime NextTraining(DateTime subjectCreationDate)
        {
            int offset = (DateTime.Today - subjectCreationDate - dayOffset).Days % Id;
            if (offset > 0) offset -= Id;
            return DateTime.Today - new TimeSpan(offset, 0, 0, 0);
        }

        public void CheckTraining(DateTime subjectCreationDate)
        {
            if (Trainable(subjectCreationDate)) SetTrainableColor();
            else SetUntrainableColor();
        }

        void SetTrainableColor()
        {
            LightColor = Colors.DarkTurquoise;
            DarkColor = Colors.DodgerBlue;
        }

        void SetUntrainableColor()
        {
            LightColor = Colors.White;
            DarkColor = Color.FromArgb("#404040");
        }

        public void Trained()
        {
            SetUntrainableColor();
        }

        public void AddQuestion(Question question)
        {
            Questions.Add(question);
            Size++;
        }

        public void RemoveQuestion(Question question)
        {
            Questions.Remove(question);
            Size--;
        }
    }
}
