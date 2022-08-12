// ****************************************************************************
// 
// Copyright (C) 2005-2018 Doom9 & al
// 
// This program is free software; you can redistribute it and/or modify
// it under the terms of the GNU General Public License as published by
// the Free Software Foundation; either version 2 of the License, or
// (at your option) any later version.
// 
// This program is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY; without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
// GNU General Public License for more details.
// 
// You should have received a copy of the GNU General Public License
// along with this program; if not, write to the Free Software
// Foundation, Inc., 59 Temple Place, Suite 330, Boston, MA  02111-1307  USA
// 
// ****************************************************************************

using System;
using System.Net;

namespace MeGUI.core.util
{
    internal class HttpProxy
    {
        #region Public Methods and Operators

        public static IWebProxy GetProxy(MeGUISettings settings)
        {
            // if None then return null
            if (settings.HttpProxyMode == ProxyMode.None)
            {
                return null;
            }

            // if SystemProxy then return the System proxy details with the logged in credentials
            if (settings.HttpProxyMode == ProxyMode.SystemProxy)
            {
                var systemProxy = WebRequest.GetSystemWebProxy();
                systemProxy.Credentials = CredentialCache.DefaultCredentials;
                return systemProxy;
            }

            // CustomProxy and CustomProxyWithLogin both require a Url
            if (String.IsNullOrEmpty(settings.HttpProxyAddress))
            {
                return null;
            }

            var address = string.IsNullOrEmpty(settings.HttpProxyPort)
                              ? settings.HttpProxyAddress
                              : string.Format("{0}:{1}", settings.HttpProxyAddress, settings.HttpProxyPort);

            // if CustomProxyWithLogin then generate the credentials
            ICredentials credentials;
            if (settings.HttpProxyMode == ProxyMode.CustomProxy || string.IsNullOrEmpty(settings.HttpProxyUid))
            {
                credentials = null;
            }
            else
            {
                credentials = new NetworkCredential(settings.HttpProxyUid, settings.HttpProxyPwd);
            }

            var proxy = new WebProxy(address, true, null, credentials);

            return proxy;
        }

        #endregion
    }
}