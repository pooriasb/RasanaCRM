using System;
using System.Web;
using System.Web.Mvc;
using Web.Insfrastructure.ManagePermission;

public static class Utilities
{
    public static bool CheckPermission(int item, int userPermission)
    {
        if (item == userPermission)
        {
            return true;
        }
        return false;

    }

    public static short[] GetUserPermission(this Controller controller)
    {
        short[] UserPermition = null;
        var session = (short[])controller.HttpContext.Session["UserPermission"];
        if (session == null)
            UserPermition = null;


        return UserPermition;
    }
    public static IHtmlString PermissionHtml(this HtmlHelper helper, string html, short display, short enable, short disable, IManagePermission managePermission)
    {

        if (managePermission == null)
            throw new Exception();

        string Perhtml = "";
        return new HtmlString(Perhtml);

    }




}
