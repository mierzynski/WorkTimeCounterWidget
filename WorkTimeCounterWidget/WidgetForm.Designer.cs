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
            components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(WidgetForm));
            label_ProjectName = new Label();
            label_ProjectTime = new Label();
            button_StartStop = new Button();
            timer = new System.Windows.Forms.Timer(components);
            button_ShowMainWindow = new Button();
            button_Up_Click = new Button();
            button_Down_Click = new Button();
            button_Break = new RoundedButton_horizontal();
            button_Infolinia = new RoundedButton_horizontal();
            SuspendLayout();
            // 
            // label_ProjectName
            // 
            label_ProjectName.AutoEllipsis = true;
            label_ProjectName.AutoSize = true;
            label_ProjectName.BackColor = Color.Transparent;
            label_ProjectName.Location = new Point(112, 27);
            label_ProjectName.Name = "label_ProjectName";
            label_ProjectName.Size = new Size(12, 15);
            label_ProjectName.TabIndex = 0;
            label_ProjectName.Text = "-";
            // 
            // label_ProjectTime
            // 
            label_ProjectTime.AutoSize = true;
            label_ProjectTime.BackColor = Color.Transparent;
            label_ProjectTime.Location = new Point(48, 27);
            label_ProjectTime.Name = "label_ProjectTime";
            label_ProjectTime.Size = new Size(49, 15);
            label_ProjectTime.TabIndex = 1;
            label_ProjectTime.Text = "00:00:00";
            // 
            // button_StartStop
            // 
            button_StartStop.BackColor = Color.Red;
            button_StartStop.FlatAppearance.BorderColor = Color.Red;
            button_StartStop.FlatAppearance.BorderSize = 0;
            button_StartStop.FlatStyle = FlatStyle.Flat;
            button_StartStop.Font = new Font("Microsoft Sans Serif", 8.999999F, FontStyle.Regular, GraphicsUnit.Point, 238);
            button_StartStop.ForeColor = Color.White;
            button_StartStop.Location = new Point(151, 13);
            button_StartStop.Margin = new Padding(0);
            button_StartStop.Name = "button_StartStop";
            button_StartStop.Size = new Size(60, 40);
            button_StartStop.TabIndex = 2;
            button_StartStop.Text = "PAUSED";
            button_StartStop.UseVisualStyleBackColor = false;
            button_StartStop.Click += button_StartStop_Click;
            // 
            // timer
            // 
            timer.Interval = 1;
            timer.Tick += timer_Tick;
            // 
            // button_ShowMainWindow
            // 
            button_ShowMainWindow.BackColor = Color.Transparent;
            button_ShowMainWindow.BackgroundImageLayout = ImageLayout.Center;
            button_ShowMainWindow.FlatAppearance.BorderColor = Color.White;
            button_ShowMainWindow.FlatAppearance.BorderSize = 0;
            button_ShowMainWindow.FlatStyle = FlatStyle.Flat;
            button_ShowMainWindow.Font = new Font("Microsoft Sans Serif", 8.999999F, FontStyle.Regular, GraphicsUnit.Point, 238);
            button_ShowMainWindow.Location = new Point(361, 10);
            button_ShowMainWindow.Margin = new Padding(0);
            button_ShowMainWindow.Name = "button_ShowMainWindow";
            button_ShowMainWindow.Size = new Size(60, 40);
            button_ShowMainWindow.TabIndex = 5;
            button_ShowMainWindow.Text = "details";
            button_ShowMainWindow.UseVisualStyleBackColor = false;
            button_ShowMainWindow.Click += button_ShowMainWindow_Click_1;
            // 
            // button_Up_Click
            // 
            button_Up_Click.BackColor = Color.FromArgb(64, 64, 64);
            button_Up_Click.BackgroundImage = (Image)resources.GetObject("button_Up_Click.BackgroundImage");
            button_Up_Click.BackgroundImageLayout = ImageLayout.Center;
            button_Up_Click.FlatAppearance.BorderSize = 0;
            button_Up_Click.FlatStyle = FlatStyle.Flat;
            button_Up_Click.Location = new Point(15, 15);
            button_Up_Click.Margin = new Padding(0);
            button_Up_Click.Name = "button_Up_Click";
            button_Up_Click.Size = new Size(20, 16);
            button_Up_Click.TabIndex = 6;
            button_Up_Click.UseVisualStyleBackColor = false;
            button_Up_Click.Click += button_Up_Click_Click;
            // 
            // button_Down_Click
            // 
            button_Down_Click.BackColor = Color.FromArgb(64, 64, 64);
            button_Down_Click.BackgroundImage = (Image)resources.GetObject("button_Down_Click.BackgroundImage");
            button_Down_Click.BackgroundImageLayout = ImageLayout.Center;
            button_Down_Click.FlatAppearance.BorderSize = 0;
            button_Down_Click.FlatStyle = FlatStyle.Flat;
            button_Down_Click.Location = new Point(15, 39);
            button_Down_Click.Margin = new Padding(0);
            button_Down_Click.Name = "button_Down_Click";
            button_Down_Click.Size = new Size(20, 16);
            button_Down_Click.TabIndex = 7;
            button_Down_Click.UseVisualStyleBackColor = false;
            button_Down_Click.Click += button_Down_Click_Click;
            // 
            // button_Break
            // 
            button_Break.BackColor = Color.Black;
            button_Break.FlatAppearance.BorderSize = 0;
            button_Break.FlatStyle = FlatStyle.Flat;
            button_Break.Font = new Font("Segoe UI", 7F);
            button_Break.ForeColor = Color.White;
            button_Break.Location = new Point(214, 22);
            button_Break.Name = "button_Break";
            button_Break.Size = new Size(69, 20);
            button_Break.TabIndex = 8;
            button_Break.Text = "przerwa";
            button_Break.UseVisualStyleBackColor = false;
            button_Break.Click += button_Break_Click;
            // 
            // button_Infolinia
            // 
            button_Infolinia.BackColor = Color.Black;
            button_Infolinia.FlatAppearance.BorderSize = 0;
            button_Infolinia.FlatStyle = FlatStyle.Flat;
            button_Infolinia.Font = new Font("Segoe UI", 7F);
            button_Infolinia.ForeColor = Color.White;
            button_Infolinia.Location = new Point(289, 22);
            button_Infolinia.Name = "button_Infolinia";
            button_Infolinia.Size = new Size(69, 20);
            button_Infolinia.TabIndex = 9;
            button_Infolinia.Text = "infolinia";
            button_Infolinia.UseVisualStyleBackColor = false;
            button_Infolinia.Click += button_Infolinia_Click;
            // 
            // WidgetForm
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            BackColor = Color.FromArgb(235, 235, 235);
            ClientSize = new Size(442, 70);
            Controls.Add(button_Infolinia);
            Controls.Add(button_Break);
            Controls.Add(button_Down_Click);
            Controls.Add(button_Up_Click);
            Controls.Add(button_ShowMainWindow);
            Controls.Add(button_StartStop);
            Controls.Add(label_ProjectTime);
            Controls.Add(label_ProjectName);
            FormBorderStyle = FormBorderStyle.None;
            Name = "WidgetForm";
            StartPosition = FormStartPosition.CenterScreen;
            Text = "Widget";
            TopMost = true;
            MouseDown += mouseDown_Event;
            MouseMove += mouseMove_Event;
            MouseUp += mouseUp_Event;
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private Label label_ProjectName;
        private Label label_ProjectTime;
        private Button button_StartStop;
        private System.Windows.Forms.Timer timer;
        private Button button_ShowMainWindow;
        private Button button_Up_Click;
        private Button button_Down_Click;
        private RoundedButton_horizontal button_Break;
        private RoundedButton_horizontal button_Infolinia;
    }
}