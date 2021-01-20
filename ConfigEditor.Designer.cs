namespace WaterGasTool
{
    partial class ConfigEditor
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
            this.richTextBox1 = new System.Windows.Forms.RichTextBox();
            this.button_Done = new System.Windows.Forms.Button();
            this.button_cancel = new System.Windows.Forms.Button();
            this.label_Notice = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // richTextBox1
            // 
            this.richTextBox1.Location = new System.Drawing.Point(12, 12);
            this.richTextBox1.Name = "richTextBox1";
            this.richTextBox1.Size = new System.Drawing.Size(666, 264);
            this.richTextBox1.TabIndex = 0;
            this.richTextBox1.Text = "";
            // 
            // button_Done
            // 
            this.button_Done.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(255)))), ((int)(((byte)(192)))));
            this.button_Done.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.button_Done.Location = new System.Drawing.Point(603, 284);
            this.button_Done.Name = "button_Done";
            this.button_Done.Size = new System.Drawing.Size(75, 23);
            this.button_Done.TabIndex = 1;
            this.button_Done.Text = "Done";
            this.button_Done.UseVisualStyleBackColor = false;
            // 
            // button_cancel
            // 
            this.button_cancel.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(192)))), ((int)(((byte)(192)))));
            this.button_cancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.button_cancel.Location = new System.Drawing.Point(12, 284);
            this.button_cancel.Name = "button_cancel";
            this.button_cancel.Size = new System.Drawing.Size(75, 23);
            this.button_cancel.TabIndex = 2;
            this.button_cancel.Text = "Cancel";
            this.button_cancel.UseVisualStyleBackColor = false;
            // 
            // label_Notice
            // 
            this.label_Notice.AutoSize = true;
            this.label_Notice.Location = new System.Drawing.Point(93, 289);
            this.label_Notice.Name = "label_Notice";
            this.label_Notice.Size = new System.Drawing.Size(35, 13);
            this.label_Notice.TabIndex = 3;
            this.label_Notice.Text = "label1";
            // 
            // ConfigEditor
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(690, 319);
            this.ControlBox = false;
            this.Controls.Add(this.label_Notice);
            this.Controls.Add(this.button_cancel);
            this.Controls.Add(this.button_Done);
            this.Controls.Add(this.richTextBox1);
            this.Name = "ConfigEditor";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Config Editor";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.RichTextBox richTextBox1;
        private System.Windows.Forms.Button button_Done;
        private System.Windows.Forms.Button button_cancel;
        private System.Windows.Forms.Label label_Notice;
    }
}