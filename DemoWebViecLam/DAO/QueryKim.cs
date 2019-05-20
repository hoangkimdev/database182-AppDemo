using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DemoWebViecLam.DAO;
using DemoWebViecLam.DAO;

namespace DemoWebViecLam.DAO
{
    public class QueryKim
    {
        private static QueryKim instance;

        public static QueryKim Instance
        {
            get { if (instance == null) instance = new QueryKim(); return QueryKim.instance; }
            private set { QueryKim.instance = value; }
        }

        //Them moi goi dich vu
        public void InsertDBO_GoiDichVu(string TenGoi, int Phi, int ThoigianSD, int SoTinTuyenDung_max, int DoUuTien)
        {         
            DataProvider.Instance.ExecuteNonQuery("exec dbo.p_ThemGoiDichVU @TenGoi , @Phi , @ThoigianSD , @SoTinTuyenDung_max , @DoUuTien ", 
                new object[] { TenGoi, Phi, ThoigianSD, SoTinTuyenDung_max, DoUuTien });
        }

        //Them dang ky

        public void InsertDBO_DangKy(string strID_dn, string strID_dv)
        {
            int ID_dn = Convert.ToInt32(strID_dn);
            int ID_dv = Convert.ToInt32(strID_dv);

            DataProvider.Instance.ExecuteNonQuery("exec dbo.p_DangKyMoi @ID_dn , @ID_dv ", new object[] { ID_dn, ID_dv });
        
        }

        

    }
}
