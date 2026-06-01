using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Gr8Food
{
    public partial class Admin : Form
    {
        // ── Sales Report controls (grpSalesReport declared in Admin_Designer.cs) ──────
        private Label lblSortMode;
        private ComboBox cmbSortMode;
        private Label lblSortOrder;
        private ComboBox cmbSortOrder;
        private Label lblFilterValue;
        private TextBox txtFilterValue;
        private Label lblFilterHint;
        private Button btnApplyFilter;
        private Button btnShowAll;
        private Button btnBackToDashboard;
        private DataGridView dgvReport;
        private Label lblReportStatus;

        // ── Connection string ─────────────────────────────────────────────────────────
        private readonly string _connectionString;

        // ── Constructor ───────────────────────────────────────────────────────────────
        public Admin()
        {
            InitializeComponent();

            ConnectionStringSettings cs =
                ConfigurationManager.ConnectionStrings["Gr8FoodConnection"];
            if (cs == null)
                throw new InvalidOperationException(
                    "Connection string 'Gr8FoodConnection' not found in App.config.");
            _connectionString = cs.ConnectionString;

            btnAddRemove.Click += BtnAddRemove_Click;
            this.Load += Admin_Load;

            BuildSalesReportPanel();

            if (btnSalesReport != null)
                btnSalesReport.Click += BtnSalesReport_Click;
        }

        // ── Form Load ─────────────────────────────────────────────────────────────────
        private void Admin_Load(object sender, EventArgs e)
        {
            try
            {
                txtGreetAdmin.Text = !string.IsNullOrEmpty(Session.CurrentUserName)
                    ? "Hello, Admin (" + Session.CurrentUserName + ")"
                    : "Hello Admin";
            }
            catch { }

            if (dgvUsers != null)
                dgvUsers.CellClick += DgvUsers_CellClick;

            if (cmbUserId != null)
            {
                cmbUserId.SelectedIndexChanged += CmbUserId_SelectedIndexChanged;
                try
                {
                    DataTable users = GetUserListFromDb();
                    cmbUserId.DisplayMember = "UserID";
                    cmbUserId.ValueMember = "UserID";
                    cmbUserId.DataSource = users;
                }
                catch { }
            }

            if (cmbRole != null && cmbRole.Items.Count == 0)
                cmbRole.Items.AddRange(new object[] { "Admin", "Customer", "Chef", "Manager" });

            try { LoadUsersIntoGrid(); } catch { }

            if (btnAddUser != null) btnAddUser.Click += BtnAddUser_Click;
            if (btnUpdateUserInner != null) btnUpdateUserInner.Click += BtnUpdateUserInner_Click;
            if (btnDeleteUser != null) btnDeleteUser.Click += BtnDeleteUser_Click;
        }

        // ══════════════════════════════════════════════════════════════════════════════
        // USER MANAGEMENT
        // ══════════════════════════════════════════════════════════════════════════════

        private void BtnAddRemove_Click(object sender, EventArgs e)
        {
            LoadUsersIntoGrid();
        }

        private void LoadUsersIntoGrid()
        {
            try
            {
                DataTable dt = GetAllUsersFromDb();
                if (dgvUsers != null) dgvUsers.DataSource = dt;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Failed to load users: " + ex.Message,
                    "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void DgvUsers_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (dgvUsers == null) return;
            if (e.RowIndex < 0 || e.RowIndex >= dgvUsers.Rows.Count) return;

            DataGridViewRow row = dgvUsers.Rows[e.RowIndex];

            string idStr = row.Cells["UserID"].Value != null
                ? row.Cells["UserID"].Value.ToString() : string.Empty;

            int id;
            if (int.TryParse(idStr, out id) && cmbUserId != null)
            {
                for (int i = 0; i < cmbUserId.Items.Count; i++)
                {
                    DataRowView drv = cmbUserId.Items[i] as DataRowView;
                    if (drv != null && Convert.ToInt32(drv["UserID"]) == id)
                    {
                        cmbUserId.SelectedIndex = i;
                        break;
                    }
                }
            }

            txtNameInput.Text = row.Cells["Name"].Value != null
                ? row.Cells["Name"].Value.ToString() : string.Empty;
            txtEmailInput.Text = row.Cells["Email"].Value != null
                ? row.Cells["Email"].Value.ToString() : string.Empty;
            txtBBCInput.Text = row.Cells["BBCBalance"].Value != null
                ? row.Cells["BBCBalance"].Value.ToString() : "0";
            txtPasswordInput.Text = row.Cells["Password"].Value != null
                ? row.Cells["Password"].Value.ToString() : string.Empty;

            string roleFromRow = row.Cells["Role"].Value != null
                ? row.Cells["Role"].Value.ToString() : string.Empty;
            if (cmbRole != null)
            {
                for (int r = 0; r < cmbRole.Items.Count; r++)
                {
                    string itemText = cmbRole.Items[r] != null
                        ? cmbRole.Items[r].ToString() : string.Empty;
                    if (string.Equals(itemText, roleFromRow, StringComparison.OrdinalIgnoreCase))
                    {
                        cmbRole.SelectedIndex = r;
                        break;
                    }
                }
            }
        }

        private void CmbUserId_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                if (cmbUserId == null || cmbUserId.SelectedItem == null) return;
                DataRowView drv = cmbUserId.SelectedItem as DataRowView;
                if (drv == null) return;

                int id = Convert.ToInt32(drv["UserID"]);
                DataRow row = GetUserByIdFromDb(id);
                if (row == null) return;

                txtNameInput.Text = row["Name"] != null
                    ? row["Name"].ToString() : string.Empty;
                txtEmailInput.Text = row["Email"] != null
                    ? row["Email"].ToString() : string.Empty;
                txtPasswordInput.Text = row["Password"] != null
                    ? row["Password"].ToString() : string.Empty;
                txtBBCInput.Text = row["BBCBalance"] != null
                    ? row["BBCBalance"].ToString() : "0";

                string roleVal = row["Role"] != null
                    ? row["Role"].ToString() : string.Empty;
                if (cmbRole != null)
                {
                    for (int i = 0; i < cmbRole.Items.Count; i++)
                    {
                        string itemText = cmbRole.Items[i] != null
                            ? cmbRole.Items[i].ToString() : string.Empty;
                        if (string.Equals(itemText, roleVal, StringComparison.OrdinalIgnoreCase))
                        {
                            cmbRole.SelectedIndex = i;
                            break;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Failed to load user details: " + ex.Message,
                    "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void BtnAddUser_Click(object sender, EventArgs e)
        {
            try
            {
                string name = txtNameInput.Text.Trim();
                string email = txtEmailInput.Text.Trim();
                string password = txtPasswordInput.Text;
                string role = cmbRole != null ? cmbRole.Text.Trim() : string.Empty;
                decimal bbc = 0m;
                decimal.TryParse(txtBBCInput.Text, out bbc);

                if (string.IsNullOrEmpty(name) || string.IsNullOrEmpty(email) ||
                    string.IsNullOrEmpty(password) || string.IsNullOrEmpty(role))
                {
                    MessageBox.Show(
                        "Please fill in Name, Email, Password and Role to add a user.",
                        "Validation", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                if (AddUserToDb(name, email, password, role, bbc))
                {
                    MessageBox.Show("User added successfully.", "Success",
                        MessageBoxButtons.OK, MessageBoxIcon.Information);
                    LoadUsersIntoGrid();
                }
                else
                {
                    MessageBox.Show("Failed to add user. Email may already exist.", "Failed",
                        MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error adding user: " + ex.Message,
                    "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void BtnUpdateUserInner_Click(object sender, EventArgs e)
        {
            try
            {
                if (cmbUserId == null || cmbUserId.SelectedItem == null)
                {
                    MessageBox.Show("Please select a user to update.", "Validation",
                        MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                DataRowView drv = cmbUserId.SelectedItem as DataRowView;
                int id;
                if (drv == null || !int.TryParse(drv["UserID"].ToString(), out id))
                {
                    MessageBox.Show("Please select a user to update.", "Validation",
                        MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                string name = txtNameInput.Text.Trim();
                string email = txtEmailInput.Text.Trim();
                string password = txtPasswordInput.Text;
                string role = cmbRole != null ? cmbRole.Text.Trim() : string.Empty;
                decimal bbc = 0m;
                decimal.TryParse(txtBBCInput.Text, out bbc);
                string passOrNull = string.IsNullOrEmpty(password) ? null : password;

                if (UpdateUserInDb(id, name, email, passOrNull, role, bbc))
                {
                    MessageBox.Show("User updated successfully.", "Success",
                        MessageBoxButtons.OK, MessageBoxIcon.Information);
                    LoadUsersIntoGrid();
                }
                else
                {
                    MessageBox.Show("No rows were updated.", "Info",
                        MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error updating user: " + ex.Message,
                    "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void BtnDeleteUser_Click(object sender, EventArgs e)
        {
            try
            {
                if (cmbUserId == null || cmbUserId.SelectedItem == null)
                {
                    MessageBox.Show("Please select a user to delete.", "Validation",
                        MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                DataRowView drv = cmbUserId.SelectedItem as DataRowView;
                int id;
                if (drv == null || !int.TryParse(drv["UserID"].ToString(), out id))
                {
                    MessageBox.Show("Please select a user to delete.", "Validation",
                        MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                if (MessageBox.Show("Are you sure you want to delete this user?",
                        "Confirm Delete", MessageBoxButtons.YesNo,
                        MessageBoxIcon.Question) != DialogResult.Yes)
                    return;

                if (DeleteUserFromDb(id))
                {
                    MessageBox.Show("User deleted successfully.", "Success",
                        MessageBoxButtons.OK, MessageBoxIcon.Information);
                    LoadUsersIntoGrid();
                }
                else
                {
                    MessageBox.Show("Failed to delete user.", "Failed",
                        MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error deleting user: " + ex.Message,
                    "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        // ── Designer stub handlers ────────────────────────────────────────────────────
        private void btnLogOut_Click(object sender, EventArgs e) { }
        private void lblPassword_Click(object sender, EventArgs e) { }
        private void lblBBC_Click(object sender, EventArgs e) { }
        private void dgvUsers_CellContentClick(object sender, DataGridViewCellEventArgs e) { }
        private void btnEditProfile_Click(object sender, EventArgs e)
        {
            new Profile().ShowDialog();
        }

        // ══════════════════════════════════════════════════════════════════════════════
        // USER DB HELPERS
        // ══════════════════════════════════════════════════════════════════════════════

        private DataTable GetAllUsersFromDb()
        {
            DataTable dt = new DataTable();
            using (SqlConnection conn = new SqlConnection(_connectionString))
            {
                conn.Open();
                using (SqlCommand cmd = conn.CreateCommand())
                {
                    cmd.CommandText =
                        "SELECT UserID, Name, Email, Password, Role, BBCBalance " +
                        "FROM Users ORDER BY UserID";
                    using (SqlDataAdapter da = new SqlDataAdapter(cmd))
                        da.Fill(dt);
                }
            }
            return dt;
        }

        private DataTable GetUserListFromDb()
        {
            DataTable dt = new DataTable();
            using (SqlConnection conn = new SqlConnection(_connectionString))
            {
                conn.Open();
                using (SqlCommand cmd = conn.CreateCommand())
                {
                    cmd.CommandText = "SELECT UserID, Name FROM Users ORDER BY UserID";
                    using (SqlDataAdapter da = new SqlDataAdapter(cmd))
                        da.Fill(dt);
                }
            }
            return dt;
        }

        private DataRow GetUserByIdFromDb(int id)
        {
            DataTable dt = new DataTable();
            using (SqlConnection conn = new SqlConnection(_connectionString))
            {
                conn.Open();
                using (SqlCommand cmd = conn.CreateCommand())
                {
                    cmd.CommandText =
                        "SELECT UserID, Name, Email, Password, Role, BBCBalance " +
                        "FROM Users WHERE UserID = @id";
                    cmd.Parameters.AddWithValue("@id", id);
                    using (SqlDataAdapter da = new SqlDataAdapter(cmd))
                        da.Fill(dt);
                }
            }
            return dt.Rows.Count == 0 ? null : dt.Rows[0];
        }

        private bool AddUserToDb(string name, string email,
                                 string password, string role, decimal bbc)
        {
            using (SqlConnection conn = new SqlConnection(_connectionString))
            {
                conn.Open();
                using (SqlCommand cmd = conn.CreateCommand())
                {
                    cmd.CommandText =
                        "INSERT INTO Users (Name, Email, Password, Role, BBCBalance) " +
                        "VALUES (@name, @email, @pass, @role, @bbc)";
                    cmd.Parameters.AddWithValue("@name", name);
                    cmd.Parameters.AddWithValue("@email", email);
                    cmd.Parameters.AddWithValue("@pass", password);
                    cmd.Parameters.AddWithValue("@role", role);
                    cmd.Parameters.AddWithValue("@bbc", bbc);
                    try
                    {
                        return cmd.ExecuteNonQuery() > 0;
                    }
                    catch (SqlException ex)
                    {
                        if (ex.Number == 2627 || ex.Number == 2601) return false;
                        throw;
                    }
                }
            }
        }

        private bool UpdateUserInDb(int userId, string name, string email,
                                    string password, string role, decimal bbc)
        {
            using (SqlConnection conn = new SqlConnection(_connectionString))
            {
                conn.Open();
                using (SqlCommand cmd = conn.CreateCommand())
                {
                    if (string.IsNullOrEmpty(password))
                    {
                        cmd.CommandText =
                            "UPDATE Users SET Name=@name, Email=@email, Role=@role, " +
                            "BBCBalance=@bbc WHERE UserID=@id";
                    }
                    else
                    {
                        cmd.CommandText =
                            "UPDATE Users SET Name=@name, Email=@email, Password=@pass, " +
                            "Role=@role, BBCBalance=@bbc WHERE UserID=@id";
                        cmd.Parameters.AddWithValue("@pass", password);
                    }
                    cmd.Parameters.AddWithValue("@name", name);
                    cmd.Parameters.AddWithValue("@email", email);
                    cmd.Parameters.AddWithValue("@role", role);
                    cmd.Parameters.AddWithValue("@bbc", bbc);
                    cmd.Parameters.AddWithValue("@id", userId);
                    return cmd.ExecuteNonQuery() > 0;
                }
            }
        }

        private bool DeleteUserFromDb(int userId)
        {
            using (SqlConnection conn = new SqlConnection(_connectionString))
            {
                conn.Open();
                using (SqlCommand cmd = conn.CreateCommand())
                {
                    cmd.CommandText = "DELETE FROM Users WHERE UserID = @id";
                    cmd.Parameters.AddWithValue("@id", userId);
                    return cmd.ExecuteNonQuery() > 0;
                }
            }
        }

        // ══════════════════════════════════════════════════════════════════════════════
        // SALES REPORT – UI CONSTRUCTION
        // ══════════════════════════════════════════════════════════════════════════════

        private void BuildSalesReportPanel()
        {
            try
            {
                Font fontLabel = new Font("Segoe UI", 9.75F, FontStyle.Regular, GraphicsUnit.Point);
                Font fontNormal = new Font("Segoe UI", 9F, FontStyle.Regular, GraphicsUnit.Point);
                Font fontBtn = new Font("Segoe UI", 11.25F, FontStyle.Bold, GraphicsUnit.Point);
                Font fontHint = new Font("Segoe UI", 8.25F, FontStyle.Italic, GraphicsUnit.Point);

                grpSalesReport = new GroupBox();
                grpSalesReport.Name = "grpSalesReport";
                grpSalesReport.Text = "Sales Report";
                grpSalesReport.Font = new Font("Segoe UI", 14.25F, FontStyle.Bold, GraphicsUnit.Point);
                grpSalesReport.BackColor = Color.Transparent;
                grpSalesReport.Location = new Point(52, 337);
                grpSalesReport.Size = new Size(2225, 1043);
                grpSalesReport.Visible = false;

                lblSortMode = new Label();
                lblSortMode.Text = "Sort By:";
                lblSortMode.Font = fontLabel;
                lblSortMode.Location = new Point(20, 50);
                lblSortMode.Size = new Size(120, 35);
                lblSortMode.TextAlign = ContentAlignment.MiddleLeft;

                cmbSortMode = new ComboBox();
                cmbSortMode.Name = "cmbSortMode";
                cmbSortMode.Font = fontNormal;
                cmbSortMode.DropDownStyle = ComboBoxStyle.DropDownList;
                cmbSortMode.FormattingEnabled = true;
                cmbSortMode.Location = new Point(145, 48);
                cmbSortMode.Size = new Size(270, 40);
                cmbSortMode.Items.AddRange(new object[]
                {
                    "Date", "Chef", "Category (No. of Items)", "Order Status"
                });
                cmbSortMode.SelectedIndex = 0;
                cmbSortMode.SelectedIndexChanged += CmbSortMode_SelectedIndexChanged;

                lblSortOrder = new Label();
                lblSortOrder.Text = "Sort Order:";
                lblSortOrder.Font = fontLabel;
                lblSortOrder.Location = new Point(435, 50);
                lblSortOrder.Size = new Size(145, 35);
                lblSortOrder.TextAlign = ContentAlignment.MiddleLeft;

                cmbSortOrder = new ComboBox();
                cmbSortOrder.Name = "cmbSortOrder";
                cmbSortOrder.Font = fontNormal;
                cmbSortOrder.DropDownStyle = ComboBoxStyle.DropDownList;
                cmbSortOrder.FormattingEnabled = true;
                cmbSortOrder.Location = new Point(585, 48);
                cmbSortOrder.Size = new Size(220, 40);
                cmbSortOrder.Items.AddRange(new object[] { "Ascending", "Descending" });
                cmbSortOrder.SelectedIndex = 0;

                lblFilterValue = new Label();
                lblFilterValue.Text = "Filter Value:";
                lblFilterValue.Font = fontLabel;
                lblFilterValue.Location = new Point(20, 112);
                lblFilterValue.Size = new Size(145, 35);
                lblFilterValue.TextAlign = ContentAlignment.MiddleLeft;

                txtFilterValue = new TextBox();
                txtFilterValue.Name = "txtFilterValue";
                txtFilterValue.Font = fontNormal;
                txtFilterValue.Location = new Point(170, 112);
                txtFilterValue.Size = new Size(440, 40);

                lblFilterHint = new Label();
                lblFilterHint.Name = "lblFilterHint";
                lblFilterHint.Text = "Format: dd/MM/yyyy  (e.g. 31/05/2026) — leave blank to show all";
                lblFilterHint.Font = fontHint;
                lblFilterHint.ForeColor = Color.DimGray;
                lblFilterHint.Location = new Point(170, 156);
                lblFilterHint.Size = new Size(800, 26);
                lblFilterHint.TextAlign = ContentAlignment.TopLeft;

                btnApplyFilter = new Button();
                btnApplyFilter.Name = "btnApplyFilter";
                btnApplyFilter.Text = "Apply Filter";
                btnApplyFilter.Font = fontBtn;
                btnApplyFilter.Location = new Point(630, 106);
                btnApplyFilter.Size = new Size(220, 58);
                btnApplyFilter.UseVisualStyleBackColor = true;
                btnApplyFilter.Click += BtnApplyFilter_Click;

                btnShowAll = new Button();
                btnShowAll.Name = "btnShowAll";
                btnShowAll.Text = "Show All Sales";
                btnShowAll.Font = fontBtn;
                btnShowAll.Location = new Point(866, 106);
                btnShowAll.Size = new Size(248, 58);
                btnShowAll.UseVisualStyleBackColor = true;
                btnShowAll.Click += BtnShowAll_Click;

                btnBackToDashboard = new Button();
                btnBackToDashboard.Name = "btnBackToDashboard";
                btnBackToDashboard.Text = "Back to Dashboard";
                btnBackToDashboard.Font = fontBtn;
                btnBackToDashboard.Location = new Point(1130, 106);
                btnBackToDashboard.Size = new Size(310, 58);
                btnBackToDashboard.UseVisualStyleBackColor = true;
                btnBackToDashboard.Click += BtnBackToDashboard_Click;

                lblReportStatus = new Label();
                lblReportStatus.Name = "lblReportStatus";
                lblReportStatus.Text = string.Empty;
                lblReportStatus.Font = fontHint;
                lblReportStatus.ForeColor = Color.DimGray;
                lblReportStatus.Location = new Point(20, 192);
                lblReportStatus.Size = new Size(800, 28);
                lblReportStatus.TextAlign = ContentAlignment.MiddleLeft;

                dgvReport = new DataGridView();
                dgvReport.Name = "dgvReport";
                dgvReport.Font = fontNormal;
                dgvReport.Location = new Point(20, 228);
                dgvReport.Size = new Size(2180, 790);
                dgvReport.ReadOnly = true;
                dgvReport.AllowUserToAddRows = false;
                dgvReport.AllowUserToDeleteRows = false;
                dgvReport.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
                dgvReport.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
                dgvReport.BackgroundColor = SystemColors.ButtonFace;
                dgvReport.ColumnHeadersHeight = 46;
                dgvReport.RowHeadersWidth = 55;

                grpSalesReport.Controls.AddRange(new Control[]
                {
                    lblSortMode,    cmbSortMode,
                    lblSortOrder,   cmbSortOrder,
                    lblFilterValue, txtFilterValue, lblFilterHint,
                    btnApplyFilter, btnShowAll,     btnBackToDashboard,
                    lblReportStatus, dgvReport
                });

                this.Controls.Add(grpSalesReport);
            }
            catch { }
        }

        // ══════════════════════════════════════════════════════════════════════════════
        // SALES REPORT – TOGGLE
        // ══════════════════════════════════════════════════════════════════════════════

        private void BtnSalesReport_Click(object sender, EventArgs e)
        {
            if (grpSalesReport == null) return;
            grpDashboard.Visible = false;
            grpSalesReport.Visible = true;
            UpdateFilterHint();
            LoadAllSalesReport();
        }

        private void BtnBackToDashboard_Click(object sender, EventArgs e)
        {
            if (grpSalesReport != null) grpSalesReport.Visible = false;
            grpDashboard.Visible = true;
        }

        private void CmbSortMode_SelectedIndexChanged(object sender, EventArgs e)
        {
            UpdateFilterHint();
        }

        private void UpdateFilterHint()
        {
            if (lblFilterHint == null || cmbSortMode == null) return;
            string mode = cmbSortMode.SelectedItem != null
                ? cmbSortMode.SelectedItem.ToString() : string.Empty;

            if (mode == "Date")
                lblFilterHint.Text =
                    "Format: dd/MM/yyyy  (e.g. 31/05/2026) — leave blank to show all dates";
            else if (mode == "Chef")
                lblFilterHint.Text =
                    "Enter chef name or part of it (e.g. Ahmad) — leave blank to show all chefs";
            else if (mode == "Category (No. of Items)")
                lblFilterHint.Text =
                    "Enter minimum number of distinct items per order (e.g. 2) — leave blank to show all";
            else if (mode == "Order Status")
                lblFilterHint.Text =
                    "Enter status keyword (e.g. Pending, Completed) — leave blank to show all";
            else
                lblFilterHint.Text = string.Empty;
        }

        // ══════════════════════════════════════════════════════════════════════════════
        // SALES REPORT – DATA LOADING
        // ══════════════════════════════════════════════════════════════════════════════

        private void BtnShowAll_Click(object sender, EventArgs e)
        {
            if (txtFilterValue != null) txtFilterValue.Text = string.Empty;
            LoadAllSalesReport();
        }

        private void BtnApplyFilter_Click(object sender, EventArgs e)
        {
            string mode = cmbSortMode != null && cmbSortMode.SelectedItem != null
                ? cmbSortMode.SelectedItem.ToString() : "Date";
            string filter = txtFilterValue != null
                ? txtFilterValue.Text.Trim() : string.Empty;

            switch (mode)
            {
                case "Date": LoadSalesByDate(filter); break;
                case "Chef": LoadSalesByChef(filter); break;
                case "Category (No. of Items)": LoadSalesByCategory(filter); break;
                case "Order Status": LoadSalesByStatus(filter); break;
                default: LoadAllSalesReport(); break;
            }
        }

        private const string ReportSelectFrom =
@"SELECT
    o.OrderID                                        AS [Order ID],
    CONVERT(varchar, o.OrderDate, 103)               AS [Order Date],
    cust.Name                                        AS [Customer Name],
    ISNULL(chef.Name, 'N/A')                         AS [Chef Name],
    o.Status                                         AS [Status],
    (
        SELECT STRING_AGG(
                   CAST(oi2.ItemID   AS varchar) + ' x' +
                   CAST(oi2.Quantity AS varchar), ', ')
        FROM OrderItems oi2
        WHERE oi2.OrderID = o.OrderID
    )                                                AS [Items (ID x Qty)],
    o.TotalCost                                      AS [Total Cost (RM)],
    (
        SELECT COUNT(DISTINCT oi3.ItemID)
        FROM OrderItems oi3
        WHERE oi3.OrderID = o.OrderID
    )                                                AS [Distinct Item Types]
FROM Orders o
LEFT JOIN Users cust ON o.CustomerID = cust.UserID
LEFT JOIN Users chef ON o.ChefID     = chef.UserID";

        private void LoadAllSalesReport()
        {
            string dir = GetSortDirection();
            string sql = ReportSelectFrom + "\nORDER BY o.OrderDate " + dir + ";";
            ApplyReportData(ExecuteQuery(sql));
        }

        private void LoadSalesByDate(string input)
        {
            if (string.IsNullOrWhiteSpace(input))
            {
                LoadAllSalesReport();
                return;
            }

            DateTime parsed;
            if (!DateTime.TryParseExact(input,
                    new string[] { "dd/MM/yyyy", "d/M/yyyy", "d/MM/yyyy", "dd/M/yyyy", "yyyy-MM-dd" },
                    CultureInfo.InvariantCulture, DateTimeStyles.None, out parsed))
            {
                MessageBox.Show(
                    "Invalid date format.\nPlease enter a date as dd/MM/yyyy  (e.g. 31/05/2026).",
                    "Invalid Date", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            string dir = GetSortDirection();
            string sql = ReportSelectFrom +
                "\nWHERE CAST(o.OrderDate AS date) = @OrderDate" +
                "\nORDER BY o.OrderDate " + dir + ";";

            ApplyReportData(ExecuteQuery(sql,
                new SqlParameter("@OrderDate", parsed.Date)));
        }

        private void LoadSalesByChef(string input)
        {
            string dir = GetSortDirection();
            string sql = ReportSelectFrom +
                "\nWHERE (@ChefName = '' OR chef.Name LIKE '%' + @ChefName + '%')" +
                "\nORDER BY chef.Name " + dir + ", o.OrderDate DESC;";

            ApplyReportData(ExecuteQuery(sql,
                new SqlParameter("@ChefName", input)));
        }

        private void LoadSalesByCategory(string input)
        {
            int minItems = 0;
            if (!string.IsNullOrWhiteSpace(input) && !int.TryParse(input, out minItems))
            {
                MessageBox.Show(
                    "Please enter a whole number for the minimum number of distinct items\n" +
                    "(e.g. 2), or leave blank to show all orders.",
                    "Invalid Input", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            string dir = GetSortDirection();
            string sql =
                "WITH SalesReport AS (\n" + ReportSelectFrom + "\n)\n" +
                "SELECT * FROM SalesReport\n" +
                "WHERE [Distinct Item Types] >= @MinItems\n" +
                "ORDER BY [Distinct Item Types] " + dir + ", [Order Date] DESC;";

            ApplyReportData(ExecuteQuery(sql,
                new SqlParameter("@MinItems", minItems)));
        }

        private void LoadSalesByStatus(string input)
        {
            string dir = GetSortDirection();
            string sql = ReportSelectFrom +
                "\nWHERE (@Status = '' OR o.Status LIKE '%' + @Status + '%')" +
                "\nORDER BY o.Status " + dir + ", o.OrderDate DESC;";

            ApplyReportData(ExecuteQuery(sql,
                new SqlParameter("@Status", input)));
        }

        private void ApplyReportData(DataTable dt)
        {
            if (dgvReport != null)
            {
                dgvReport.DataSource = dt;

                if (dgvReport.Columns.Contains("Total Cost (RM)"))
                    dgvReport.Columns["Total Cost (RM)"].DefaultCellStyle.Alignment =
                        DataGridViewContentAlignment.MiddleRight;

                if (dgvReport.Columns.Contains("Distinct Item Types"))
                    dgvReport.Columns["Distinct Item Types"].DefaultCellStyle.Alignment =
                        DataGridViewContentAlignment.MiddleCenter;
            }

            if (lblReportStatus != null)
            {
                int count = dt != null ? dt.Rows.Count : 0;
                lblReportStatus.Text = count == 0
                    ? "No records found for the given filter."
                    : count + " record(s) displayed.";
                lblReportStatus.ForeColor = count == 0 ? Color.DarkRed : Color.DimGray;
            }
        }

        private string GetSortDirection()
        {
            return cmbSortOrder != null &&
                   cmbSortOrder.SelectedItem != null &&
                   cmbSortOrder.SelectedItem.ToString() == "Descending"
                ? "DESC" : "ASC";
        }

        // ══════════════════════════════════════════════════════════════════════════════
        // SHARED DB HELPER
        // ══════════════════════════════════════════════════════════════════════════════

        private DataTable ExecuteQuery(string sql, params SqlParameter[] parameters)
        {
            DataTable dt = new DataTable();
            try
            {
                using (SqlConnection conn = new SqlConnection(_connectionString))
                using (SqlCommand cmd = new SqlCommand(sql, conn))
                using (SqlDataAdapter adapter = new SqlDataAdapter(cmd))
                {
                    if (parameters != null && parameters.Length > 0)
                        cmd.Parameters.AddRange(parameters);
                    adapter.Fill(dt);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Database query failed: " + ex.Message,
                    "DB Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            return dt;
        }
    }
}