using ActioBP.Security;
using Microsoft.Extensions.Configuration;
using System;
using System.ServiceModel;

namespace ActioBP.General.WS
{
    public class ConnectionClassWS
    {
        public IConfiguration _config;
        private bool sw_validation_encrypted = false;
        public string BASE_WS_CONFIG { get; set; }
        public string Company { get; set; }

        public ConnectionClassWS(string baseWsConfig)
        {
            this.BASE_WS_CONFIG = baseWsConfig;
        }
        protected string getUrlService(string Servicio, IConfiguration config, string company = null, bool defaultCompanyIfEmpty = true, bool isCodeUnit = false, bool isSystem = false)
        {
            this._config = config;
            string sURL, sServer, sDataNAV, sUrlWS, sEmpresa;
            sServer = _config[$"{BASE_WS_CONFIG}:Servidor"];
            sDataNAV = _config[$"{BASE_WS_CONFIG}:BaseNAV"];
            sUrlWS = _config[$"{BASE_WS_CONFIG}:UrlWS"];
            sEmpresa = _config[$"{BASE_WS_CONFIG}:EmpresaWS"];

            sURL = ((isSystem)?"":(isCodeUnit) ? "Codeunit/" : "Page/") + Servicio;

            if (string.IsNullOrEmpty(company))
            {
                company = string.Empty;
                if (defaultCompanyIfEmpty) company = sEmpresa;
            }
            sEmpresa = company;
            Company = sEmpresa;

            sUrlWS = sUrlWS.Replace("[ser]", sServer).Replace("[bd]", sDataNAV).Replace("[emp]", sEmpresa);
            sUrlWS += sURL;

            return sUrlWS;
        }

        protected System.Net.NetworkCredential GetCredentials()
        {
            System.Net.NetworkCredential oPort = null;

            // Cada servicio Web XML necesita un espacio de nombres único para que las aplicaciones de cliente puedan distinguir este servicio de otros 
            // servicios del Web. http://tempuri.org/ está disponible para servicios Web XML que están en desarrollo, pero los servicios Web XML publicados deberían 
            // utilizar un espacio de nombres más permanente.
            Uri uri = new Uri("http://tempuri.org/"); // 

            string userWS, passWS, domainWS;
            userWS = _config[$"{BASE_WS_CONFIG}:UserWS"];
            passWS = _config[$"{BASE_WS_CONFIG}:PassWS"];
            domainWS = _config[$"{BASE_WS_CONFIG}:DomainWS"];

            if (sw_validation_encrypted)
            {
                userWS = Sec.DesencriptarCadenaDeCaracteres(userWS);
                passWS = Sec.DesencriptarCadenaDeCaracteres(passWS);
                domainWS = Sec.DesencriptarCadenaDeCaracteres(domainWS);
            }
            // TODO encriptar credenciales en Web.Config
            System.Net.ICredentials credentials = new System.Net.NetworkCredential(userWS, passWS, domainWS);

            oPort = credentials.GetCredential(uri, "Windows");

            return oPort;
        }

        #region
        protected void DefineClientCredentials(System.ServiceModel.Description.ClientCredentials client)
        {
            switch (GetCredentialMode())
            {
                case HttpClientCredentialType.Basic:
                default:

                    client.UserName.UserName = _config[$"{BASE_WS_CONFIG}:UserWS"];
                    client.UserName.Password = _config[$"{BASE_WS_CONFIG}:PassWS"];
                    break;
                case HttpClientCredentialType.Ntlm:
                case HttpClientCredentialType.Windows:
                    client.Windows.ClientCredential = GetCredentials();
                    break;
            }
        }

        #endregion
        #region variables
        protected HttpClientCredentialType GetCredentialMode()
        {
            HttpClientCredentialType credentialMode = HttpClientCredentialType.Basic;
            Enum.TryParse<HttpClientCredentialType>(_config[$"{BASE_WS_CONFIG}:CredentialMode"], out credentialMode);
            return credentialMode;
        }
        protected BasicHttpSecurityMode GetHttpMode()
        {
            var httpMode = BasicHttpSecurityMode.TransportCredentialOnly;

            Enum.TryParse<BasicHttpSecurityMode>(_config[$"{BASE_WS_CONFIG}:httpMode"], out httpMode);
            return httpMode;
        }
        #endregion
        protected BasicHttpBinding GetBinding()
        {
            return GetBinding(GetHttpMode(), GetCredentialMode());            
        }
        protected BasicHttpBinding GetBinding(BasicHttpSecurityMode httpMode, HttpClientCredentialType credentialMode)
        {
            var binding = new BasicHttpBinding(httpMode);
            binding.OpenTimeout = binding.CloseTimeout =
            binding.SendTimeout = binding.ReceiveTimeout = TimeSpan.FromMinutes(1);
            binding.MaxReceivedMessageSize = int.MaxValue; //20971520;
            binding.MaxBufferPoolSize = binding.MaxBufferSize = int.MaxValue; //20971520;
            binding.ReaderQuotas.MaxArrayLength =
            binding.ReaderQuotas.MaxStringContentLength =
            binding.ReaderQuotas.MaxBytesPerRead =
            binding.ReaderQuotas.MaxNameTableCharCount = 2097152;

            binding.TextEncoding = System.Text.Encoding.UTF8;
            binding.Security.Transport.ClientCredentialType = credentialMode;
            //binding.Security.Transport.ProxyCredentialType = HttpProxyCredentialType.Ntlm;
            binding.TransferMode = TransferMode.Buffered;
            binding.AllowCookies = false;

            return binding;
        }
    }
}
