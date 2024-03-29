﻿namespace ParallelOperationsOnDataServers
{
    partial class MainForm
    {
        /// <summary>
        ///  Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        ///  Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        ///  Required method for Designer support - do not modify
        ///  the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.panel1 = new System.Windows.Forms.Panel();
            this.btnStop = new System.Windows.Forms.Button();
            this.labelnumeric = new System.Windows.Forms.Label();
            this.numericUpDownS = new System.Windows.Forms.NumericUpDown();
            this.numericUpDownM = new System.Windows.Forms.NumericUpDown();
            this.numericUpDownH = new System.Windows.Forms.NumericUpDown();
            this.btnStart = new System.Windows.Forms.Button();
            this.panel2 = new System.Windows.Forms.Panel();
            this.dataGridViewMain = new System.Windows.Forms.DataGridView();
            this.panel3 = new System.Windows.Forms.Panel();
            this.labelConsole = new System.Windows.Forms.Label();
            this.textBoxFilter = new System.Windows.Forms.TextBox();
            this.panel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownS)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownM)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownH)).BeginInit();
            this.panel2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewMain)).BeginInit();
            this.panel3.SuspendLayout();
            this.SuspendLayout();
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.btnStop);
            this.panel1.Controls.Add(this.labelnumeric);
            this.panel1.Controls.Add(this.numericUpDownS);
            this.panel1.Controls.Add(this.numericUpDownM);
            this.panel1.Controls.Add(this.numericUpDownH);
            this.panel1.Controls.Add(this.btnStart);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Left;
            this.panel1.Location = new System.Drawing.Point(0, 0);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(200, 450);
            this.panel1.TabIndex = 0;
            // 
            // btnStop
            // 
            this.btnStop.Location = new System.Drawing.Point(23, 115);
            this.btnStop.Name = "btnStop";
            this.btnStop.Size = new System.Drawing.Size(153, 23);
            this.btnStop.TabIndex = 6;
            this.btnStop.Text = "Стоп";
            this.btnStop.UseVisualStyleBackColor = true;
            this.btnStop.Click += new System.EventHandler(this.btnStop_Click);
            // 
            // labelnumeric
            // 
            this.labelnumeric.AutoSize = true;
            this.labelnumeric.Location = new System.Drawing.Point(39, 39);
            this.labelnumeric.Name = "labelnumeric";
            this.labelnumeric.Size = new System.Drawing.Size(117, 15);
            this.labelnumeric.TabIndex = 4;
            this.labelnumeric.Text = "Час       Мин        Сек";
            // 
            // numericUpDownS
            // 
            this.numericUpDownS.Location = new System.Drawing.Point(122, 57);
            this.numericUpDownS.Name = "numericUpDownS";
            this.numericUpDownS.Size = new System.Drawing.Size(37, 23);
            this.numericUpDownS.TabIndex = 3;
            // 
            // numericUpDownM
            // 
            this.numericUpDownM.Location = new System.Drawing.Point(79, 57);
            this.numericUpDownM.Name = "numericUpDownM";
            this.numericUpDownM.Size = new System.Drawing.Size(37, 23);
            this.numericUpDownM.TabIndex = 2;
            // 
            // numericUpDownH
            // 
            this.numericUpDownH.Location = new System.Drawing.Point(36, 57);
            this.numericUpDownH.Name = "numericUpDownH";
            this.numericUpDownH.Size = new System.Drawing.Size(37, 23);
            this.numericUpDownH.TabIndex = 1;
            // 
            // btnStart
            // 
            this.btnStart.Location = new System.Drawing.Point(23, 86);
            this.btnStart.Name = "btnStart";
            this.btnStart.Size = new System.Drawing.Size(153, 23);
            this.btnStart.TabIndex = 0;
            this.btnStart.Text = "Пуск";
            this.btnStart.UseVisualStyleBackColor = true;
            this.btnStart.Click += new System.EventHandler(this.btnStart_Click);
            // 
            // panel2
            // 
            this.panel2.Controls.Add(this.dataGridViewMain);
            this.panel2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel2.Location = new System.Drawing.Point(200, 23);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(600, 387);
            this.panel2.TabIndex = 1;
            // 
            // dataGridViewMain
            // 
            this.dataGridViewMain.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridViewMain.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dataGridViewMain.Location = new System.Drawing.Point(0, 0);
            this.dataGridViewMain.Name = "dataGridViewMain";
            this.dataGridViewMain.RowTemplate.Height = 25;
            this.dataGridViewMain.Size = new System.Drawing.Size(600, 387);
            this.dataGridViewMain.TabIndex = 0;
            // 
            // panel3
            // 
            this.panel3.Controls.Add(this.labelConsole);
            this.panel3.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panel3.Location = new System.Drawing.Point(200, 410);
            this.panel3.Name = "panel3";
            this.panel3.Size = new System.Drawing.Size(600, 40);
            this.panel3.TabIndex = 2;
            // 
            // labelConsole
            // 
            this.labelConsole.AutoSize = true;
            this.labelConsole.Location = new System.Drawing.Point(6, 8);
            this.labelConsole.Name = "labelConsole";
            this.labelConsole.Size = new System.Drawing.Size(75, 15);
            this.labelConsole.TabIndex = 5;
            this.labelConsole.Text = "labelConsole";
            // 
            // textBoxFilter
            // 
            this.textBoxFilter.Dock = System.Windows.Forms.DockStyle.Top;
            this.textBoxFilter.Location = new System.Drawing.Point(200, 0);
            this.textBoxFilter.Name = "textBoxFilter";
            this.textBoxFilter.Size = new System.Drawing.Size(600, 23);
            this.textBoxFilter.TabIndex = 3;
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Controls.Add(this.panel2);
            this.Controls.Add(this.textBoxFilter);
            this.Controls.Add(this.panel3);
            this.Controls.Add(this.panel1);
            this.Name = "MainForm";
            this.Text = "Параллельные операции на серверах с данными";
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownS)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownM)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownH)).EndInit();
            this.panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewMain)).EndInit();
            this.panel3.ResumeLayout(false);
            this.panel3.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private Panel panel1;
        private Button btnStart;
        private Panel panel2;
        private Panel panel3;
        private Label labelnumeric;
        private NumericUpDown numericUpDownS;
        private NumericUpDown numericUpDownM;
        private NumericUpDown numericUpDownH;
        private Label labelConsole;
        private DataGridView dataGridViewMain;
        private TextBox textBoxFilter;
        private Button btnStop;
    }
}