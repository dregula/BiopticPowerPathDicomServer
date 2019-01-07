using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ReactiveUI;

namespace BiopticPowerPathDicomServer.ViewModels
{
    public class  PPLoginViewModel : ReactiveObject
    {
        //TODO: this belongs somewhere
        //TODO: REFACTOR db can be opened, but without an initial catalog 
/*        var databases = db.GetSchema("Databases");
        if (databases?.Rows != null)
        {
            foreach (System.Data.DataRow row in databases.Rows)
            {
                this.AvailableDatabases.Add(row.Field<string>(@"database_name"));
            }
        } */

        private SqlConnectionStringBuilder builder;
        public SqlConnectionStringBuilder Builder
        {
            get
            {
                return builder;
            }
            set
            {
                builder = value;
            }
        }
//public SqlConnectionStringBuilder CopyOfBuilder()
//{
//    return new SqlConnectionStringBuilder(builder.ConnectionString);
//}

        public string ConnectionString
        {
            get { return builder.ConnectionString; }
        }
        public string DataSource
        {
            get { return builder.DataSource; }
            set
            {
                string ds = "";
                this.RaiseAndSetIfChanged(ref ds, value);
                builder.DataSource = ds;
            }
        }
        public string UserID
        {
            get { return builder.UserID; }
            set
            {
                string uid = "";
                this.RaiseAndSetIfChanged(ref uid, value);
                builder.UserID = uid;
            }
        }
        public string Password
        {
            get { return builder.Password; }
            set
            {
                string p = "";
                this.RaiseAndSetIfChanged(ref p, value);
                builder.Password = p;
            }
        }

        public string InitialCatalog
        {
            get { return builder.InitialCatalog; }
            set
            {
                string ic = "";
                this.RaiseAndSetIfChanged(ref ic, value);
                builder.InitialCatalog = ic;
            }
        }

        private List<string> recentServers;
        public List<string> RecentServers
        {
            get { return recentServers; }
            set { this.RaiseAndSetIfChanged(ref recentServers, value); }
        }

        private List<string> availableDatabases;
        public List<string> AvailableDatabases
        {
            get { return recentServers; }
            set { this.RaiseAndSetIfChanged(ref availableDatabases, value); }
        }

        //static-type error        public ReactiveCommand OKCmd { get; private set; }

        //this.confirm = new Interaction<string, bool>();
        //            var OKCmd = ReactiveCommand.Create(() => { Status = EnteredText + " is saved."; }
        //            , this.WhenAny(vm => vm.EnteredText, s => !string.IsNullOrWhiteSpace(s.Value)));
    }

    /*            public Interaction<string, bool> Confirm => this.confirm;

                public async Task DeleteFileAsync()
                {
                    var fileName = "";

                // this will throw an exception if nothing handles the interaction
                var delete = true; // await this.confirm.Handle(fileName);

                    if (delete)
                    {
                        // delete the file
                    }
                }
            }
    */
    /*        public class View
            {
                public View()
                {
                    this.WhenActivated(
                        d =>
                        {
                            d(this
                                .ViewModel
                                .Confirm
                                .RegisterHandler(
                                    async interaction =>
                                    {
                                        var deleteIt = await this.DisplayAlert(
                                            "Confirm Delete",
                                            $"Are you sure you want to delete '{interaction.Input}'?",
                                            "YES",
                                            "NO");

                                        interaction.SetOutput(deleteIt);
                                    }));
                        });
                }
            }
        }
    */
}
