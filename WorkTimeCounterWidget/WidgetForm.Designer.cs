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
            button_Break = new Button();
            button_Infolinia = new Button();
            timer = new System.Windows.Forms.Timer(components);
            button_ShowMainWindow = new Button();
            button_Up_Click = new Button();
            button_Down_Click = new Button();
            SuspendLayout();
            // 
            // label_ProjectName
            // 
            label_ProjectName.AutoSize = true;
            label_ProjectName.Location = new Point(12, 13);
            label_ProjectName.Name = "label_ProjectName";
            label_ProjectName.Size = new Size(25, 15);
            label_ProjectName.TabIndex = 0;
            label_ProjectName.Text = "WB";
            // 
            // label_ProjectTime
            // 
            label_ProjectTime.AutoSize = true;
            label_ProjectTime.Location = new Point(43, 13);
            label_ProjectTime.Name = "label_ProjectTime";
            label_ProjectTime.Size = new Size(49, 15);
            label_ProjectTime.TabIndex = 1;
            label_ProjectTime.Text = "00:00:00";
            // 
            // button_StartStop
            // 
            button_StartStop.BackColor = Color.Gray;
            button_StartStop.ForeColor = SystemColors.ControlText;
            button_StartStop.Location = new Point(140, 5);
            button_StartStop.Name = "button_StartStop";
            button_StartStop.Size = new Size(53, 31);
            button_StartStop.TabIndex = 2;
            button_StartStop.Text = "stop";
            button_StartStop.UseVisualStyleBackColor = false;
            button_StartStop.Click += button_StartStop_Click;
            // 
            // button_Break
            // 
            button_Break.BackColor = Color.Gray;
            button_Break.BackgroundImage = (Image)resources.GetObject("button_Break.BackgroundImage");
            button_Break.BackgroundImageLayout = ImageLayout.Center;
            button_Break.Location = new Point(199, 5);
            button_Break.Name = "button_Break";
            button_Break.Size = new Size(31, 31);
            button_Break.TabIndex = 3;
            button_Break.UseVisualStyleBackColor = false;
            button_Break.Click += button_Break_Click;
            // 
            // button_Infolinia
            // 
            button_Infolinia.BackColor = Color.Gray;
            button_Infolinia.BackgroundImage = (Image)resources.GetObject("button_Infolinia.BackgroundImage");
            button_Infolinia.BackgroundImageLayout = ImageLayout.Center;
            button_Infolinia.Location = new Point(235, 5);
            button_Infolinia.Name = "button_Infolinia";
            button_Infolinia.Size = new Size(31, 31);
            button_Infolinia.TabIndex = 4;
            button_Infolinia.UseVisualStyleBackColor = false;
            button_Infolinia.Click += button_Infolinia_Click;
            // 
            // timer
            // 
            timer.Interval = 10;
            timer.Tick += timer_Tick;
            // 
            // button_ShowMainWindow
            // 
            button_ShowMainWindow.BackColor = Color.Gray;
            button_ShowMainWindow.BackgroundImage = (Image)resources.GetObject("button_ShowMainWindow.BackgroundImage");
            button_ShowMainWindow.BackgroundImageLayout = ImageLayout.Center;
            button_ShowMainWindow.Location = new Point(271, 5);
            button_ShowMainWindow.Name = "button_ShowMainWindow";
            button_ShowMainWindow.Size = new Size(20, 31);
            button_ShowMainWindow.TabIndex = 5;
            button_ShowMainWindow.UseVisualStyleBackColor = false;
            // 
            // button_Up_Click
            // 
            button_Up_Click.BackgroundImage = (Image)resources.GetObject("button_Up_Click.BackgroundImage");
            button_Up_Click.BackgroundImageLayout = ImageLayout.Center;
            button_Up_Click.Location = new Point(101, 5);
            button_Up_Click.Margin = new Padding(0);
            button_Up_Click.Name = "button_Up_Click";
            button_Up_Click.Size = new Size(36, 16);
            button_Up_Click.TabIndex = 6;
            button_Up_Click.UseVisualStyleBackColor = true;
            button_Up_Click.Click += button_Up_Click_Click;
            // 
            // button_Down_Click
            // 
            button_Down_Click.BackgroundImage = (Image)resources.GetObject("button_Down_Click.BackgroundImage");
            button_Down_Click.BackgroundImageLayout = ImageLayout.Center;
            button_Down_Click.Location = new Point(101, 20);
            button_Down_Click.Margin = new Padding(0);
            button_Down_Click.Name = "button_Down_Click";
            button_Down_Click.Size = new Size(36, 16);
            button_Down_Click.TabIndex = 7;
            button_Down_Click.UseVisualStyleBackColor = true;
            button_Down_Click.Click += button_Down_Click_Click;
            // 
            // WidgetForm
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(296, 39);
            Controls.Add(button_Down_Click);
            Controls.Add(button_Up_Click);
            Controls.Add(button_ShowMainWindow);
            Controls.Add(button_Infolinia);
            Controls.Add(button_Break);
            Controls.Add(button_StartStop);
            Controls.Add(label_ProjectTime);
            Controls.Add(label_ProjectName);
            FormBorderStyle = FormBorderStyle.FixedSingle;
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
        private System.Windows.Forms.Timer timer;
        private Button button_ShowMainWindow;
        private Button button_Up_Click;
        private Button button_Down_Click;
    }
}