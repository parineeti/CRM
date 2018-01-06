using CRM.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CRM.Database
{
    public class contactServices
    {
        public LabelModel getAllLablesByAdminid(int adminId)
        {
            LabelModel _profile = new Models.LabelModel();
            List<Labelitem> _listitem = new List<Labelitem>();
            Global _global = new Database.Global();
            CRMClassDataContext _idc = new Database.CRMClassDataContext(_global.con);
            try
            {
                var list = _idc.tbllabels.Where(a => a.AdminId == adminId && a.Isactive == true).ToList();
                foreach (var item in list)
                {
                    Labelitem _item = new Labelitem();
                    _item.LabelId = item.Id;
                    _item.Name = item.LableName;
                    _item.description = item.Description;
                    _item.Totalcontacts = item.TotalContacts.HasValue ? item.TotalContacts.Value : 0;
                    _listitem.Add(_item);
                }
                _profile._labelList = _listitem;
            }
            catch (Exception ex)
            {
            }
            finally
            {
                _idc.Dispose();
            }
            return _profile;
        }

        public Labelitem getLablesByLabelid(int labelId)
        {
            Labelitem _item = new Labelitem();
            Global _global = new Database.Global();
            CRMClassDataContext _idc = new Database.CRMClassDataContext(_global.con);
            try
            {
                var list = _idc.tbllabels.Where(a => a.Id == labelId).ToList();
                foreach (var item in list)
                {
                    _item.LabelId = item.Id;
                    _item.Name = item.LableName;
                    _item.description = item.Description;
                    _item.Active = item.Isactive.Value;
                    _item.Totalcontacts = item.TotalContacts.HasValue ? item.TotalContacts.Value : 0;
                }
            }
            catch (Exception ex)
            {
            }
            finally
            {
                _idc.Dispose();
            }
            return _item;
        }

        public Labelitem addNewLabel(int adminId, string LabelName, string description)
        {
            Labelitem _item = new Labelitem();
            Global _global = new Database.Global();
            CRMClassDataContext _idc = new Database.CRMClassDataContext(_global.con);
            try
            {
                var list = _idc.tbllabels.Where(a => a.AdminId == adminId && a.Isactive == true && a.LableName == LabelName).ToList();
                if (list.Count() > 0)
                {
                    _item.Error = "Error : Label already exist with " + LabelName + " ,please choose any other name";
                }
                else
                {
                    var tbl = new tbllabel { AdminId = adminId, CreatedDate = DateTime.Now, Description = description, Isactive = true, LableName = LabelName, TotalContacts = 0 };
                    _idc.tbllabels.InsertOnSubmit(tbl);
                    _idc.SubmitChanges();
                    _item.LabelId = tbl.Id;
                    _item.Name = tbl.LableName;
                    _item.description = tbl.Description;
                    _item.Totalcontacts = 0;
                }
            }
            catch (Exception ex)
            {
            }
            finally
            {
                _idc.Dispose();
            }
            return _item;
        }

        public Labelitem updateLabelById(int labelId, bool active, int adminId, string LabelName, string description)
        {
            Labelitem _item = new Labelitem();
            Global _global = new Database.Global();
            CRMClassDataContext _idc = new Database.CRMClassDataContext(_global.con);
            try
            {
                var list = _idc.tbllabels.Where(a => a.AdminId == adminId && a.Id != labelId && a.Isactive == true && a.LableName == LabelName).ToList();
                if (list.Count() > 0)
                {
                    _item.Error = "Error : Label already exist with " + LabelName + " ,please choose any other name";
                }
                else
                {
                    var item = _idc.tbllabels.Where(a => a.Id == labelId).FirstOrDefault();
                    item.Description = description;
                    item.LableName = LabelName;
                    item.Isactive = active;
                    item.AdminId = adminId;
                    _idc.SubmitChanges();
                    _item.LabelId = item.Id;
                    _item.Name = item.LableName;
                    _item.description = item.Description;
                    _item.Totalcontacts = item.TotalContacts.Value;
                }
            }
            catch (Exception ex)
            {
            }
            finally
            {
                _idc.Dispose();
            }
            return _item;
        }

        public void deleteContact(int contactId)
        {
            Global _global = new Database.Global();
            CRMClassDataContext _idc = new Database.CRMClassDataContext(_global.con);
            try
            {
                var list = _idc.tblcontacts.Where(a => a.ContactId == contactId).FirstOrDefault();
                var label = _idc.tbllabels.Where(a => a.Id == list.LabelId).FirstOrDefault();
                label.TotalContacts = label.TotalContacts - 1;
                list.IsActive = false;
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

        public void deletelabel(int labelId)
        {
            Global _global = new Database.Global();
            CRMClassDataContext _idc = new Database.CRMClassDataContext(_global.con);
            try
            {
                 var label = _idc.tbllabels.Where(a => a.Id == labelId).FirstOrDefault();
                label.Isactive = false;
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

        public tblcontact getContactsByContactid(int ContactId)
        {
            tblcontact _item = new tblcontact();
            Global _global = new Database.Global();
            CRMClassDataContext _idc = new Database.CRMClassDataContext(_global.con);
            try
            {
                _item = _idc.tblcontacts.Where(a => a.ContactId == ContactId).FirstOrDefault();
            }
            catch (Exception ex)
            {
            }
            finally
            {
                _idc.Dispose();
            }
            return _item;
        }

        public string AddNewContact(tblcontact Contact)
        {
            string msg = string.Empty;
            tblcontact _item = new tblcontact();
            Global _global = new Database.Global();
            CRMClassDataContext _idc = new Database.CRMClassDataContext(_global.con);
            try
            {
                _idc.tblcontacts.InsertOnSubmit(Contact);
                var label = _idc.tbllabels.Where(a => a.Id == Contact.LabelId).FirstOrDefault();
                label.TotalContacts = label.TotalContacts + 1;
                _idc.SubmitChanges();
            }
            catch (Exception ex)
            {
                msg = "Error :" + ex.Message;
            }
            finally
            {
                _idc.Dispose();
            }
            return msg;
        }

        public string upDateContact(tblcontact Contact)
        {
            string msg = string.Empty;
            tblcontact _item = new tblcontact();
            Global _global = new Database.Global();
            CRMClassDataContext _idc = new Database.CRMClassDataContext(_global.con);
            try
            {
                var oldcontact = _idc.tblcontacts.Where(a => a.ContactId == Contact.ContactId).FirstOrDefault();
                oldcontact.Age = Contact.Age;
                oldcontact.email = Contact.email;
                oldcontact.Name = Contact.Name;
                oldcontact.ImagePath = !string.IsNullOrEmpty(Contact.ImagePath) ? Contact.ImagePath : oldcontact.ImagePath;
                oldcontact.phone = Contact.phone;
                oldcontact.Role = Contact.Role;
                oldcontact.salary = Contact.salary;
                oldcontact.LabelId = Contact.LabelId;
                oldcontact.Joiningdate = Contact.Joiningdate;
                oldcontact.IsActive = Contact.IsActive;
                _idc.SubmitChanges();
            }
            catch (Exception ex)
            {
                msg = "Error :" + ex.Message;
            }
            finally
            {
                _idc.Dispose();
            }
            return msg;
        }

        public ContactViewModal GetaAllContactsByAdminId(int adminId)
        {
            ContactViewModal _item = new ContactViewModal();
            Global _global = new Database.Global();
            CRMClassDataContext _idc = new Database.CRMClassDataContext(_global.con);
            try
            {
               var item = _idc.tblcontacts.Where(a => a.AdminId == adminId&&a.IsActive==true).ToList();
                _item.Contacts = item;
            }
            catch (Exception ex)
            {
            }
            finally
            {
                _idc.Dispose();
            }
            return _item; 
        }
    }
}