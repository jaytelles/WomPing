namespace WomPing
{
    partial class WomPingForm
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
            this.pingTable = new System.Windows.Forms.ListView();
            this.hostname = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.ipAddress = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.ping = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.SuspendLayout();
            // 
            // pingTable
            // 
            this.pingTable.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.hostname,
            this.ipAddress,
            this.ping});
            this.pingTable.Location = new System.Drawing.Point(-4, -1);
            this.pingTable.Name = "pingTable";
            this.pingTable.Size = new System.Drawing.Size(388, 464);
            this.pingTable.TabIndex = 0;
            this.pingTable.UseCompatibleStateImageBehavior = false;
            // 
            // hostname
            // 
            this.hostname.Text = "Hostname";
            // 
            // ipAddress
            // 
            this.ipAddress.Text = "IP Address";
            // 
            // ping
            // 
            this.ping.Text = "Ping";
            // 
            // WomPingForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(384, 462);
            this.Controls.Add(this.pingTable);
            this.MaximizeBox = false;
            this.Name = "WomPingForm";
            this.Text = "WomPing";
            this.Load += new System.EventHandler(this.Form1_Load);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.ListView pingTable;
        private System.Windows.Forms.ColumnHeader hostname;
        private System.Windows.Forms.ColumnHeader ipAddress;
        private System.Windows.Forms.ColumnHeader ping;
    }
}

