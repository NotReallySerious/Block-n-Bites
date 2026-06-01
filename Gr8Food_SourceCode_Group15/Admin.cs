using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Microsoft.Data.SqlClient;
using System.Globalization;

namespace Gr8Food_SourceCode_Group15
{
    public partial class Admin : Form
    {
        // ── Sales Report controls (grpSalesReport field declared in Admin_Designer.cs) ──
        private Label? lblSortMode;
        private ComboBox? cmbSortMode;
        private Label? lblSortOrder;
        private ComboBox? cmbSortOrder;
        private Label? lblFilterValue;
        private TextBox? txtFilterValue;
        private Label? lblFilterHint;
        private Button? btnApplyFilter;
        private Button? btnShowAll;
        private Button? btnBackToDashboard;
        private DataGridView? dgvReport;
        private Label? lblReportStatus;

        // ── Connection string ─────────────────────────────────────────────────────────
        private readonly string _connectionString =
            System.Configuration.ConfigurationManager
                  .ConnectionStrings["Gr8FoodConnection"]
                  ?.ConnectionString
            ?? throw new InvalidOperationException(
                   "Connection string 'Gr8FoodConnection' not found.");

        // ── Constructor ───────────────────────────────────────────────────────────────
        public Admin()
        {
            InitializeComponent();

            btnAddRemove.Click += BtnAddRemove_Click;
            this.Load += Admin_Load;

            BuildSalesReportPanel();

            if (btnSalesReport != null)
                btnSalesReport.Click += BtnSalesReport_Click;
        }

        // ── Form Load ─────────────────────────────────────────────────────────────────
        private void Admin_Load(object? sender, EventArgs e)
        {
            try
            {
                txtGreetAdmin.Text = !string.IsNullOrEmpty(Session.CurrentUserName)
                    ? $"Hello, Admin ({Session.CurrentUserName})"
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
                    var users = GetUserListFromDb();
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

        private void BtnAddRemove_Click(object? sender, EventArgs e) => LoadUsersIntoGrid();

        private void LoadUsersIntoGrid()
        {
            try
            {
                var dt = GetAllUsersFromDb();
                if (dgvUsers != null) dgvUsers.DataSource = dt;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Failed to load users: " + ex.Message,
                    "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void DgvUsers_CellClick(object? sender, DataGridViewCellEventArgs e)
        {
            if (dgvUsers == null) return;
            if (e.RowIndex < 0 || e.RowIndex >= dgvUsers.Rows.Count) return;

            var row = dgvUsers.Rows[e.RowIndex];
            var idStr = row.Cells["UserID"].Value?.ToString() ?? string.Empty;

            if (int.TryParse(idStr, out var id) && cmbUserId != null)
            {
                for (int i = 0; i < cmbUserId.Items.Count; i++)
                {
                    var drv = cmbUserId.Items[i] as DataRowView;
                    if (drv != null && Convert.ToInt32(drv["UserID"]) == id)
                    {
                        cmbUserId.SelectedIndex = i;
                        break;
                    }
                }
            }

            txtNameInput!.Text = row.Cells["Name"].Value?.ToString() ?? string.Empty;
            txtEmailInput!.Text = row.Cells["Email"].Value?.ToString() ?? string.Empty;
            txtBBCInput!.Text = row.Cells["BBCBalance"].Value?.ToString() ?? "0";
            txtPasswordInput!.Text = row.Cells["Password"].Value?.ToString() ?? string.Empty;

            var roleFromRow = row.Cells["Role"].Value?.ToString() ?? string.Empty;
            if (cmbRole != null)
            {
                for (int r = 0; r < cmbRole.Items.Count; r++)
                {
                    if (string.Equals(cmbRole.Items[r]?.ToString(), roleFromRow,
                            StringComparison.OrdinalIgnoreCase))
                    {
                        cmbRole.SelectedIndex = r;
                        break;
                    }
                }
            }
        }

        private void CmbUserId_SelectedIndexChanged(object? sender, EventArgs e)
        {
            try
            {
                if (cmbUserId?.SelectedItem == null) return;
                var drv = cmbUserId.SelectedItem as DataRowView;
                if (drv == null) return;

                var id = Convert.ToInt32(drv["UserID"]);
                var row = GetUserByIdFromDb(id);
                if (row == null) return;

                txtNameInput!.Text = row["Name"]?.ToString() ?? string.Empty;
                txtEmailInput!.Text = row["Email"]?.ToString() ?? string.Empty;
                txtPasswordInput!.Text = row["Password"]?.ToString() ?? string.Empty;
                txtBBCInput!.Text = row["BBCBalance"]?.ToString() ?? "0";

                var roleVal = row["Role"]?.ToString() ?? string.Empty;
                if (cmbRole != null)
                {
                    for (int i = 0; i < cmbRole.Items.Count; i++)
                    {
                        if (string.Equals(cmbRole.Items[i]?.ToString(), roleVal,
                                StringComparison.OrdinalIgnoreCase))
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

        private void BtnAddUser_Click(object? sender, EventArgs e)
        {
            try
            {
                var name = txtNameInput!.Text.Trim();
                var email = txtEmailInput!.Text.Trim();
                var password = txtPasswordInput!.Text;
                var role = cmbRole != null ? cmbRole.Text.Trim() : string.Empty;
                decimal bbc = 0m;
                decimal.TryParse(txtBBCInput!.Text, out bbc);

                if (string.IsNullOrEmpty(name) || string.IsNullOrEmpty(email) ||
                    string.IsNullOrEmpty(password) || string.IsNullOrEmpty(role))
                {
                    MessageBox.Show("Please fill in Name, Email, Password and Role to add a user.",
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

        private void BtnUpdateUserInner_Click(object? sender, EventArgs e)
        {
            try
            {
                int id;
                if (cmbUserId?.SelectedItem == null ||
                    !int.TryParse(
                        (cmbUserId.SelectedItem as DataRowView)?["UserID"]?.ToString(), out id))
                {
                    MessageBox.Show("Please select a user to update.", "Validation",
                        MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                var name = txtNameInput!.Text.Trim();
                var email = txtEmailInput!.Text.Trim();
                var password = txtPasswordInput!.Text;
                var role = cmbRole != null ? cmbRole.Text.Trim() : string.Empty;
                decimal bbc = 0m;
                decimal.TryParse(txtBBCInput!.Text, out bbc);

                if (UpdateUserInDb(id, name, email,
                        string.IsNullOrEmpty(password) ? null : password, role, bbc))
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

        private void BtnDeleteUser_Click(object? sender, EventArgs e)
        {
            try
            {
                int id;
                if (cmbUserId?.SelectedItem == null ||
                    !int.TryParse(
                        (cmbUserId.SelectedItem as DataRowView)?["UserID"]?.ToString(), out id))
                {
                    MessageBox.Show("Please select a user to delete.", "Validation",
                        MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                if (MessageBox.Show("Are you sure you want to delete this user?", "Confirm Delete",
                        MessageBoxButtons.YesNo, MessageBoxIcon.Question) != DialogResult.Yes)
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
        private void btnLogOut_Click(object sender, EventArgs e)
        {
            frmLogin loginform = new frmLogin();
            this.Close();
            loginform.Show();
        }
        private void lblPassword_Click(object sender, EventArgs e) { }
        private void lblBBC_Click(object sender, EventArgs e) { }
        private void dgvUsers_CellContentClick(object sender, DataGridViewCellEventArgs e) { }

        // ══════════════════════════════════════════════════════════════════════════════
        // USER DB HELPERS
        // ══════════════════════════════════════════════════════════════════════════════

        private DataTable GetAllUsersFromDb()
        {
            var dt = new DataTable();
            using var conn = new SqlConnection(_connectionString);
            conn.Open();
            using var cmd = conn.CreateCommand();
            cmd.CommandText =
                "SELECT UserID, Name, Email, Password, Role, BBCBalance " +
                "FROM Users ORDER BY UserID";
            using var da = new SqlDataAdapter(cmd);
            da.Fill(dt);
            return dt;
        }

        private DataTable GetUserListFromDb()
        {
            var dt = new DataTable();
            using var conn = new SqlConnection(_connectionString);
            conn.Open();
            using var cmd = conn.CreateCommand();
            cmd.CommandText = "SELECT UserID, Name FROM Users ORDER BY UserID";
            using var da = new SqlDataAdapter(cmd);
            da.Fill(dt);
            return dt;
        }

        private DataRow? GetUserByIdFromDb(int id)
        {
            var dt = new DataTable();
            using var conn = new SqlConnection(_connectionString);
            conn.Open();
            using var cmd = conn.CreateCommand();
            cmd.CommandText =
                "SELECT UserID, Name, Email, Password, Role, BBCBalance " +
                "FROM Users WHERE UserID = @id";
            cmd.Parameters.AddWithValue("@id", id);
            using var da = new SqlDataAdapter(cmd);
            da.Fill(dt);
            return dt.Rows.Count == 0 ? null : dt.Rows[0];
        }

        private bool AddUserToDb(string name, string email,
                                 string password, string role, decimal bbc)
        {
            using var conn = new SqlConnection(_connectionString);
            conn.Open();
            using var cmd = conn.CreateCommand();
            cmd.CommandText =
                "INSERT INTO Users (Name, Email, Password, Role, BBCBalance) " +
                "VALUES (@name, @email, @pass, @role, @bbc)";
            cmd.Parameters.AddWithValue("@name", name);
            cmd.Parameters.AddWithValue("@email", email);
            cmd.Parameters.AddWithValue("@pass", password);
            cmd.Parameters.AddWithValue("@role", role);
            cmd.Parameters.AddWithValue("@bbc", bbc);
            try { return cmd.ExecuteNonQuery() > 0; }
            catch (SqlException ex) when (ex.Number == 2627 || ex.Number == 2601)
            { return false; }
        }

        private bool UpdateUserInDb(int userId, string name, string email,
                                    string? password, string role, decimal bbc)
        {
            using var conn = new SqlConnection(_connectionString);
            conn.Open();
            using var cmd = conn.CreateCommand();
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

        private bool DeleteUserFromDb(int userId)
        {
            using var conn = new SqlConnection(_connectionString);
            conn.Open();
            using var cmd = conn.CreateCommand();
            cmd.CommandText = "DELETE FROM Users WHERE UserID = @id";
            cmd.Parameters.AddWithValue("@id", userId);
            return cmd.ExecuteNonQuery() > 0;
        }

        // ══════════════════════════════════════════════════════════════════════════════
        // SALES REPORT – UI CONSTRUCTION
        //
        // grpSalesReport sits at the same Location+Size as grpDashboard.
        // "View Sales Report" hides grpDashboard and shows grpSalesReport.
        // "← Back to Dashboard" reverses this. No scrolling needed.
        // ══════════════════════════════════════════════════════════════════════════════

        private void BuildSalesReportPanel()
        {
            try
            {
                var fontLabel = new Font("Segoe UI", 9.75F, FontStyle.Regular, GraphicsUnit.Point);
                var fontNormal = new Font("Segoe UI", 9F, FontStyle.Regular, GraphicsUnit.Point);
                var fontBtn = new Font("Segoe UI", 11.25F, FontStyle.Bold, GraphicsUnit.Point);
                var fontHint = new Font("Segoe UI", 8.25F, FontStyle.Italic, GraphicsUnit.Point);

                // Same Location + Size as grpDashboard (52,337 / 2225×1043)
                grpSalesReport = new GroupBox()
                {
                    Name = "grpSalesReport",
                    Text = "Sales Report",
                    Font = new Font("Segoe UI", 14.25F, FontStyle.Bold, GraphicsUnit.Point),
                    BackColor = Color.Transparent,
                    Location = new Point(52, 337),
                    Size = new Size(2225, 1043),
                    Visible = false
                };

                // ── Row 1: Sort By ────────────────────────────────────────────────────
                lblSortMode = new Label()
                {
                    Text = "Sort By:",
                    Font = fontLabel,
                    Location = new Point(20, 50),
                    Size = new Size(120, 35),
                    TextAlign = ContentAlignment.MiddleLeft
                };

                cmbSortMode = new ComboBox()
                {
                    Name = "cmbSortMode",
                    Font = fontNormal,
                    DropDownStyle = ComboBoxStyle.DropDownList,
                    FormattingEnabled = true,
                    Location = new Point(145, 48),
                    Size = new Size(240, 40)
                };
                // Four sort modes — Date, Chef, Category (order item count), Order Status
                cmbSortMode.Items.AddRange(new object[]
                {
                    "Date",
                    "Chef",
                    "Category (No. of Items)",
                    "Order Status"
                });
                cmbSortMode.SelectedIndex = 0;
                cmbSortMode.SelectedIndexChanged += CmbSortMode_SelectedIndexChanged;

                // ── Sort Order ────────────────────────────────────────────────────────
                lblSortOrder = new Label()
                {
                    Text = "Sort Order:",
                    Font = fontLabel,
                    Location = new Point(410, 50),
                    Size = new Size(145, 35),
                    TextAlign = ContentAlignment.MiddleLeft
                };

                cmbSortOrder = new ComboBox()
                {
                    Name = "cmbSortOrder",
                    Font = fontNormal,
                    DropDownStyle = ComboBoxStyle.DropDownList,
                    FormattingEnabled = true,
                    Location = new Point(560, 48),
                    Size = new Size(220, 40)
                };
                cmbSortOrder.Items.AddRange(new object[] { "Ascending", "Descending" });
                cmbSortOrder.SelectedIndex = 0;

                // ── Row 2: Filter Value ───────────────────────────────────────────────
                lblFilterValue = new Label()
                {
                    Text = "Filter Value:",
                    Font = fontLabel,
                    Location = new Point(20, 112),
                    Size = new Size(145, 35),
                    TextAlign = ContentAlignment.MiddleLeft
                };

                txtFilterValue = new TextBox()
                {
                    Name = "txtFilterValue",
                    Font = fontNormal,
                    Location = new Point(170, 112),
                    Size = new Size(440, 40)
                };

                lblFilterHint = new Label()
                {
                    Name = "lblFilterHint",
                    Text = "Format: dd/MM/yyyy  (e.g. 31/05/2026) — leave blank to show all",
                    Font = fontHint,
                    ForeColor = Color.DimGray,
                    Location = new Point(170, 156),
                    Size = new Size(800, 26),
                    TextAlign = ContentAlignment.TopLeft
                };

                // ── Buttons ───────────────────────────────────────────────────────────
                btnApplyFilter = new Button()
                {
                    Name = "btnApplyFilter",
                    Text = "Apply Filter",
                    Font = fontBtn,
                    Location = new Point(630, 106),
                    Size = new Size(220, 58),
                    UseVisualStyleBackColor = true
                };
                btnApplyFilter.Click += BtnApplyFilter_Click;

                btnShowAll = new Button()
                {
                    Name = "btnShowAll",
                    Text = "Show All Sales",
                    Font = fontBtn,
                    Location = new Point(866, 106),
                    Size = new Size(248, 58),
                    UseVisualStyleBackColor = true
                };
                btnShowAll.Click += BtnShowAll_Click;

                btnBackToDashboard = new Button()
                {
                    Name = "btnBackToDashboard",
                    Text = "← Back to Dashboard",
                    Font = fontBtn,
                    Location = new Point(1130, 106),
                    Size = new Size(310, 58),
                    UseVisualStyleBackColor = true
                };
                btnBackToDashboard.Click += BtnBackToDashboard_Click;

                // ── Status label ──────────────────────────────────────────────────────
                lblReportStatus = new Label()
                {
                    Name = "lblReportStatus",
                    Text = "",
                    Font = fontHint,
                    ForeColor = Color.DimGray,
                    Location = new Point(20, 192),
                    Size = new Size(800, 28),
                    TextAlign = ContentAlignment.MiddleLeft
                };

                // ── DataGridView ──────────────────────────────────────────────────────
                dgvReport = new DataGridView()
                {
                    Name = "dgvReport",
                    Font = fontNormal,
                    Location = new Point(20, 228),
                    Size = new Size(2180, 790),
                    ReadOnly = true,
                    AllowUserToAddRows = false,
                    AllowUserToDeleteRows = false,
                    AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill,
                    SelectionMode = DataGridViewSelectionMode.FullRowSelect,
                    BackgroundColor = SystemColors.ButtonFace,
                    ColumnHeadersHeight = 46,
                    RowHeadersWidth = 55
                };

                grpSalesReport.Controls.AddRange(new Control[]
                {
                    lblSortMode,    cmbSortMode,
                    lblSortOrder,   cmbSortOrder,
                    lblFilterValue, txtFilterValue, lblFilterHint,
                    btnApplyFilter, btnShowAll,     btnBackToDashboard,
                    lblReportStatus,
                    dgvReport
                });

                this.Controls.Add(grpSalesReport);
            }
            catch { }
        }

        // ══════════════════════════════════════════════════════════════════════════════
        // SALES REPORT – TOGGLE
        // ══════════════════════════════════════════════════════════════════════════════

        private void BtnSalesReport_Click(object? sender, EventArgs e)
        {
            if (grpSalesReport == null) return;
            grpDashboard.Visible = false;
            grpSalesReport.Visible = true;
            UpdateFilterHint();
            LoadAllSalesReport();
        }

        private void BtnBackToDashboard_Click(object? sender, EventArgs e)
        {
            if (grpSalesReport != null) grpSalesReport.Visible = false;
            grpDashboard.Visible = true;
        }

        private void CmbSortMode_SelectedIndexChanged(object? sender, EventArgs e)
            => UpdateFilterHint();

        private void UpdateFilterHint()
        {
            if (lblFilterHint == null || cmbSortMode == null) return;

            lblFilterHint.Text = cmbSortMode.SelectedItem?.ToString() switch
            {
                "Date" =>
                    "Format: dd/MM/yyyy  (e.g. 31/05/2026) — leave blank to show all dates",
                "Chef" =>
                    "Enter chef name or part of it (e.g. Ahmad) — leave blank to show all chefs",
                "Category (No. of Items)" =>
                    "Enter minimum number of distinct items per order (e.g. 2) — leave blank to show all",
                "Order Status" =>
                    "Enter status keyword (e.g. Pending, Completed) — leave blank to show all",
                _ => ""
            };
        }

        // ══════════════════════════════════════════════════════════════════════════════
        // SALES REPORT – DATA LOADING
        // ══════════════════════════════════════════════════════════════════════════════

        private void BtnShowAll_Click(object? sender, EventArgs e)
        {
            if (txtFilterValue != null) txtFilterValue.Text = string.Empty;
            LoadAllSalesReport();
        }

        private void BtnApplyFilter_Click(object? sender, EventArgs e)
        {
            var mode = cmbSortMode?.SelectedItem?.ToString() ?? "Date";
            var filter = txtFilterValue?.Text.Trim() ?? string.Empty;

            switch (mode)
            {
                case "Date": LoadSalesByDate(filter); break;
                case "Chef": LoadSalesByChef(filter); break;
                case "Category (No. of Items)": LoadSalesByCategory(filter); break;
                case "Order Status": LoadSalesByStatus(filter); break;
                default: LoadAllSalesReport(); break;
            }
        }

        // ── Shared SELECT columns — keeps all four queries identical above the WHERE ─
        // Returns every order enriched with:
        //   Customer name, Chef name, comma-joined items, total cost, item-count
        private const string ReportSelectFrom = @"
SELECT
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

        // ── All sales ─────────────────────────────────────────────────────────────────
        private void LoadAllSalesReport()
        {
            var dir = GetSortDirection();
            var sql = $"{ReportSelectFrom}\nORDER BY o.OrderDate {dir};";
            ApplyReportData(ExecuteQuery(sql));
        }

        // ── Sort/filter by Date ───────────────────────────────────────────────────────
        private void LoadSalesByDate(string input)
        {
            if (string.IsNullOrWhiteSpace(input))
            {
                LoadAllSalesReport();
                return;
            }

            if (!DateTime.TryParseExact(input,
                    new[] { "dd/MM/yyyy", "d/M/yyyy", "d/MM/yyyy", "dd/M/yyyy", "yyyy-MM-dd" },
                    CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTime parsed))
            {
                MessageBox.Show(
                    "Invalid date format.\nPlease enter a date as dd/MM/yyyy  (e.g. 31/05/2026).",
                    "Invalid Date", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            var dir = GetSortDirection();
            var sql = $@"{ReportSelectFrom}
WHERE CAST(o.OrderDate AS date) = @OrderDate
ORDER BY o.OrderDate {dir};";

            ApplyReportData(ExecuteQuery(sql,
                new SqlParameter("@OrderDate", parsed.Date)));
        }

        // ── Sort/filter by Chef name ──────────────────────────────────────────────────
        private void LoadSalesByChef(string input)
        {
            var dir = GetSortDirection();
            var sql = $@"{ReportSelectFrom}
WHERE (@ChefName = '' OR chef.Name LIKE '%' + @ChefName + '%')
ORDER BY chef.Name {dir}, o.OrderDate DESC;";

            ApplyReportData(ExecuteQuery(sql,
                new SqlParameter("@ChefName", input)));
        }

        // ── Sort/filter by Category (number of distinct item types per order) ─────────
        // Because the DB has no Category column, "category" is represented as the count
        // of distinct ItemIDs in an order.  The user can optionally enter a minimum count
        // to show only orders with at least that many distinct item types.
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

            var dir = GetSortDirection();

            // We wrap the base query in a CTE so we can filter + sort on the computed column
            var sql = $@"
WITH SalesReport AS (
{ReportSelectFrom}
)
SELECT *
FROM SalesReport
WHERE [Distinct Item Types] >= @MinItems
ORDER BY [Distinct Item Types] {dir}, [Order Date] DESC;";

            ApplyReportData(ExecuteQuery(sql,
                new SqlParameter("@MinItems", minItems)));
        }

        // ── Sort/filter by Order Status ───────────────────────────────────────────────
        private void LoadSalesByStatus(string input)
        {
            var dir = GetSortDirection();
            var sql = $@"{ReportSelectFrom}
WHERE (@Status = '' OR o.Status LIKE '%' + @Status + '%')
ORDER BY o.Status {dir}, o.OrderDate DESC;";

            ApplyReportData(ExecuteQuery(sql,
                new SqlParameter("@Status", input)));
        }

        // ── Push DataTable into grid + update status label ────────────────────────────
        private void ApplyReportData(DataTable dt)
        {
            if (dgvReport != null)
            {
                dgvReport.DataSource = dt;

                // Right-align money column
                if (dgvReport.Columns.Contains("Total Cost (RM)"))
                    dgvReport.Columns["Total Cost (RM)"]!.DefaultCellStyle.Alignment =
                        DataGridViewContentAlignment.MiddleRight;

                // Centre the item-count column
                if (dgvReport.Columns.Contains("Distinct Item Types"))
                    dgvReport.Columns["Distinct Item Types"]!.DefaultCellStyle.Alignment =
                        DataGridViewContentAlignment.MiddleCenter;
            }

            if (lblReportStatus != null)
            {
                int count = dt?.Rows.Count ?? 0;
                lblReportStatus.Text = count == 0
                    ? "No records found for the given filter."
                    : $"{count} record(s) displayed.";
                lblReportStatus.ForeColor = count == 0 ? Color.DarkRed : Color.DimGray;
            }
        }

        private string GetSortDirection() =>
            cmbSortOrder?.SelectedItem?.ToString() == "Descending" ? "DESC" : "ASC";

        // ══════════════════════════════════════════════════════════════════════════════
        // SHARED DB HELPER
        // ══════════════════════════════════════════════════════════════════════════════

        private DataTable ExecuteQuery(string sql, params SqlParameter[] parameters)
        {
            var dt = new DataTable();
            try
            {
                using var conn = new SqlConnection(_connectionString);
                using var cmd = new SqlCommand(sql, conn);
                using var adapter = new SqlDataAdapter(cmd);
                if (parameters?.Length > 0)
                    cmd.Parameters.AddRange(parameters);
                adapter.Fill(dt);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Database query failed: " + ex.Message,
                    "DB Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            return dt;
        }

        private void btnEditProfile_Click(object sender, EventArgs e)
        {

        }
    }
}