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
            ExitMenu();
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

        }

        private void btnTestCon_Click(object sender, EventArgs e)
        {

        }
    }
}