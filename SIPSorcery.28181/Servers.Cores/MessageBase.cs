using SIPSorcery.GB28181.Servers.SIPMessage;
using SIPSorcery.GB28181.SIP;
using SIPSorcery.GB28181.SIP.App;
using SIPSorcery.GB28181.Sys.Config;
using SIPSorcery.GB28181.Sys.XML;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SIPSorcery.GB28181.Servers.Cores
{
    /// <summary>
    /// 28181平台消息
    /// </summary>
    public class MessageBase
    {
        private SIPAuthenticateRequestDelegate _sipRequestAuthenticator;
        /// <summary>
        /// sip传输请求
        /// </summary>
        public SIPTransport Transport;
        /// <summary>
        /// 消息处理集合
        /// </summary>
        public Dictionary<string, MessageBase> MessageCore;
        /// <summary>
        /// 平台类型
        /// </summary>
        internal PlatformType PlatformType;
        /// <summary>
        /// 本地sip编码
        /// </summary>
        internal string LocalSIPId;
        /// <summary>
        /// 远程sip编码
        /// </summary>
        internal string RemoteSIPId;
        /// <summary>
        /// 本地sip终结点
        /// </summary>
        internal SIPEndPoint LocalEndPoint;
        /// <summary>
        /// 远程sip终结点
        /// </summary>
        internal SIPEndPoint RemoteEndPoint;
        protected string UserAgent;

        /// <summary>
        /// 设备目录接收
        /// </summary>
        public virtual event Action<Catalog, string> OnCatalogReceived;

        public MessageBase()
        {

        }

        public MessageBase(SIPAuthenticateRequestDelegate sipRequestAuthenticator, SIPTransport transport, string userAgent)
        {
            MessageCore = new Dictionary<string, MessageBase>();
            Transport = transport;
            UserAgent = userAgent;
            _sipRequestAuthenticator = sipRequestAuthenticator;
        }

        public virtual void Initialize(SIPAuthenticateRequestDelegate sipRequestAuthenticator, SIPAccount account, SIPTransport trasnport)
        {

        }

        /// <summary>
        /// sip请求消息
        /// </summary>
        /// <param name="localEndPoint">本地终结点</param>
        /// <param name="remoteEndPoint"b>远程终结点</param>
        /// <param name="request">sip请求</param>
        public virtual void AddMessageRequest(SIPEndPoint localEndPoint, SIPEndPoint remoteEndPoint, SIPRequest request)
        {
            MessageCore[remoteEndPoint.ToHost()].AddMessageRequest(localEndPoint, remoteEndPoint, request);
        }

        /// <summary>
        /// sip响应消息
        /// </summary>
        /// <param name="localSIPEndPoint">本地终结点</param>
        /// <param name="remoteEndPoint">远程终结点</param>
        /// <param name="response">sip响应</param>
        public virtual void AddMessageResponse(SIPEndPoint localSIPEndPoint, SIPEndPoint remoteEndPoint, SIPResponse response)
        {
            MessageCore[remoteEndPoint.ToHost()].AddMessageResponse(localSIPEndPoint, remoteEndPoint, response);
        }

        /// <summary>
        /// 设备目录查询
        /// </summary>
        public virtual void DeviceCatalogQuery()
        {

        }

        public virtual void Stop()
        {

        }

        public void Initialize(SIPAuthenticateRequestDelegate SIPAuthenticateRequest_External, List<SIPAccount> accounts)
        {
            foreach (var account in accounts)
            {
                MessageBase msg = null;
                switch (account.PlatformVersion)
                {
                    case PlatformVersion.VERSION_2011:
                        msg = new SIPMessageCore_2011();
                        break;
                    case PlatformVersion.VERSION_2014:
                        break;
                    case PlatformVersion.VERSION_2016:
                        msg = new SIPMessageCore_2016();
                        break;
                }
                msg.Initialize(_sipRequestAuthenticator, account, Transport);
                string msgKey = account.RemoteIP + ":" + account.RemotePort;
                MessageCore.Add(msgKey, msg);
            }
        }
    }
}
