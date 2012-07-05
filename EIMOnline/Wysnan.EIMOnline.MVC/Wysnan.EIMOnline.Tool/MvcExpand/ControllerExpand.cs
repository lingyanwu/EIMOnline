using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Mvc;

namespace Wysnan.EIMOnline.Tool.MvcExpand
{
    public static class ControllerExpand
    {
        public static ActionResult Alert(this Controller controller, string message, string script = null, MessageType messageType = MessageType.Ok)
        {
            string str=Alert(message, null, null, script, messageType);
            JavaScriptResult result = new JavaScriptResult();
            result.Script = str;
            return result;
        }

        private static string Alert(string message, string controller, string action, string script = null, MessageType messageType = MessageType.Ok)
        {
            StringBuilder messageStr = new StringBuilder();
            messageStr.Append("<script>");
            messageStr.Append("$(function () {$(\"#dialog:ui-dialog\" ).dialog(\"destroy\");");
            messageStr.Append("$(\"#dialog-message\").dialog({autoOpen: true, modal: true");
            switch (messageType)
            {
                case MessageType.Ok:
                    messageStr.Append(", buttons: {Ok: function() {");
                    if (!string.IsNullOrEmpty(controller))
                    {
                        messageStr.AppendFormat("window.location.href=\"/{0}/{1}\";", controller, action);
                    }
                    messageStr.Append("$(this).dialog( \"close\" );}}");
                    break;
                case MessageType.YesOrNo:
                    messageStr.Append(",buttons:{\"Yes\": function(){");
                    messageStr.Append("$(this).dialog( \"close\");},");
                    messageStr.Append("Cancel:function(){$(this).dialog(\"close\" );}}");
                    break;
            }
            messageStr.Append("});});");
            if (!string.IsNullOrEmpty(script))
            {
                messageStr.Append(script);
            }
            messageStr.Append("</script>");
            messageStr.Append("<div id=\"dialog-message\" title=\"提示\" style=\"display:none\">");
            messageStr.AppendFormat("<p>{0}</p>", message);
            messageStr.Append("</div>");
            return messageStr.ToString();
        }
    }

    public class Message
    {
        public Message()
        {

        }

        public Message(string message, string controller, string action, MessageType messageType)
        {
            this.Msg = message;
            this.Controller = controller;
            this.Action = action;
            this.MessageType = messageType;
        }

        public string Msg { get; set; }

        public string Controller { get; set; }

        public string Action { get; set; }

        private MessageType type = MessageType.Ok;
        public MessageType MessageType { get { return type; } set { type = value; } }
    }

    public enum MessageType
    {
        Ok,
        YesOrNo
    }
}
