using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace RPSExport
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void BtnGetData_Click(object sender, RoutedEventArgs e)
        {

            try
            {
                lblErrorTxt.Content = "";
                SqlConnection conn = new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["SqlServer"].ConnectionString);
                var outCsvFile1 = @"C:\test\result1.csv";
                var outCsvFile2 = @"C:\test\result2.csv";
                var sqlCmd = new SqlDataAdapter("CustOrdersDetail", conn);

                sqlCmd.SelectCommand.CommandType = CommandType.StoredProcedure;
                SqlParameter orderIDParameter = sqlCmd.SelectCommand.Parameters.Add("@OrderID", SqlDbType.Int);
                orderIDParameter.Value = Convert.ToInt32(txtOrderID.Text);

                var ds1 = new DataSet();
                var ds2 = new DataTable();

                using (ds1)
                {
                    sqlCmd.Fill(ds1);
                }
                using (ds2)
                {
                    sqlCmd.Fill(ds2);
                }

                //created by me
                StringBuilder csvFileData = new StringBuilder();

                if(ds1.Tables[0].Rows.Count > 0)
                {
                    DataRow dr1 = ds1.Tables[0].Rows[0];
                    var headerCount = dr1.Table.Columns.Count;
                    int index = 1;

                    foreach(DataColumn dataColumn in dr1.Table.Columns)
                    {
                        csvFileData.Append(String.Format("\"{0}\"", dataColumn.ColumnName));
                        if (index < headerCount)
                            csvFileData.Append(",");
                        else
                            csvFileData.Append("\r\n");
                        index++;
                    }

                    foreach (DataRow dataRow in dr1.Table.Rows)
                    {
                        string strRow = string.Empty;
                        for (int x = 0; x <= headerCount - 1; x++)
                        {
                            strRow += "\"" + dataRow[x].ToString() + "\"";

                            if (x < headerCount - 1 && x >= 0)
                                strRow += ",";
                        }
                        csvFileData.Append(strRow + "\r\n");
                    }

                    File.WriteAllText(outCsvFile1, csvFileData.ToString());
                }


                //code found on internet static class use
                using (StreamWriter writer = new StreamWriter(outCsvFile2))
                {
                    Rfc4180Writer.WriteDataTable(ds2, writer, true);
                }
                conn.Close();

                lblErrorTxt.Content = "Files have been sucessfully exported.";
            }
            catch (Exception ex)
            {
                lblErrorTxt.Content = ex.ToString();
            }
        }
    }
}
