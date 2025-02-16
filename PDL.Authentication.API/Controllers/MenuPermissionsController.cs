using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using PDL.Authentication.Logics.Helper;
using System.Resources;
using System.Security.Claims;
using System;
using PDL.Authentication.Interfaces.Interfaces;
using PDL.Authentication.Entites.VM;
using Microsoft.AspNetCore.Authorization;
using Org.BouncyCastle.Asn1.Cms;

namespace PDL.Authentication.API.Controllers
{
    public class MenuPermissionsController : BaseApiController
    {
        private readonly IMenuInterface _menuInterface;
        private readonly IConfiguration _configuration;
        public MenuPermissionsController(IMenuInterface menuInterface, IConfiguration configuration) : base(configuration)
        {

            _menuInterface = menuInterface;
            _configuration = configuration;

        }

        #region--- (GetMenu, InsertMenu, DeleteMenu ,Updatemenu) ---BY----------- Satish Maurya --------------- 
        [HttpPost]
        [Authorize]
        public IActionResult InsertMenuData(MenuVM menuVM)
        {
            if (menuVM == null || string.IsNullOrEmpty(menuVM.Title))
            {
                return BadRequest("Invalid menu data.");
            }
            string dbname = GetDBName();
            string activeuser = User.FindFirstValue(ClaimTypes.NameIdentifier);
            try
            {
                if (!string.IsNullOrEmpty(dbname))
                {
                    int rowsAffected = _menuInterface.InsertMenu(menuVM, dbname, GetIslive(), activeuser);
                    if (rowsAffected > 0)
                    {
                        return Ok(new
                        {
                            statuscode = 200,
                            message = (resourceManager.GetString("INSERTSUCCESS")),
                            data = rowsAffected
                        });
                    }
                    else if (rowsAffected == -1)
                    {
                        return Ok(new
                        {
                            StatusCode = 203,
                            message = (resourceManager.GetString("DETAILEXISTS")),
                            data = rowsAffected
                        });
                    }
                    else
                    {
                        return Ok(new
                        {
                            statuscode = 201,
                            message = (resourceManager.GetString("INSERTFAILDAT")),
                            data = rowsAffected
                        });
                    }
                }
                else
                {
                    return Ok(new { statuscode = 405, message = (resourceManager.GetString("NULLDBNAME")), data = "" });
                }

            }
            catch (Exception ex)
            {
                ExceptionLog.InsertLogException(ex, _configuration, GetDBName(), GetIslive(), "InsertMenuData_MenuPermissions");
                return Ok(new { statuscode = 400, message = (resourceManager.GetString("BADREQUEST")), data = "" });
            }
        }
        [HttpGet]
        public IActionResult GetMainMenu(int pageNumber, int pageSize)
        {
            try
            {

                string dbname = GetDBName();

                if (!string.IsNullOrEmpty(dbname))
                {
                    bool isLive = GetIslive();
                    List<GetMenuVM> menuList = _menuInterface.GetMainMenu(pageNumber,pageSize, dbname, isLive);
                    if (menuList != null && menuList.Count > 0)
                    {
                        return Ok(new
                        {
                            statuscode = 200,
                            message = resourceManager.GetString("GETSUCCESS"),
                            data = menuList
                        });
                    }
                    else
                    {
                        return Ok(new { statuscode = 201, message = resourceManager.GetString("GETFAIL"), data = "" });
                    }
                }
                else
                {
                    return Ok(new { statuscode = 405, message = resourceManager.GetString("NULLDBNAME"), data = "" });
                }
            }
            catch (Exception ex)
            {
                ExceptionLog.InsertLogException(ex, _configuration, GetDBName(), GetIslive(), "GetMainMenu_MenuPermissions");
                return Ok(new { statuscode = 400, message = resourceManager.GetString("BADREQUEST"), data = "" });
            }
        }
        [HttpGet]
        public IActionResult GetSubMenuList(int pageNumber, int pageSize)
        {
            try
            {
                string dbname = GetDBName();
                string activeuser = User.FindFirstValue(ClaimTypes.Name);
                if (!string.IsNullOrEmpty(dbname))
                {
                    List<AllSubMenuList> res = _menuInterface.GetSubMenuList(pageNumber, pageSize, dbname, GetIslive());

                    if (res.Count > 0)
                    {
                        return Ok(new
                        {
                            statuscode = 200,
                            message = (resourceManager.GetString("GETSUCCESS")),
                            data = res
                        });
                    }
                    else
                    {
                        return Ok(new
                        {
                            statuscode = 201,
                            message = (resourceManager.GetString("GETFAIL")),
                            data = res
                        });
                    }
                }
                else
                {
                    return Ok(new { statuscode = 405, message = (resourceManager.GetString("NULLDBNAME")), data = "" });
                }
            }
            catch (Exception ex)
            {
                ExceptionLog.InsertLogException(ex, _configuration, GetDBName(), GetIslive(), "GetSubMenuList_MenuPermissions");
                return Ok(new { statuscode = 400, message = (resourceManager.GetString("BADREQUEST")), data = "" });
            }
        }
        [HttpGet]
        public IActionResult GetPageMenuList(int pageNumber, int pageSize)
        {
            try
            {
                string dbname = GetDBName();
                string activeuser = User.FindFirstValue(ClaimTypes.Name);
                if (!string.IsNullOrEmpty(dbname))
                {
                    List<PageMenuList> res = _menuInterface.GetPageMenuList(pageNumber, pageSize, dbname, GetIslive());

                    if (res.Count > 0)
                    {
                        return Ok(new
                        {
                            statuscode = 200,
                            message = (resourceManager.GetString("GETSUCCESS")),
                            data = res
                        });
                    }
                    else
                    {
                        return Ok(new
                        {
                            statuscode = 201,
                            message = (resourceManager.GetString("GETFAIL")),
                            data = res
                        });
                    }
                }
                else
                {
                    return Ok(new { statuscode = 405, message = (resourceManager.GetString("NULLDBNAME")), data = "" });
                }
            }
            catch (Exception ex)
            {
                ExceptionLog.InsertLogException(ex, _configuration, GetDBName(), GetIslive(), "GetPageMenuList_MenuPermissions");
                return Ok(new { statuscode = 400, message = (resourceManager.GetString("BADREQUEST")), data = "" });
            }
        }
        [HttpPost]
        [Authorize]
        public IActionResult UpdateMenuData(MenuVM menuVM)
        {

            string dbname = GetDBName();
            string activeuser = User.FindFirstValue(ClaimTypes.NameIdentifier);
            try
            {
                if (!string.IsNullOrEmpty(dbname))
                {
                    int rowsAffected = _menuInterface.UpdateMenuData(menuVM, dbname, GetIslive(), activeuser);
                    if (rowsAffected > 0)
                    {
                        return Ok(new
                        {
                            statuscode = 200,
                            message = (resourceManager.GetString("UPDATESUCCESS")),
                            data = rowsAffected
                        });
                    }
                    else if (rowsAffected == -1)
                    {
                        return Ok(new
                        {
                            statuscode = 203,
                            message = (resourceManager.GetString("NOTEXIST")),
                            data = rowsAffected
                        });
                    }
                    else
                    {
                        return Ok(new
                        {
                            statuscode = 201,
                            message = (resourceManager.GetString("UPDATEFAIL")),
                            data = rowsAffected
                        });
                    }
                }
                else
                {
                    return Ok(new { statuscode = 405, message = (resourceManager.GetString("NULLDBNAME")), data = "" });

                }
            }
            catch (Exception ex)
            {
                ExceptionLog.InsertLogException(ex, _configuration, GetDBName(), GetIslive(), "UpdateMenuData_MenuPermissions");
                return Ok(new { statuscode = 400, message = (resourceManager.GetString("BADREQUEST")), data = "" });
            }
        }
        [HttpPost]
        public IActionResult DeleteMenuData(MenuVM menuVM)
        {

            string dbname = GetDBName();
            string activeuser = User.FindFirstValue(ClaimTypes.NameIdentifier);
            try
            {
                if (!string.IsNullOrEmpty(dbname))
                {
                    int rowsAffected = _menuInterface.DeleteMenuData(menuVM, dbname, GetIslive(), activeuser);
                    if (rowsAffected > 0)
                    {
                        return Ok(new
                        {
                            statuscode = 200,
                            message = (resourceManager.GetString("DELETESUCCESS")),
                            data = rowsAffected
                        });
                    }
                    else if (rowsAffected == -1)
                    {
                        return Ok(new
                        {
                            statuscode = 203,
                            message = (resourceManager.GetString("NOTEXIST")),
                            data = rowsAffected
                        });
                    }
                    else
                    {
                        return Ok(new
                        {
                            statuscode = 201,
                            message = (resourceManager.GetString("DELETEFAIL")),
                            data = rowsAffected
                        });
                    }
                }
                else
                {
                    return Ok(new { statuscode = 405, message = (resourceManager.GetString("NULLDBNAME")), data = "" });
                }
            }
            catch (Exception ex)
            {
                ExceptionLog.InsertLogException(ex, _configuration, GetDBName(), GetIslive(), "DeleteMenuData_MenuPermissions");
                return Ok(new { statuscode = 400, message = (resourceManager.GetString("BADREQUEST")), data = "" });
            }
        }
        #endregion
        #region  GETDETAILSLIST ------------- BY SATISH MAURYA ------------
        [HttpGet]
        public IActionResult GetRoles()
        {
            try
            {
                string dbname = GetDBName();
                string activeuser = User.FindFirstValue(ClaimTypes.NameIdentifier);
                if (!string.IsNullOrEmpty(dbname))
                {
                    List<GetRolesMasterVM> reslist = _menuInterface.GetRoles(dbname, GetIslive());
                    if (reslist.Count > 0)
                    {
                        return Ok(new
                        {
                            statuscode = 200,
                            message = (resourceManager.GetString("GETSUCCESS")),
                            data = reslist
                        });
                    }
                    else
                    {
                        return Ok(new
                        {
                            statuscode = 201,
                            message = (resourceManager.GetString("GETFAIL")),
                            data = reslist
                        });
                    }
                }
                else
                {
                    return Ok(new { statuscode = 405, message = (resourceManager.GetString("NULLDBNAME")), data = "" });
                }

            }
            catch (ArgumentException ex)
            {
                return BadRequest(new { message = ex.Message });
            }
            catch (Exception ex)
            {
                ExceptionLog.InsertLogException(ex, _configuration, GetDBName(), GetIslive(), "GetRoles_MenuPermissions");
                return Ok(new { statuscode = 400, message = (resourceManager.GetString("BADREQUEST")), data = "" });
            }
        }
        #endregion
    }
}
