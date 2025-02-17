using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using PDL.Authentication.Entites.VM;
using PDL.Authentication.Logics.Credentials;
using PDL.Authentication.Logics.Helper;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace PDL.Authentication.Logics.BLL
{
    public class MasterBLL : BaseBLL
    {
        private readonly IConfiguration _configuration;
        private readonly CredManager _credManager;
        public MasterBLL(IConfiguration configuration)
        {
            _configuration = configuration;
            _credManager = new CredManager(configuration);
        }
        #region Api Permission API (INSERT , UPDATE, DELETE AND GETDETAILS) BY--------------- Satish Maurya-------
        public int CreatePageApiPermission(APIModule aPIModule, string activeuser, string dbName, bool isLive)
        {
            int affected = 0;
            string query = "sp_InsertOrUpdateApiModule";
            try
            {
                using (SqlConnection con = _credManager.getConnections(dbName, isLive))
                {
                    using (var cmd = new SqlCommand(query, con))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;

                        cmd.Parameters.AddWithValue("@Mode", "Insert");
                        cmd.Parameters.AddWithValue("@ServiceName", aPIModule?.ServiceName ?? string.Empty);
                        cmd.Parameters.AddWithValue("@ControllerName", aPIModule?.ControllerName ?? string.Empty);
                        cmd.Parameters.AddWithValue("@ActionName", aPIModule?.ActionsName ?? string.Empty);
                        cmd.Parameters.AddWithValue("@CreatedBy", activeuser);
                        cmd.Parameters.AddWithValue("@UrlId", aPIModule?.UrlId ?? 0);
                        con.Open();
                        affected = cmd.ExecuteNonQuery();
                        con.Close();
                        cmd.Dispose();
                    }
                }
            }
            catch (Exception ex)
            {
                ExceptionLog.InsertLogException(ex, _configuration, dbName, isLive, "CreatePageApiPermission");
                throw new Exception("Error: " + ex.Message);
            }
            return affected;
        }
        public int UpdatePageApiPermission(APIData aPIModule, string activeuser, string dbname, bool islive)
        {
            int affected = 0;
            string query = "sp_InsertOrUpdateApiModule";
            try
            {
                using (SqlConnection con = _credManager.getConnections(dbname, islive))
                {
                    using (var cmd = new SqlCommand(query, con))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@Mode", "Update");
                        cmd.Parameters.AddWithValue("@Id", aPIModule.Id);
                        cmd.Parameters.AddWithValue("@ServiceName", aPIModule?.ServiceName ?? string.Empty);
                        cmd.Parameters.AddWithValue("@ControllerName", aPIModule?.ControllerName ?? string.Empty);
                        cmd.Parameters.AddWithValue("@ActionName", aPIModule?.ActionsName ?? string.Empty);
                        cmd.Parameters.AddWithValue("@ModifiedBy", activeuser);

                        con.Open();
                        affected = cmd.ExecuteNonQuery();
                        con.Close();
                        cmd.Dispose();
                    }
                }
            }
            catch (Exception ex)
            {
                ExceptionLog.InsertLogException(ex, _configuration, dbname, islive, "UpdatePageApiPermission");
                throw new Exception("Error: " + ex.Message);
            }
            return affected;
        }
        public int DeletePageApiPermission(int Id, string activeuser, string dbname, bool islive)
        {
            int affected = 0;
            string query = "sp_InsertOrUpdateApiModule";
            try
            {
                using (SqlConnection con = _credManager.getConnections(dbname, islive))
                {
                    using (var cmd = new SqlCommand(query, con))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@Mode", "Delete");
                        cmd.Parameters.AddWithValue("@Id", Id);
                        cmd.Parameters.AddWithValue("@ModifiedBy", activeuser);
                        con.Open();
                        affected = cmd.ExecuteNonQuery();
                        con.Close();
                    }
                }
            }
            catch (Exception ex)
            {
                ExceptionLog.InsertLogException(ex, _configuration, dbname, islive, "DeletePageApiPermission");
                throw new Exception("Error: " + ex.Message);
            }
            return affected;
        }
        public List<APIModule> GetRolePermission(string dbName, bool isLive)
        {
            List<APIModule> apiModuleList = new List<APIModule>();
            string query = "sp_InsertOrUpdateApiModule";

            try
            {
                using (SqlConnection con = _credManager.getConnections(dbName, isLive))
                {
                    using (SqlCommand cmd = new SqlCommand(query, con))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@Mode", "GetRoleAccess");
                        con.Open();

                        using (SqlDataReader reader = cmd.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                APIModule apiModule = new APIModule
                                {
                                    ApiModuleId = reader["ApiModuleId"] != DBNull.Value ? Convert.ToInt32(reader["ApiModuleId"]) : 0,
                                    RoleId = reader["RoleId"] != DBNull.Value ? Convert.ToInt32(reader["RoleId"]) : 0,
                                    ControllerName = reader["ControllerName"] != DBNull.Value ? reader["ControllerName"].ToString() : null,
                                    ActionsName = reader["ActionName"] != DBNull.Value ? reader["ActionName"].ToString() : null,
                                    ServiceName = reader["ServiceName"] != DBNull.Value ? reader["ServiceName"].ToString() : null,
                                    IsActive = reader["IsActive"] != DBNull.Value ? Convert.ToBoolean(reader["IsActive"]) : (bool?)null,
                                    IsDeleted = reader["IsDeleted"] != DBNull.Value ? Convert.ToBoolean(reader["IsDeleted"]) : (bool?)null,
                                    CreatedBy = reader["CreatedBy"] != DBNull.Value ? Convert.ToInt32(reader["CreatedBy"]) : 0,
                                    CreatedOn = reader["CreatedOn"] != DBNull.Value ? Convert.ToDateTime(reader["CreatedOn"]) : (DateTime?)null
                                };

                                apiModuleList.Add(apiModule);
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                ExceptionLog.InsertLogException(ex, _configuration, dbName, isLive, "GetRolePermission");
                throw new Exception("Error: " + ex.Message);
            }

            return apiModuleList;
        }

        public List<APIModule> GetapiPermission(PagepermissionVM pagePermission, string dbName, bool isLive)
        {
            List<APIModule> apiModuleList = new List<APIModule>();
            string query = "sp_InsertOrUpdateApiModule";

            try
            {
                int pageNumber = pagePermission.pageNumber;
                int pageSize = pagePermission.pageSize;

                using (SqlConnection con = _credManager.getConnections(dbName, isLive))
                {
                    using (SqlCommand cmd = new SqlCommand(query, con))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;

                        cmd.Parameters.AddWithValue("@Mode", "GetAccess");
                        cmd.Parameters.AddWithValue("@pageno", pageNumber);
                        cmd.Parameters.AddWithValue("@pagination", pageSize);

                        con.Open();

                        using (SqlDataReader reader = cmd.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                APIModule apiModule = new APIModule
                                {
                                    Id = reader["Id"] != DBNull.Value ? Convert.ToInt32(reader["Id"]) : 0,
                                    ControllerName = reader["ControllerName"] != DBNull.Value ? reader["ControllerName"].ToString() : null,
                                    ActionsName = reader["ActionName"] != DBNull.Value ? reader["ActionName"].ToString() : null,
                                    ServiceName = reader["ServiceName"] != DBNull.Value ? reader["ServiceName"].ToString() : null,
                                    IsActive = reader["IsActive"] != DBNull.Value ? Convert.ToBoolean(reader["IsActive"]) : (bool?)null,
                                    IsDeleted = reader["IsDeleted"] != DBNull.Value ? Convert.ToBoolean(reader["IsDeleted"]) : (bool?)null,
                                    CreatedBy = reader["CreatedBy"] != DBNull.Value ? Convert.ToInt32(reader["CreatedBy"]) : 0,
                                    CreatedOn = reader["CreatedOn"] != DBNull.Value ? Convert.ToDateTime(reader["CreatedOn"]) : (DateTime?)null,
                                    UrlId = reader["UrlId"] != DBNull.Value ? Convert.ToInt32(reader["UrlId"]) : 0,
                                    Title = reader["Title"] != DBNull.Value ? reader["Title"].ToString() : null
                                };

                                apiModuleList.Add(apiModule);
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                ExceptionLog.InsertLogException(ex, _configuration, dbName, isLive, "GetapiPermission");
                throw new Exception("Error: " + ex.Message);
            }

            return apiModuleList;
        }

        #endregion
    }
}
