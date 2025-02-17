using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PDL.Authentication.Entites.VM;
using PDL.Authentication.Interfaces.Interfaces;
using PDL.Authentication.Logics.Helper;
using System.Security.Claims;

namespace PDL.Authentication.API.Controllers
{
    public class MastersController : BaseApiController
    {
        private readonly IConfiguration _configuration;
        private readonly IMasterService _masterService;
        public MastersController(IMasterService masterService, IConfiguration configuration) : base(configuration)
        {
            _masterService = masterService;
            _configuration = configuration;
        }
        #region Api Permission API (INSERT , UPDATE, DELETE AND GETDETAILS) BY--------------- Satish Maurya-------
        [HttpPost]
        [Authorize]
        public IActionResult CreatePageApiPermission(APIModule aPIModule)
        {
            try
            {
                string dbname = GetDBName();
                string activeuser = User.FindFirstValue(ClaimTypes.NameIdentifier);

                if (!string.IsNullOrEmpty(dbname))
                {
                    int res = _masterService.CreatePageApiPermission(aPIModule, activeuser, dbname, GetIslive());

                    if (res > 0)
                    {
                        return Ok(new
                        {
                            statuscode = 200,
                            message = resourceManager.GetString("INSERTSUCCESS"),
                            data = res
                        });
                    }
                    else if (res == -1)
                    {
                        return Ok(new
                        {
                            StatusCode = 203,
                            message = (resourceManager.GetString("DETAILEXISTS")),
                            data = res
                        });
                    }
                    else
                    {
                        return Ok(new
                        {
                            statuscode = 201,
                            message = resourceManager.GetString("INSERTFAILDATA"),
                            data = res
                        });
                    }
                }
                else
                {
                    return Ok(new { statuscode = 405, message = resourceManager.GetString("NULLDBNAME"), data = "" });
                }
            }
            catch (Exception ex)
            {
                ExceptionLog.InsertLogException(ex, _configuration, GetDBName(), GetIslive(), "CreatePageApiPermission_Masters");
                return Ok(new { statuscode = 400, message = resourceManager.GetString("BADREQUEST"), data = "" });
            }
        }
        [HttpPost]
        [Authorize]
        public IActionResult UpdatePageApiPermission(APIData data)
        {
            try
            {
                string dbname = GetDBName();
                string activeuser = User.FindFirstValue(ClaimTypes.NameIdentifier);

                if (!string.IsNullOrEmpty(dbname))
                {
                    int res = _masterService.UpdatePageApiPermission(data, activeuser, dbname, GetIslive());

                    if (res > 0)
                    {
                        return Ok(new
                        {
                            statuscode = 200,
                            message = resourceManager.GetString("UPDATESUCCESS"),
                            data = res
                        });
                    }
                    else if (res == -1)
                    {
                        return Ok(new
                        {
                            statuscode = 203,
                            message = (resourceManager.GetString("NOTEXIST")),
                            data = res
                        });
                    }
                    else
                    {
                        return Ok(new
                        {
                            statuscode = 201,
                            message = resourceManager.GetString("UPDATEFAIL"),
                            data = res
                        });
                    }
                }
                else
                {
                    return Ok(new { statuscode = 405, message = resourceManager.GetString("NULLDBNAME"), data = "" });
                }
            }
            catch (Exception ex)
            {
                ExceptionLog.InsertLogException(ex, _configuration, GetDBName(), GetIslive(), "UpdatePagePermission_Masters");
                return Ok(new { statuscode = 400, message = resourceManager.GetString("BADREQUEST"), data = "" });
            }
        }
        [HttpGet]
        [Authorize]
        public IActionResult DeletePageApiPermission(int Id)
        {
            try
            {
                string dbname = GetDBName();
                string activeuser = User.FindFirstValue(ClaimTypes.NameIdentifier);
                if (!string.IsNullOrEmpty(dbname))
                {
                    int res = _masterService.DeletePageApiPermission(Id, activeuser, dbname, GetIslive());
                    if (res > 0)
                    {
                        return Ok(new
                        {
                            statuscode = 200,
                            message = (resourceManager.GetString("DELETESUCCESS")),
                            data = res
                        });
                    }
                    else if (res == -1)
                    {
                        return Ok(new
                        {
                            statuscode = 203,
                            message = (resourceManager.GetString("NOTEXIST")),
                            data = res
                        });
                    }
                    else
                    {
                        return Ok(new
                        {
                            statuscode = 201,
                            message = (resourceManager.GetString("DELETEFAIL")),
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
                ExceptionLog.InsertLogException(ex, _configuration, GetDBName(), GetIslive(), "DeletePageApiPermission_Masters");
                return Ok(new { statuscode = 400, message = (resourceManager.GetString("BADREQUEST")), data = "" });
            }
        }
        [HttpGet]
        public IActionResult GetRolePermissiondata()
        {
            try
            {
                string dbname = GetDBName();

                if (!string.IsNullOrEmpty(dbname))
                {
                    bool isLive = GetIslive();

                    List<APIModule> List = _masterService.GetRolePermission(dbname, isLive);

                    if (List != null && List.Count > 0)
                    {
                        return Ok(new
                        {
                            statuscode = 200,
                            message = resourceManager.GetString("GETSUCCESS"),
                            data = List
                        });
                    }
                    else
                    {
                        return Ok(new
                        {
                            statuscode = 201,
                            message = resourceManager.GetString("GETFAIL"),
                            data = ""
                        });
                    }
                }
                else
                {
                    return Ok(new
                    {
                        statuscode = 405,
                        message = resourceManager.GetString("NULLDBNAME"),
                        data = ""
                    });
                }
            }
            catch (Exception ex)
            {
                ExceptionLog.InsertLogException(ex, _configuration, GetDBName(), GetIslive(), "GetRolePermissiondata_Masters");

                return Ok(new
                {
                    statuscode = 400,
                    message = resourceManager.GetString("BADREQUEST"),
                    data = ""
                });
            }
        }
        [HttpPost]
        [Authorize]
        public IActionResult GetApiPermissiondata(PagepermissionVM pagePermission)
        {
            try
            {
                string dbname = GetDBName();

                if (!string.IsNullOrEmpty(dbname))
                {
                    bool isLive = GetIslive();
                    List<APIModule> List = _masterService.GetPermission(pagePermission, dbname, isLive);

                    if (List != null && List.Count > 0)
                    {
                        return Ok(new
                        {
                            statuscode = 200,
                            message = resourceManager.GetString("GETSUCCESS"),
                            data = List
                        });
                    }
                    else
                    {
                        return Ok(new
                        {
                            statuscode = 201,
                            message = resourceManager.GetString("GETFAIL"),
                            data = ""
                        });
                    }
                }
                else
                {
                    return Ok(new
                    {
                        statuscode = 405,
                        message = resourceManager.GetString("NULLDBNAME"),
                        data = ""
                    });
                }
            }
            catch (Exception ex)
            {
                ExceptionLog.InsertLogException(ex, _configuration, GetDBName(), GetIslive(), "GetPermissiondata_Masters");

                return Ok(new
                {
                    statuscode = 400,
                    message = resourceManager.GetString("BADREQUEST"),
                    data = ""
                });
            }
        }

        #endregion
    }
}
