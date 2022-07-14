using ParallelOperationsOnDataServers.Models;

namespace ParallelOperationsOnDataServers
{
    public partial class MainForm : Form
    {
        MainModel mModel = new MainModel();
        BindingSource bindingSource = new BindingSource();
        public MainForm()
        {
            InitializeComponent();
            dataGridView1.RowPrePaint += DataGridView1_RowPrePaint;
            textBoxFilter.TextChanged += TextBoxFilter_TextChanged;

            dataGridView1.AllowUserToAddRows = false;
            dataGridView1.AutoGenerateColumns = true;
            DataGridViewRow row = dataGridView1.RowTemplate;
            row.DefaultCellStyle.BackColor = Color.AliceBlue;
            dataGridView1.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.AllCells;
            dataGridView1.ColumnHeadersDefaultCellStyle.Font = new Font("Consolas", 12F, FontStyle.Bold, GraphicsUnit.Pixel);
            row.Height = 5;
            row.MinimumHeight = 17;
            btnStart.Select();
            labelConsole.ForeColor = Color.Green;

            // Проверка подключения
            labelConsole.Text = mModel.Console;
            if(mModel.Console.Contains("Подключение выполнено успешно!"))
            {
                RefreshGrid();
                btnStart.Enabled = true;
            }
            else
                btnStart.Enabled = false;
        }

        

        private void btnStart_Click(object sender, EventArgs e)
        {
            textBoxFilter.Text = String.Empty;
            textBoxFilter.Enabled = false;
            btnStart.Enabled = false;
            btnTestCon.Enabled = false;
            mModel.Hour = Convert.ToInt32(numericUpDownH.Value);
            mModel.Minute = Convert.ToInt32(numericUpDownM.Value);
            mModel.Second = Convert.ToInt32(numericUpDownS.Value);

            mModel.Generate_ConnectionString();
            RefreshGrid();
        }
        void RefreshGrid()
        {
            bindingSource.DataSource = null;
            dataGridView1.DataSource = mModel.dtTable;
            bindingSource.DataSource = dataGridView1.DataSource;
        }
        private void btnTestCon_Click(object sender, EventArgs e)
        {
            mModel.ConnTest();
            labelConsole.Text = mModel.Console;
        }
        private void DataGridView1_RowPrePaint(object? sender, DataGridViewRowPrePaintEventArgs e)
        {
            for (int i = 0; i < dataGridView1.Rows.Count; i++)
            {
                String difference = Convert.ToString(dataGridView1.Rows[i].Cells["Difference"].Value);
                if (difference.Contains("Ошибка!"))
                {
                    dataGridView1.Rows[i].DefaultCellStyle.BackColor = Color.Salmon;
                }
                else if (difference.Contains("Все в порядке!"))
                {
                    dataGridView1.Rows[i].DefaultCellStyle.BackColor = Color.LightGreen;
                }
                else if (difference.Contains("Разница"))
                {
                    dataGridView1.Rows[i].DefaultCellStyle.BackColor = Color.CadetBlue;
                }
                else if (difference.Contains("Waiting"))
                {
                    dataGridView1.Rows[i].DefaultCellStyle.BackColor = Color.LightYellow;
                }
            }
            textBoxFilter.Enabled = 
                btnStart.Enabled =
                    btnTestCon.Enabled = mModel.Work == true ? false : true;
            labelConsole.Text = mModel.Console;
        }
        private void TextBoxFilter_TextChanged(object? sender, EventArgs e)// Фильтр по гриду
        {
            if (!String.IsNullOrEmpty(textBoxFilter.Text))
            {
                bindingSource.Filter = dataGridView1.Columns["Dsc"].HeaderText.ToString() + "LIKE '%" + textBoxFilter.Text + "%'";
                dataGridView1.DataSource = bindingSource;
            }
            else bindingSource.Filter = String.Empty;
        }
    }
}