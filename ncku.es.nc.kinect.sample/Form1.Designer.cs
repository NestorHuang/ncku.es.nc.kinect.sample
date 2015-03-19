namespace ncku.es.nc.kinect.sample
{
    partial class Form1
    {
        /// <summary>
        /// 設計工具所需的變數。
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// 清除任何使用中的資源。
        /// </summary>
        /// <param name="disposing">如果應該處置 Managed 資源則為 true，否則為 false。</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form 設計工具產生的程式碼

        /// <summary>
        /// 此為設計工具支援所需的方法 - 請勿使用程式碼編輯器
        /// 修改這個方法的內容。
        /// </summary>
        private void InitializeComponent()
        {
            this.btnConnectedKinects = new System.Windows.Forms.Button();
            this.btnAngleChange = new System.Windows.Forms.Button();
            this.lblAngle = new System.Windows.Forms.Label();
            this.txtAngle = new System.Windows.Forms.TextBox();
            this.SuspendLayout();
            // 
            // btnConnectedKinects
            // 
            this.btnConnectedKinects.Location = new System.Drawing.Point(12, 12);
            this.btnConnectedKinects.Name = "btnConnectedKinects";
            this.btnConnectedKinects.Size = new System.Drawing.Size(133, 23);
            this.btnConnectedKinects.TabIndex = 0;
            this.btnConnectedKinects.Text = "連線所有Kinect";
            this.btnConnectedKinects.UseVisualStyleBackColor = true;
            this.btnConnectedKinects.Click += new System.EventHandler(this.btnConnectedKinects_Click);
            // 
            // btnAngleChange
            // 
            this.btnAngleChange.Location = new System.Drawing.Point(127, 51);
            this.btnAngleChange.Name = "btnAngleChange";
            this.btnAngleChange.Size = new System.Drawing.Size(75, 23);
            this.btnAngleChange.TabIndex = 1;
            this.btnAngleChange.Text = "變更角度";
            this.btnAngleChange.UseVisualStyleBackColor = true;
            this.btnAngleChange.Click += new System.EventHandler(this.btnAngleChange_Click);
            // 
            // lblAngle
            // 
            this.lblAngle.AutoSize = true;
            this.lblAngle.Location = new System.Drawing.Point(10, 54);
            this.lblAngle.Name = "lblAngle";
            this.lblAngle.Size = new System.Drawing.Size(41, 12);
            this.lblAngle.TabIndex = 2;
            this.lblAngle.Text = "角度：";
            // 
            // txtAngle
            // 
            this.txtAngle.Location = new System.Drawing.Point(57, 51);
            this.txtAngle.Name = "txtAngle";
            this.txtAngle.Size = new System.Drawing.Size(64, 22);
            this.txtAngle.TabIndex = 3;
            this.txtAngle.Text = "0";
            this.txtAngle.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(778, 261);
            this.Controls.Add(this.txtAngle);
            this.Controls.Add(this.lblAngle);
            this.Controls.Add(this.btnAngleChange);
            this.Controls.Add(this.btnConnectedKinects);
            this.Name = "Form1";
            this.Text = "Form1";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button btnConnectedKinects;
        private System.Windows.Forms.Button btnAngleChange;
        private System.Windows.Forms.Label lblAngle;
        private System.Windows.Forms.TextBox txtAngle;
    }
}

