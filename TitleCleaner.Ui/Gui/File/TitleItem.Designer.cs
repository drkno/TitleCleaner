namespace TitleCleaner.Ui
{
    partial class TitleItem
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

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.tableLayoutPanelMain = new System.Windows.Forms.TableLayoutPanel();
            this.checkBox = new System.Windows.Forms.CheckBox();
            this.tableLayoutPanel2 = new System.Windows.Forms.TableLayoutPanel();
            this.labelRenamed = new System.Windows.Forms.Label();
            this.labelType = new System.Windows.Forms.Label();
            this.labelIndex = new System.Windows.Forms.Label();
            this.labelOrigional = new System.Windows.Forms.Label();
            this.tableLayoutPanelMain.SuspendLayout();
            this.tableLayoutPanel2.SuspendLayout();
            this.SuspendLayout();
            // 
            // tableLayoutPanelMain
            // 
            this.tableLayoutPanelMain.ColumnCount = 2;
            this.tableLayoutPanelMain.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 5.522683F));
            this.tableLayoutPanelMain.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 94.47732F));
            this.tableLayoutPanelMain.Controls.Add(this.checkBox, 0, 0);
            this.tableLayoutPanelMain.Controls.Add(this.tableLayoutPanel2, 1, 0);
            this.tableLayoutPanelMain.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanelMain.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanelMain.Margin = new System.Windows.Forms.Padding(0);
            this.tableLayoutPanelMain.Name = "tableLayoutPanelMain";
            this.tableLayoutPanelMain.RowCount = 1;
            this.tableLayoutPanelMain.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanelMain.Size = new System.Drawing.Size(507, 43);
            this.tableLayoutPanelMain.TabIndex = 0;
            // 
            // checkBox
            // 
            this.checkBox.AutoSize = true;
            this.checkBox.BackColor = System.Drawing.Color.Gainsboro;
            this.checkBox.CheckAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.checkBox.Dock = System.Windows.Forms.DockStyle.Fill;
            this.checkBox.Location = new System.Drawing.Point(0, 1);
            this.checkBox.Margin = new System.Windows.Forms.Padding(0, 1, 0, 1);
            this.checkBox.Name = "checkBox";
            this.checkBox.Size = new System.Drawing.Size(28, 41);
            this.checkBox.TabIndex = 0;
            this.checkBox.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.checkBox.UseVisualStyleBackColor = false;
            // 
            // tableLayoutPanel2
            // 
            this.tableLayoutPanel2.ColumnCount = 2;
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 18.90756F));
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 81.09244F));
            this.tableLayoutPanel2.Controls.Add(this.labelRenamed, 1, 1);
            this.tableLayoutPanel2.Controls.Add(this.labelType, 0, 1);
            this.tableLayoutPanel2.Controls.Add(this.labelIndex, 0, 0);
            this.tableLayoutPanel2.Controls.Add(this.labelOrigional, 1, 0);
            this.tableLayoutPanel2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel2.Location = new System.Drawing.Point(28, 1);
            this.tableLayoutPanel2.Margin = new System.Windows.Forms.Padding(0, 1, 0, 1);
            this.tableLayoutPanel2.Name = "tableLayoutPanel2";
            this.tableLayoutPanel2.RowCount = 2;
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel2.Size = new System.Drawing.Size(479, 41);
            this.tableLayoutPanel2.TabIndex = 1;
            // 
            // labelRenamed
            // 
            this.labelRenamed.AutoSize = true;
            this.labelRenamed.BackColor = System.Drawing.Color.WhiteSmoke;
            this.labelRenamed.Dock = System.Windows.Forms.DockStyle.Fill;
            this.labelRenamed.Font = new System.Drawing.Font("Arial", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelRenamed.Location = new System.Drawing.Point(90, 20);
            this.labelRenamed.Margin = new System.Windows.Forms.Padding(0);
            this.labelRenamed.Name = "labelRenamed";
            this.labelRenamed.Size = new System.Drawing.Size(389, 21);
            this.labelRenamed.TabIndex = 3;
            this.labelRenamed.Text = "[renamed]";
            this.labelRenamed.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // labelType
            // 
            this.labelType.AutoSize = true;
            this.labelType.BackColor = System.Drawing.Color.Gainsboro;
            this.labelType.Dock = System.Windows.Forms.DockStyle.Fill;
            this.labelType.Font = new System.Drawing.Font("Arial", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelType.Location = new System.Drawing.Point(0, 20);
            this.labelType.Margin = new System.Windows.Forms.Padding(0);
            this.labelType.Name = "labelType";
            this.labelType.Size = new System.Drawing.Size(90, 21);
            this.labelType.TabIndex = 2;
            this.labelType.Text = "[type]";
            this.labelType.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // labelIndex
            // 
            this.labelIndex.AutoSize = true;
            this.labelIndex.BackColor = System.Drawing.Color.Gainsboro;
            this.labelIndex.Dock = System.Windows.Forms.DockStyle.Fill;
            this.labelIndex.Font = new System.Drawing.Font("Arial", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelIndex.Location = new System.Drawing.Point(0, 0);
            this.labelIndex.Margin = new System.Windows.Forms.Padding(0);
            this.labelIndex.Name = "labelIndex";
            this.labelIndex.Size = new System.Drawing.Size(90, 20);
            this.labelIndex.TabIndex = 1;
            this.labelIndex.Text = "0";
            this.labelIndex.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // labelOrigional
            // 
            this.labelOrigional.AutoSize = true;
            this.labelOrigional.BackColor = System.Drawing.Color.WhiteSmoke;
            this.labelOrigional.Dock = System.Windows.Forms.DockStyle.Fill;
            this.labelOrigional.Font = new System.Drawing.Font("Arial", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelOrigional.Location = new System.Drawing.Point(90, 0);
            this.labelOrigional.Margin = new System.Windows.Forms.Padding(0);
            this.labelOrigional.Name = "labelOrigional";
            this.labelOrigional.Size = new System.Drawing.Size(389, 20);
            this.labelOrigional.TabIndex = 0;
            this.labelOrigional.Text = "[origional]";
            this.labelOrigional.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // TitleItem
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.White;
            this.Controls.Add(this.tableLayoutPanelMain);
            this.Margin = new System.Windows.Forms.Padding(0);
            this.Name = "TitleItem";
            this.Size = new System.Drawing.Size(507, 43);
            this.tableLayoutPanelMain.ResumeLayout(false);
            this.tableLayoutPanelMain.PerformLayout();
            this.tableLayoutPanel2.ResumeLayout(false);
            this.tableLayoutPanel2.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TableLayoutPanel tableLayoutPanelMain;
        private System.Windows.Forms.CheckBox checkBox;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel2;
        private System.Windows.Forms.Label labelRenamed;
        private System.Windows.Forms.Label labelType;
        private System.Windows.Forms.Label labelIndex;
        private System.Windows.Forms.Label labelOrigional;
    }
}
