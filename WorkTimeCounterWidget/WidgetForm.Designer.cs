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
            label_ProjectName = new Label();
            label_ProjectTime = new Label();
            timer = new System.Windows.Forms.Timer(components);
            SuspendLayout();
            // 
            // label_ProjectName
            // 
            label_ProjectName.AutoEllipsis = true;
            label_ProjectName.AutoSize = true;
            label_ProjectName.BackColor = Color.Transparent;
            label_ProjectName.Location = new Point(140, 9);
            label_ProjectName.Name = "label_ProjectName";
            label_ProjectName.Size = new Size(12, 15);
            label_ProjectName.TabIndex = 0;
            label_ProjectName.Text = "-";
            // 
            // label_ProjectTime
            // 
            label_ProjectTime.AutoSize = true;
            label_ProjectTime.BackColor = Color.Transparent;
            label_ProjectTime.Location = new Point(85, 9);
            label_ProjectTime.Name = "label_ProjectTime";
            label_ProjectTime.Size = new Size(49, 15);
            label_ProjectTime.TabIndex = 1;
            label_ProjectTime.Text = "00:00:00";
            // 
            // timer
            // 
            timer.Interval = 1;
            timer.Tick += timer_Tick;
            // 
            // WidgetForm
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            BackColor = Color.FromArgb(235, 235, 235);
            ClientSize = new Size(310, 30);
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
        private System.Windows.Forms.Timer timer;
    }
}