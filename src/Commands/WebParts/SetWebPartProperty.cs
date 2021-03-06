﻿using System;
using System.Management.Automation;
using Microsoft.SharePoint.Client;
using PnP.Framework.Utilities;

namespace PnP.PowerShell.Commands.WebParts
{
    [Cmdlet(VerbsCommon.Set, "WebPartProperty")]
    public class SetWebPartProperty : PnPWebCmdlet
    {
        [Parameter(Mandatory = true)]
        [Alias("PageUrl")]
        public string ServerRelativePageUrl = string.Empty;

        [Parameter(Mandatory = true)]
        public Guid Identity;

        [Parameter(Mandatory = true)]
        public string Key = string.Empty;

        [Parameter(Mandatory = true)]
        public PSObject Value = string.Empty;

        protected override void ExecuteCmdlet()
        {
            var serverRelativeWebUrl = SelectedWeb.EnsureProperty(w => w.ServerRelativeUrl);

            if (!ServerRelativePageUrl.ToLowerInvariant().StartsWith(serverRelativeWebUrl.ToLowerInvariant()))
            {
                ServerRelativePageUrl = UrlUtility.Combine(serverRelativeWebUrl, ServerRelativePageUrl);
            }

            if (Value.BaseObject is string)
            {
                SelectedWeb.SetWebPartProperty(Key, Value.ToString(), Identity, ServerRelativePageUrl);
            }
            else if (Value.BaseObject is int)
            {
                SelectedWeb.SetWebPartProperty(Key, (int)Value.BaseObject, Identity, ServerRelativePageUrl);
            } else if (Value.BaseObject is bool)
            {
                SelectedWeb.SetWebPartProperty(Key, (bool)Value.BaseObject, Identity, ServerRelativePageUrl);
            }
            else
            {
                ThrowTerminatingError(new ErrorRecord(new Exception("Type of value is not supported. Has to be of type string, int or bool"), "UNSUPPORTEDTYPE",ErrorCategory.InvalidType, this));
            }
        }
    }
}
