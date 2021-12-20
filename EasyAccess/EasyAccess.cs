/**
 *Access database genarate operation class
 *Author: jinwenfu@gmail.com
 *Create Time: 2011-03-16
 */
using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using System.Data.OleDb;
using System.Configuration;
using System.Web;
using System.IO;


namespace DBUtility
{
    public class EasyAccess
    {                
        /// <summary>
        /// connection string for access database
        /// </summary>
        private string _connectionString;

        /// <summary>
        /// save the connection to DB
        /// </summary>
        protected OleDbConnection _connection;

        /// <summary>
        /// Constructor function:assignment value to connectionString
        /// </summary>
        /// 
        public EasyAccess()
        {
            string connectionString = "Provider=Microsoft.Jet.OLEDB.4.0;";
            connectionString += "Data Source=.\\data530.mdb;ACE.OLEDB:Database;";
            connectionString += "Persist Security Info=False";
            //connectionString = "Provider=Microsoft.ACE.OLEDB.12.0; Data Source =.\\data530.accdb;";
            connectionString = "Provider=Microsoft.ACE.OLEDB.12.0; Data Source =.\\data530.mdb;";
            _connectionString = connectionString;
            _connection = new OleDbConnection(_connectionString);

            /*
            string connectionString = "Provider=Microsoft.ACE.OLEDB.12.0;";
            connectionString += "Data Source=.\\data530.mdb;Jet.OLEDB:Database;";
            connectionString += "Persist Security Info=False";
          //connectionString = "Provider=Microsoft.ACE.OLEDB.12.0; Data Source =.\\data530.accdb;";
           connectionString = "Provider=Microsoft.Jet.OLEDB.4.0; Data Source =.\\data530.mdb;";
            _connectionString = connectionString;
            _connection = new OleDbConnection(_connectionString);
            */
        }

         ~EasyAccess()
        {
            this.CloseConnection();
        }

        /// <summary>
        /// Execute SQL that do not return object. Exp:delete,reflash,insert
        /// </summary>
        /// <param name="SQL_"></param>
        /// <returns>the flag of transaction result
        public bool ExecuteNoneQuery(string SQL_)
        {
            bool rst = false;
            if (_connection.State == ConnectionState.Closed)
            {
                _connection.Open();
            }
            OleDbTransaction transaction = _connection.BeginTransaction();
            OleDbCommand command = new OleDbCommand(SQL_, _connection, transaction);

            try
            {
                command.ExecuteNonQuery();
                transaction.Commit();
                rst = true;
            }
            catch (Exception ex)
            {
                transaction.Rollback();
                rst = false;
            }
            finally
            {
                _connection.Close();
            }
            return rst;
        }

        /// <summary>
        /// Execute SQL and response a DataReader
        /// </summary>
        /// <param name="SQL_"></param>
        /// <returns>dataReader</returns>
        public OleDbDataReader ExecuteDataReader(string SQL_)
        {
            //try
            //{
                if(_connection.State == ConnectionState.Closed)
                {
                    _connection.Open();
                }
                OleDbCommand command = new OleDbCommand(SQL_, _connection);
                OleDbDataReader dataReader = command.ExecuteReader();
                //_connection.Close();
                return dataReader;
            //}
           // catch (Exception ex)
           // {                
           //     return null;
           // }        
        }

        /// <summary>
        /// Execute SQL and return the result in the form of dataSet
        /// </summary>
        /// <param name="SQL_"></param>
        /// <returns>DataSet</returns>
        public DataSet ExecuteDataSet(string SQL_)
        {
            try
            {
                _connection.Open();
                DataSet dataSet = new DataSet();
                OleDbDataAdapter OLEDbDA = new OleDbDataAdapter(SQL_, _connection);
                OLEDbDA.Fill(dataSet, "myDataSet");
                _connection.Close();
                return dataSet;
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        /// <summary>
        /// Close the connection to the database
        /// </summary>
        /// <param></param>
        /// <return></return>
        public void CloseConnection()
        {
            try
            {
                _connection.Close();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message.ToString());
                return;
            }
        }

    }
}
