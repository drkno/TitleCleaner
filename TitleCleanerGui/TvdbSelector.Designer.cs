namespace TitleCleanerGui
{
    partial class TvdbSelector
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
            this.tableLayoutPanelSeries = new System.Windows.Forms.TableLayoutPanel();
            this.buttonNone = new System.Windows.Forms.Button();
            this.tableLayoutPanelSeriesSelection = new System.Windows.Forms.TableLayoutPanel();
            this.tableLayoutPanelSeries.SuspendLayout();
            this.SuspendLayout();
            // 
            // tableLayoutPanelSeries
            // 
            this.tableLayoutPanelSeries.ColumnCount = 1;
            this.tableLayoutPanelSeries.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanelSeries.Controls.Add(this.buttonNone, 0, 1);
            this.tableLayoutPanelSeries.Controls.Add(this.tableLayoutPanelSeriesSelection, 0, 0);
            this.tableLayoutPanelSeries.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanelSeries.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanelSeries.Name = "tableLayoutPanelSeries";
            this.tableLayoutPanelSeries.RowCount = 2;
            this.tableLayoutPanelSeries.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanelSeries.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 31F));
            this.tableLayoutPanelSeries.Size = new System.Drawing.Size(531, 348);
            this.tableLayoutPanelSeries.TabIndex = 0;
            // 
            // buttonNone
            // 
            this.buttonNone.Location = new System.Drawing.Point(3, 320);
            this.buttonNone.Name = "buttonNone";
            this.buttonNone.Size = new System.Drawing.Size(108, 23);
            this.buttonNone.TabIndex = 0;
            this.buttonNone.Text = "None of These";
            this.buttonNone.UseVisualStyleBackColor = true;
            this.buttonNone.Click += new System.EventHandler(this.ButtonNoneClick);
            // 
            // tableLayoutPanelSeriesSelection
            // 
            this.tableLayoutPanelSeriesSelection.AutoScroll = true;
            this.tableLayoutPanelSeriesSelection.AutoSize = true;
            this.tableLayoutPanelSeriesSelection.ColumnCount = 1;
            this.tableLayoutPanelSeriesSelection.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanelSeriesSelection.Location = new System.Drawing.Point(3, 3);
            this.tableLayoutPanelSeriesSelection.Name = "tableLayoutPanelSeriesSelection";
            this.tableLayoutPanelSeriesSelection.RowCount = 1;
            this.tableLayoutPanelSeriesSelection.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanelSeriesSelection.Size = new System.Drawing.Size(0, 0);
            this.tableLayoutPanelSeriesSelection.TabIndex = 1;
            // 
            // TvdbSelector
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(531, 348);
            this.Controls.Add(this.tableLayoutPanelSeries);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Name = "TvdbSelector";
            this.ShowIcon = false;
            this.Text = "Which Series";
            this.tableLayoutPanelSeries.ResumeLayout(false);
            this.tableLayoutPanelSeries.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TableLayoutPanel tableLayoutPanelSeries;
        private System.Windows.Forms.Button buttonNone;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanelSeriesSelection;
    }
}