using log4net;
using SIPSorcery.GB28181.Servers.Cores;
using SIPSorcery.GB28181.SIP;
using SIPSorcery.GB28181.SIP.App;
using SIPSorcery.GB28181.Sys;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SIPSorcery.GB28181.Servers.SIPMessage
{
    /// <summary>
    /// sip消息核心处理(版本2016)
    /// </summary>
    public class SIPMessageCore_2016 : MessageBase
    {
        private static ILog logger = AppState.logger;
        private SIPAuthenticationHeader _auth;
        private System.Timers.Timer _regTimer;
        private int _regInterval = 10;

        public SIPMessageCore_2016()
        {
            _regTimer = new System.Timers.Timer();
        }

        public override void Initialize(SIPAuthenticateRequestDelegate sipRequestAuthenticator, SIPAccount account, SIPTransport transport)
        {
            PlatformType = account.PlatformType;
            LocalSIPId = account.LocalID;
            LocalEndPoint = new SIPEndPoint(SIPProtocolsEnum.udp, account.LocalIP, account.LocalPort);
            RemoteSIPId = account.RemoteID;
            RemoteEndPoint = new SIPEndPoint(SIPProtocolsEnum.udp, account.RemoteIP, account.RemotePort);
            Transport = transport;
            if (PlatformType == Cores.PlatformType.UpPlatform)
            {
                Start();
            }
        }

        public override void AddMessageRequest(SIPEndPoint localEndPoint, SIPEndPoint remoteEndPoint, SIPRequest request)
        {

        }

        public override void AddMessageResponse(SIPEndPoint localSIPEndPoint, SIPEndPoint remoteEndPoint, SIPResponse response)
        {
            if (response.Status == SIPResponseStatusCodesEnum.Unauthorised)
            {
                logger.Info("需要提供身份认证字段" + remoteEndPoint.ToHost());
                //安全身份认证
                SIPAuthorisationDigest digest = new SIPAuthorisationDigest(SIPAuthorisationHeadersEnum.Authorize);
                digest.Username = response.Header.AuthenticationHeader.SIPDigest.Username;
                digest.Password = "12345678";
                digest.Realm = response.Header.AuthenticationHeader.SIPDigest.Realm;
                digest.Nonce = response.Header.AuthenticationHeader.SIPDigest.Nonce;
                digest.Response = digest.Digest;
                string md5Pass = digest.ToString();
                SIPAuthenticationHeader auth = new SIPAuthenticationHeader(digest);
                _auth = auth;
                RegisterToPlatform();
            }
        }

        public override void DeviceCatalogQuery()
        {

        }

        private void Start()
        {
            _regTimer.Interval = _regInterval * 1000;
            _regTimer.Enabled = true;
            _regTimer.Elapsed += _regTimer_Elapsed;
            _regTimer.Start();
            RegisterToPlatform();
        }

        void _regTimer_Elapsed(object sender, System.Timers.ElapsedEventArgs e)
        {
            RegisterToPlatform();
        }

        /// <summary>
        /// 向上级平台注册
        /// </summary>
        private void RegisterToPlatform()
        {
            string fromTag = CallProperties.CreateNewTag();
            string callId = CallProperties.CreateNewCallId();
            int cSeq = CallProperties.CreateNewCSeq();
            string branch = CallProperties.CreateBranchId();

            SIPURI localUri = new SIPURI(LocalSIPId, LocalEndPoint.ToHost(), "");
            SIPURI remoteUri = new SIPURI(RemoteSIPId, RemoteEndPoint.ToHost(), "");
            SIPRequest registerReq = new SIPRequest(SIPMethodsEnum.REGISTER, remoteUri);

            SIPViaHeader via = new SIPViaHeader(LocalEndPoint, branch);
            SIPViaSet viaSet = new SIPViaSet();
            viaSet.PushViaHeader(via);

            SIPFromHeader from = new SIPFromHeader(null, localUri, fromTag);
            SIPToHeader to = new SIPToHeader(null, localUri, null);
            SIPHeader header = new SIPHeader(from, to, cSeq, callId);

            registerReq.Header = header;

            SIPContactHeader contact = new SIPContactHeader(null, localUri);
            header.Contact = new List<SIPContactHeader>();
            header.Contact.Add(contact);

            header.Vias = viaSet;

            header.AuthenticationHeader =_auth;

            header.Expires = 3600;
            header.CSeqMethod = SIPMethodsEnum.REGISTER;
            header.MaxForwards = 70;
            header.ContentLength = 0;
            header.UserAgent = SIPConstants.SIP_SERVER_STRING;
            Transport.SendRequest(RemoteEndPoint, registerReq);
        }
    }
}
