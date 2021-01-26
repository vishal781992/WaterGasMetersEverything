namespace WaterGasTool
{
    partial class MeterUploadPOP
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
            this.comboBoxPOP_StatusCode = new System.Windows.Forms.ComboBox();
            this.label15 = new System.Windows.Forms.Label();
            this.label14 = new System.Windows.Forms.Label();
            this.comboBoxPOP_MeterTypeCode = new System.Windows.Forms.ComboBox();
            this.label13 = new System.Windows.Forms.Label();
            this.label12 = new System.Windows.Forms.Label();
            this.textBoxPOP_GasFW = new System.Windows.Forms.TextBox();
            this.textBoxPOP_WaterFw = new System.Windows.Forms.TextBox();
            this.buttonPOP_cancel = new System.Windows.Forms.Button();
            this.buttonPOP_Done = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // comboBoxPOP_StatusCode
            // 
            this.comboBoxPOP_StatusCode.FormattingEnabled = true;
            this.comboBoxPOP_StatusCode.Location = new System.Drawing.Point(185, 92);
            this.comboBoxPOP_StatusCode.Name = "comboBoxPOP_StatusCode";
            this.comboBoxPOP_StatusCode.Size = new System.Drawing.Size(267, 21);
            this.comboBoxPOP_StatusCode.TabIndex = 22;
            this.comboBoxPOP_StatusCode.MouseEnter += new System.EventHandler(this.comboBox_StatusCode_MouseEnter);
            // 
            // label15
            // 
            this.label15.AutoSize = true;
            this.label15.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label15.Location = new System.Drawing.Point(101, 93);
            this.label15.Name = "label15";
            this.label15.Size = new System.Drawing.Size(78, 16);
            this.label15.TabIndex = 21;
            this.label15.Text = "StatusCode";
            // 
            // label14
            // 
            this.label14.AutoSize = true;
            this.label14.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label14.Location = new System.Drawing.Point(72, 44);
            this.label14.Name = "label14";
            this.label14.Size = new System.Drawing.Size(107, 16);
            this.label14.TabIndex = 20;
            this.label14.Text = "MeterTypeCode";
            // 
            // comboBoxPOP_MeterTypeCode
            // 
            this.comboBoxPOP_MeterTypeCode.FormattingEnabled = true;
            this.comboBoxPOP_MeterTypeCode.Location = new System.Drawing.Point(185, 44);
            this.comboBoxPOP_MeterTypeCode.Name = "comboBoxPOP_MeterTypeCode";
            this.comboBoxPOP_MeterTypeCode.Size = new System.Drawing.Size(267, 21);
            this.comboBoxPOP_MeterTypeCode.TabIndex = 19;
            // 
            // label13
            // 
            this.label13.AutoSize = true;
            this.label13.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Italic, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label13.ForeColor = System.Drawing.SystemColors.ControlDarkDark;
            this.label13.Location = new System.Drawing.Point(321, 153);
            this.label13.Name = "label13";
            this.label13.Size = new System.Drawing.Size(50, 15);
            this.label13.TabIndex = 18;
            this.label13.Text = "Gas FW";
            // 
            // label12
            // 
            this.label12.AutoSize = true;
            this.label12.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Italic, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label12.ForeColor = System.Drawing.SystemColors.ControlDarkDark;
            this.label12.Location = new System.Drawing.Point(183, 153);
            this.label12.Name = "label12";
            this.label12.Size = new System.Drawing.Size(60, 15);
            this.label12.TabIndex = 17;
            this.label12.Text = "Water FW";
            // 
            // textBoxPOP_GasFW
            // 
            this.textBoxPOP_GasFW.Location = new System.Drawing.Point(324, 130);
            this.textBoxPOP_GasFW.Name = "textBoxPOP_GasFW";
            this.textBoxPOP_GasFW.Size = new System.Drawing.Size(128, 20);
            this.textBoxPOP_GasFW.TabIndex = 16;
            // 
            // textBoxPOP_WaterFw
            // 
            this.textBoxPOP_WaterFw.Location = new System.Drawing.Point(185, 130);
            this.textBoxPOP_WaterFw.Name = "textBoxPOP_WaterFw";
            this.textBoxPOP_WaterFw.Size = new System.Drawing.Size(129, 20);
            this.textBoxPOP_WaterFw.TabIndex = 15;
            this.textBoxPOP_WaterFw.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.textBoxPOP_WaterFw_KeyPress);
            // 
            // buttonPOP_cancel
            // 
            this.buttonPOP_cancel.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(192)))), ((int)(((byte)(192)))));
            this.buttonPOP_cancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.buttonPOP_cancel.Location = new System.Drawing.Point(12, 195);
            this.buttonPOP_cancel.Name = "buttonPOP_cancel";
            this.buttonPOP_cancel.Size = new System.Drawing.Size(122, 23);
            this.buttonPOP_cancel.TabIndex = 24;
            this.buttonPOP_cancel.Text = "Cancel";
            this.buttonPOP_cancel.UseVisualStyleBackColor = false;
            // 
            // buttonPOP_Done
            // 
            this.buttonPOP_Done.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(255)))), ((int)(((byte)(192)))));
            this.buttonPOP_Done.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.buttonPOP_Done.Location = new System.Drawing.Point(427, 195);
            this.buttonPOP_Done.Name = "buttonPOP_Done";
            this.buttonPOP_Done.Size = new System.Drawing.Size(122, 23);
            this.buttonPOP_Done.TabIndex = 23;
            this.buttonPOP_Done.Text = "Done";
            this.buttonPOP_Done.UseVisualStyleBackColor = false;
            this.buttonPOP_Done.MouseEnter += new System.EventHandler(this.buttonPOP_Done_MouseEnter);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Italic, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.ForeColor = System.Drawing.SystemColors.ControlDarkDark;
            this.label1.Location = new System.Drawing.Point(364, 68);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(88, 15);
            this.label1.TabIndex = 25;
            this.label1.Text = "List Updating...";
            // 
            // MeterUploadPOP
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(561, 229);
            this.ControlBox = false;
            this.Controls.Add(this.label1);
            this.Controls.Add(this.buttonPOP_cancel);
            this.Controls.Add(this.buttonPOP_Done);
            this.Controls.Add(this.comboBoxPOP_StatusCode);
            this.Controls.Add(this.label15);
            this.Controls.Add(this.label14);
            this.Controls.Add(this.comboBoxPOP_MeterTypeCode);
            this.Controls.Add(this.label13);
            this.Controls.Add(this.label12);
            this.Controls.Add(this.textBoxPOP_GasFW);
            this.Controls.Add(this.textBoxPOP_WaterFw);
            this.Name = "MeterUploadPOP";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "MeterUpload Details";
            this.Load += new System.EventHandler(this.MeterUploadPOP_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ComboBox comboBoxPOP_StatusCode;
        private System.Windows.Forms.Label label15;
        private System.Windows.Forms.Label label14;
        private System.Windows.Forms.ComboBox comboBoxPOP_MeterTypeCode;
        private System.Windows.Forms.Label label13;
        private System.Windows.Forms.Label label12;
        private System.Windows.Forms.TextBox textBoxPOP_GasFW;
        private System.Windows.Forms.TextBox textBoxPOP_WaterFw;
        private System.Windows.Forms.Button buttonPOP_cancel;
        private System.Windows.Forms.Button buttonPOP_Done;
        private System.Windows.Forms.Label label1;
    }
}