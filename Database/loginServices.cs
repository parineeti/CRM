using CRM.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;

namespace CRM.Database
{
    public class loginServices
    {
        public int Validatelogin(string userName, string password)
        {
            Global _global = new Database.Global();
            CRMClassDataContext _idc = new Database.CRMClassDataContext(_global.con);
            try
            {
                var id = _idc.tbllogins.Where(a => a.Email == userName && a.Password == password).ToList();
                if (id.Count() > 0)
                {
                    return id.FirstOrDefault().UserId;
                }
                else
                {
                    return 0;
                }
            }
            catch (Exception ex)
            {
                string path = HttpContext.Current.Server.MapPath("~/App_Data/error.text");

                if (!File.Exists(path))
                {
                    File.Create(path).Dispose();
                    using (TextWriter tw = new StreamWriter(path))
                    {
                        tw.WriteLine(ex.Message);
                        tw.Close();
                    }

                }

                return 0;
            }
            finally
            {
                _idc.Dispose();

            }
        }

        public int Validateregister(LoginViewModel _model)
        {
            Global _global = new Database.Global();
            CRMClassDataContext _idc = new Database.CRMClassDataContext(_global.con);
            var usrId = 0;
            try
            {
                var id = _idc.tbllogins.Where(a => a.Email == _model.Email || a.PhoneNumber == _model.phoneNumber).ToList();
                if (id.Count() == 0)
                {
                    tbllogin login = new tbllogin {
                        CreatedDate = DateTime.Now,
                        Email=_model.Email,
                        FirstName=_model.firstname,
                        IsActive="Y",
                        LastName=_model.lastname,
                        Password=_model.Password,
                        PhoneNumber=_model.phoneNumber 
                    };
                    _idc.tbllogins.InsertOnSubmit(login);
                    _idc.SubmitChanges();
                    usrId = login.UserId;
                } 
            }
            catch (Exception ex)
            {
                return 0;
            }
            finally
            {
                _idc.Dispose(); 
            }
            return usrId;
        }

        public void Deletuser(int id)
        {
            Global _global = new Database.Global();
            CRMClassDataContext _idc = new Database.CRMClassDataContext(_global.con);
            try
            {
                var user = _idc.tbllogins.Where(a => a.UserId == id).FirstOrDefault();
                user.IsActive = "N";
                _idc.SubmitChanges();
            }
            catch (Exception ex)
            {
                
            }
            finally
            {
                _idc.Dispose(); 
            }
        }
    }
}

