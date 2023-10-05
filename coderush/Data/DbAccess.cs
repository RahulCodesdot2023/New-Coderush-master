using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;

namespace coderush.Data
{
    public class DbAccess
    {
        public static DataSet RunSelectStatement(string query, string ConnectionString)
        {
            DataSet dataSet = new DataSet();
            SqlConnection sqlConnection = new SqlConnection(ConnectionString);
            SqlCommand sqlCommand = new SqlCommand(query);
            sqlCommand.CommandType = CommandType.Text;
            sqlCommand.Connection = sqlConnection;
            SqlDataAdapter sqlDataAdapter = new SqlDataAdapter();
            try
            {
                sqlConnection.Open();
                sqlDataAdapter.SelectCommand = sqlCommand;
                sqlDataAdapter.Fill(dataSet);
            }
            catch (Exception ex)
            {
            }
            finally
            {
                sqlConnection.Close();
                sqlCommand.Dispose();
                sqlDataAdapter.Dispose();
            }
            return dataSet;
        }

        public static DataSet ExecuteStoreProc(string procName, string ConnectionString, Dictionary<string, object> parameters = null)
        {
            DataSet dataSet = new DataSet();
            SqlConnection sqlConnection = new SqlConnection(ConnectionString);
            SqlCommand sqlCommand = new SqlCommand(procName);
            sqlCommand.CommandType = CommandType.StoredProcedure;
            sqlCommand.Connection = sqlConnection;
            if (parameters != null)
            {
                foreach (KeyValuePair<string, object> parameter in parameters)
                    sqlCommand.Parameters.Add(new SqlParameter(parameter.Key, parameter.Value));
            }
            SqlDataAdapter sqlDataAdapter = new SqlDataAdapter();
            try
            {
                sqlConnection.Open();
                sqlDataAdapter.SelectCommand = sqlCommand;
                sqlDataAdapter.Fill(dataSet);
            }
            catch (Exception ex)
            {
            }
            finally
            {
                sqlConnection.Close();
                sqlCommand.Dispose();
                sqlDataAdapter.Dispose();
            }
            return dataSet;
        }

        public static object ExecuteNonQuery(string query, string ConnectionString)
        {
            object obj = new object();
            SqlConnection sqlConnection = new SqlConnection(ConnectionString);
            SqlCommand sqlCommand = new SqlCommand(query);
            sqlCommand.CommandType = CommandType.Text;
            sqlCommand.Connection = sqlConnection;
            try
            {
                sqlConnection.Open();
                obj = sqlCommand.ExecuteScalar();
            }
            catch (Exception ex)
            {
            }
            finally
            {
                sqlConnection.Close();
                sqlCommand.Dispose();
            }
            return obj;
        }

        public static object ExecuteScalar(string query, string ConnectionString)
        {
            object obj = new object();
            SqlConnection sqlConnection = new SqlConnection(ConnectionString);
            SqlCommand sqlCommand = new SqlCommand(query);
            sqlCommand.CommandType = CommandType.Text;
            sqlCommand.Connection = sqlConnection;
            try
            {
                sqlConnection.Open();
                obj = sqlCommand.ExecuteScalar();
            }
            catch (Exception ex)
            {
            }
            finally
            {
                sqlConnection.Close();
                sqlCommand.Dispose();
            }
            return obj;
        }

        public static DataSet ExecuteStoredProc(string procName, Dictionary<string, object> parameters = null, string connectionstring = null)
        {
            DataSet result = new DataSet();
            if (!string.IsNullOrEmpty(connectionstring))
            {
                SqlConnection con = new SqlConnection(connectionstring);
                SqlCommand cmd = new SqlCommand(procName);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Connection = con;
                if (parameters != null)
                {
                    foreach (var item in parameters)
                    {
                        cmd.Parameters.Add(new SqlParameter(item.Key, item.Value));
                    }
                }
                SqlDataAdapter sda = new SqlDataAdapter();
                try
                {
                    con.Open();
                    sda.SelectCommand = cmd;
                    sda.Fill(result);
                }
                catch
                {
                    throw;
                }
                finally
                {
                    con.Close();
                    cmd.Dispose();
                    sda.Dispose();
                }
            }
            return result;
        }

        public static List<T> ConvertDataTableToList<T>(DataTable dt)
        {
            List<T> data = new List<T>();
            foreach (DataRow row in dt.Rows)
            {
                T item = GetItem<T>(row);
                data.Add(item);
            }
            return data;
        }

        public static T GetItem<T>(DataRow dr)
        {
            Type temp = typeof(T);
            T obj = Activator.CreateInstance<T>();
            foreach (DataColumn column in dr.Table.Columns)
            {
                foreach (System.Reflection.PropertyInfo pro in temp.GetProperties())
                {
                    if (pro.Name == column.ColumnName)
                        if (dr[column.ColumnName] != System.DBNull.Value && column.DataType.Name != "Byte[]")
                            pro.SetValue(obj, dr[column.ColumnName], null);
                        else
                            continue;
                }
            }
            return obj;
        }
    }
}
