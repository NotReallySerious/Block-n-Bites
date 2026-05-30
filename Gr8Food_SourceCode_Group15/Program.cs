namespace Gr8Food_SourceCode_Group15
{
    internal static class Program
    {
        /// <summary>
        ///  The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            // To customize application configuration such as set high DPI settings or default font,
            // see https://aka.ms/applicationconfiguration.
            ApplicationConfiguration.Initialize();
            while (true)
            {
                try
                {
                    Database.TestConnection();
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Database connection failed: " + ex.Message + "\nPlease check App.config connection string.", "Database Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
                using var login = new frmLogin();
                var result = login.ShowDialog();
                if (result == System.Windows.Forms.DialogResult.OK)
                {
                    var rawRole = (login.Tag as string) ?? string.Empty;
                    var role = rawRole.Trim();
                    Form main;
                    switch (role?.ToLowerInvariant())
                    {
                        case "admin":
                            main = new Admin();
                            break;
                        case "manager":
                            main = new Manager();
                            break;
                        case "chef":
                            main = new Chef();
                            break;
                        case "customer":
                            main = new Customer();
                            break;
                        default:
                            main = new Profile();
                            break;
                    }

                    Application.Run(main);
                    break; // main closed -> exit app
                }
                else
                {
                    // if login requested registration, open register form
                    if ((login.Tag as string) == "OpenRegister")
                    {
                        using var reg = new frmregister();
                        var r = reg.ShowDialog();
                        // After registration closes, loop back to show login again
                        continue;
                    }
                    // any other result -> exit
                    break;
                }
            }
        }
    }
}