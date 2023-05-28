using System.Collections.Generic;
using System.Web.Mvc;

namespace Web.Models.Utilities
{
    public static class UTLAlert
    {
        public const string TempDataKey = "TempDataAlerts";
        public static void Success(this Controller controller, string message, string iconClass = "", bool dismissable = false)
        {
            AddAlert(controller, AlertStyles.Success, message, iconClass, dismissable);
        }
        public static void Information(this Controller controller, string message, string iconClass = "", bool dismissable = false)
        {
            AddAlert(controller, AlertStyles.Information, message, iconClass, dismissable);
        }
        public static void Warning(this Controller controller, string message, string iconClass = "", bool dismissable = false)
        {
            AddAlert(controller, AlertStyles.Warning, message, iconClass, dismissable);
        }
        public static void Danger(this Controller controller, string message,string iconClass="", bool dismissable = false)
        {
            AddAlert(controller, AlertStyles.Danger, message,iconClass, dismissable);
        }
        private static void AddAlert(this Controller controller, string alertStyle, string message,string iconClass, bool dismissable)
        {
            var alerts = controller.TempData.ContainsKey(UTLAlert.TempDataKey)
                ? (List<UtlAlertVIewModel>)controller.TempData[UTLAlert.TempDataKey]
                : new List<UtlAlertVIewModel>();

            var messageText = message;
            if (iconClass!="")
            {
                messageText = $"{message} <i class='{iconClass}'></i>";
            }
            alerts.Add(new UtlAlertVIewModel
            {
                AlertStyle = alertStyle,
                Message = messageText,
                Dismissable = dismissable
            });

            controller.TempData[UTLAlert.TempDataKey] = alerts;
        }
        internal class AlertStyles
        {
            public const string Success = "success";
            public const string Information = "info";
            public const string Warning = "warning";
            public const string Danger = "danger";
        }
    }
    public class UtlAlertVIewModel
    {
        public const string TempDataKey = "TempDataAlerts";
        public string AlertStyle { get; set; }
        public string Message { get; set; }
        public bool Dismissable { get; set; }
    }
     
    public class UtlAlertJsonModel
    {
        public string Message { get; set; }
        public bool Success{ get; set; }

    }

}
