using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace лр26
{
    public partial class Form1 : Form
    {
        // Поле для хранения объекта базы данных
        private Database _database;

        // Поле для хранения выбранного ID консультации
        private int _selectedId = -1;

        public Form1()
        {
            InitializeComponent();
            // Инициализация объекта базы данных с подключением к файлу базы данных
            _database = new Database(@"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=C:\работа\Практика КПиЯП\лр26\лр26\ConsultationDB.mdf;Integrated Security=True;Connect Timeout=30");
            // Загрузка консультаций при запуске формы
            LoadConsultations();
        }

        // Метод для загрузки консультаций из базы данных в DataGridView
        private void LoadConsultations()
        {
            var consultations = _database.GetConsultations();
            dataGridView1.DataSource = consultations.ToList();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            // Загрузка данных в таблицу Consultations из DataSet при загрузке формы
            this.consultationsTableAdapter.Fill(this.consultationDBDataSet.Consultations);
        }

        // Обработчик события нажатия кнопки добавления новой консультации
        private void button1_Click(object sender, EventArgs e)
        {
            var consultation = new Consultation
            {
                FullName = textBox1.Text,
                Subject = comboBox1.Text,
                Date = dateTimePicker1.Value,
                TimeSlot = maskedTextBox1.Text,
                IsBooked = false
            };
            // Добавление новой консультации в базу данных
            _database.AddConsultation(consultation);
            // Перезагрузка списка консультаций
            LoadConsultations();
            // Очистка полей ввода
            ClearInputFields();
        }

        // Обработчик события нажатия кнопки удаления выбранной консультации
        private void button2_Click(object sender, EventArgs e)
        {
            if (_selectedId > -1)
            {
                _database.DeleteConsultation(_selectedId);
                LoadConsultations();
                ClearInputFields();
            }
        }

        // Обработчик события нажатия кнопки бронирования выбранной консультации
        private void button3_Click(object sender, EventArgs e)
        {
            if (_selectedId > -1)
            {
                var selectedConsultation = (Consultation)dataGridView1.CurrentRow.DataBoundItem;
                if (!selectedConsultation.IsBooked)
                {
                    _database.UpdateConsultationBooking(_selectedId, true);
                    LoadConsultations();
                    ClearInputFields();
                }
                else
                {
                    MessageBox.Show("Консультация уже занята.");
                }
            }
        }

        // Обработчик события нажатия кнопки отмены бронирования выбранной консультации
        private void button4_Click(object sender, EventArgs e)
        {
            if (_selectedId > -1)
            {
                var selectedConsultation = (Consultation)dataGridView1.CurrentRow.DataBoundItem;
                if (selectedConsultation.IsBooked)
                {
                    _database.UpdateConsultationBooking(_selectedId, false);
                    LoadConsultations();
                    ClearInputFields();
                }
                else
                {
                    MessageBox.Show("Консультация уже свободна.");
                }
            }
        }

        // Обработчик события нажатия кнопки обновления списка консультаций
        private void button6_Click(object sender, EventArgs e)
        {
            LoadConsultations();
        }

        // Обработчик события нажатия кнопки поиска консультаций по предмету
        private void button5_Click(object sender, EventArgs e)
        {
            var consultations = _database.SearchConsultations(comboBox1.Text);
            dataGridView1.DataSource = consultations.ToList();
        }

        // Обработчик события клика по ячейке DataGridView для выбора консультации
        private void dataGridView1_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                _selectedId = (int)dataGridView1.Rows[e.RowIndex].Cells[0].Value;
            }
        }

        // Метод для очистки полей ввода
        private void ClearInputFields()
        {
            textBox1.Clear();
            comboBox1.SelectedIndex = -1;
            dateTimePicker1.Value = DateTime.Now;
            maskedTextBox1.Clear();
        }
    }
}
