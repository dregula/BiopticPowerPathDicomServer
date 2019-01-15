using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BiopticPowerPathDicomServer
{
    //PRESENTER
    public class PowerPathConfigurationPresenter
    {
        readonly PPLoginFormView _ppLoginFormView;
        readonly PowerPathConfigurationModel _model;
        public PowerPathConfigurationPresenter(PPLoginFormView view)
        {
            _ppLoginFormView = view;
            _model = new PowerPathConfigurationModel();

            _ppLoginFormView.OnViewLoad += Load;
            _ppLoginFormView.OnValidateDbConnection += ValidateDbConnection;
            _ppLoginFormView.OnQuit += Quit;
        }

        private void Load()
        {
            _ppLoginFormView.serverlogin = _model.PowerPathConfiguration;
        }
        //TODO: this should set the model databacking?
        private void LoadConfig(PowerPathConfigurationViewModel ppc)
        {
            _ppLoginFormView.serverlogin = _model.PowerPathConfiguration;
        }

        private void ValidateDbConnection()
        {
            _model.ValidateDbConnection();
        }

        private void Quit()
        {
            _model.Quit();
        }
    }
}
