using CommunityToolkit.Mvvm.ComponentModel;

namespace LeitnerBox.Model
{
    public partial class Subject : ObservableObject
    {
        public const int NUM_DRAWERS = 7;

        [ObservableProperty]
        string name;

        [ObservableProperty]
        Drawer[] drawers;

        [ObservableProperty]
        bool trainedToday;

        [ObservableProperty]
        DateTime creationDate;

        public Subject(string name, bool trained=false) 
        {
            Name = name;
            TrainedToday = trained;
            CreationDate = DateTime.Today;

            Drawers = new Drawer[NUM_DRAWERS];
            int[] dayOffsets = new int[] { 0, 0, 2, 1, 4, 2, 6 };
            for (int i = 0; i < NUM_DRAWERS; i++)
            {
                Drawers[i] = new Drawer(i+1, dayOffsets[i]);
            }
        }

        public bool Trainable()
        {
            for (int i = 0; i < NUM_DRAWERS; i++)
            {
                if (Drawers[i].Trainable(CreationDate)) return true;
            }
            return false;
        }

        public DateTime NextTrainingDate()
        {
            var minNextTraining = DateTime.MaxValue;
            for (int i = 0; i < NUM_DRAWERS; i++)
            {
                var nextTraining = Drawers[i].NextTraining(CreationDate);
                if (nextTraining < minNextTraining) minNextTraining = nextTraining;
            }
            return minNextTraining;
        }

        public void AddQuestion(Question question)
        {
            Drawers[0].AddQuestion(question);
            if (!TrainedToday) Drawers[0].CheckTraining(CreationDate);
        }

        public bool HasQuestions()
        {
            for (int i = 0; i < NUM_DRAWERS; i++)
            {
                if (Drawers[i].Size > 0) return true;
            }
            return false;
        }

        private string Serialize()
        {
            string serial = CreationDate + "\n";
            serial += DateTime.Today + "\n"; // save date
            serial += TrainedToday + "\n";
            serial += Name + "\n";
            foreach (var drawer in Drawers)
            {
                serial += drawer.Id + " " + drawer.Size + "\n";
                foreach (var question in drawer.Questions)
                {
                    serial += "[QUESTION]\n" + question.Q + "\n[ANSWER]\n" + question.A + "\n[END_QUESTION]\n";
                }
            }
            return serial;
        }

        private static Subject Deserialize(string serial)
        {
            var lines = serial.Split('\n');
            int i = 0;

            var creationDate = DateTime.Parse(lines[i++]);
            var saveDate = DateTime.Parse(lines[i++]);
            var trained = bool.Parse(lines[i++]);
            var trainedToday = saveDate == DateTime.Today && trained;
            var s = new Subject(lines[i++], trainedToday);
            s.CreationDate = creationDate;

            foreach (var drawer in s.Drawers)
            {
                var drawerInfo = lines[i++].Split(' ');
                if (drawerInfo[0] != drawer.Id.ToString()) throw new Exception();
                int num_questions = int.Parse(drawerInfo[1]);

                for (int j = 0; j < num_questions; j++)
                {
                    string q = "";
                    string a = "";

                    if (lines[i++] != "[QUESTION]") throw new Exception();
                    while (lines[i] != "[ANSWER]") q += lines[i++] + '\n';

                    if (lines[i++] != "[ANSWER]") throw new Exception();
                    while (lines[i] != "[END_QUESTION]") a += lines[i++] + '\n';

                    drawer.AddQuestion(new Question(q, a));

                    i++;
                }
                if (!trainedToday) drawer.CheckTraining(creationDate);
            }
            return s;
        }

        public void Save()
        {
            var path = FileSystem.Current.AppDataDirectory;
            var fullPath = Path.Combine(path, Name + ".txt");

            File.WriteAllText(fullPath, Serialize());
        }

        public static Subject Load(string fullPath)
        {
            string serial = File.ReadAllText(fullPath);
            return Deserialize(serial);
        }

        public void DeleteSave()
        {
            var path = FileSystem.Current.AppDataDirectory;
            var fullPath = Path.Combine(path, Name + ".txt");

            File.Delete(fullPath);
        }
    }
}
