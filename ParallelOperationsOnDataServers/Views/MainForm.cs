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
            dataGridViewMain.RowPrePaint += dataGridViewMain_RowPrePaint;
            textBoxFilter.TextChanged += TextBoxFilter_TextChanged;

            dataGridViewMain.AllowUserToAddRows = false;
            dataGridViewMain.AutoGenerateColumns = true;
            DataGridViewRow row = dataGridViewMain.RowTemplate;
            row.DefaultCellStyle.BackColor = Color.AliceBlue;
            dataGridViewMain.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.AllCells;
            dataGridViewMain.ColumnHeadersDefaultCellStyle.Font = new Font("Consolas", 12F, FontStyle.Bold, GraphicsUnit.Pixel);
            row.Height = 5;
            row.MinimumHeight = 17;
            btnStart.Select();
            labelConsole.ForeColor = Color.Green;

            // Проверка подключения
            labelConsole.Text = mModel.Console;
            btnStart.Enabled = mModel.GetServersData().Columns["Error"] != null ? false : true;
            if (btnStart.Enabled == true)
                RefreshGrid();
        }

        

        private void btnStart_Click(object sender, EventArgs e)
        {
            textBoxFilter.Text = String.Empty;
            textBoxFilter.Enabled = false;
            btnStart.Enabled = false;
            mModel.Hour = Convert.ToInt32(numericUpDownH.Value);
            mModel.Minute = Convert.ToInt32(numericUpDownM.Value);
            mModel.Second = Convert.ToInt32(numericUpDownS.Value);

            mModel.Generate_ConnectionString();
            RefreshGrid();
        }
        void RefreshGrid()
        {
            bindingSource.DataSource = null;
            dataGridViewMain.DataSource = mModel.dtTable;
            bindingSource.DataSource = dataGridViewMain.DataSource;
        }
        private void dataGridViewMain_RowPrePaint(object sender, DataGridViewRowPrePaintEventArgs e)
        {
            for (int i = 0; i < dataGridViewMain.Rows.Count; i++)
            {
                String difference = Convert.ToString(dataGridViewMain.Rows[i].Cells["Difference"].Value);
                if (difference.Contains("Ошибка!"))
                {
                    dataGridViewMain.Rows[i].DefaultCellStyle.BackColor = Color.Salmon;
                }
                else if (difference.Contains("Все в порядке!"))
                {
                    dataGridViewMain.Rows[i].DefaultCellStyle.BackColor = Color.LightGreen;
                }
                else if (difference.Contains("Разница"))
                {
                    dataGridViewMain.Rows[i].DefaultCellStyle.BackColor = Color.CadetBlue;
                }
                else if (difference.Contains("Waiting"))
                {
                    dataGridViewMain.Rows[i].DefaultCellStyle.BackColor = Color.LightYellow;
                }
            }
            textBoxFilter.Enabled = 
                btnStart.Enabled = mModel.Work == true ? false : true;
            labelConsole.Text = mModel.Console;
        }
        private void TextBoxFilter_TextChanged(object sender, EventArgs e)// Фильтр по гриду
        {
            if (!String.IsNullOrEmpty(textBoxFilter.Text))
            {
                bindingSource.Filter = dataGridViewMain.Columns["Dsc"].HeaderText.ToString() + "LIKE '%" + textBoxFilter.Text + "%'";
                dataGridViewMain.DataSource = bindingSource;
            }
            else bindingSource.Filter = String.Empty;
        }

        private void btnStop_Click(object sender, EventArgs e)// Stop
        {
            mModel.CancelTokenSource();
        }
    }
}