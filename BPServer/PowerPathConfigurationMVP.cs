using Common.Logging;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data.SqlClient;
using System.Windows.Forms;

namespace BiopticPowerPathDicomServer
{
     // ViewModel
    public class PowerPathConfigurationViewModel : INotifyPropertyChanged
    {
        //TODO: where does this deep-copy method belong?
        public PowerPathConfigurationViewModel CopyPowerPathConfiguration()
        {
            PowerPathConfigurationViewModel cp = new PowerPathConfigurationViewModel();
            cp.Builder = this.Builder;
            cp.ListDatabases = new List<string>(this.ListDatabases);
            cp.ListServers = new List<string>(this.ListServers);
            cp.ValidDbConnection = this.ValidDbConnection;
            return cp;
        }

        //TODO: refactor database specific stuff
        #region "Extract the database specific stuff to another class"
        private bool _validdbconnection;
        public bool ValidDbConnection
        {
            get
            {
                return _validdbconnection;
            }
            set
            {
                    if (value != _validdbconnection)
                    {
                        _validdbconnection = value;
                        NotifyPropertyChanged("ValidDbConnection");
                    }
            }
        }
        private string _scheduledexamstable;
        public string ScheduledExamsTable
        {
            get
            {
                return _scheduledexamstable;
            }
            set
            {
                if (value != _scheduledexamstable)
                {
                    _scheduledexamstable = value;
                    NotifyPropertyChanged("ScheduledExamsTable");
                }
            }
        }
        private SqlConnectionStringBuilder _builder;
        public SqlConnectionStringBuilder Builder
        {
            get
            {
                    return _builder;
            }
            set
            {
                    if (value != _builder)
                    {
                        _builder = value;
                        NotifyPropertyChanged("Builder");
                    }
            }
        }
        public string ConnectionString
        {
            get { return _builder.ConnectionString; }
            set
            {
                if (value != _builder.ConnectionString)
                {
                    _builder.ConnectionString = value;
                    NotifyPropertyChanged("ConnectionString");
                }
            }
        }
        public string WorkstationID
        {
            get { return _builder.WorkstationID; }
            set
            {
                if (value != _builder.WorkstationID)
                {
                    _builder.WorkstationID = value;
                    NotifyPropertyChanged("WorkstationID");
                }
            }
        }
        public bool IntegratedSecurity
        {
            get { return _builder.IntegratedSecurity; }
            set
            {
                if (value != _builder.IntegratedSecurity)
                {
                    _builder.IntegratedSecurity = value;
                    NotifyPropertyChanged("IntegratedSecurity");
                }
            }
        }
        #endregion

        public event PropertyChangedEventHandler PropertyChanged;
        private void NotifyPropertyChanged(String info)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(info));
            }
        }

        #region "Properties"
        //TODO: reconcile Server property and ListServers property
        private List<string> _listServers;
        public List<string> ListServers
        {
            get { return _listServers; }
            set
            {
                if (value != _listServers)
                {
                    _listServers = value;
                    NotifyPropertyChanged("ListServers");
                }
            }
        }
        public string DataSource
        {
            get { return _builder.DataSource; }
            set
            {
                if (value != _builder.DataSource)
                {
                    _builder.DataSource = value;
                    NotifyPropertyChanged("DataSource");
                }
            }
        }
        private List<string> _listDatabases;
        public List<string> ListDatabases
        {
            get { return _listDatabases; }
            set
            {
                if (value != _listDatabases)
                {
                    _listDatabases = value;
                    NotifyPropertyChanged("ListDatabases");
                }
            }
        }
        public string InitialCatalog
        {
            get { return _builder.InitialCatalog; }
            set
            {
                if (value != _builder.InitialCatalog)
                {
                    _builder.InitialCatalog = value;
                    NotifyPropertyChanged("InitialCatalog");
                }
            }
        }
        public string UserID
        {
            get { return _builder.UserID; }
            set
            {
                if (value != _builder.UserID)
                {
                    _builder.UserID = value;
                    NotifyPropertyChanged("UserID");
                }
            }
        }
        public string Password
        {
            get { return _builder.Password; }
            set
            {
                if (value != _builder.Password)
                {
                    _builder.Password = value;
                    NotifyPropertyChanged("Password");
                }
            }
        }
        #endregion
    }   // end-ViewModel
}

