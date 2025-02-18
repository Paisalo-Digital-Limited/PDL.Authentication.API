using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using PDL.Authentication.Entites.VM;
using PDL.Authentication.Logics.Credentials;
using PDL.Authentication.Logics.Helper;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PDL.Authentication.Logics.BLL
{
    public class MenuBLL : BaseBLL
    {
        private readonly IConfiguration _configuration;
        private readonly CredManager _credManager;
        public MenuBLL(IConfiguration configuration)
        {
            _configuration = configuration;
            _credManager = new CredManager(configuration);

        }
        #region--- (GetMenu, InsertMenu, DeleteMenu ,Updatemenu) ---BY----------- Satish Maurya --------------- 
        public int InsertMenus(MenuVM obj, string dbname, bool islive, string activeuser)
        {
            int aff = 0;
            try
            {
                string query = "Usp_MenuListdata";

                using (SqlConnection con = _credManager.getConnections(dbname, islive))
                {
                    using (var cmd = new SqlCommand(query, con))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@Mode", "Insert");
                        cmd.Parameters.AddWithValue("@Title", string.IsNullOrEmpty(obj.Title) ? string.Empty : obj.Title);
                        cmd.Parameters.AddWithValue("@ParentId", obj.ParentId.HasValue ? (object)obj.ParentId.Value : DBNull.Value);
                        cmd.Parameters.AddWithValue("@PageUrl", string.IsNullOrEmpty(obj.PageUrl) ? string.Empty : obj.PageUrl);
                        cmd.Parameters.AddWithValue("@Icon", string.IsNullOrEmpty(obj.Icon) ? string.Empty : obj.Icon);
                        cmd.Parameters.AddWithValue("@IsPage", obj.IsPage != null ? Convert.ToBoolean((bool)obj.IsPage) : false);
                        cmd.Parameters.AddWithValue("@CreatedBy", activeuser);

                        con.Open();
                        aff = cmd.ExecuteNonQuery();
                        con.Close();
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Error: " + ex.Message);
            }
            return aff;
        }
        public List<GetMenuVM> GetMainMenu(int pageNumber, int pageSize, string dbname, bool islive)
        {
            List<GetMenuVM> menuDataList = new List<GetMenuVM>();
            string query = "Usp_MenuListdata";

            try
            {
                using (var con = _credManager.getConnections(dbname, islive))
                using (var cmd = new SqlCommand(query, con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@Mode", "GetMainMenuData");
                    cmd.Parameters.AddWithValue("@PageNumber", pageNumber);
                    cmd.Parameters.AddWithValue("@PageSize", pageSize);
                    con.Open();
                    using (var reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            var menuData = new GetMenuVM
                            {
                                Id = reader["Id"] != DBNull.Value ? Convert.ToInt32(reader["Id"]) : 0,
                                Title = reader["Title"]?.ToString() ?? string.Empty,
                                Icon = reader["Icon"]?.ToString() ?? string.Empty,
                                IsActive = reader["IsActive"] != DBNull.Value ? Convert.ToInt32(reader["IsActive"]) : 0,
                                IsDeleted = reader["IsDeleted"] != DBNull.Value ? Convert.ToInt32(reader["IsDeleted"]) : 0,
                            };
                            menuDataList.Add(menuData);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                ExceptionLog.InsertLogException(ex, _configuration, dbname, islive, "GetMenusData");
                throw new Exception("Error: " + ex.Message);
            }

            return menuDataList;
        }
        public List<AllSubMenuList> GetSubMenuList(int pageNumber, int pageSize, string dbname, bool islive)
        {
            List<AllSubMenuList> data = new List<AllSubMenuList>();

            using (SqlConnection con = _credManager.getConnections(dbname, islive))
            {
                string query = "Usp_MenuListdata";
                using (SqlCommand cmd = new SqlCommand(query, con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@Mode", "GetAllSubMenuList");
                    cmd.Parameters.AddWithValue("@PageNumber", pageNumber);
                    cmd.Parameters.AddWithValue("@PageSize", pageSize);
                    if (con.State == ConnectionState.Closed)
                        con.Open();

                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            AllSubMenuList submenuData = new AllSubMenuList
                            {
                                Id = reader["Id"] != DBNull.Value ? Convert.ToInt32(reader["Id"]) : 0,
                                ParentId = reader["ParentId"] != DBNull.Value ? Convert.ToInt32(reader["ParentId"]) : 0,
                                Title = reader["Title"] != DBNull.Value ? reader["Title"].ToString() : null,
                                Icon = reader["Icon"]?.ToString() ?? string.Empty,
                                PageUrl = reader["PageUrl"] != DBNull.Value ? reader["PageUrl"].ToString() : null,
                                IsActive = reader["IsActive"] != DBNull.Value ? Convert.ToInt32(reader["IsActive"]) : 0,
                                IsDeleted = reader["IsDeleted"] != DBNull.Value ? Convert.ToInt32(reader["IsDeleted"]) : 0,
                            };
                            data.Add(submenuData);
                        }
                    }

                    con.Close();
                }
            }

            return data;
        }
        public List<PageMenuList> GetPageMenuList(int pageNumber, int pageSize, string dbname, bool islive)
        {
            List<PageMenuList> data = new List<PageMenuList>();

            using (SqlConnection con = _credManager.getConnections(dbname, islive))
            {
                string query = "Usp_MenuListdata";
                using (SqlCommand cmd = new SqlCommand(query, con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@Mode", "GetPageMenuList");
                    cmd.Parameters.AddWithValue("@PageNumber", pageNumber);
                    cmd.Parameters.AddWithValue("@PageSize", pageSize);
                    if (con.State == ConnectionState.Closed)
                        con.Open();

                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            PageMenuList submenuData = new PageMenuList
                            {
                                Id = reader["Id"] != DBNull.Value ? Convert.ToInt32(reader["Id"]) : 0,
                                ParentId = reader["ParentId"] != DBNull.Value ? Convert.ToInt32(reader["ParentId"]) : 0,
                                Title = reader["Title"] != DBNull.Value ? reader["Title"].ToString() : null,
                                Icon = reader["Icon"]?.ToString() ?? string.Empty,
                                PageUrl = reader["PageUrl"] != DBNull.Value ? reader["PageUrl"].ToString() : null,
                                IsActive = reader["IsActive"] != DBNull.Value ? Convert.ToInt32(reader["IsActive"]) : 0,
                                IsDeleted = reader["IsDeleted"] != DBNull.Value ? Convert.ToInt32(reader["IsDeleted"]) : 0,
                                IsPage = reader["IsPage"] != DBNull.Value ? Convert.ToBoolean(reader["IsPage"]) : false,
                            };
                            data.Add(submenuData);
                        }
                    }

                    con.Close();
                }
            }

            return data;
        }
        public int UpdateMenuData(MenuVM obj, string dbname, bool islive, string activeuser)
        {
            int aff = 0;
            try
            {
                string query = "Usp_MenuListdata";

                using (SqlConnection con = _credManager.getConnections(dbname, islive))
                {
                    using (var cmd = new SqlCommand(query, con))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        if (obj.type == "mainMenu")
                        {
                            cmd.Parameters.AddWithValue("@Mode", "updateMainmenu");
                            cmd.Parameters.AddWithValue("@Title", string.IsNullOrEmpty(obj.Title) ? string.Empty : obj.Title);
                            cmd.Parameters.AddWithValue("@Icon", string.IsNullOrEmpty(obj.Icon) ? string.Empty : obj.Icon);
                            cmd.Parameters.AddWithValue("@PageUrl", string.IsNullOrEmpty(obj.PageUrl) ? string.Empty : obj.PageUrl);
                            cmd.Parameters.AddWithValue("@id", obj.mainid);
                            cmd.Parameters.AddWithValue("@ModifiedBy", activeuser);
                        }
                        else if (obj.type == "submenu")
                        {
                            cmd.Parameters.AddWithValue("@Mode", "updateMainmenu");
                            cmd.Parameters.AddWithValue("@Title", string.IsNullOrEmpty(obj.Title) ? string.Empty : obj.Title);
                            cmd.Parameters.AddWithValue("@Icon", string.IsNullOrEmpty(obj.Icon) ? string.Empty : obj.Icon);
                            cmd.Parameters.AddWithValue("@PageUrl", string.IsNullOrEmpty(obj.PageUrl) ? string.Empty : obj.PageUrl);
                            cmd.Parameters.AddWithValue("@ParentId", obj.ParentId);
                            cmd.Parameters.AddWithValue("@id", obj.mainid);
                            cmd.Parameters.AddWithValue("@ModifiedBy", activeuser);
                        }
                        else if (obj.type == "pagemenu")
                        {
                            cmd.Parameters.AddWithValue("@Mode", "updatePagemenu");
                            cmd.Parameters.AddWithValue("@Title", string.IsNullOrEmpty(obj.Title) ? string.Empty : obj.Title);
                            cmd.Parameters.AddWithValue("@Icon", string.IsNullOrEmpty(obj.Icon) ? string.Empty : obj.Icon);
                            cmd.Parameters.AddWithValue("@PageUrl", string.IsNullOrEmpty(obj.PageUrl) ? string.Empty : obj.PageUrl);
                            cmd.Parameters.AddWithValue("@ParentId", obj.ParentId);
                            cmd.Parameters.AddWithValue("@id", obj.mainid);
                            cmd.Parameters.AddWithValue("@ModifiedBy", activeuser);

                        }
                        con.Open();
                        aff = cmd.ExecuteNonQuery();
                        con.Close();
                    }
                }
            }
            catch (Exception ex)
            {
                ExceptionLog.InsertLogException(ex, _configuration, dbname, islive, "UpdateMenuData");
                throw new Exception("Error: " + ex.Message);
            }
            return aff;
        }
        public int DeleteMenuData(MenuVM obj, string dbname, bool islive, string activeuser)
        {
            int aff = 0;
            try
            {
                string query = "Usp_MenuListdata";

                using (SqlConnection con = _credManager.getConnections(dbname, islive))
                {
                    using (var cmd = new SqlCommand(query, con))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@Mode", "DeleteMenuData");
                        if (obj.IsActive == true && obj.IsDeleted == false)
                        {
                            cmd.Parameters.AddWithValue("@IsActive", obj.IsActive ? string.Empty : false);
                            cmd.Parameters.AddWithValue("@IsDeleted", obj.IsDeleted ? string.Empty : true);
                            cmd.Parameters.AddWithValue("@id", obj.mainid);
                            cmd.Parameters.AddWithValue("@ModifiedBy", activeuser);
                        }
                        else if (obj.IsActive == false && obj.IsDeleted == true)
                        {
                            cmd.Parameters.AddWithValue("@IsActive", obj.IsActive ? string.Empty : true);
                            cmd.Parameters.AddWithValue("@IsDeleted", obj.IsDeleted ? string.Empty : false);
                            cmd.Parameters.AddWithValue("@id", obj.mainid);
                            cmd.Parameters.AddWithValue("@ModifiedBy", activeuser);
                        }

                        con.Open();
                        aff = cmd.ExecuteNonQuery();
                        con.Close();
                    }
                }
            }
            catch (Exception ex)
            {
                ExceptionLog.InsertLogException(ex, _configuration, dbname, islive, "DeleteMenuData");
                throw new Exception("Error: " + ex.Message);
            }
            return aff;
        }
        #endregion
        #region  GETDETAILSLIST ------------- BY SATISH MAURYA ------------
        public List<GetRolesMasterVM> GetRoles(string dbname, bool islive)
        {
            List<GetRolesMasterVM> list = new List<GetRolesMasterVM>();
            string query = "Usp_MenuListdata";
            try
            {
                using (SqlConnection con = _credManager.getConnections(dbname, islive))
                {
                    using (var cmd = new SqlCommand(query, con))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@Mode", "GETDATAROLE");
                        con.Open();
                        using (SqlDataReader reader = cmd.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                GetRolesMasterVM rolesMaster = new GetRolesMasterVM
                                {
                                    Id = reader["Id"] != DBNull.Value ? Convert.ToInt32(reader["Id"]) : 0,
                                    Name = reader["Name"] != DBNull.Value ? reader["Name"].ToString() : null,
                                    IsActive = Convert.ToBoolean(reader["IsActive"]),
                                    IsDeleted = Convert.ToBoolean(reader["IsDeleted"]),
                                };
                                list.Add(rolesMaster);
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                ExceptionLog.InsertLogException(ex, _configuration, dbname, islive, "GetRoles");
                throw new Exception("Error: " + ex.Message);
            }
            return list;
        }
        #endregion
        #region  -----Get User DDL ------------- BY SATISH MAURYA ------------
        public List<GetUSerMasterVM> GetUserDDL(int roleid, string dbname, bool islive)
        {
            List<GetUSerMasterVM> list = new List<GetUSerMasterVM>();
            string query = "Usp_MenuListdata";
            try
            {
                using (SqlConnection con = _credManager.getConnections(dbname, islive))
                {
                    using (var cmd = new SqlCommand(query, con))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@Mode", "GETUSERDATA");
                        cmd.Parameters.AddWithValue("@roleid", roleid);
                        con.Open();
                        using (SqlDataReader reader = cmd.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                GetUSerMasterVM userMaster = new GetUSerMasterVM
                                {
                                    Id = reader["Id"] != DBNull.Value ? Convert.ToInt32(reader["Id"]) : 0,
                                    Name = reader["Name"] != DBNull.Value ? reader["Name"].ToString() : null,
                                    Email = reader["Email"] != DBNull.Value ? reader["Email"].ToString() : null,
                                };
                                list.Add(userMaster);
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                ExceptionLog.InsertLogException(ex, _configuration, dbname, islive, "GetUserDDL");
                throw new Exception("Error: " + ex.Message);
            }
            return list;
        }
        #endregion
        #region  -----Assign User Role Page ------------- BY SATISH MAURYA ------------
        public List<MenuPagePermission> AssignInsertMenus(List<MenuPagePermission> obj, string dbname, bool islive, string activeuser)
        {
            try
            {
                string query = "Usp_MenuPagePermission";
                using (SqlConnection con = _credManager.getConnections(dbname, islive))
                {
                    using (var cmd = new SqlCommand(query, con))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.Add("@Mode", SqlDbType.VarChar).Value = "updatePageMaster";

                        DataTable dt = new DataTable();
                        dt.Columns.Add("RoleId", typeof(int));
                        dt.Columns.Add("PageMasterId", typeof(int));
                        dt.Columns.Add("userid", typeof(int));

                        foreach (MenuPagePermission item in obj)
                        {
                            dt.Rows.Add(item.RoleId, item.PageMasterId, item.userid);
                        }

                        cmd.Parameters.Add("@UserRolePage", SqlDbType.Structured).Value = dt;
                        cmd.Parameters.Add("@CreatedBy", SqlDbType.VarChar).Value = activeuser;
                        con.Open();
                        cmd.ExecuteNonQuery();
                        con.Close();
                    }
                }
            }
            catch (Exception ex)
            {
                ExceptionLog.InsertLogException(ex, _configuration, dbname, islive, "AssignInsertMenus");
                throw new Exception("Error: " + ex.Message);
            }
            return obj;
        }
        public List<GetMenuPermissionVM> GetPermissionPageList(int RoleId, string dbname, bool islive)
        {
            var menuDataList = new List<GetMenuPermissionVM>();

            const string query = "Usp_MenuListdata";
            try
            {
                using (var con = _credManager.getConnections(dbname, islive))
                {
                    using (var cmd = new SqlCommand(query, con))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@Mode", "GetDataUserPageMaster");
                        cmd.Parameters.AddWithValue("@RoleId", RoleId);
                        con.Open();
                        using (var reader = cmd.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                var menuData = new GetMenuPermissionVM
                                {
                                    Id = reader["Id"] != DBNull.Value ? Convert.ToInt32(reader["Id"]) : 0,
                                    RoleId = reader["RoleId"] != DBNull.Value ? Convert.ToInt32(reader["RoleId"]) : 0,
                                    PageMasterId = reader["PageMasterId"] != DBNull.Value ? Convert.ToInt32(reader["PageMasterId"]) : 0,
                                    AssignStatus = reader["AssignStatus"] != DBNull.Value ? reader["AssignStatus"].ToString() : null,
                                    Title = reader["Title"] != DBNull.Value ? reader["Title"].ToString() : null,
                                };
                                menuDataList.Add(menuData);
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                ExceptionLog.InsertLogException(ex, _configuration, dbname, islive, "GetPermissionPageMaster");
                throw new Exception("Error: " + ex.Message);
            }
            return menuDataList;
        }
        #endregion
    }

}
