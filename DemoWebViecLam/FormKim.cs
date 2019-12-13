using DemoWebViecLam.DAO;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Data.SqlClient;



namespace DemoWebViecLam
{
    public partial class FormKim : Form
    {
        //@@@@@BEGIN_ delete data on datagridview
        SqlConnection sqlCon = new SqlConnection("Data Source=.\\SQLEXPRESS;Initial Catalog=WebViecLam;Integrated Security=True");

        SqlCommandBuilder sqlCommand = null;

        SqlDataAdapter sqlAdapter = null;

        DataSet dataset = null;
        //@@@@@@@@END

        public FormKim()                    //FORM MAIN
        {
            InitializeComponent();
            LoadDangKy();
            LoadDoanhNghiep_DV();
            LoadDoanhNghiep();
        }

        //load lich su dang ky
        void LoadDangKy()
        {
            try
            {
                string query = "SELECT * FROM dbo.DangKy";
                dataGridViewDangKy.DataSource = DataProvider.Instance.ExecuteQuery(query);

                comboBox_f_ID_dn.Items.Clear();
                string query1 = "SELECT ID_dn FROM dbo.DoanhNghiep";
                DataTable data1 = DataProvider.Instance.ExecuteQuery(query1);
                foreach (DataRow item in data1.Rows)
                {
                    comboBox_f_ID_dn.Items.Add(item[0].ToString());
                }

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "ERROR ", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        void LoadDoanhNghiep()
        {
            try
            {
                string query = "SELECT * FROM dbo.DoanhNghiep";
                dataGridViewDoanhNghiep.DataSource = DataProvider.Instance.ExecuteQuery(query);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "ERROR ", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        

        //them moi dich vu
        private void buttonThemDV_Click(object sender, EventArgs e)
        {
            try
            {
                if (textBoxDV1.Text == null || textBoxDV2.Text == null || textBoxDV3.Text == null || textBoxDV4.Text == null)
                {
                    MessageBox.Show("Vui lòng nhập đây đủ thông tin !");    //SOS !!!!!!!!!!!!!!!!!!!!! chua xu ly
                }
                else
                {
                    string TenGoi = textBoxDV1.Text.ToString();
                    int Phi = Convert.ToInt32(textBoxDV2.Text.ToString());
                    int ThoigianSD = Convert.ToInt32(textBoxDV3.Text.ToString());
                    int SoTinTuyenDung_max = Convert.ToInt32(textBoxDV4.Text.ToString());
                    int DoUuTien;

                    if (radioButtonDV1.Checked) DoUuTien = 1;
                    else if (radioButtonDV3.Checked) DoUuTien = 3;
                    else DoUuTien = 2;

                    QueryKim.Instance.InsertDBO_GoiDichVu(TenGoi, Phi, ThoigianSD, SoTinTuyenDung_max, DoUuTien);
                    //Cap nhap lai danh sach goi dich vu, danh sach chon dang ky
                    LoadData();
                    LoadDoanhNghiep_DV();
                    MessageBox.Show("Thêm dịch vụ thành công !");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "ERROR ", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

        }



        //@@@@@@@@@BEGIN_ delete data on datagirdview
        private void LoadData()
        {
            try
            {
                sqlAdapter = new SqlDataAdapter("SELECT *, 'Delete' AS [Delete] FROM dbo.GoiDichVu", sqlCon);
                sqlCommand = new SqlCommandBuilder(sqlAdapter);
                //sqlAdapter.InsertCommand = sqlCommand.GetInsertCommand();
                sqlAdapter.UpdateCommand = sqlCommand.GetUpdateCommand();
                sqlAdapter.DeleteCommand = sqlCommand.GetDeleteCommand();
                dataset = new DataSet();
                sqlAdapter.Fill(dataset, "dbo.GoiDichVu");
                dataGridViewDV.DataSource = null;
                dataGridViewDV.DataSource = dataset.Tables["dbo.GoiDichVu"];
                for (int i = 0; i < dataGridViewDV.Rows.Count; i++)
                {
                    DataGridViewLinkCell linkCell = new DataGridViewLinkCell();
                    dataGridViewDV[6, i] = linkCell;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void FormKim_Load(object sender, EventArgs e)
        {
            try
            {
                sqlCon.Open();
                LoadData();
            }
            catch (Exception ex) { MessageBox.Show(ex.Message); }
        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                if (e.ColumnIndex == 6)
                {
                    string Task = dataGridViewDV.Rows[e.RowIndex].Cells[6].Value.ToString();
                    if (Task == "Delete")
                    {
                        if (MessageBox.Show("Bạn có chắc chắm muốn xóa không?", "Đang xóa...", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                        {
                            int rowIndex = e.RowIndex;
                            dataGridViewDV.Rows.RemoveAt(rowIndex);
                            dataset.Tables["dbo.GoiDichVu"].Rows[rowIndex].Delete();
                            sqlAdapter.Update(dataset, "dbo.GoiDichVu");
                        }
                    }                  
                    else if (Task == "Update")
                    {
                        int r = e.RowIndex;

                        dataset.Tables["dbo.GoiDichVu"].Rows[r]["ID_dv"] = Convert.ToInt32(dataGridViewDV.Rows[r].Cells["ID_dv"].Value.ToString());
                        dataset.Tables["dbo.GoiDichVu"].Rows[r]["TenGoi_dv"] = dataGridViewDV.Rows[r].Cells["TenGoi_dv"].Value.ToString();
                        dataset.Tables["dbo.GoiDichVu"].Rows[r]["Phi_dv"] = Convert.ToInt32(dataGridViewDV.Rows[r].Cells["Phi_dv"].Value.ToString());
                        dataset.Tables["dbo.GoiDichVu"].Rows[r]["ThoiGianDuDung"] = Convert.ToInt32(dataGridViewDV.Rows[r].Cells["ThoiGianSuDung"].Value.ToString());
                        dataset.Tables["dbo.GoiDichVu"].Rows[r]["SoTinTuyenDung_max"] = Convert.ToInt32(dataGridViewDV.Rows[r].Cells["SoTinTuyenDung_max"].Value.ToString());
                        dataset.Tables["dbo.GoiDichVu"].Rows[r]["DoUuTien"] = Convert.ToInt32(dataGridViewDV.Rows[r].Cells["DoUuTien"].Value.ToString());

                        sqlAdapter.Update(dataset, "dbo.GoiDichVu");
                        dataGridViewDV.Rows[e.RowIndex].Cells[6].Value = "Delete";
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void dataGridView1_CellContentDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                int lastRow = e.RowIndex;
                DataGridViewRow nRow = dataGridViewDV.Rows[lastRow];
                DataGridViewLinkCell linkCell = new DataGridViewLinkCell();
                dataGridViewDV[6, lastRow] = linkCell;
                nRow.Cells["Delete"].Value = "Update";
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            //@@@@@@@@@END


        }

        //@@@@@@@@@@@@END


        //load data len combobox -> dang ky 
        void LoadDoanhNghiep_DV()
        {
            try
            {
                comboBoxID_DN.Items.Clear();
                comboBoxID_dv.Items.Clear();
                string query1 = "SELECT ID_dn FROM dbo.DoanhNghiep";
                DataTable data1 = DataProvider.Instance.ExecuteQuery(query1);
                foreach (DataRow item in data1.Rows)
                {
                    comboBoxID_DN.Items.Add(item[0].ToString());
                }

                string query3 = "SELECT ID_dv FROM dbo.GoiDichVu";
                DataTable data3 = DataProvider.Instance.ExecuteQuery(query3);
                foreach (DataRow item in data3.Rows)
                {
                    comboBoxID_dv.Items.Add(item[0].ToString());
                }

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "ERROR ", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
 
        // xac nhan dang ky
        private void buttonDK_Click(object sender, EventArgs e)
        {
            try
            {
                string strID_dn = comboBoxID_DN.SelectedItem.ToString();
                string strID_dv = comboBoxID_dv.SelectedItem.ToString();
                DemoWebViecLam.DAO.QueryKim.Instance.InsertDBO_DangKy(strID_dn, strID_dv);
                //Cap nhap lich su dang ky
                LoadDangKy();
                MessageBox.Show("Đăng ký dịch vụ thành công !");
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "ERROR ", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }


        void LoadSerchMode_datagridviewDV()
        {
            try
            {
                string Phi_dv = textBoxSerchPhi.Text.ToString();
                string ThoiGianSudung = textBoxSerchTime.Text.ToString();
                string SotinMAX = textBoxSerchMax.Text.ToString();

                string query = "SELECT * FROM dbo.GoiDichVu WHERE Phi_dv <=" + Phi_dv +
                    "AND ThoiGianSuDung >=" + ThoiGianSudung + "AND SoTinTuyenDung_max >=" + SotinMAX;
                dataGridViewDV.DataSource = DataProvider.Instance.ExecuteQuery(query);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "ERROR ", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void buttonSerch_Click(object sender, EventArgs e)
        {
            LoadSerchMode_datagridviewDV();
        }

        private void buttonHuy_Click(object sender, EventArgs e)
        {
            LoadData();
            LoadDoanhNghiep_DV();
        }

        private void buttonReset_Click(object sender, EventArgs e)
        {
            LoadDangKy();
        }

        // 2 cau thu tuc
        void LoadSapXep()
        {
            try
            {
                string query = "EXEC dbo.p_SapXepChiPhi";
                dataGridViewDangKy.DataSource = DataProvider.Instance.ExecuteQuery(query);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "ERROR ", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void buttonSapXep_Click(object sender, EventArgs e)
        {
            LoadSapXep();
        }

        

        void Load_p_Xem()
        {
            try
            {
                string query = "EXEC dbo.p_DoanhNghiep_max_20";
                dataGridViewDangKy.DataSource = DataProvider.Instance.ExecuteQuery(query);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "ERROR ", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        private void button_p_Xem_Click(object sender, EventArgs e)
        {
            Load_p_Xem();
        }




        // 2 cau tinh ham
        private void button_f_tong_Click(object sender, EventArgs e)
        {
            try
            {
                string strID_dn = comboBox_f_ID_dn.SelectedItem.ToString();
                string query = "SELECT dbo.f_TongChiPhi(" + strID_dn +")";
                textBox_f_Tong.Text = DataProvider.Instance.ExecuteScalar(query).ToString();

               
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "ERROR ", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void button_f_sotin_Click(object sender, EventArgs e)
        {
            try
            {
                string strID_dn = comboBox_f_ID_dn.SelectedItem.ToString();
                string query = "SELECT dbo.f_SoTinTuyenDung(" + strID_dn + ")";
                textBox_f_sotin.Text = DataProvider.Instance.ExecuteScalar(query).ToString();

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "ERROR ", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }


      




    }
}

