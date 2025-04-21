using System.Windows.Forms;

namespace InputDialogLibWindows
{
    public class TextInputForm : Form
    {
        public string UserInput { get; private set; } = "";

        public TextInputForm()
        {
            var textBox = new TextBox { Dock = DockStyle.Fill, Text = "192.168."};
            var button = new Button { Text = "OK", Dock = DockStyle.Bottom };

            button.Click += (s, e) =>
            {
                UserInput = textBox.Text;
                Close();
            };

            Controls.Add(textBox);
            Controls.Add(button);

            Text = "Enter IP address";
            Width = 300;
            Height = 100;
            StartPosition = FormStartPosition.CenterScreen;
            TopMost = true;
        }
    }
}