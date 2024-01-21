using ChumaClasses.Chuma;
using System;
using System.Drawing;
using System.Windows.Forms;

namespace Chuma
{
    public partial class Form1 : Form
    {
        private ChumaModel model = new ChumaModel();

        public Form1()
        {
            InitializeComponent();
            comboInfected.Visible = labelInfected.Visible = false;
            ////var person1 = new Person(x: 0, y: 0, speed: 5, id: Guid.NewGuid());
            ////var person2 = new Person(x: 20, y: 20, speed: 3, id: Guid.NewGuid());
            ////var doctor = new Doctor(x: 30, y: 30, speed: 2, id: Guid.NewGuid());

            ////var json = doctor.SerializeToJson();
            ////var doctor = JsonConvert.DeserializeObject<Doctor>(json);
        }

        /// <summary>
        /// Добавить нового агента в модель Чумы
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void buttonAddAgent_Click(object sender, EventArgs e)
        {
            switch(comboRoles.SelectedItem) 
            {
                case "Доктор":
                    var doctor = new Doctor(x: (int)numX.Value, y: (int)numY.Value, speed: (int)numSpeed.Value, id: Guid.NewGuid());
                    model.AddAgent(doctor);
                    MessageBox.Show("Успешно!");
                    break;
                case "Человек":
                    bool infected = comboRoles.SelectedItem != null && comboInfected.SelectedItem == "Нет"; 
                    var person = new Person(x: (int)numX.Value, y: (int)numY.Value, speed: (int)numSpeed.Value, isInfected: infected, id: Guid.NewGuid());
                    model.AddAgent(person);
                    MessageBox.Show("Успешно!");
                    break;
                default: MessageBox.Show("Ошибка! Не выбрана роль!"); break;
            }
        }

        /// <summary>
        /// Запустить в действие модель Чумы
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void buttonRunChuma_Click(object sender, EventArgs e)
        {
            PaintGrid();
            rtbLog.Clear();

            for (int i = 0; i < 1; i++) //i < 10
            {
                model.UpdateAgents(0.5); // обновляем модель каждые 0.5 секунды
                rtbLog.Text += model.PrintCurrentStateAllAgents();
            }
        }

        /// <summary>
        /// Получить цвет исходя из состояния агента
        /// </summary>
        /// <param name="state"></param>
        /// <returns></returns>
        private Color GetColorByState(int state)
        {
            switch (state)
            {
                case 0:
                    return Color.Pink;
                case 1:
                    return Color.Green;
                case 2:
                    return Color.Blue;
                default: return Color.White;
            }
        }

        /// <summary>
        /// Отрисовка грида
        /// </summary>
        private void PaintGrid()
        {
            ClearGrid();

            foreach (var d in model.GetCurrentPaintInfo())
            {
                if (d.Value.Item1 < dataGridView1.RowCount && d.Value.Item2 < dataGridView1.ColumnCount)
                    dataGridView1[d.Value.Item1, d.Value.Item2].Style.BackColor = GetColorByState(d.Value.Item3);
            }

            dataGridView1.ClearSelection();
        }

        /// <summary>
        /// Очистка грида (закраска белым всех ячеек)
        /// </summary>
        private void ClearGrid()
        {
            dataGridView1.ColumnCount = 30;
            dataGridView1.RowCount = 30;

            for (int x = 0; x < dataGridView1.RowCount; x++)
            {
                for (int i = 0; i < dataGridView1.ColumnCount; i++)
                {
                    dataGridView1[x, i].Style.BackColor = Color.White;
                }
            }
        }

        /// <summary>
        /// Выбор роли
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void comboRoles_SelectedIndexChanged(object sender, EventArgs e) 
            => comboInfected.Visible = labelInfected.Visible = comboRoles.SelectedItem == "Человек";

        /// <summary>
        /// Для отрисовки нумерации грида
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void dataGridView1_RowPrePaint(object sender, DataGridViewRowPrePaintEventArgs e)
        {
            int index = e.RowIndex;
            string indexStr = (index + 1).ToString();
            object header = this.dataGridView1.Rows[index].HeaderCell.Value;
            if (header == null || !header.Equals(indexStr))
                this.dataGridView1.Rows[index].HeaderCell.Value = indexStr;
        }
    }
}