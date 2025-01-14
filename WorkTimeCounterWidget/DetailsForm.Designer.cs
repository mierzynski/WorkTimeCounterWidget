namespace WorkTimeCounterWidget
{
    partial class DetailsForm
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
            button_AddProject = new Button();
            button_DeleteProject = new Button();
            label_TimeSum = new Label();
            textBox_ProjectName = new TextBox();
            listBox_Projects = new ListBox();
            listBox_ProjectTimesList = new ListBox();
            SuspendLayout();
            // 
            // button_AddProject
            // 
            button_AddProject.Location = new Point(139, 12);
            button_AddProject.Name = "button_AddProject";
            button_AddProject.Size = new Size(75, 23);
            button_AddProject.TabIndex = 1;
            button_AddProject.Text = "add project";
            button_AddProject.UseVisualStyleBackColor = true;
            button_AddProject.Click += button_AddProject_Click;
            // 
            // button_DeleteProject
            // 
            button_DeleteProject.Location = new Point(220, 12);
            button_DeleteProject.Name = "button_DeleteProject";
            button_DeleteProject.Size = new Size(89, 23);
            button_DeleteProject.TabIndex = 2;
            button_DeleteProject.Text = "delete project";
            button_DeleteProject.UseVisualStyleBackColor = true;
            button_DeleteProject.Click += button_DeleteProject_Click;
            // 
            // label_TimeSum
            // 
            label_TimeSum.AutoSize = true;
            label_TimeSum.Location = new Point(315, 109);
            label_TimeSum.Name = "label_TimeSum";
            label_TimeSum.Size = new Size(29, 15);
            label_TimeSum.TabIndex = 3;
            label_TimeSum.Text = "5,5h";
            // 
            // textBox_ProjectName
            // 
            textBox_ProjectName.Location = new Point(139, 41);
            textBox_ProjectName.Name = "textBox_ProjectName";
            textBox_ProjectName.Size = new Size(100, 23);
            textBox_ProjectName.TabIndex = 4;
            // 
            // listBox_Projects
            // 
            listBox_Projects.FormattingEnabled = true;
            listBox_Projects.ItemHeight = 15;
            listBox_Projects.Location = new Point(12, 12);
            listBox_Projects.Name = "listBox_Projects";
            listBox_Projects.Size = new Size(120, 94);
            listBox_Projects.TabIndex = 5;
            // 
            // listBox_ProjectTimesList
            // 
            listBox_ProjectTimesList.FormattingEnabled = true;
            listBox_ProjectTimesList.ItemHeight = 15;
            listBox_ProjectTimesList.Location = new Point(315, 12);
            listBox_ProjectTimesList.Name = "listBox_ProjectTimesList";
            listBox_ProjectTimesList.Size = new Size(120, 94);
            listBox_ProjectTimesList.TabIndex = 6;
            // 
            // DetailsForm
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(800, 450);
            Controls.Add(listBox_ProjectTimesList);
            Controls.Add(listBox_Projects);
            Controls.Add(textBox_ProjectName);
            Controls.Add(label_TimeSum);
            Controls.Add(button_DeleteProject);
            Controls.Add(button_AddProject);
            Name = "DetailsForm";
            Text = "Details";
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion
        private Button button_AddProject;
        private Button button_DeleteProject;
        private Label label_TimeSum;
        private TextBox textBox_ProjectName;
        private ListBox listBox_Projects;
        private ListBox listBox_ProjectTimesList;
    }
}
