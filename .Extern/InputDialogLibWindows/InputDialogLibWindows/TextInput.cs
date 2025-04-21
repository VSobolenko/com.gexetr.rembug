using System.Threading;
using System.Windows.Forms;

namespace InputDialogLibWindows
{
    public static class TextInput
    {
        public static string ShowInputWindow()
        {
            string result = null;

            Thread t = new Thread(() =>
            {
                Application.EnableVisualStyles();
                var form = new TextInputForm();

                form.FormClosed += (s, e) =>
                {
                    result = form.UserInput;
                };

                form.ShowDialog();
            });

            t.SetApartmentState(ApartmentState.STA);
            t.Start();
            t.Join();

            return result;
        }
    }
}