using System.Media;
using NAudio.Wave;

namespace FrogGameWinFormsApp
{
    public partial class MainForm : Form
    {
        private WinGameForm form2 = new WinGameForm();
        private List<PictureBox> allPictureBoxes;
        private List<PictureBox> leftFrogs;
        private List<PictureBox> rightFrogs;
        private int maximumSteps = 24;

        private SoundPlayer startGameSoundPlayer;
        private WaveStream frogVoice;
        private WaveOut outFrogVoice;
        public MainForm()
        {
            InitializeComponent();
        }
        private void MainForm_Load(object sender, EventArgs e)
        {
            AddPictureBoxesToLists();

            startGameSoundPlayer = new SoundPlayer();
            startGameSoundPlayer.SoundLocation = @"..\..\..\Resources\startGame.wav"; // "D:\Project\OnlineCourse12Stream_Kurators\FrogGame\FrogGameWinFormsApp\Resources"
            startGameSoundPlayer.PlayLooping();

        }
        private void restartToolStripMenuItem_Click(object sender, EventArgs e)
        {
            RestartGame();
        }
        private void showRulesToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            MessageBox.Show("Цель игры - расположить лягушек, которые смотрят влево, в левую часть, " +
                "а остальных - в правую часть за минимальное количество перепрыгиваний.\r\n\r\n" +
                "Прыгать можно на листок, если он находится рядом или через 1 лягушку\r\n\r\n");
        }

        private void pictureBox_Click(object sender, EventArgs e)
        {
            ChangeFrogPosition((PictureBox)sender);
        }

        void ChangeFrogPosition(PictureBox frog)
        {

            var maxDistance = Math.Abs(frog.Location.X - emptyFlowerpictureBox.Location.X) / frog.Width;

            if (maxDistance > 2)
            {
                MessageBox.Show("Недопустимый ход");
            }
            else
            {
                PlayFrogVoice();

                var newLocation = frog.Location;
                frog.Location = emptyFlowerpictureBox.Location;
                emptyFlowerpictureBox.Location = newLocation;
                stepsLabel.Text = (Convert.ToInt32(stepsLabel.Text) + 1).ToString();
            }

            EndGame(isEnded());

        }

        /// <summary>
        /// Проверяем положение левых и правых лягушек относительно кувшинки
        /// Если в обоих случаях true, то функция вернет true, иначе false
        /// </summary>
        private bool isEnded()
        {
            bool leftSideFrogsInEndGame = leftFrogs.All(x => x.Location.X > emptyFlowerpictureBox.Location.X);
            bool rightSideFrogsInEndGame = rightFrogs.All(x => x.Location.X < emptyFlowerpictureBox.Location.X);

            return (leftSideFrogsInEndGame && rightSideFrogsInEndGame);
        }

        /// <summary>
        /// Проверяем полученное булевое значение
        /// Если True, то заканчиваем игру и считаем количество ходов
        /// </summary>
        private void EndGame(bool boolValue)
        {
            if (boolValue)
            {
                form2.ShowDialog();

                if (Convert.ToInt32(stepsLabel.Text) > maximumSteps)
                {
                    MessageBox.Show($"Вы совершили {stepsLabel.Text} ходов, " +
                        $"Улучшить результат можно, сделав на " +
                        $"{Convert.ToInt32(stepsLabel.Text) - maximumSteps} ходов меньше");
                }

                else MessageBox.Show("Вы выйграли игру, сделав минимальное количество шагов, так держать!");

                RestartGame();
            }
        }

        private void RestartGame()
        {
            for (int i = 0; i < allPictureBoxes.Count; i++)
                allPictureBoxes[i].Location = new Point(allPictureBoxes[i].Width * i, 0);

            stepsLabel.Text = "0";
        }

        private void AddPictureBoxesToLists()
        {
            allPictureBoxes = new List<PictureBox>()
            {
                leftFrogPictureBox1, leftFrogPictureBox2, leftFrogPictureBox3,leftFrogPictureBox4,

                emptyFlowerpictureBox,

                rightFrogPictureBox1, rightFrogPictureBox2, rightFrogPictureBox3,rightFrogPictureBox4
            };

            leftFrogs = new List<PictureBox>()
            {
                leftFrogPictureBox1, leftFrogPictureBox2, leftFrogPictureBox3,leftFrogPictureBox4
            };

            rightFrogs = new List<PictureBox>()
            {
                rightFrogPictureBox1, rightFrogPictureBox2,rightFrogPictureBox3,rightFrogPictureBox4
            };
        }

        private void PlayFrogVoice()
        {
            frogVoice = new AudioFileReader(@"..\..\..\Resources\frogSound.wav");
            outFrogVoice = new();
            outFrogVoice.Init(frogVoice);
            outFrogVoice.Play();
        }


    }
}