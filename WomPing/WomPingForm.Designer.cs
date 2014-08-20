namespace WomPing
{
    partial class womPingForm
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
            this.delta = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.average = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.SuspendLayout();
            // 
            // pingTable
            // 
            this.pingTable.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.hostname,
            this.ipAddress,
            this.ping,
            this.delta,
            this.average});
            this.pingTable.Location = new System.Drawing.Point(-4, -1);
            this.pingTable.Name = "pingTable";
            this.pingTable.Size = new System.Drawing.Size(495, 565);
            this.pingTable.TabIndex = 0;
            this.pingTable.UseCompatibleStateImageBehavior = false;
            this.pingTable.View = System.Windows.Forms.View.Details;
            // 
            // hostname
            // 
            this.hostname.Text = "Hostname";
            this.hostname.Width = 120;
            // 
            // ipAddress
            // 
            this.ipAddress.Text = "IP Address";
            this.ipAddress.Width = 90;
            // 
            // ping
            // 
            this.ping.Text = "Ping(ms)";
            // 
            // delta
            // 
            this.delta.Text = "Delta(ms)";
            // 
            // average
            // 
            this.average.Text = "Average(ms)";
            this.average.Width = 90;
            // 
            // womPingForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(484, 562);
            this.Controls.Add(this.pingTable);
            this.MaximizeBox = false;
            this.Name = "womPingForm";
            this.Text = "WomPing";
            this.Load += new System.EventHandler(this.Form1_Load);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.ListView pingTable;
        private System.Windows.Forms.ColumnHeader hostname;
        private System.Windows.Forms.ColumnHeader ipAddress;
        private System.Windows.Forms.ColumnHeader ping;
        private System.Windows.Forms.ColumnHeader delta;
        private System.Windows.Forms.ColumnHeader average;
    }
}

