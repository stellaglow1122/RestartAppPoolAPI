using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Management.Automation;
using System.Management.Automation.Runspaces;
using RestartAppPoolAPI.Models;
using System.Web.Http.Cors;

namespace RestartAppPoolAPI.Controllers
{
    public class RestartController : ApiController
    {
        [HttpPost]
        [EnableCors(origins: "*", headers: "*", methods: "*")]
        public ApiResult<string> RestartAppPool(AppPoolInfo appPoolInfo)
        {
            string result = "";
            using (PowerShell powershell = PowerShell.Create())
            {
                powershell.AddScript("try { Invoke-Command -ComputerName " + appPoolInfo.ServerName + @" -ScriptBlock{
                                        try
                                        {
                                            Import-Module WebAdministration
                                            if ((Get-WebAppPoolState -Name '" + appPoolInfo.AppPoolName  + @"') -eq 'started')
                                            {
                                                Stop-WebAppPool -Name '" + appPoolInfo.AppPoolName + @"'
                                                while ((Get-WebAppPoolState -Name '" + appPoolInfo.AppPoolName + @"') -ne 'stopped')
                                                {
                                                    sleep(1)
                                                }
                                                Start-WebAppPool -Name '" + appPoolInfo.AppPoolName + @"'
                                            }
                                            else
                                            {
                                                Start-WebAppPool -Name '" + appPoolInfo.AppPoolName + @"'
                                            }
                                            'Success'
                                        }
                                        catch
                                        {
                                            'Error occurred while trying to restart app pool.'
                                        }
                                    }
                                }
                                catch
                                {
                                        'Not able to run invoke-command for the server.'
                                }
");
                foreach (string str in powershell.Invoke<string>())
                {
                    result = str;
                }
            }
            if (result == "Success")
            {
                return new ApiResult<string>("Success", "200", true);
            }
            else
            {
                return new ApiResult<string>("Failed to restart App pool for app pool " + appPoolInfo.AppPoolName, "500", false);
            }
        }
    }
}
