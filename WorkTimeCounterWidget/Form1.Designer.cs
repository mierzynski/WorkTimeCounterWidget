namespace WorkTimeCounterWidget
{
    partial class Form1
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
            comboBox_ProjectToDisplay = new ComboBox();
            button_AddProject = new Button();
            button_DeleteProject = new Button();
            label_TimeSum = new Label();
            SuspendLayout();
            // 
            // comboBox_ProjectToDisplay
            // 
            comboBox_ProjectToDisplay.FormattingEnabled = true;
            comboBox_ProjectToDisplay.Location = new Point(12, 12);
            comboBox_ProjectToDisplay.Name = "comboBox_ProjectToDisplay";
            comboBox_ProjectToDisplay.Size = new Size(121, 23);
            comboBox_ProjectToDisplay.TabIndex = 0;
            // 
            // button_AddProject
            // 
            button_AddProject.Location = new Point(139, 12);
            button_AddProject.Name = "button_AddProject";
            button_AddProject.Size = new Size(75, 23);
            button_AddProject.TabIndex = 1;
            button_AddProject.Text = "add project";
            button_AddProject.UseVisualStyleBackColor = true;
            // 
            // button_DeleteProject
            // 
            button_DeleteProject.Location = new Point(220, 12);
            button_DeleteProject.Name = "button_DeleteProject";
            button_DeleteProject.Size = new Size(89, 23);
            button_DeleteProject.TabIndex = 2;
            button_DeleteProject.Text = "delete project";
            button_DeleteProject.UseVisualStyleBackColor = true;
            // 
            // label_TimeSum
            // 
            label_TimeSum.AutoSize = true;
            label_TimeSum.Location = new Point(315, 16);
            label_TimeSum.Name = "label_TimeSum";
            label_TimeSum.Size = new Size(29, 15);
            label_TimeSum.TabIndex = 3;
            label_TimeSum.Text = "5,5h";
            // 
            // Form1
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(800, 450);
            Controls.Add(label_TimeSum);
            Controls.Add(button_DeleteProject);
            Controls.Add(button_AddProject);
            Controls.Add(comboBox_ProjectToDisplay);
            Name = "Form1";
            Text = "Form1";
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private ComboBox comboBox_ProjectToDisplay;
        private Button button_AddProject;
        private Button button_DeleteProject;
        private Label label_TimeSum;
    }
}
