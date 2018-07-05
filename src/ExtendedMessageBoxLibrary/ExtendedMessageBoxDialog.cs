using System;
using System.Drawing;
using System.Windows.Forms;

namespace ExtendedMessageBoxLibrary {
	public partial class ExtendedMessageBoxDialog : Form {
		private bool _checkboxexisting;
		private ExtendedMessageBox _msgBoxOwner;

		public ExtendedMessageBoxDialog() {
			InitializeComponent();
		}

		public void ExtendedMessageBoxDialog_ResizeEnd(object sender, EventArgs e) { }

		private void ExtendedMessageBoxDialog_SizeChanged(object sender, EventArgs e) {
			checkableBox.Location =
				new Point(checkableBox.Location.X, 9 + MainText_tb.Size.Height + 3 + MainText_tb.Location.Y);
			int btnY = checkableBox.Location.Y;
			if (_checkboxexisting) {
				btnY = btnY + checkableBox.Size.Height + 6;
			}

			double btnsize =
				(double) (Size.Width - 12 - 16 - 6) / ((ExtendedMessageBox) Tag).ExMsgBoxcfg.NumberOfBbuttons - 6;
			double currentleftallign = 12;

			foreach (Button i in MsgBoxDialogbtns) {
				i.Size = new Size((int) Math.Round(btnsize), Height - 51 - btnY);
				i.Location = new Point((int) Math.Round(currentleftallign), btnY);
				currentleftallign = 6 + btnsize + currentleftallign;
			}
		}

		private void ExtendedMessageBoxDialog_Load(object sender, EventArgs e) {
			_msgBoxOwner = (ExtendedMessageBox) Tag;
			_checkboxexisting = _msgBoxOwner.ExMsgBoxcfg.ShowCheckBox;
			Text = _msgBoxOwner.ExMsgBoxcfg.Title;
			MainText_tb.Text = _msgBoxOwner.ExMsgBoxcfg.Text;
			MsgBoxDialogbtns = new Button[_msgBoxOwner.ExMsgBoxcfg.NumberOfBbuttons];
			for (int i = 0; i < MsgBoxDialogbtns.Length; i++) {
				MsgBoxDialogbtns[i] = new Button {
					Location = new Point(12, 48),
					Name = $"button{i}",
					Size = new Size(75, 23),
					TabIndex = 2,
					Text = _msgBoxOwner.ExMsgBoxcfg.Buttons[i],
					UseVisualStyleBackColor = true,
					Tag = i
				};
				MsgBoxDialogbtns[i].Click += ExtendedMessageBoxDialogbtn_click;
				MsgBoxDialogbtns[i].Anchor = AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Top;
				Controls.Add(MsgBoxDialogbtns[i]);
			}

			if (_msgBoxOwner.ExMsgBoxcfg.DefaultButton != -1 &&
			    _msgBoxOwner.ExMsgBoxcfg.DefaultButton < MsgBoxDialogbtns.Length) {
				MsgBoxDialogbtns[_msgBoxOwner.ExMsgBoxcfg.DefaultButton].TabIndex = 1;
			}

			ExtendedMessageBoxDialog_SizeChanged(null, null);

			//   this.Size = new Size(this.Size.Width,MsgBoxDialogbtns[0].Location.Y+MsgBoxDialogbtns[0].Size.Height+51);
			if (_msgBoxOwner.ExMsgBoxcfg.ShowCheckBox) {
				checkableBox.Text = "Auswahl merken";
			}
			else {
				Controls.Remove(checkableBox);
				checkableBox.Visible = false;
			}
		}

		private void ExtendedMessageBoxDialogbtn_click(object sender, EventArgs e) {
			_msgBoxOwner.ToBeReturned =
				new ExtendedMessageBoxResult(_msgBoxOwner.ExMsgBoxcfg, (int) ((Button) sender).Tag);

			Close();
		}

		private void ExtendedMessageBoxDialog_FormClosed(object sender, FormClosedEventArgs e) { }
	}
}