using Modeller.CustomControls;

namespace Modeller.WindowsApplication
{
    partial class ModellerWindow
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
            this._trafficFlowDensity = new System.Windows.Forms.TextBox();
            this._trafficFlowSpeed = new System.Windows.Forms.TextBox();
            this._endTrafficFlow = new System.Windows.Forms.Button();
            this._startTrafficFlow = new System.Windows.Forms.Button();
            this._currentMapElementDownToLeftTurn = new Modeller.CustomControls.Turn();
            this._currentMapElementRightToDownTurn = new Modeller.CustomControls.Turn();
            this._currentMapElementUpToRightTurn = new Modeller.CustomControls.Turn();
            this._currentMapElementLeftToUpTurn = new Modeller.CustomControls.Turn();
            this._currentMapElementHorizontalRoad = new Modeller.CustomControls.Road();
            this._currentMapElementVerticalRoad = new Modeller.CustomControls.Road();
            this._currentMapElementCrossroad = new Modeller.CustomControls.Crossroad();
            this._workField = new System.Windows.Forms.Panel();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this._lblTrafficFlowSpeed = new System.Windows.Forms.Label();
            this._lblTrafficFlowDensity = new System.Windows.Forms.Label();
            this._btnStep = new System.Windows.Forms.Button();
            this._tbxLog = new System.Windows.Forms.TextBox();
            this._tcSimulator = new System.Windows.Forms.TabControl();
            this._tpSimulation = new System.Windows.Forms.TabPage();
            this._btnEndSim = new System.Windows.Forms.Button();
            this._btnStartSim = new System.Windows.Forms.Button();
            this._tpLog = new System.Windows.Forms.TabPage();
            this._msMenu = new System.Windows.Forms.MenuStrip();
            this.fileToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this._tsmiSaveConfig = new System.Windows.Forms.ToolStripMenuItem();
            this._tsmiOpenConfig = new System.Windows.Forms.ToolStripMenuItem();
            this.groupBox1.SuspendLayout();
            this._tcSimulator.SuspendLayout();
            this._tpSimulation.SuspendLayout();
            this._tpLog.SuspendLayout();
            this._msMenu.SuspendLayout();
            this.SuspendLayout();
            // 
            // _trafficFlowDensity
            // 
            this._trafficFlowDensity.Location = new System.Drawing.Point(578, 254);
            this._trafficFlowDensity.Name = "_trafficFlowDensity";
            this._trafficFlowDensity.Size = new System.Drawing.Size(60, 20);
            this._trafficFlowDensity.TabIndex = 11;
            // 
            // _trafficFlowSpeed
            // 
            this._trafficFlowSpeed.Location = new System.Drawing.Point(578, 225);
            this._trafficFlowSpeed.Name = "_trafficFlowSpeed";
            this._trafficFlowSpeed.Size = new System.Drawing.Size(60, 20);
            this._trafficFlowSpeed.TabIndex = 10;
            // 
            // _endTrafficFlow
            // 
            this._endTrafficFlow.Location = new System.Drawing.Point(524, 186);
            this._endTrafficFlow.Name = "_endTrafficFlow";
            this._endTrafficFlow.Size = new System.Drawing.Size(114, 23);
            this._endTrafficFlow.TabIndex = 9;
            this._endTrafficFlow.Text = "End traffic flow";
            this._endTrafficFlow.UseVisualStyleBackColor = true;
            this._endTrafficFlow.Click += new System.EventHandler(this._endTrafficFlow_Click);
            // 
            // _startTrafficFlow
            // 
            this._startTrafficFlow.Location = new System.Drawing.Point(524, 157);
            this._startTrafficFlow.Name = "_startTrafficFlow";
            this._startTrafficFlow.Size = new System.Drawing.Size(114, 23);
            this._startTrafficFlow.TabIndex = 8;
            this._startTrafficFlow.Text = "Start traffic flow";
            this._startTrafficFlow.UseVisualStyleBackColor = true;
            this._startTrafficFlow.Click += new System.EventHandler(this._startTrafficFlow_Click);
            // 
            // _currentMapElementDownToLeftTurn
            // 
            this._currentMapElementDownToLeftTurn.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this._currentMapElementDownToLeftTurn.Location = new System.Drawing.Point(179, 74);
            this._currentMapElementDownToLeftTurn.Name = "_currentMapElementDownToLeftTurn";
            this._currentMapElementDownToLeftTurn.Size = new System.Drawing.Size(50, 50);
            this._currentMapElementDownToLeftTurn.TabIndex = 7;
            this._currentMapElementDownToLeftTurn.Type = Modeller.CustomControls.TurnType.DownToLeft;
            this._currentMapElementDownToLeftTurn.MouseClick += new System.Windows.Forms.MouseEventHandler(this._currentMapElementDownToLeftTurn_MouseClick);
            // 
            // _currentMapElementRightToDownTurn
            // 
            this._currentMapElementRightToDownTurn.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this._currentMapElementRightToDownTurn.Location = new System.Drawing.Point(123, 74);
            this._currentMapElementRightToDownTurn.Name = "_currentMapElementRightToDownTurn";
            this._currentMapElementRightToDownTurn.Size = new System.Drawing.Size(50, 50);
            this._currentMapElementRightToDownTurn.TabIndex = 6;
            this._currentMapElementRightToDownTurn.Type = Modeller.CustomControls.TurnType.RightToDown;
            this._currentMapElementRightToDownTurn.MouseClick += new System.Windows.Forms.MouseEventHandler(this._currentMapElementRightToDownTurn_MouseClick);
            // 
            // _currentMapElementUpToRightTurn
            // 
            this._currentMapElementUpToRightTurn.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this._currentMapElementUpToRightTurn.Location = new System.Drawing.Point(67, 74);
            this._currentMapElementUpToRightTurn.Name = "_currentMapElementUpToRightTurn";
            this._currentMapElementUpToRightTurn.Size = new System.Drawing.Size(50, 50);
            this._currentMapElementUpToRightTurn.TabIndex = 5;
            this._currentMapElementUpToRightTurn.Type = Modeller.CustomControls.TurnType.UpToRight;
            this._currentMapElementUpToRightTurn.MouseClick += new System.Windows.Forms.MouseEventHandler(this._currentMapElementUpToRightTurn_MouseClick);
            // 
            // _currentMapElementLeftToUpTurn
            // 
            this._currentMapElementLeftToUpTurn.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this._currentMapElementLeftToUpTurn.Location = new System.Drawing.Point(11, 74);
            this._currentMapElementLeftToUpTurn.Name = "_currentMapElementLeftToUpTurn";
            this._currentMapElementLeftToUpTurn.Size = new System.Drawing.Size(50, 50);
            this._currentMapElementLeftToUpTurn.TabIndex = 4;
            this._currentMapElementLeftToUpTurn.Type = Modeller.CustomControls.TurnType.LeftToUp;
            this._currentMapElementLeftToUpTurn.MouseClick += new System.Windows.Forms.MouseEventHandler(this._currentMapElementLeftToUpTurn_MouseClick);
            // 
            // _currentMapElementHorizontalRoad
            // 
            this._currentMapElementHorizontalRoad.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this._currentMapElementHorizontalRoad.Location = new System.Drawing.Point(67, 18);
            this._currentMapElementHorizontalRoad.Name = "_currentMapElementHorizontalRoad";
            this._currentMapElementHorizontalRoad.Size = new System.Drawing.Size(50, 50);
            this._currentMapElementHorizontalRoad.TabIndex = 3;
            this._currentMapElementHorizontalRoad.Type = Modeller.CustomControls.RoadType.Horizontal;
            this._currentMapElementHorizontalRoad.MouseClick += new System.Windows.Forms.MouseEventHandler(this._currentMapElementHorizontalRoad_MouseClick);
            // 
            // _currentMapElementVerticalRoad
            // 
            this._currentMapElementVerticalRoad.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this._currentMapElementVerticalRoad.Location = new System.Drawing.Point(123, 18);
            this._currentMapElementVerticalRoad.Name = "_currentMapElementVerticalRoad";
            this._currentMapElementVerticalRoad.Size = new System.Drawing.Size(50, 50);
            this._currentMapElementVerticalRoad.TabIndex = 2;
            this._currentMapElementVerticalRoad.Type = Modeller.CustomControls.RoadType.Vertical;
            this._currentMapElementVerticalRoad.MouseClick += new System.Windows.Forms.MouseEventHandler(this._currentMapElementVerticalRoad_MouseClick);
            // 
            // _currentMapElementCrossroad
            // 
            this._currentMapElementCrossroad.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this._currentMapElementCrossroad.Location = new System.Drawing.Point(11, 18);
            this._currentMapElementCrossroad.Name = "_currentMapElementCrossroad";
            this._currentMapElementCrossroad.Size = new System.Drawing.Size(50, 50);
            this._currentMapElementCrossroad.TabIndex = 1;
            this._currentMapElementCrossroad.MouseClick += new System.Windows.Forms.MouseEventHandler(this._currentMapElementCrossroad_MouseClick);
            // 
            // _workField
            // 
            this._workField.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this._workField.Location = new System.Drawing.Point(6, 6);
            this._workField.Name = "_workField";
            this._workField.Size = new System.Drawing.Size(509, 509);
            this._workField.TabIndex = 0;
            this._workField.MouseClick += new System.Windows.Forms.MouseEventHandler(this._workField_MouseClick);
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this._currentMapElementRightToDownTurn);
            this.groupBox1.Controls.Add(this._currentMapElementLeftToUpTurn);
            this.groupBox1.Controls.Add(this._currentMapElementCrossroad);
            this.groupBox1.Controls.Add(this._currentMapElementHorizontalRoad);
            this.groupBox1.Controls.Add(this._currentMapElementVerticalRoad);
            this.groupBox1.Controls.Add(this._currentMapElementUpToRightTurn);
            this.groupBox1.Controls.Add(this._currentMapElementDownToLeftTurn);
            this.groupBox1.Location = new System.Drawing.Point(521, 6);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(240, 133);
            this.groupBox1.TabIndex = 12;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Map configuration";
            // 
            // _lblTrafficFlowSpeed
            // 
            this._lblTrafficFlowSpeed.AutoSize = true;
            this._lblTrafficFlowSpeed.Location = new System.Drawing.Point(527, 228);
            this._lblTrafficFlowSpeed.Name = "_lblTrafficFlowSpeed";
            this._lblTrafficFlowSpeed.Size = new System.Drawing.Size(41, 13);
            this._lblTrafficFlowSpeed.TabIndex = 13;
            this._lblTrafficFlowSpeed.Text = "Speed:";
            // 
            // _lblTrafficFlowDensity
            // 
            this._lblTrafficFlowDensity.AutoSize = true;
            this._lblTrafficFlowDensity.Location = new System.Drawing.Point(527, 257);
            this._lblTrafficFlowDensity.Name = "_lblTrafficFlowDensity";
            this._lblTrafficFlowDensity.Size = new System.Drawing.Size(45, 13);
            this._lblTrafficFlowDensity.TabIndex = 14;
            this._lblTrafficFlowDensity.Text = "Density:";
            // 
            // _btnStep
            // 
            this._btnStep.Location = new System.Drawing.Point(644, 418);
            this._btnStep.Name = "_btnStep";
            this._btnStep.Size = new System.Drawing.Size(101, 54);
            this._btnStep.TabIndex = 15;
            this._btnStep.Text = "Step";
            this._btnStep.UseVisualStyleBackColor = true;
            this._btnStep.Click += new System.EventHandler(this._btnStep_Click);
            // 
            // _tbxLog
            // 
            this._tbxLog.Font = new System.Drawing.Font("Courier New", 8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this._tbxLog.Location = new System.Drawing.Point(6, 6);
            this._tbxLog.Multiline = true;
            this._tbxLog.Name = "_tbxLog";
            this._tbxLog.ReadOnly = true;
            this._tbxLog.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this._tbxLog.Size = new System.Drawing.Size(900, 512);
            this._tbxLog.TabIndex = 16;
            // 
            // _tcSimulator
            // 
            this._tcSimulator.Controls.Add(this._tpSimulation);
            this._tcSimulator.Controls.Add(this._tpLog);
            this._tcSimulator.Location = new System.Drawing.Point(12, 29);
            this._tcSimulator.Name = "_tcSimulator";
            this._tcSimulator.SelectedIndex = 0;
            this._tcSimulator.Size = new System.Drawing.Size(920, 550);
            this._tcSimulator.TabIndex = 17;
            // 
            // _tpSimulation
            // 
            this._tpSimulation.Controls.Add(this._btnEndSim);
            this._tpSimulation.Controls.Add(this._btnStartSim);
            this._tpSimulation.Controls.Add(this.groupBox1);
            this._tpSimulation.Controls.Add(this._workField);
            this._tpSimulation.Controls.Add(this._btnStep);
            this._tpSimulation.Controls.Add(this._lblTrafficFlowDensity);
            this._tpSimulation.Controls.Add(this._trafficFlowSpeed);
            this._tpSimulation.Controls.Add(this._lblTrafficFlowSpeed);
            this._tpSimulation.Controls.Add(this._trafficFlowDensity);
            this._tpSimulation.Controls.Add(this._endTrafficFlow);
            this._tpSimulation.Controls.Add(this._startTrafficFlow);
            this._tpSimulation.Location = new System.Drawing.Point(4, 22);
            this._tpSimulation.Name = "_tpSimulation";
            this._tpSimulation.Padding = new System.Windows.Forms.Padding(3);
            this._tpSimulation.Size = new System.Drawing.Size(912, 524);
            this._tpSimulation.TabIndex = 0;
            this._tpSimulation.Text = "Working field";
            this._tpSimulation.UseVisualStyleBackColor = true;
            // 
            // _btnEndSim
            // 
            this._btnEndSim.Location = new System.Drawing.Point(699, 338);
            this._btnEndSim.Name = "_btnEndSim";
            this._btnEndSim.Size = new System.Drawing.Size(116, 62);
            this._btnEndSim.TabIndex = 17;
            this._btnEndSim.Text = "End simulation";
            this._btnEndSim.UseVisualStyleBackColor = true;
            this._btnEndSim.Click += new System.EventHandler(this._btnEndSim_Click);
            // 
            // _btnStartSim
            // 
            this._btnStartSim.Location = new System.Drawing.Point(578, 338);
            this._btnStartSim.Name = "_btnStartSim";
            this._btnStartSim.Size = new System.Drawing.Size(115, 62);
            this._btnStartSim.TabIndex = 16;
            this._btnStartSim.Text = "Start simulation";
            this._btnStartSim.UseVisualStyleBackColor = true;
            this._btnStartSim.Click += new System.EventHandler(this._btnStartSim_Click);
            // 
            // _tpLog
            // 
            this._tpLog.Controls.Add(this._tbxLog);
            this._tpLog.Location = new System.Drawing.Point(4, 22);
            this._tpLog.Name = "_tpLog";
            this._tpLog.Padding = new System.Windows.Forms.Padding(3);
            this._tpLog.Size = new System.Drawing.Size(912, 524);
            this._tpLog.TabIndex = 1;
            this._tpLog.Text = "Logging field";
            this._tpLog.UseVisualStyleBackColor = true;
            // 
            // _msMenu
            // 
            this._msMenu.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.fileToolStripMenuItem});
            this._msMenu.Location = new System.Drawing.Point(0, 0);
            this._msMenu.Name = "_msMenu";
            this._msMenu.Size = new System.Drawing.Size(944, 24);
            this._msMenu.TabIndex = 18;
            this._msMenu.Text = "menuStrip1";
            // 
            // fileToolStripMenuItem
            // 
            this.fileToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this._tsmiSaveConfig,
            this._tsmiOpenConfig});
            this.fileToolStripMenuItem.Name = "fileToolStripMenuItem";
            this.fileToolStripMenuItem.Size = new System.Drawing.Size(37, 20);
            this.fileToolStripMenuItem.Text = "File";
            // 
            // _tsmiSaveConfig
            // 
            this._tsmiSaveConfig.Name = "_tsmiSaveConfig";
            this._tsmiSaveConfig.Size = new System.Drawing.Size(140, 22);
            this._tsmiSaveConfig.Text = "Save config";
            this._tsmiSaveConfig.Click += new System.EventHandler(this._tsmiSaveConfig_Click);
            // 
            // _tsmiOpenConfig
            // 
            this._tsmiOpenConfig.Name = "_tsmiOpenConfig";
            this._tsmiOpenConfig.Size = new System.Drawing.Size(140, 22);
            this._tsmiOpenConfig.Text = "Open config";
            this._tsmiOpenConfig.Click += new System.EventHandler(this._tsmiOpenConfig_Click);
            // 
            // ModellerWindow
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(944, 593);
            this.Controls.Add(this._tcSimulator);
            this.Controls.Add(this._msMenu);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MainMenuStrip = this._msMenu;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "ModellerWindow";
            this.Text = "Crossroad modeller";
            this.Load += new System.EventHandler(this.Modeller_Load);
            this.groupBox1.ResumeLayout(false);
            this._tcSimulator.ResumeLayout(false);
            this._tpSimulation.ResumeLayout(false);
            this._tpSimulation.PerformLayout();
            this._tpLog.ResumeLayout(false);
            this._tpLog.PerformLayout();
            this._msMenu.ResumeLayout(false);
            this._msMenu.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Panel _workField;
        private Road _currentMapElementHorizontalRoad;
        private Road _currentMapElementVerticalRoad;
        private Crossroad _currentMapElementCrossroad;
        private Turn _currentMapElementDownToLeftTurn;
        private Turn _currentMapElementRightToDownTurn;
        private Turn _currentMapElementUpToRightTurn;
        private Turn _currentMapElementLeftToUpTurn;
        private System.Windows.Forms.TextBox _trafficFlowDensity;
        private System.Windows.Forms.TextBox _trafficFlowSpeed;
        private System.Windows.Forms.Button _endTrafficFlow;
        private System.Windows.Forms.Button _startTrafficFlow;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Label _lblTrafficFlowSpeed;
        private System.Windows.Forms.Label _lblTrafficFlowDensity;
        private System.Windows.Forms.Button _btnStep;
        private System.Windows.Forms.TextBox _tbxLog;
        private System.Windows.Forms.TabControl _tcSimulator;
        private System.Windows.Forms.TabPage _tpSimulation;
        private System.Windows.Forms.TabPage _tpLog;
        private System.Windows.Forms.MenuStrip _msMenu;
        private System.Windows.Forms.ToolStripMenuItem fileToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem _tsmiSaveConfig;
        private System.Windows.Forms.ToolStripMenuItem _tsmiOpenConfig;
        private System.Windows.Forms.Button _btnEndSim;
        private System.Windows.Forms.Button _btnStartSim;
    }
}

