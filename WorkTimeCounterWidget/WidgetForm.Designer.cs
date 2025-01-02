namespace WorkTimeCounterWidget
{
    partial class WidgetForm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
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
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            label_ProjectName = new Label();
            label_ProjectTime = new Label();
            button_StartStop = new Button();
            button_Break = new Button();
            button_Infolinia = new Button();
            radioButton_ShowConfigWindow = new RadioButton();
            SuspendLayout();
            // 
            // label_ProjectName
            // 
            label_ProjectName.AutoSize = true;
            label_ProjectName.Location = new Point(12, 9);
            label_ProjectName.Name = "label_ProjectName";
            label_ProjectName.Size = new Size(25, 15);
            label_ProjectName.TabIndex = 0;
            label_ProjectName.Text = "WB";
            // 
            // label_ProjectTime
            // 
            label_ProjectTime.AutoSize = true;
            label_ProjectTime.Location = new Point(43, 9);
            label_ProjectTime.Name = "label_ProjectTime";
            label_ProjectTime.Size = new Size(49, 15);
            label_ProjectTime.TabIndex = 1;
            label_ProjectTime.Text = "00:00:00";
            // 
            // button_StartStop
            // 
            button_StartStop.BackColor = Color.Firebrick;
            button_StartStop.ForeColor = SystemColors.ControlText;
            button_StartStop.Location = new Point(98, 5);
            button_StartStop.Name = "button_StartStop";
            button_StartStop.Size = new Size(53, 23);
            button_StartStop.TabIndex = 2;
            button_StartStop.Text = "stop";
            button_StartStop.UseVisualStyleBackColor = false;
            button_StartStop.Click += button_StartStop_Click;
            // 
            // button_Break
            // 
            button_Break.BackColor = Color.IndianRed;
            button_Break.Location = new Point(157, 5);
            button_Break.Name = "button_Break";
            button_Break.Size = new Size(75, 23);
            button_Break.TabIndex = 3;
            button_Break.Text = "break";
            button_Break.UseVisualStyleBackColor = false;
            button_Break.Click += button_Break_Click;
            // 
            // button_Infolinia
            // 
            button_Infolinia.BackColor = Color.IndianRed;
            button_Infolinia.Location = new Point(238, 5);
            button_Infolinia.Name = "button_Infolinia";
            button_Infolinia.Size = new Size(75, 23);
            button_Infolinia.TabIndex = 4;
            button_Infolinia.Text = "infolinia";
            button_Infolinia.UseVisualStyleBackColor = false;
            button_Infolinia.Click += button_Infolinia_Click;
            // 
            // radioButton_ShowConfigWindow
            // 
            radioButton_ShowConfigWindow.AutoSize = true;
            radioButton_ShowConfigWindow.Location = new Point(319, 10);
            radioButton_ShowConfigWindow.Name = "radioButton_ShowConfigWindow";
            radioButton_ShowConfigWindow.Size = new Size(14, 13);
            radioButton_ShowConfigWindow.TabIndex = 5;
            radioButton_ShowConfigWindow.TabStop = true;
            radioButton_ShowConfigWindow.UseVisualStyleBackColor = true;
            radioButton_ShowConfigWindow.CheckedChanged += radioButton_ShowConfigWindow_CheckedChanged;
            // 
            // WidgetForm
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(340, 33);
            Controls.Add(radioButton_ShowConfigWindow);
            Controls.Add(button_Infolinia);
            Controls.Add(button_Break);
            Controls.Add(button_StartStop);
            Controls.Add(label_ProjectTime);
            Controls.Add(label_ProjectName);
            FormBorderStyle = FormBorderStyle.None;
            Name = "WidgetForm";
            StartPosition = FormStartPosition.Manual;
            Text = "WidgetForm";
            TopMost = true;
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private Label label_ProjectName;
        private Label label_ProjectTime;
        private Button button_StartStop;
        private Button button_Break;
        private Button button_Infolinia;
        private RadioButton radioButton_ShowConfigWindow;
    }
}