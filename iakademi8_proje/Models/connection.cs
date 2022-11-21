using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace iakademi8_proje.Models
{
    public class connection
    {

        public static SqlConnection baglanti
        {
            get
            {
                SqlConnection sqlcon = new SqlConnection("Server=DESKTOP-D6N7I1U;Database=Cetin_Kirtasiye;trusted_connection=true;");
                return sqlcon;
            }
        }

    }
}