﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;

namespace BiopticPowerPathDicomServer
{
    public class PowerPathLoginConfig
    {
        private SqlConnectionStringBuilder builder;
        private List<string> listServers = new List<string>();
        private List<string> listDatabases = new List<string>();

        #region "Properties"
        private bool validdbconnection;
        public bool ValidDbConnection
        {
            get { return validdbconnection; }
            set { validdbconnection =value; }
        }

        private string scheduledexamstable;

        public string ScheduledExamsTable
        {
            get { return scheduledexamstable; }
            set { scheduledexamstable = value; }
        }

        public SqlConnectionStringBuilder Builder
        {
            get { return builder; }
            set { builder = value; }
        }
        public PowerPathLoginConfig Copy()
        {
            PowerPathLoginConfig cp = new PowerPathLoginConfig(builder);
            cp.ListDatabases = new List<string>(listDatabases);
            cp.ListServers = new List<string>(listServers);
            cp.ValidDbConnection = this.ValidDbConnection;
            return cp;
        }
        //TODO: reconcile Server property and ListServers property
        public List<string> ListServers
        {
            get { return listServers; }
            set { listServers = value; }
        }
        public List<string> ListDatabases
        {
            get { return listDatabases; }
            set { listDatabases = value; }
        }

        public string ConnectionString
        {
            get { return builder.ConnectionString; }
            set { builder.ConnectionString = value; }
        }
        public string DataSource
        {
            get { return builder.DataSource; }
            set { builder.DataSource = value; }
        }
        public string UserID
        {
            get { return builder.UserID; }
            set { builder.UserID = value; }
        }
        public string Password
        {
            get { return builder.Password; }
            set { builder.Password = value; }
        }
        public string InitialCatalog
        {
            get { return builder.InitialCatalog; }
            set { builder.InitialCatalog = value; }
        }
        public string WorkstationID
        {
            get { return builder.WorkstationID; }
            set { builder.WorkstationID = value; }
        }
        public bool IntegratedSecurity
        {
            get { return builder.IntegratedSecurity; }
            set { builder.IntegratedSecurity = value; }
        }
        #endregion

        #region "Constructors"
        public PowerPathLoginConfig()
        {

        }
        public PowerPathLoginConfig(string connectionString)
        {
            builder = new SqlConnectionStringBuilder(connectionString);
        }
        public PowerPathLoginConfig(SqlConnectionStringBuilder Builder)
        {
            builder = new SqlConnectionStringBuilder(Builder.ConnectionString);
        }
        #endregion
    }
}
