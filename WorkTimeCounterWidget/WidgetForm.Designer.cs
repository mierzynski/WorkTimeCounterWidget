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
            timer = new System.Windows.Forms.Timer(components);
            blinkTimer = new System.Windows.Forms.Timer(components);
            SuspendLayout();
            // 
            // timer
            // 
            timer.Interval = 1000;
            timer.Tick += timer_Tick;
            // 
            // blinkTimer
            // 
            blinkTimer.Interval = 500;
            blinkTimer.Tick += blinkTimer_Tick;
            // 
            // WidgetForm
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            BackColor = Color.FromArgb(235, 235, 235);
            ClientSize = new Size(310, 30);
            FormBorderStyle = FormBorderStyle.None;
            Name = "WidgetForm";
            StartPosition = FormStartPosition.Manual;
            Text = "Widget";
            TopMost = true;
            MouseDown += mouseDown_Event;
            MouseMove += mouseMove_Event;
            MouseUp += mouseUp_Event;
            ResumeLayout(false);
        }

        #endregion
        private System.Windows.Forms.Timer timer;
        private System.Windows.Forms.Timer blinkTimer;
    }
}