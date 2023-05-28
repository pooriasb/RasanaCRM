using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Web;
using System.Web.Mvc;

namespace Web.Insfrastructure.ManagePermission
{
    public interface IManagePermission
    {
        bool BinarySearch(short code);
        bool CheckAction(string area,string action,string controller);
        bool CheckFild(object obj,string name,PermissionCodeField codeFild);
        bool CheckField();
        //bool CheckAll(string action,string controller);
     
    }
    public enum PermissionCodeField
    {
        insert,
        update,
        delete,
        read
    }
    public class ManagePermission : IManagePermission
    {
       
     
        private short[] fildPer=null;
        public static short[] UserPermissions { get; private set; }
        public ManagePermission(Controller controller)
        {

            try
            {
                var session = controller.Session["permission"];
                if (session != null)
                    UserPermissions = (short[])session;
            }
            catch
            {
                UserPermissions = null;
            }
             
            
        }
        public ManagePermission(short[] userPermissions)
        {
            if (userPermissions != null)
                UserPermissions = userPermissions;
          
        }

        public bool BinarySearch(short code)
        {
            if (code < 0)
                return false;
            if (UserPermissions == null)
                return false;
            if (Array.BinarySearch(UserPermissions, code) > -1)
            {
                return true;
            }
            return false;
        }

        public bool CheckAction(string area,string action,string controller)
        {
            string item = "";
            if(area!=string.Empty)
           item= area+"_"+controller + "_" + action;
            else
            {
                item= controller + "_" + action;
            }
            
            if (item == "Account_Login")
                return true;
            try
            {
                var pers = Enum.Parse(typeof(SitePermistion), item.ToLower());
                if (pers != null)
                {
                    SitePermistion per = (SitePermistion)pers;
                    return BinarySearch((short)per);
                }
            }
            catch
            {

            }
      
            return true;

        }
        public bool CheckAction(short action)
        {
            return BinarySearch(action);
        }
        //public bool CheckAll(string action,string controller)
        //{
        //    return (CheckField() && CheckAction(action,controller));
        //}

        public bool CheckField()
        {
            if (fildPer == null)
                return false;

            foreach(short code in fildPer)
            {
                if (!BinarySearch(code))
                    return false;
            }
            return true;
        }
     
        public bool CheckFild(object obj,string name,PermissionCodeField codeFild)
        {
            if (string.IsNullOrEmpty(name))
                name = "";
            short code = GetCodeField(obj.GetType(),name,codeFild);
           
            return BinarySearch(code);
        }

      
        public short GetCodeField(System.Type item,string name,PermissionCodeField codeField)
        {
           
            PropertyInfo[] listProprty = item.GetProperties();
            foreach (PropertyInfo info in listProprty)
            {
                if (info.Name == name)
                {
                    object[] list = info.GetCustomAttributes(true);

                    foreach (var l in list)
                    {
                        CodeAttribute code = l as CodeAttribute;
                        if (code != null)
                        {
                            switch (codeField)
                            {
                                case PermissionCodeField.delete:
                                    return code.CodeDelete;

                                case PermissionCodeField.insert:
                                    return code.CodeInsert;
                                case PermissionCodeField.update:
                                    return code.CodeUpdate;
                                case PermissionCodeField.read:
                                    return code.CodeRead;
                            }

                        }

                    }
                }
                

            }
            return -1;
        }
    


    }
    public class CodeAttribute:System.Attribute
    {
        short codeInsert;
        short codeUpdate;
        short codeDelete;
        short codeRead;
    
        public CodeAttribute(short insert=-1, short update=-1, short delete=-1, short read=-1)
        {
            codeInsert = insert;
            codeUpdate = update;
           codeDelete = delete;
            codeRead = read;
        }
      
        public short CodeRead { get { return codeRead; } }
        public short CodeInsert { get { return codeInsert; } }
        public short CodeUpdate { get { return codeUpdate; } }
        public short CodeDelete { get { return codeDelete; } }

    }
    public enum SitePermistion
    {
        control_home = 0,
        home_index = 1,
        home_test,
        home_about,
        home_contact,
        table_user,
        user_name_insert,
        user_name_update,
        user_email_insert,
        user_email_delet,
        control_factor,
        factor_add,
        table_factor,
        factor_status_inset,
        control_employee_home,
        employee_home_index

    }
}